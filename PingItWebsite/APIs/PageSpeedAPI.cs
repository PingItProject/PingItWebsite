﻿using Newtonsoft.Json;
using PingItWebsite.Controllers;
using PingItWebsite.JsonModels;
using PingItWebsite.Models;
using PingItWebsite.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PingItWebsite.APIs
{
    public class PageSpeedAPI
    {
        string _APIkey = "AIzaSyBuJEUbGXKZ_DH76YM8uY4RQGzIK7dojxI";
        string _url = "https://www.googleapis.com/pagespeedonline/v4/runPagespeed?url={0}&strategy=desktop&key={1}";

        #region
        public PageSpeedAPI()
        {

        }
        #endregion

        #region Inserts
        /// <summary>
        /// Gets page speed
        /// </summary>
        /// <param name="website"></param>
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
                        //deserialize json to congress members
                        var data = msg.Content.ReadAsStringAsync().Result;
                        var json = JsonConvert.DeserializeObject<PageSpeed>(data);

                        //get guid
                        if (json != null)
                        {
                            GoogleTest pst = new GoogleTest();
                            WebTest wt = new WebTest();

                            decimal webspeed = (decimal) ((json.stats.bytes / seconds) * .000008);
                       
                            Debug.WriteLine("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRr " + webspeed);
                            //Get the foreign key
                            //guid = wt.GetGuid(HomeController._username, Driver._batch, HomeController._database);

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
