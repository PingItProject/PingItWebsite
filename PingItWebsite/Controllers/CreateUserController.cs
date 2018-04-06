using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PingItWebsite.Models;

namespace PingItWebsite.Controllers
{
    public class CreateUserController : Controller
    {
        #region Variables
        public static bool _VisitedCreatedUser = false;
        #endregion

        #region View-Controllers
        /// <summary>
        /// Return CreateUser Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// Registers an account 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="email"></param>
        /// <param name="pass1"></param>
        /// <param name="pass2"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IActionResult RegisterAccount(string username, string fname, string lname, string email, string pass1, string pass2, string type)
        {
            if (!pass1.Equals(pass2))
            {
                return Json(new { failPass = true, exists = false });
            }
            User user = new User();
            if (HomeController._database == null)
            {
                HomeController._database = new Database();
            }
            HomeController._database.CheckConnection();

            if (user.UserExists(username, HomeController._database))
            {
                return Json(new { failPass = false, exists = true });
            }
            _VisitedCreatedUser = true;
            user.CreateUser(username, fname, lname, email, pass1, type, HomeController._database);
            return Json(new { failPass = false, exists = false });
        }
        #endregion
    }
}