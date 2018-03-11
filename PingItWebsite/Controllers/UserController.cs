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
    public class UserController : Controller
    {
        //Database _database;

        public IActionResult Index()
        {
            return View();
        }

    }

}
 