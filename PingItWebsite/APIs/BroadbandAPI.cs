using Newtonsoft.Json;
using PingItWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PingItWebsite.APIs
{
    public class BroadbandAPI
    {
        string _url = "https://opendata.fcc.gov/resource/if4k-kzsc.json?$where=blockcode like '%25{0}%25'";

        #region Constructors
        public BroadbandAPI()
        {

        }
        #endregion

        /// <summary>
        /// Calls broadband api to get address
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Broadband GetBroadbandSpeed(string code)
        {
            HttpClient httpClient = new HttpClient();

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

            Broadband bb = null;
            using (msg)
            {
                if (msg.IsSuccessStatusCode)
                {
                    var data = msg.Content.ReadAsStringAsync().Result;
                    var json = JsonConvert.DeserializeObject<JsonModels.Broadband>(data);

                    //get guid
                    if (json != null)
                    {
                        bb = new Broadband
                        {
                            blockcode = json.blockcode,
                            provider = json.provider,
                            state = json.state,
                            speed = json.speed
                        };
                    }
                }
            }
            return bb;
        }
    }
}
