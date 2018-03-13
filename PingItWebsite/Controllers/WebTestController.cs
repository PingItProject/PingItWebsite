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
    }
}