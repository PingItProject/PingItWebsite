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
        /// <param name="order"></param>
        /// <returns></returns>
        public IActionResult GeneralTable(string city, string state, string browser, bool order)
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

            //Checks the options for the filters to perform stored proc
            if (String.IsNullOrEmpty(city) && browser.Equals("all"))
            {
                tests = wt.GetWebTests(null, null, null, order, HomeController._database);
            } else if (String.IsNullOrEmpty(city))
            {
                tests = wt.GetWebTests(null, null, browser, order, HomeController._database);
            } else if (browser.Equals("all"))
            {
                tests = wt.GetWebTests(city, state, null, order, HomeController._database);
            } else
            {
                tests = wt.GetWebTests(city, state, browser, order, HomeController._database);
            }
            return PartialView(tests);
        }

        /// <summary>
        /// Load detailed table partial view
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="browser"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IActionResult DetailedTable(string city, string state, string browser, bool order)
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

            if (String.IsNullOrEmpty(city) && browser.Equals("all"))
            {
                tests = gt.GetGoogleTests(null, null, null, order, HomeController._database);
            }
            else if (String.IsNullOrEmpty(city))
            {
                tests = gt.GetGoogleTests(null, null, browser, order, HomeController._database);
            }
            else if (browser.Equals("all"))
            {
                tests = gt.GetGoogleTests(city, state, null, order, HomeController._database);
            }
            else
            {
                tests = gt.GetGoogleTests(city, state, browser, order, HomeController._database);
            }
            return PartialView(tests);
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
            Debug.WriteLine(broadbands.Count);

            return PartialView(broadbands);
        }
        #endregion

    }
}