using Newtonsoft.Json;
using PingItWebsite.JsonModels;
using PingItWebsite.Models;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace PingItWebsite.APIs
{
    public class IPAddressAPI
    {
        string _url = "http://ip-api.com/json/{0}";

        #region Constructors
        public IPAddressAPI()
        {

        }
        #endregion

        #region Requests
        /// <summary>
        /// Calls IP Address api to get location
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public IPLocation GetLocation(string ipAddress)
        {
            HttpClient httpClient = new HttpClient();

            string url = String.Format(_url, ipAddress);
            HttpResponseMessage msg = null;
            try
            {
                msg = httpClient.GetAsync(url).Result;
            }
            catch (AggregateException)
            {
                Debug.WriteLine("API (Page Speed): Cannot get speed results.");
            }
            if (msg == null)
            {
                return null;
            }

            IPLocation ipl = null;
            using (msg)
            {
                if (msg.IsSuccessStatusCode)
                {
                    var data = msg.Content.ReadAsStringAsync().Result;
                    var json = JsonConvert.DeserializeObject<IP>(data);

                    //get guid
                    if (json != null)
                    {
                        ipl = new IPLocation
                        {
                            country = json.country,
                            regionName = json.regionName,
                            city = json.city,
                            state = json.region
                        };
                    }
                }
            }
            return ipl;
        }
        #endregion
    }
}
