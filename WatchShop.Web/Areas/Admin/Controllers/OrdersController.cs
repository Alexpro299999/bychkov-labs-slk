using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchShop.DataAccess.Models;
using WatchShop.DataAccess.Repositories;

namespace WatchShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<OrderDetail> _orderDetailRepo;

        public OrdersController(IRepository<Order> orderRepo, IRepository<OrderDetail> orderDetailRepo)
        {
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepo.GetAllAsync(includeProperties: "ApplicationUser");
            return View(orders.OrderByDescending(o => o.OrderDate));
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepo.GetAsync(id, includeProperties: "ApplicationUser");
            if (order == null) return NotFound();

            var orderDetails = await _orderDetailRepo.GetFilteredAsync(
                filter: d => d.OrderId == id,
                includeProperties: "Watch"
            );

            order.OrderDetails = orderDetails.ToList();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return RedirectToAction(nameof(Details), new { id = id });
            }

            var order = await _orderRepo.GetAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _orderRepo.UpdateAsync(order);

            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}