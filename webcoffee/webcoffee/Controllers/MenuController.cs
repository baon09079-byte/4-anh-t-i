using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webcoffee.Data;
using webcoffee.Models;

namespace webcoffee.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MenuController : Controller
    {
        private readonly AppDbContext _context;

        public MenuController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId, int? tableId)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            ViewBag.TableId = tableId;

            if (tableId.HasValue)
            {
                var table = await _context.Tables.FindAsync(tableId);
                ViewBag.TableName = table?.Name;
            }

            var products = _context.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId);
                ViewBag.CurrentCategory = categories.FirstOrDefault(c => c.Id == categoryId)?.Name;
            }
            else
            {
                ViewBag.CurrentCategory = "Tất cả sản phẩm";
            }

            return View(await products.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }
    }
}
