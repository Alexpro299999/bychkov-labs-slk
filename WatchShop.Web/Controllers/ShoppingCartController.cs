using Microsoft.AspNetCore.Mvc;
using WatchShop.DataAccess.Models;
using WatchShop.DataAccess.Repositories;
using WatchShop.Web.Models;
using WatchShop.Web.Utility;

namespace WatchShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IRepository<Watch> _watchRepo;

        public ShoppingCartController(IRepository<Watch> watchRepo)
        {
            _watchRepo = watchRepo;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

            ViewBag.Total = cart.Sum(item => item.Total);

            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int watchId)
        {
            var watch = await _watchRepo.GetAsync(watchId);
            if (watch == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.WatchId == watchId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    WatchId = watch.Id,
                    WatchName = watch.Name,
                    Price = watch.Price,
                    ImageUrl = watch.ImageUrl,
                    Quantity = 1
                });
            }

            HttpContext.Session.Set("Cart", cart);
            TempData["SuccessMessage"] = "Товар добавлен в корзину";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int watchId)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart != null)
            {
                var item = cart.FirstOrDefault(c => c.WatchId == watchId);
                if (item != null)
                {
                    cart.Remove(item);
                    HttpContext.Session.Set("Cart", cart);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DecreaseQuantity(int watchId)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart != null)
            {
                var item = cart.FirstOrDefault(c => c.WatchId == watchId);
                if (item != null)
                {
                    item.Quantity--;
                    if (item.Quantity <= 0)
                    {
                        cart.Remove(item);
                    }
                    HttpContext.Session.Set("Cart", cart);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}