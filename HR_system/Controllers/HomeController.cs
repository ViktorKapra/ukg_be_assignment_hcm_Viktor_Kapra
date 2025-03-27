using HR_system.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HR_system.Controllers
{
    /// <summary>
    /// Controller for the home page and error handling.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET action for the home page.
        /// </summary>
        /// <returns>The view for the home page.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET action for the privacy page.
        /// </summary>
        /// <returns>The view for the privacy page.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// GET action for the error page.
        /// </summary>
        /// </summary>
        /// <returns>The view for the error page.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
