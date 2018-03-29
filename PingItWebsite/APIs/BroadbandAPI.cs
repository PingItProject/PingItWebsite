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

        public void GetBroadbandSpeed(string code)
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
                Debug.WriteLine("API (Page Speed): Cannot get speed results.");
            }
            /*if (msg == null)
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
                            city = json.city
                        };
                    }
                }
            }
            return ipl;
            */
        }
    }
}
