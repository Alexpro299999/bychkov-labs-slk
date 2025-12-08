using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WatchShop.DataAccess.Models;
using WatchShop.DataAccess.Repositories;
using WatchShop.Web.Areas.Admin.Models;

namespace WatchShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class WatchesController : Controller
    {
        private readonly IRepository<Watch> _watchRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Manufacturer> _manufacturerRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WatchesController(IRepository<Watch> watchRepo,
                                 IRepository<Category> categoryRepo,
                                 IRepository<Manufacturer> manufacturerRepo,
                                 IWebHostEnvironment webHostEnvironment)
        {
            _watchRepo = watchRepo;
            _categoryRepo = categoryRepo;
            _manufacturerRepo = manufacturerRepo;
            _webHostEnvironment = webHostEnvironment;
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

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name");
            ViewBag.Manufacturers = new SelectList(await _manufacturerRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WatchViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = await UploadImage(vm.ImageFile);
                Watch watch = new Watch
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    Price = vm.Price,
                    ImageUrl = uniqueFileName != null ? "/images/" + uniqueFileName : "/images/tclassic.jpg",
                    CategoryId = vm.CategoryId,
                    ManufacturerId = vm.ManufacturerId
                };

                await _watchRepo.AddAsync(watch);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name", vm.CategoryId);
            ViewBag.Manufacturers = new SelectList(await _manufacturerRepo.GetAllAsync(), "Id", "Name", vm.ManufacturerId);
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var watch = await _watchRepo.GetAsync(id);
            if (watch == null)
            {
                return NotFound();
            }

            var vm = new WatchViewModel
            {
                Id = watch.Id,
                Name = watch.Name,
                Description = watch.Description,
                Price = watch.Price,
                ImageUrl = watch.ImageUrl,
                CategoryId = watch.CategoryId,
                ManufacturerId = watch.ManufacturerId
            };

            ViewBag.Categories = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name", vm.CategoryId);
            ViewBag.Manufacturers = new SelectList(await _manufacturerRepo.GetAllAsync(), "Id", "Name", vm.ManufacturerId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WatchViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var watch = await _watchRepo.GetAsync(id);
                if (watch == null) return NotFound();

                if (vm.ImageFile != null)
                {
                    if (!string.IsNullOrEmpty(watch.ImageUrl) && watch.ImageUrl != "/images/tclassic.jpg")
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, watch.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string uniqueFileName = await UploadImage(vm.ImageFile);
                    watch.ImageUrl = "/images/" + uniqueFileName;
                }

                watch.Name = vm.Name;
                watch.Description = vm.Description;
                watch.Price = vm.Price;
                watch.CategoryId = vm.CategoryId;
                watch.ManufacturerId = vm.ManufacturerId;

                await _watchRepo.UpdateAsync(watch);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name", vm.CategoryId);
            ViewBag.Manufacturers = new SelectList(await _manufacturerRepo.GetAllAsync(), "Id", "Name", vm.ManufacturerId);
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var watch = await _watchRepo.GetAsync(id, includeProperties: "Category,Manufacturer");
            if (watch == null)
            {
                return NotFound();
            }
            return View(watch);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var watch = await _watchRepo.GetAsync(id);
            if (watch == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(watch.ImageUrl) && watch.ImageUrl != "/images/tclassic.jpg")
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, watch.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            await _watchRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null) return null;

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return uniqueFileName;
        }
    }
}