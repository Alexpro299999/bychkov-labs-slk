using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WatchShop.DataAccess.Models;
using WatchShop.DataAccess.Repositories;
using WatchShop.Web.Models;

namespace WatchShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Watch> _watchRepo;

        public HomeController(ILogger<HomeController> logger, IRepository<Watch> watchRepo)
        {
            _logger = logger;
            _watchRepo = watchRepo;
        }

        public async Task<IActionResult> Index()
        {
            var watches = await _watchRepo.GetAllAsync(includeProperties: "Category,Manufacturer");
            return View(watches);
        }

        public async Task<IActionResult> Details(int id)
        {
            var watch = await _watchRepo.GetAsync(id, includeProperties: "Category,Manufacturer");
            if (watch == null)
            {
                return NotFound();
            }
            return View(watch);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}