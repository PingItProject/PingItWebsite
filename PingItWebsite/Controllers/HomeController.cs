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
        public static Database _database;
        public static string _username;

        /// <summary>
        /// View Home Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            
            return View();
        }


        /// <summary>
        /// View Error
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Validate Login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IActionResult ValidateLogin(string username, string password)
        {
            _username = username;

            //check password to see if it matches
            User user = new User();
            if (_database == null)
            {
                _database = new Database();
            }
            _database.CheckConnection();
            bool valid = user.IsValid(username, password, _database);
            if (valid)
            {
                return Json(new { valid = true });
            }
            return Json(new { valid = false });

        }


    }
}
