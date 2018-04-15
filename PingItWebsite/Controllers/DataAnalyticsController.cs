using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PingItWebsite.APIs;
using PingItWebsite.Graphs;
using PingItWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PingItWebsite.Controllers
{
    public class DataAnalyticsController : Controller
    {
        #region Variables
        private long UnixEpochTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
        #endregion

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
            List<LoadtimeAvgGraph> loadtime = new List<LoadtimeAvgGraph>();
            List<SpeedAvgGraph> speed = new List<SpeedAvgGraph>();
            List<ScoreAvgGraph> score = new List<ScoreAvgGraph>();

            UserPlotKey upk = new UserPlotKey();
            List<UserPlotKey> keys = upk.GetPlotKeys(HomeController._database);

            int i = 0; 
            foreach (UserTestAvgs avg in tests)
            {
                if (i < keys.Count)
                {
                    avg.userKey = keys[i];
                }

                loadtime.Add(new LoadtimeAvgGraph(avg.key, avg.loadtime));
                speed.Add(new SpeedAvgGraph(avg.key, avg.speed));
                score.Add(new ScoreAvgGraph(avg.key, avg.score));

                i++;
            }

            ViewBag.Loadtime = JsonConvert.SerializeObject(loadtime);
            ViewBag.Speed = JsonConvert.SerializeObject(speed);
            ViewBag.Score = JsonConvert.SerializeObject(score);

            return PartialView(tests);
        }

        /// <summary>
        /// Load user time plot partial views
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public IActionResult UserTimePlotSection(string city, string state, string provider)
        {
            UserTimePlot utp = new UserTimePlot();
            List<UserTimePlot> tests = utp.GetUserTestsOrdered(HomeController._database);

            //Prepare datapoints
            List<SpeedTimePlotGraph> speed1 = new List<SpeedTimePlotGraph>();
            List<SpeedTimePlotGraph> speed2 = new List<SpeedTimePlotGraph>();
            List<SpeedTimePlotGraph> speed3 = new List<SpeedTimePlotGraph>();
            List<SpeedTimePlotGraph> speed4 = new List<SpeedTimePlotGraph>();
            List<SpeedTimePlotGraph> speed5 = new List<SpeedTimePlotGraph>();
            List<LoadtimePlotGraph> loadtime6 = new List<LoadtimePlotGraph>();
            List<LoadtimePlotGraph> loadtime7 = new List<LoadtimePlotGraph>();
            List<LoadtimePlotGraph> loadtime8 = new List<LoadtimePlotGraph>();
            List<LoadtimePlotGraph> loadtime9 = new List<LoadtimePlotGraph>();
            List<LoadtimePlotGraph> loadtime10 = new List<LoadtimePlotGraph>();

            UserPlotKey upk = new UserPlotKey();
            List<UserPlotKey> keys = upk.GetPlotKeys(HomeController._database);

            //Loop through tests and only get those from the top 5
            for (int i = 0; i < tests.Count; i++)
            {
                //Hack version to allow 2 models at once-- not the best coding practice, but for the scope of project ok
                UserTimePlot plot = tests[i];
                if (i < keys.Count)
                {
                    plot.key = keys[i];
                }

                if (plot.rank < 6)
                {
                    //Convert to date to fit JavaScript format
                    long longDate = ToJsonDate(plot.date);
                    switch (plot.rank)
                    {
                        case 1:
                            speed1.Add(new SpeedTimePlotGraph(longDate, plot.speed));
                            loadtime6.Add(new LoadtimePlotGraph(longDate, plot.loadtime.Seconds));
                            break;
                        case 2:
                            speed2.Add(new SpeedTimePlotGraph(longDate, plot.speed));
                            loadtime7.Add(new LoadtimePlotGraph(longDate, plot.loadtime.Seconds));
                            break;
                        case 3:
                            speed3.Add(new SpeedTimePlotGraph(longDate, plot.speed));
                            loadtime8.Add(new LoadtimePlotGraph(longDate, plot.loadtime.Seconds));
                            break;
                        case 4:
                            speed4.Add(new SpeedTimePlotGraph(longDate, plot.speed));
                            loadtime9.Add(new LoadtimePlotGraph(longDate, plot.loadtime.Seconds));
                            break;
                        case 5:
                            speed5.Add(new SpeedTimePlotGraph(longDate, plot.speed));
                            loadtime10.Add(new LoadtimePlotGraph(longDate, plot.loadtime.Seconds));
                            break;
                    }
                }

            }

            //load speed datapoints
            ViewBag.Speed1 = JsonConvert.SerializeObject(speed1);
            ViewBag.Speed2 = JsonConvert.SerializeObject(speed2);
            ViewBag.Speed3 = JsonConvert.SerializeObject(speed3);
            ViewBag.Speed4 = JsonConvert.SerializeObject(speed4);
            ViewBag.Speed5 = JsonConvert.SerializeObject(speed5);
            //load loadtime datapoints
            ViewBag.Loadtime6 = JsonConvert.SerializeObject(loadtime6);
            ViewBag.Loadtime7 = JsonConvert.SerializeObject(loadtime7);
            ViewBag.Loadtime8 = JsonConvert.SerializeObject(loadtime8);
            ViewBag.Loadtime9 = JsonConvert.SerializeObject(loadtime9);
            ViewBag.Loadtime10 = JsonConvert.SerializeObject(loadtime10);

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
        /// Load the compare data partial view
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IActionResult CompareDataSection(string city, string state, string domain)
        {
            County county = new County();
            Website web = new Website();

            //Prepare the appropriate graphs
            List<BroadbandSpeedGraph> fccData = PrepareFCCData(city, state, domain, county);
            List<DomainLoadtimeAvgGraph> loadtimeData = PrepareLoadtimeData(web);
            List<CityLoadtimeAvgGraph> cityLoadtimeData = PrepareCityLoadtimeData(web);
            List<DomainLoadtimeAvgGraph> compareLoadtimeData = PrepareCompareLoadtimeData(domain, web);
            List<CityLoadtimeAvgGraph> compareCityLoadtimeData = PrepareCompareCityLoadtimeData(domain, web);

            ViewBag.FCCData = JsonConvert.SerializeObject(fccData);
            ViewBag.LoadtimeData = JsonConvert.SerializeObject(loadtimeData);
            ViewBag.CityLoadtimeData = JsonConvert.SerializeObject(cityLoadtimeData);
            ViewBag.CompareLoadtimeData = JsonConvert.SerializeObject(compareLoadtimeData);
            ViewBag.CompareCityLoadtimeData = JsonConvert.SerializeObject(compareCityLoadtimeData);
            return PartialView();
        }

        /// <summary>
        /// Helper method that prepares FCC data
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="county"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private List<BroadbandSpeedGraph> PrepareFCCData(string city, string state, string domain, County county)
        {
            //Get the census code
            string newCity = char.ToUpper(city[0]) + city.Substring(1);
            int code = county.GetCensusCode(city, state, HomeController._database);

            //Get list of broadbands in that census code
            BroadbandAPI ba = new BroadbandAPI();
            List<Broadband> broadbands = (ba.GetBroadbandSpeed(code, city, state)).OrderByDescending(o => o.provider).ToList();

            //Prepare graph data
            List<BroadbandSpeedGraph> fccData = new List<BroadbandSpeedGraph>();
            foreach (Broadband br in broadbands)
            {
                if (br.speed != 0)
                {
                    fccData.Add(new BroadbandSpeedGraph(br.provider, br.speed));
                }
            }

            //Add user broadband
            Broadband b = new Broadband();
            List<Broadband> userBroadband = b.GetUserProviderData(city, state, domain, HomeController._database);
            foreach (Broadband br in userBroadband)
            {
                fccData.Add(new BroadbandSpeedGraph(br.provider, br.speed));
            }

            return fccData;
        }

        /// <summary>
        /// Helper method that prepares domain loadtime data
        /// </summary>
        /// <param name="web"></param>
        /// <returns></returns>
        private List<DomainLoadtimeAvgGraph> PrepareLoadtimeData(Website web)
        {
            List<Website> webAvgs = web.GetAvgDomainLoadtime("null", HomeController._database);
            List<DomainLoadtimeAvgGraph> loadtimeData = new List<DomainLoadtimeAvgGraph>();

            foreach (Website w in webAvgs)
            {
                loadtimeData.Add(new DomainLoadtimeAvgGraph(w.website, w.total));
            }

            return loadtimeData;
        }

        /// <summary>
        /// Helper method that prepares city loadtime data
        /// </summary>
        /// <param name="web"></param>
        /// <returns></returns>
        private List<CityLoadtimeAvgGraph> PrepareCityLoadtimeData(Website web)
        {
            List<Website> cityAvgs = web.GetAvgCityLoadtime("null", HomeController._database);
            List<CityLoadtimeAvgGraph> cityLoadtimeData = new List<CityLoadtimeAvgGraph>();

            foreach (Website c in cityAvgs)
            {
                cityLoadtimeData.Add(new CityLoadtimeAvgGraph(c.location, c.total));
            }

            return cityLoadtimeData;
        }

        /// <summary>
        /// Prepare domain loadtime data for 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="web"></param>
        /// <returns></returns>
        private List<DomainLoadtimeAvgGraph> PrepareCompareLoadtimeData(string domain, Website web)
        {
            List<Website> webAvgs = web.GetAvgDomainLoadtime(domain, HomeController._database);
            List<DomainLoadtimeAvgGraph> loadtimeData = new List<DomainLoadtimeAvgGraph>();
            loadtimeData.Add(new DomainLoadtimeAvgGraph("Global", webAvgs[0].total));
            loadtimeData.Add(new DomainLoadtimeAvgGraph("User", web.GetUserAvgDomainLoadtime(domain, HomeController._database).total));
            return loadtimeData;
        }

        /// <summary>
        /// Helper method that prepares compare city loadtime data
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="web"></param>
        /// <returns></returns>
        private List<CityLoadtimeAvgGraph> PrepareCompareCityLoadtimeData(string domain, Website web)
        {
            List<Website> cityAvgs = web.GetAvgCityLoadtime(domain, HomeController._database);
            List<Website> userCityAvgs = web.GetDomainLoadtimeByLocation(domain, HomeController._database);

            List<CityLoadtimeAvgGraph> cityLoadtimeData = new List<CityLoadtimeAvgGraph>();

            foreach (Website c in cityAvgs)
            {
                cityLoadtimeData.Add(new CityLoadtimeAvgGraph(c.location, c.total));
            }

            foreach (Website uc in userCityAvgs)
            {
                cityLoadtimeData.Add(new CityLoadtimeAvgGraph(uc.location, uc.total));
            }

            return cityLoadtimeData;
        }
        #endregion

        #region Shared Helper Methods
        /// <summary>
        /// Given a url, find the website domain
        /// </summary>
        /// <param name="website"></param>
        /// <returns></returns>
        private string FindWebsiteDomain(string website)
        {
            string retWebsite;
            Uri uri = new Uri(website);
            string host = uri.Host;

            //count the number of periods and appropiately find the domain
            int count = host.Count(p => p == '.');
            if (count == 1)
            {
                retWebsite = host.Split(".")[0];
            }
            else
            {
                retWebsite = host.Split(".")[1];
            }
            return retWebsite;
        }

        /// <summary>
        /// Get the average broadband speed
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private double GetAverage(List<Broadband> list)
        {
            double sum = 0;
            foreach (Broadband b in list)
            {
                sum += b.speed;
            }
            return sum / list.Count;
        }
        #endregion
    }
}