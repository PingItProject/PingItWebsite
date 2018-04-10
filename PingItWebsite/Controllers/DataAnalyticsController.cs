using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PingItWebsite.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace PingItWebsite.Controllers
{
    public class DataAnalyticsController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Load user section partial view
        /// </summary>
        /// <returns></returns>
        public IActionResult UserSection()
        {
            UserTestAvgs wtc = new UserTestAvgs();
            List<UserTestAvgs> tests = wtc.GetWebsiteTestCounts(HomeController._database);

            //Prepare data points
            List<LoadTimeAvg> data1 = new List<LoadTimeAvg>();
            List<SpeedAvg> data2 = new List<SpeedAvg>();
            foreach (UserTestAvgs avg in tests)
            {
                data1.Add(new LoadTimeAvg(avg.key, avg.loadtime));
                data2.Add(new SpeedAvg(avg.key, avg.speed));
            }

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(data1);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(data2);

            return PartialView(tests);
        }



    }
}