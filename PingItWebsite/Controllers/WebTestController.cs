using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PingItWebsite.APIs;
using PingItWebsite.Models;
using PingItWebsite.Selenium;

namespace PingItWebsite.Controllers
{
    public class WebTestController : Controller
    {
        List<WebTest> tests;
        /// <summary>
        /// Returns Webtext Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        #region Selenium Methods
        /// <summary>
        /// Creates a web test
        /// </summary>
        /// <param name="url"></param>
        /// <param name="browser"></param>
        public void TestWebsite(string url, string browser)
        {
            DateTime now = DateTime.Now;
            Driver driver = new Driver();
            driver.LoadDriver(url, browser);
        }
        #endregion

        #region Table Methods
        /// <summary>
        /// Prepare a dynamic table of webtests
        /// </summary>
        /// <returns></returns>
        public IActionResult Table()
        {
            WebTest wt = new WebTest();
            while (!Driver._complete)
            {

            }
            tests = wt.GetWebTests(HomeController._username, Driver._batch, HomeController._database);
            int seconds = tests[0].loadtime.Seconds;

            PageSpeedAPI psa = new PageSpeedAPI();
            psa.InsertPageSpeed(tests[0].url, tests[0].loadtime.Seconds, tests[0].guid);
            GoogleTest gt = new GoogleTest();
            List<GoogleTest> gtList = gt.GetGoogleTests(tests[0].guid, HomeController._database);
            tests[0].googleTest = gtList[0];
            return PartialView(tests);
        }

  
        #endregion
    }
}