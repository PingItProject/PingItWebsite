using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PingItWebsite.APIs;
using PingItWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PingItWebsite.Controllers
{
    public class DataAnalyticsController : Controller
    {

        private long UnixEpochTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

        #region View-Controllers
        /// <summary>
        /// Load the main index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Load user avgs section partial view
        /// </summary>
        /// <returns></returns>
        public IActionResult UserAvgsSection()
        {
            UserTestAvgs wtc = new UserTestAvgs();
            List<UserTestAvgs> tests = wtc.GetUserTestAvgs(HomeController._database);

            //Prepare data points
            List<LoadTimeAvgGraph> data1 = new List<LoadTimeAvgGraph>();
            List<SpeedAvgGraph> data2 = new List<SpeedAvgGraph>();
            List<ScoreAvgGraph> data3 = new List<ScoreAvgGraph>();

            foreach (UserTestAvgs avg in tests)
            {
                data1.Add(new LoadTimeAvgGraph(avg.key, avg.loadtime));
                data2.Add(new SpeedAvgGraph(avg.key, avg.speed));
                data3.Add(new ScoreAvgGraph(avg.key, avg.score));
            }

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(data1);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(data2);
            ViewBag.DataPoints3 = JsonConvert.SerializeObject(data3);

            return PartialView(tests);
        }

        /// <summary>
        /// Load user 
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public IActionResult UserTimePlotSection(string city, string state, string provider)
        {
            UserTimePlot utp = new UserTimePlot();
            List<UserTimePlot> tests = utp.GetUserTestChron(HomeController._database);

            //Prepare datapoints
            List<SpeedTimePlotGraph> data1 = new List<SpeedTimePlotGraph>();
            List<SpeedTimePlotGraph> data2 = new List<SpeedTimePlotGraph>();
            List<SpeedTimePlotGraph> data3 = new List<SpeedTimePlotGraph>();
            List<SpeedTimePlotGraph> data4 = new List<SpeedTimePlotGraph>();
            List<SpeedTimePlotGraph> data5 = new List<SpeedTimePlotGraph>();

            //Prepare datapoints
            List<LoadtimePlotGraph> data6 = new List<LoadtimePlotGraph>();
            List<LoadtimePlotGraph> data7 = new List<LoadtimePlotGraph>();
            List<LoadtimePlotGraph> data8 = new List<LoadtimePlotGraph>();
            List<LoadtimePlotGraph> data9 = new List<LoadtimePlotGraph>();
            List<LoadtimePlotGraph> data10 = new List<LoadtimePlotGraph>();

            //Loop through tests and only get those from the top 5
            for (int i = 0; i < tests.Count; i++)
            {
                UserTimePlot plot = tests[i];
                if (plot.rank < 6)
                {
                    //Convert to date to fit JavaScript format
                    long longDate = ToJsonDate(plot.date);

                    switch (plot.rank)
                    {
                        case 1:
                            data1.Add(new SpeedTimePlotGraph(longDate, plot.speed));
                            data6.Add(new LoadtimePlotGraph(longDate, plot.loadtime.Seconds));
                            break;
                        case 2:
                            data2.Add(new SpeedTimePlotGraph(longDate, plot.speed));
                            data7.Add(new LoadtimePlotGraph(longDate, plot.loadtime.Seconds));
                            break;
                        case 3:
                            data3.Add(new SpeedTimePlotGraph(longDate, plot.speed));
                            data8.Add(new LoadtimePlotGraph(longDate, plot.loadtime.Seconds));
                            break;
                        case 4:
                            data4.Add(new SpeedTimePlotGraph(longDate, plot.speed));
                            data9.Add(new LoadtimePlotGraph(longDate, plot.loadtime.Seconds));
                            break;
                        case 5:
                            data5.Add(new SpeedTimePlotGraph(longDate, plot.speed));
                            data10.Add(new LoadtimePlotGraph(longDate, plot.loadtime.Seconds));
                            break;
                    }
                }

            }

            //load speed datapoints
            ViewBag.DataPoints1 = JsonConvert.SerializeObject(data1);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(data2);
            ViewBag.DataPoints3 = JsonConvert.SerializeObject(data3);
            ViewBag.DataPoints4 = JsonConvert.SerializeObject(data4);
            ViewBag.DataPoints5 = JsonConvert.SerializeObject(data5);
            //load loadtime datapoints
            ViewBag.DataPoints6 = JsonConvert.SerializeObject(data6);
            ViewBag.DataPoints7 = JsonConvert.SerializeObject(data7);
            ViewBag.DataPoints8 = JsonConvert.SerializeObject(data8);
            ViewBag.DataPoints9 = JsonConvert.SerializeObject(data9);
            ViewBag.DataPoints10 = JsonConvert.SerializeObject(data10);
            return PartialView(tests);

        }

        /// <summary>
        /// Convert to JsonDate time
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private long ToJsonDate(DateTime value)
        {
            return (value.ToUniversalTime().Ticks - UnixEpochTicks) / 10000;
        }

        /// <summary>
        /// Load the FCC Data Section partial view
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IActionResult FCCDataSection(string city, string state)
        {
            //Get census code
            County county = new County();
            string newCity = char.ToUpper(city[0]) + city.Substring(1);
            int code = county.GetCensusCode(city, state, HomeController._database);

            //Get list of broadbands
            BroadbandAPI ba = new BroadbandAPI();
            List<Broadband> broadbands = (ba.GetBroadbandSpeed(code, city, state)).OrderByDescending(o => o.provider).ToList();

            List<BroadbandSpeedGraph> data = new List<BroadbandSpeedGraph>();
            foreach (Broadband b in broadbands)
            {
                data.Add(new BroadbandSpeedGraph(b.provider, b.speed));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(data);
            return PartialView();
        }
#endregion
    }
}