using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchShop.DataAccess.Models;
using WatchShop.DataAccess.Repositories;

namespace WatchShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManufacturersController : Controller
    {
        private readonly IRepository<Manufacturer> _manufacturerRepo;

        public ManufacturersController(IRepository<Manufacturer> manufacturerRepo)
        {
            _manufacturerRepo = manufacturerRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _manufacturerRepo.GetAllAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Manufacturer manufacturer)
        {
            ModelState.Remove("Watches");

            if (ModelState.IsValid)
            {
                await _manufacturerRepo.AddAsync(manufacturer);
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturer);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var manufacturer = await _manufacturerRepo.GetAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Manufacturer manufacturer)
        {
            if (id != manufacturer.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Watches");

            if (ModelState.IsValid)
            {
                await _manufacturerRepo.UpdateAsync(manufacturer);
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var manufacturer = await _manufacturerRepo.GetAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _manufacturerRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}