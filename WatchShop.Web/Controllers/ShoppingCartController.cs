using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchShop.DataAccess.Models;
using WatchShop.DataAccess.Repositories;
using WatchShop.Web.Models;
using WatchShop.Web.Utility;

namespace WatchShop.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class ShoppingCartController : Controller
    {
        private readonly IRepository<Watch> _watchRepo;

        public ShoppingCartController(IRepository<Watch> watchRepo)
        {
            _watchRepo = watchRepo;
        }

        private List<CartItem> GetCart()
        {
            return HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.Set("Cart", cart);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart,
                Total = cart.Sum(item => item.Total)
            };
            return View(viewModel);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var watch = await _watchRepo.GetAsync(id);
            if (watch == null)
            {
                return NotFound();
            }

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.WatchId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    WatchId = watch.Id,
                    WatchName = watch.Name,
                    Price = watch.Price,
                    Quantity = 1,
                    ImageUrl = watch.ImageUrl
                });
            }
            else
            {
                cartItem.Quantity++;
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(int watchId, int quantity)
        {
            if (quantity <= 0)
            {
                return RemoveFromCart(watchId);
            }

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.WatchId == watchId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                SaveCart(cart);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.WatchId == id);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCart(cart);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}