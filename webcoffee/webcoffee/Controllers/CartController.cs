using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webcoffee.Data;
using webcoffee.Helpers;
using webcoffee.Models;

namespace webcoffee.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? tableId)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            ViewBag.TableId = tableId;
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int id, int? tableId)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = 1
                });
            }

            HttpContext.Session.Set("Cart", cart);
            return RedirectToAction("Index", new { tableId = tableId });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmToTable(int tableId)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart == null || !cart.Any()) return RedirectToAction("Index", "Home");

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.TableId == tableId && o.Status != OrderStatus.DaThanhToan && o.Status != OrderStatus.DaHuy);

            if (order == null) return NotFound();

            foreach (var item in cart)
            {
                var existingDetail = order.OrderDetails.FirstOrDefault(od => od.ProductId == item.ProductId);
                if (existingDetail != null)
                {
                    existingDetail.Quantity += item.Quantity;
                }
                else
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
                }
            }

            order.TotalAmount = order.OrderDetails.Sum(od => od.Price * od.Quantity);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Details", "Table", new { id = tableId });
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart != null)
            {
                var item = cart.FirstOrDefault(c => c.ProductId == id);
                if (item != null)
                {
                    cart.Remove(item);
                    HttpContext.Session.Set("Cart", cart);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }
    }
}
