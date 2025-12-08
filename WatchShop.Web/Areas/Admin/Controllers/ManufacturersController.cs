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

        // GET: Admin/Manufacturers
        public async Task<IActionResult> Index()
        {
            return View(await _manufacturerRepo.GetAllAsync());
        }

        // GET: Admin/Manufacturers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Manufacturers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                await _manufacturerRepo.AddAsync(manufacturer);
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturer);
        }

        // GET: Admin/Manufacturers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var manufacturer = await _manufacturerRepo.GetAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        // POST: Admin/Manufacturers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Manufacturer manufacturer)
        {
            if (id != manufacturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _manufacturerRepo.UpdateAsync(manufacturer);
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturer);
        }

        // GET: Admin/Manufacturers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var manufacturer = await _manufacturerRepo.GetAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        // POST: Admin/Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _manufacturerRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}