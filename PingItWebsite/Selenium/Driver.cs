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
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.CodeAnalysis;
using System.Text;

namespace PingItWebsite.Selenium
{
    public class Driver
    {
        #region variables
        public static int _batch;
        public static bool _complete = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Driver()
        {

        }
        #endregion

        #region Driver Methods
        /// <summary>
        /// Load the Selenium driver
        /// </summary>
        /// <param name="url"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="browser"></param>
        public void LoadDriver(string url, string city, string state, string browser)
        {
            DateTime now = DateTime.Now;
            IWebDriver driver;
            if (browser.Equals("chrome"))
            {
                //add preferences
                ChromeOptions options = new ChromeOptions();
                options.AddArguments(new List<string>() { "headless" });
                driver = new ChromeDriver(Directory.GetCurrentDirectory(), options);
            }
            else if (browser.Equals("firefox"))
            {
                FirefoxOptions options = new FirefoxOptions();
                options.AddArguments("--headless");

                //allows encoding in .NET core
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                //add new geolocation
                var profile = new FirefoxProfile();
                profile.SetPreference("geo.prompt.testing", true);
                profile.SetPreference("geo.prompt.testing.allow", true);
                profile.SetPreference("geo.enabled", true);
                //profile.SetPreference("geo.wifi.uri", @"C:\Users\Brighton\source\repos\PingItWebsite\PingItWebsite\Selenium\location.json");
                profile.SetPreference("geo.wifi.uri", "~/Selenium/location.json");
                options.Profile = profile;
                driver = new FirefoxDriver(Directory.GetCurrentDirectory(), options);
            }
            else
            {
                driver = new PhantomJSDriver(Directory.GetCurrentDirectory());
            }

            //Create timer to time response, stop timer, and record time
            Stopwatch timer = new Stopwatch();
            timer.Start();
            driver.Navigate().GoToUrl(url);
            timer.Stop();
            TimeSpan loadtime = timer.Elapsed;
            driver.Close();

            //Create a batch number to keep track of user's tests
            WebTest wt = new WebTest();
            int batch = wt.GetBatch(HomeController._username, HomeController._database);
            _batch = batch + 1;

            if (String.IsNullOrEmpty(city))
            {
                city = "not specified";
                state = "N/A";
            }

            //Add to database
            city = city.ToLower();

            wt.CreateWebTest(HomeController._username, now, url, loadtime, 1, city, state, 
                browser, "GTLAWN", _batch, Guid.NewGuid(), HomeController._database);
            _complete = true;
        }

        /// <summary>
        /// Helper function that helps a Chrome driver execute 
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static IJavaScriptExecutor Scripts(IWebDriver driver)
        {
            return (IJavaScriptExecutor)driver;
        }

        #endregion


    }
}
