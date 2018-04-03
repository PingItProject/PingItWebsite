using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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

            //Get ipv4 information
            IPAddress ipv4 = Array.FindLast(
                Dns.GetHostEntry(string.Empty).AddressList,
                a => a.AddressFamily == AddressFamily.InterNetwork);
            IPAddressAPI ipa = new IPAddressAPI();

            driver.LoadDriver(url, ipa.GetLocation(ipv4.ToString()).city, ipa.GetLocation(ipv4.ToString()).state, browser);
        }
        #endregion

        #region Table Methods
        /// <summary>
        /// Prepare table partial view
        /// </summary>
        /// <returns></returns>
        public IActionResult Table()
        {
            WebTest wt = new WebTest();
            while (!Driver._complete)
            {

            }

            //Get the webtests info
            tests = wt.GetUserWebTests(HomeController._username, Driver._batch, HomeController._database);
            int seconds = tests[0].loadtime.Seconds;
            
            //Using the page speed API, insert into database and then add to the table view
            PageSpeedAPI psa = new PageSpeedAPI();
            psa.InsertPageSpeed(tests[0].url, tests[0].loadtime.Seconds, tests[0].guid);
            GoogleTest gt = new GoogleTest();
            List<GoogleTest> gtList = gt.GetUserGoogleTests(tests[0].guid, HomeController._database);
            tests[0].googleTest = gtList[0];

            return PartialView(tests);
        }

  
        #endregion
    }
}