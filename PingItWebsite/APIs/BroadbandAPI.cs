using Newtonsoft.Json;
using PingItWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                    HashSet<String> providers = new HashSet<String>();

                    foreach (var j in json)
                    {
                        if (!providers.Contains(j.provider))
                        {
                            providers.Add(j.provider);

                            //SoQL query sometimes gets same city name in different states, so filter
                            if (j.state.Equals(state))
                            {
                                Broadband bb = new Broadband
                                {
                                    blockcode = j.blockcode,
                                    provider = j.provider,
                                    state = j.state,
                                    city = city,
                                    speed = j.speed
                                };

                                broadbandList.Add(bb);
                            }

                        }
                    }
                }
            }
            return broadbandList;
        }
        #endregion
    }
}
