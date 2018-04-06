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
using System.Text;

namespace PingItWebsite.Selenium
{
    public class Driver
    {
        #region Variables
        public static int _batch;
        public static int _requests = Int32.MaxValue;

        private WebTest wt;
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
        /// <param name="provider"></param>
        /// <param name="requests"></param>
        public void LoadDriver(string url, string city, string state, string browser, string provider, int requests)
        {
            DateTime now = DateTime.Now;
            IWebDriver driver;
            
            if (String.IsNullOrEmpty(provider))
            {
                provider = "not specified";
            }

            //Create drivers depending on browser type
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

            if (String.IsNullOrEmpty(city) || String.IsNullOrEmpty(state))
            {
                city = "not specified";
                state = "N/A";
            }

            //Add to database
            city = city.ToLower();

            wt = new WebTest();
            wt.CreateWebTest(HomeController._username, now, url, loadtime, requests, city, state,
                browser, provider, _batch, Guid.NewGuid(), HomeController._database);

            _requests--;
        }

        #endregion


    }
}
