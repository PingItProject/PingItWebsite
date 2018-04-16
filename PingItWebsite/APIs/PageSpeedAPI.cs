using Newtonsoft.Json;
using PingItWebsite.Controllers;
using PingItWebsite.JsonModels;
using PingItWebsite.Models;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace PingItWebsite.APIs
{
    public class PageSpeedAPI
    {
        #region Variables
        string _APIkey = "[INSERT CREDENTIALS]";
        string _url = "https://www.googleapis.com/pagespeedonline/v4/runPagespeed?url={0}&strategy=desktop&key={1}";
        #endregion

        #region Constructors
        public PageSpeedAPI()
        {

        }
        #endregion

        #region Requests
        /// <summary>
        /// Grabs the page speed and inserts into database
        /// </summary>
        /// <param name="website"></param>
        /// <param name="seconds"></param>
        /// <param name="guid"></param>
        public void InsertPageSpeed(string website, int seconds, Guid guid)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-API-Key", _APIkey);

            string url = String.Format(_url, website, _APIkey);
            HttpResponseMessage msg = null;
            try
            {
                msg = httpClient.GetAsync(url).Result;
            } catch (AggregateException)
            {
                Debug.WriteLine("API (Page Speed): Cannot get speed results.");
            }
            if (msg != null)
            {
                using (msg)
                {
                    if (msg.IsSuccessStatusCode)
                    {
                        var data = msg.Content.ReadAsStringAsync().Result;
                        var json = JsonConvert.DeserializeObject<PageSpeed>(data);

                        if (json != null)
                        {
                            GoogleTest pst = new GoogleTest();
                            WebTest wt = new WebTest();

                            decimal webspeed = (decimal) ((json.stats.bytes / seconds) * .000008);

                            pst.CreateGoogleTest(guid, json.ruleGroups.speed.score, json.experience.category, json.stats.numResources,
                                json.stats.numHosts, json.stats.bytes, json.stats.htmlBytes, json.stats.cssBytes, json.stats.imageBytes,
                                webspeed, HomeController._database);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
