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
        private TimeSpan _loadtime;
        private double _pageSize;

        #region Getters/Setters
        public TimeSpan LoadTime
        {
            get { return _loadtime; }
            set { _loadtime = value; }
        }

        public double PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
        #endregion

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
            _pageSize = bytes / 100;

            _loadtime = timer.Elapsed;
            driver.Close();

            //Add to database
            WebTest tests = new WebTest(HomeController._username, now, url, _loadtime, _pageSize, 1, null, browser, Guid.NewGuid(), HomeController._database);
        }


        #endregion


    }
}
