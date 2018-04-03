using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PingItWebsite.Models;

namespace PingItWebsite.Controllers
{
    public class CrowdSourceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Load general partial view
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

        public IActionResult DataOptions()
        {
            return PartialView();
        }
        
        public IActionResult CrowdSourceTable(string city, string state)
        {
            return null;
        }

    }
}