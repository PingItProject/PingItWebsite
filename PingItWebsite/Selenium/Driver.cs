using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
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
        /// Load the Selenium driver
        /// </summary>
        /// <param name="url"></param>
        /// <param name="location"></param>
        /// <param name="browser"></param>
        public void LoadDriver(string url, string location, string browser)
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

            TimeSpan loadtime = timer.Elapsed;
            driver.Close();

            WebTest wt = new WebTest();
            int batch = wt.GetBatch(HomeController._username, HomeController._database);
            _batch = batch + 1;

            if (String.IsNullOrEmpty(location))
            {
                location = "not specified";
            }

            //Add to database
            wt.CreateWebTest(HomeController._username, now, url, loadtime, 1, location, 
                browser, _batch, Guid.NewGuid(), HomeController._database);
            _complete = true;
        }


        #endregion


    }
}
