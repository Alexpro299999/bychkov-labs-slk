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

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart,
                Total = cart.Sum(item => item.Total)
            };

            return View(viewModel);
        }

        // ИСПРАВЛЕНО: параметр переименован в id
        public async Task<IActionResult> AddToCart(int id)
        {
            var watch = await _watchRepo.GetAsync(id);
            if (watch == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.WatchId == id);

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

        [HttpPost]
        public IActionResult UpdateCart(int watchId, int quantity)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart != null)
            {
                var item = cart.FirstOrDefault(c => c.WatchId == watchId);
                if (item != null)
                {
                    if (quantity > 0)
                        item.Quantity = quantity;
                    else
                        cart.Remove(item);

                    HttpContext.Session.Set("Cart", cart);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // ИСПРАВЛЕНО: параметр переименован в id
        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart != null)
            {
                var item = cart.FirstOrDefault(c => c.WatchId == id);
                if (item != null)
                {
                    cart.Remove(item);
                    HttpContext.Session.Set("Cart", cart);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // ИСПРАВЛЕНО: параметр переименован в id
        public IActionResult DecreaseQuantity(int id)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart != null)
            {
                var item = cart.FirstOrDefault(c => c.WatchId == id);
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