using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webcoffee.Data;
using webcoffee.Helpers;
using webcoffee.Models;

namespace webcoffee.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                order.TotalAmount = cart.Sum(item => item.Total);
                
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
                
                await _context.SaveChangesAsync();
                
                HttpContext.Session.Remove("Cart");
                return RedirectToAction("OrderConfirmation", new { id = order.Id });
            }

            return View(order);
        }

        public IActionResult OrderConfirmation(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
