using Newtonsoft.Json;
using PingItWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace PingItWebsite.APIs
{
    public class BroadbandAPI
    {
        #region Variables
        string _url = "https://opendata.fcc.gov/resource/if4k-kzsc.json?$where=blockcode like '%25{0}%25'";
        #endregion

        #region Constructors
        public BroadbandAPI()
        {

        }
        #endregion

        #region Requests
        /// <summary>
        /// Calls broadband api to get speed information
        /// </summary>
        /// <param name="code"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<Broadband> GetBroadbandSpeed(int code, string city, string state)
        {
            HttpClient httpClient = new HttpClient();
            List<Broadband> broadbandList = new List<Broadband>();

            string url = String.Format(_url, code);
            HttpResponseMessage msg = null;
            try
            {
                msg = httpClient.GetAsync(url).Result;
            }
            catch (AggregateException)
            {
                Debug.WriteLine("API (Broadband): Cannot get speed results.");
            }


            if (msg == null)
            {
                return null;
            }

            using (msg)
            {
                if (msg.IsSuccessStatusCode)
                {
                    var data = msg.Content.ReadAsStringAsync().Result;
                    
                    //Deserialize in this format because there are many tuples of broadband
                    var json = JsonConvert.DeserializeObject<List<JsonModels.Broadband>>(data);

                    Dictionary<String, Dictionary<double, int>> providers = new Dictionary<String, Dictionary<double, int>>();

                    foreach (var j in json)
                    {
                        //If there is no repeats, htne only allow 1 provider in the list
                        if (!providers.ContainsKey(j.provider))
                        {

                            //SoQL query sometimes gets same city name in different states, so filter
                            if (j.state.Equals(state))
                            {
                                //Create a dictionary of speed so you can later get the avg
                                Dictionary<double, int> speedDict = new Dictionary<double, int>();
                                speedDict.Add(j.speed, 1);

                                Broadband bb = new Broadband
                                {
                                    blockcode = j.blockcode,
                                    provider = j.provider,
                                    state = j.state,
                                    city = city,
                                    speedDict = speedDict
                                };

                                providers.Add(j.provider, speedDict);
                                broadbandList.Add(bb);
                            }
                        } else
                        {
                            //the dictionary is only of size 0
                            Dictionary<double, int> tempSpeed = providers[j.provider];

                            //update the total evaluated and the total speed
                            double key = tempSpeed.Keys.First();
                            int total = tempSpeed[key];
                            tempSpeed.Remove(key);
                            tempSpeed.Add(key + j.speed, total + 1);
                            providers[j.provider] = tempSpeed;
                        }
                    }

                    //After finishing speed map, update the speeds
                    foreach (Broadband b in broadbandList)
                    {
                        double key = b.speedDict.Keys.First();
                        b.speed = key / b.speedDict[key];
                    }
                }
            }
            return broadbandList;
        }

        #endregion
    }
}
