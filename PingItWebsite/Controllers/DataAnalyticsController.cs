using Microsoft.AspNetCore.Mvc;

namespace PingItWebsite.Controllers
{
    public class DataAnalyticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}