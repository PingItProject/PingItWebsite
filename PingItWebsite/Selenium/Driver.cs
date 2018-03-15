using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using PingItWebsite.Models;
using PingItWebsite.Controllers;

namespace PingItWebsite.Selenium
{
    public class Driver
    {
        public static int _batch;
        public static bool _complete = false;

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Driver()
        {

        }
        #endregion

        #region Drivers
        /// <summary>
        /// Loads a web driver
        /// </summary>
        /// <param name="url"></param>
        public void LoadDriver(string url, string browser)
        {
            DateTime now = DateTime.Now;

            IWebDriver driver;
            if (browser.Equals("chrome"))
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArguments(new List<string>() { "headless" });
                driver = new ChromeDriver(Directory.GetCurrentDirectory(), options);
            }
            else if (browser.Equals("firefox"))
            {
                FirefoxOptions options = new FirefoxOptions();
                options.AddArguments("--headless");
                driver = new FirefoxDriver(Directory.GetCurrentDirectory(), options);
            }
            else
            {
                driver = new PhantomJSDriver(Directory.GetCurrentDirectory());
            }

            //Create timer to time response
            Stopwatch timer = new Stopwatch();
            timer.Start();
            driver.Navigate().GoToUrl(url);

            timer.Stop();

            //get size of the page
            int bytes = driver.PageSource.Length;
            double pagesize = bytes / 1024;

            TimeSpan loadtime = timer.Elapsed;
            double webspeed = (pagesize / 1000) / loadtime.Seconds;
            driver.Close();

            WebTest wt = new WebTest();
            int batch = wt.GetBatch(HomeController._username, HomeController._database);
            _batch = batch + 1;

            //Add to database
            wt.CreateWebTest(HomeController._username, now, url, webspeed, loadtime, pagesize, 1, "not specified", 
                browser, _batch, Guid.NewGuid(), HomeController._database);
            _complete = true;
        }


        #endregion


    }
}
