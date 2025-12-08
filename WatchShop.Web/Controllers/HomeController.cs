using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WatchShop.DataAccess.Models;
using WatchShop.DataAccess.Repositories;
using WatchShop.Web.Models;
using WatchShop.DataAccess;

namespace WatchShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Watch> _watchRepo;
        private readonly IRepository<Category> _categoryRepo;

        public HomeController(IRepository<Watch> watchRepo, IRepository<Category> categoryRepo)
        {
            _watchRepo = watchRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index(string category, string searchString, string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentCategory = category;

            var watches = await _watchRepo.GetAllAsync(includeProperties: "Category,Manufacturer");

            if (!String.IsNullOrEmpty(searchString))
            {
                watches = watches.Where(w => w.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            if (!String.IsNullOrEmpty(category))
            {
                watches = watches.Where(w => w.Category.Name == category);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    watches = watches.OrderByDescending(w => w.Name);
                    break;
                case "Price":
                    watches = watches.OrderBy(w => w.Price);
                    break;
                case "price_desc":
                    watches = watches.OrderByDescending(w => w.Price);
                    break;
                default:
                    watches = watches.OrderBy(w => w.Name);
                    break;
            }

            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            return View(watches.ToList());
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