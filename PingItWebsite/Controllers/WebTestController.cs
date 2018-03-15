using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PingItWebsite.Models;
using PingItWebsite.Selenium;

namespace PingItWebsite.Controllers
{
    public class WebTestController : Controller
    {
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
            List<WebTest> tests = wt.GetWebTests(HomeController._username, Driver._batch, HomeController._database);
            return PartialView(tests);
        }
        #endregion
    }
}