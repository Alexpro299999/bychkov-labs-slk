using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WatchShop.DataAccess;
using WatchShop.DataAccess.Models;
using WatchShop.Web.Models;

namespace WatchShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId, string sortOrder)
        {
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.CurrentCategory = categoryId;

            var watchesQuery = _context.Watches
                .Include(w => w.Category)
                .Include(w => w.Manufacturer)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                watchesQuery = watchesQuery.Where(w => w.CategoryId == categoryId.Value);
            }

            // Сначала получаем данные из базы в список
            var watches = await watchesQuery.ToListAsync();

            // Затем сортируем этот список в памяти (C# сортирует decimal правильно)
            switch (sortOrder)
            {
                case "name_desc":
                    watches = watches.OrderByDescending(w => w.Name).ToList();
                    break;
                case "Price":
                    watches = watches.OrderBy(w => w.Price).ToList();
                    break;
                case "price_desc":
                    watches = watches.OrderByDescending(w => w.Price).ToList();
                    break;
                default:
                    watches = watches.OrderBy(w => w.Name).ToList();
                    break;
            }

            return View(watches);
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