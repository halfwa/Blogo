using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Blogoblog.DAL.Models;


namespace Blogoblog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog подключен к HomeController");
        }

        public IActionResult Index()
        {
            _logger.LogInformation("HomeController - Index");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("HomeController - Privacy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError(Activity.Current?.Id, "Server error");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
