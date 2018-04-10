using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PingItWebsite.Models;
using System.Collections.Generic;

namespace PingItWebsite.Controllers
{
    public class DataAnalyticsController : Controller
    {
        static List<UserTestAvgs> _tests;
        static bool _tableLoaded = false;

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Load user avg table partial view
        /// </summary>
        /// <returns></returns>
        public IActionResult UserTestAvgsTable()
        {
            UserTestAvgs wtc = new UserTestAvgs();
            _tests = wtc.GetWebsiteTestCounts(HomeController._database);
            _tableLoaded = true;

            while (!_tableLoaded)
            {

            }
            //Prepare data points
            List<LoadTimeAvg> dataPoints = new List<LoadTimeAvg>();
            foreach (UserTestAvgs avg in _tests)
            {
                dataPoints.Add(new LoadTimeAvg(avg.key, avg.loadtime));
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return PartialView(_tests);
        }

        public IActionResult UserTestAvgsGraph()
        {
            //Wait for the table to be loaded first
            while (!_tableLoaded)
            {

            }
            UserTestAvgs wtc = new UserTestAvgs();

            //Prepare data points
            List<UserTestAvgs> dataPoints = new List<UserTestAvgs>();
            foreach (UserTestAvgs avg in _tests)
            {
                //dataPoints.Add(new UserTestAvgs(avg.key, avg.speed, avg.loadtime, avg.score));
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
    }
}