using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PingItWebsite.APIs;
using PingItWebsite.Models;
using PingItWebsite.Selenium;

namespace PingItWebsite.Controllers
{
    public class WebTestController : Controller
    {
        #region Variables
        private List<WebTest> tests;

        private static Object wtLock = new Object();
        #endregion

        #region View-Controllers
        /// <summary>
        /// Returns Webtext Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Selenium Methods
        /// <summary>
        /// Pre-processing method that sets batch number and requests
        /// </summary>
        /// <param name="requests"></param>
        public void PrepareBatch(string requests)
        {
            //Create a batch number to keep track of user's tests
            WebTest wt = new WebTest();
            int batch = wt.GetBatch(HomeController._username, HomeController._database);
            Driver._batch = batch + 1;

            int norequests = Convert.ToInt32(requests);
            Driver._requests = norequests;
        }
        /// <summary>
        /// Test a user's website
        /// </summary>
        /// <param name="url"></param>
        /// <param name="browser"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="provider"></param>
        /// <param name="requests"></param>
        public void TestWebsite(string url, string browser, string city, string state, string provider, string requests)
        {
            lock(wtLock)
            {
                Driver driver = new Driver();

                //Get today's date
                DateTime now = DateTime.Now;

                int norequests = Convert.ToInt32(requests);
                //If either is empty, then use the ipv4 information to get computer's location
                if (!browser.Equals("firefox") || String.IsNullOrEmpty(city) || String.IsNullOrEmpty(state))
                {
                    //Get ipv4 information
                    IPAddress ipv4 = Array.FindLast(
                    Dns.GetHostEntry(string.Empty).AddressList,
                    a => a.AddressFamily == AddressFamily.InterNetwork);
                    IPAddressAPI ipa = new IPAddressAPI();
                    driver.LoadDriver(url, ipa.GetLocation(ipv4.ToString()).city, ipa.GetLocation(ipv4.ToString()).state, browser, provider, norequests);

                }
                else
                {
                    //Get coordinates using the Geocoding API
                    GeocodingAPI ga = new GeocodingAPI();
                    IList<double> coord = ga.GetLocationCoords(city, state);
                    double lat = coord[0];
                    double lng = coord[1];

                    string path = Path.Combine(Directory.GetCurrentDirectory(), "Selenium/location.json");

                    //Update JSON with the correct latitude and longitude of the chosen dest.
                    string json = System.IO.File.ReadAllText(path);
                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    jsonObj["location"]["lat"] = lat;
                    jsonObj["location"]["lng"] = lng;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

                    System.IO.File.WriteAllText(path, output);
                    driver.LoadDriver(url, city, state, browser, provider, norequests);

                }
            }
            
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
            while (Driver._requests != 0)
            {

            }

            //Get the webtests info
            tests = wt.GetUserWebTests(HomeController._username, Driver._batch, HomeController._database);

            PageSpeedAPI psa = new PageSpeedAPI();
            GoogleTest gt = new GoogleTest();

            Debug.WriteLine(tests.Count + "IIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII");
            //Loop through the number of general tests and create a corresponding google test
            for (int i = 0; i < tests.Count; i++)
            {
                Debug.WriteLine("SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSssssss" + i); 
                int seconds = tests[i].loadtime.Seconds;

                //Using the page speed API, insert into database and then add to the table view
                psa.InsertPageSpeed(tests[i].url, tests[i].loadtime.Seconds, tests[i].guid);
                
                //Find the matching 
                //List<GoogleTest> gtList = gt.GetUserGoogleTests(tests[i].guid, HomeController._database);

                //Debug.WriteLine("The list size " + gtList.Count);
                /*tests[i].googleTest = gtList[i];*/
            }

            return PartialView(tests);
        }  
        #endregion
    }
}