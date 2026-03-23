using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webcoffee.Data;
using webcoffee.Models;

namespace webcoffee.Controllers
{
    [Authorize]
    public class TableController : Controller
    {
        private readonly AppDbContext _context;

        public TableController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            var table = await _context.Tables
                .Include(t => t.Orders)
                .ThenInclude(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (table == null) return NotFound();

            // Find current active order (not paid/cancelled)
            var currentOrder = table.Orders.FirstOrDefault(o => o.Status != OrderStatus.DaThanhToan && o.Status != OrderStatus.DaHuy);
            ViewBag.CurrentOrder = currentOrder;

            return View(table);
        }

        [HttpPost]
        public async Task<IActionResult> OpenOrder(int tableId)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null) return NotFound();

            if (table.IsOccupied) return BadRequest("Bàn đang bận");

            var order = new Order
            {
                TableId = tableId,
                Status = OrderStatus.Moi,
                OrderDate = DateTime.Now
            };

            table.IsOccupied = true;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = tableId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            order.Status = status;

            if (status == OrderStatus.DaThanhToan || status == OrderStatus.DaHuy)
            {
                var table = await _context.Tables.FindAsync(order.TableId);
                if (table != null) table.IsOccupied = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = order.TableId });
        }
    }
}
