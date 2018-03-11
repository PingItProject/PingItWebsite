using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PingItWebsite.Controllers
{
    public class OptionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}