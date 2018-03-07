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
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
      
            return View();
        }

        public IActionResult About()
        {
  
            ViewData["Message"] = "Your application description page.";
            //Testing Purposes Only
            //Database db = new Database();
            //db.Initialize();
            Driver driver = new Driver();
            driver.LoadChromeDriver("https://www.facebook.com/brighton.trugman", "phantom JS");


            return View();
        }
        
        public void Test()
        {
            Debug.WriteLine("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
