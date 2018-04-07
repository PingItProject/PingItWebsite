using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PingItWebsite.APIs;
using PingItWebsite.Models;

namespace PingItWebsite.Controllers
{
    public class CrowdSourceController : Controller
    {
        #region View-Controllers
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Load general table partial view
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="browser"></param>
        /// <param name="website"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IActionResult GeneralTable(string city, string state, string browser, string website, bool order)
        {
            WebTest wt = new WebTest();
            List<WebTest> tests;

            //Check if a specific city was entered, else assume all
            if (!String.IsNullOrEmpty(city))
            {
                string temp = city;
                city = city.ToLower();
                //Check if a city was entered, if not get the state that corresponds
                if (String.IsNullOrEmpty(state))
                {
                    Counties counties = new Counties();
                    state = counties.GetState(temp, HomeController._database);
                }
            }

            //Get the default array
            string[] defArr = GetDefault(browser, website);

            tests = wt.GetWebTests(city, state, defArr[0], defArr[1], order, HomeController._database);

            return PartialView(tests);
        }

        /// <summary>
        /// Load detailed table partial view
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="browser"></param>
        /// <param name="website"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IActionResult DetailedTable(string city, string state, string browser, string website, bool order)
        {
            GoogleTest gt = new GoogleTest();
            List<GoogleTest> tests;

            //Check if a specific city was entered, else assume all
            if (!String.IsNullOrEmpty(city))
            {
                string temp = city;
                city = city.ToLower();
                if (String.IsNullOrEmpty(state))
                {
                    Counties counties = new Counties();
                    state = counties.GetState(temp, HomeController._database);
                }
            }

            //Get the default array
            string[] defArr = GetDefault(browser, website);

            tests = gt.GetGoogleTests(city, state, defArr[0], defArr[1], order, HomeController._database);
            return PartialView(tests);
        }

        /// <summary>
        /// Helper method that sets browser and website
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="website"></param>
        /// <returns></returns>
        private string[] GetDefault(string browser, string website)
        {
            string[] retArr = new string[2];
            //Checks if the browser is all, then the query selects all browsers
            if (browser.Equals("all"))
            {
                retArr[0] = null;
            } else
            {
                retArr[0] = browser;
            }

            string tempWebsite = null;
            if (!String.IsNullOrEmpty(website))
            {
                Uri uri = new Uri(website);
                string host = uri.Host;

                //count the number of periods and appropiately find the domain
                int count = host.Count(p => p == '.');
                if (count == 1)
                {
                    tempWebsite = host.Split(".")[0];
                }
                else
                {
                    tempWebsite = host.Split(".")[1];
                }
            }
            retArr[1] = tempWebsite;
            return retArr;

        }

        /// <summary>
        /// Load FCC crowd source table partial view
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="ordering"></param>
        /// <returns></returns>
        public IActionResult FCCTable(string city, string state, bool ordering)
        {
            //Get census code
            Counties counties = new Counties();
            string newCity = char.ToUpper(city[0]) + city.Substring(1);
            int code = counties.GetCensusCode(city, state, HomeController._database);

            //Get list of broadbands
            BroadbandAPI ba = new BroadbandAPI();
            List<Broadband> broadbands;

            //If ordering is ascending
            if (ordering)
            {
                broadbands = (ba.GetBroadbandSpeed(code, city, state)).OrderByDescending(o => o.provider).ToList();
            } else
            {
                broadbands = (ba.GetBroadbandSpeed(code, city, state)).OrderBy(o => o.provider).ToList();
            }

            return PartialView(broadbands);
        }
        #endregion

    }
}