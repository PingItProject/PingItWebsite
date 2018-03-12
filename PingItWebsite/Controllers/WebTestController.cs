using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PingItWebsite.Models;
using PingItWebsite.Selenium;

namespace PingItWebsite.Controllers
{
    public class WebTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

   
        public void TestWebsite(string url)
        {
            DateTime now = DateTime.Now;
            Driver driver = new Driver();
            driver.LoadDriver(url, "phantom JS");
            Debug.WriteLine("TESSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSST");

        }
    }
}