using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PingItWebsite.Models;
using PingItWebsite.Selenium;

namespace PingItWebsite.Controllers
{
    public class HomeController : Controller
    {
        public Database _database;

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

        /*[HttpGet]
        public IActionResult Login()
        {
            return View();
        }*/

        public IActionResult ValidateLogin(string username, string password)
        {
            //initialize database
            if (_database == null)
            {
                _database = new Database();
                _database.CheckConnection();
            }
    
            //check password to see if it matches
            User user = new User();
            bool valid = user.IsValid(username, password, _database);
            if (valid)
            {
                return Json(new { valid = true });
            }
            return Json(new { valid = false });

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

        public void Testing()
        {
            Debug.WriteLine(Directory.GetCurrentDirectory());
            Debug.WriteLine("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRr");
        }
    }
}
