using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        /// Load general table partial view
        /// </summary>
        /// <returns></returns>
        public IActionResult GeneralTable(string location, string browser, bool order)
        {
            WebTest wt = new WebTest();
            List<WebTest> tests;

            if (!String.IsNullOrEmpty(location))
            {
                location = location.ToLower();
            }

            if (String.IsNullOrEmpty(location) && browser.Equals("all"))
            {
                tests = wt.GetWebTests(null, null, order, HomeController._database);
            } else if (String.IsNullOrEmpty(location))
            {
                tests = wt.GetWebTests(null, browser, order, HomeController._database);
            } else if (browser.Equals("all"))
            {
                tests = wt.GetWebTests(location, null, order, HomeController._database);
            } else
            {
                tests = wt.GetWebTests(location, browser, order, HomeController._database);
            }
            return PartialView(tests);
        }

        /// <summary>
        /// Load detailed table partial view
        /// </summary>
        /// <param name="location"></param>
        /// <param name="browser"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IActionResult DetailedTable(string location, string browser, bool order)
        {
            GoogleTest gt = new GoogleTest();
            List<GoogleTest> tests;

            if (!String.IsNullOrEmpty(location))
            {
                location = location.ToLower();
            }

            if (String.IsNullOrEmpty(location) && browser.Equals("all"))
            {
                tests = gt.GetGoogleTests(null, null, order, HomeController._database);
            }
            else if (String.IsNullOrEmpty(location))
            {
                tests = gt.GetGoogleTests(null, browser, order, HomeController._database);
            }
            else if (browser.Equals("all"))
            {
                tests = gt.GetGoogleTests(location, null, order, HomeController._database);
            }
            else
            {
                tests = gt.GetGoogleTests(location, browser, order, HomeController._database);
            }
            return PartialView(tests);
        }

    }
}