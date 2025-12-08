using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WatchShop.DataAccess.Models;
using WatchShop.DataAccess.Repositories;
using WatchShop.Web.Models;
using WatchShop.Web.Utility;

namespace WatchShop.Web.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class OrderController : Controller
    {
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<OrderDetail> _orderDetailRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IRepository<Order> orderRepo, IRepository<OrderDetail> orderDetailRepo, UserManager<ApplicationUser> userManager)
        {
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _userManager = userManager;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "Ваша корзина пуста.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var order = new Order
            {
                ApplicationUserId = userId,
                CustomerName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
            };

            return View(order);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (cart.Count == 0)
            {
                ModelState.AddModelError("", "Ваша корзина пуста.");
                return View(order);
            }

            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                order.Total = cart.Sum(c => c.Total);
                order.Status = "Принят";
                order.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                await _orderRepo.AddAsync(order);

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        WatchId = item.WatchId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    await _orderDetailRepo.AddAsync(orderDetail);
                }

                HttpContext.Session.Set<List<CartItem>>("Cart", null);

                return RedirectToAction(nameof(Confirmation), new { id = order.Id });
            }

            return View(order);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _orderRepo.GetAsync(id);
            if (order == null || order.ApplicationUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }
            return View(order);
        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderRepo.GetFilteredAsync(o => o.ApplicationUserId == userId, includeProperties: "OrderDetails");
            return View(orders.OrderByDescending(o => o.OrderDate));
        }
    }
}