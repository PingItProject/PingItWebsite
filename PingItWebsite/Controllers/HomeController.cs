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

        public IActionResult Test()
        {
            return View();
        }

        public IActionResult IsMatchingPassword(string username, string password)
        {
            Debug.WriteLine("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRSSSS");
            //initialize database
            if (_database == null)
            {
                _database = new Database();
                _database.CheckConnection();
            }
    
            //check password to see if it matches
            User user = new User();
            string dbPassword = user.GetValue(username, "password", _database);
            Debug.WriteLine("TTTTTTTTTTTTTTTTT " + dbPassword); 
            if (dbPassword.Equals(password))
            {
                Debug.WriteLine("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRrr");
                return Json(new { match = true });
            } else
            {
                return Json(new { match = false });
            }

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
