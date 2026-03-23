using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webquanlykhachsan.Data;
using webquanlykhachsan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace webquanlykhachsan.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Booking/Search
        public async Task<IActionResult> Search(DateTime? checkIn, DateTime? checkOut)
        {
            if (checkIn == null || checkOut == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (checkIn >= checkOut)
            {
                TempData["Error"] = "Ngày trả phòng phải sau ngày nhận phòng.";
                return RedirectToAction("Index", "Home");
            }

            // Find rooms that are NOT booked during the requested dates
            // Standard overlap check: s1 < e2 && e1 > s2
            var bookedRoomIds = await _context.Bookings
                .Where(b => b.Status != BookingStatus.Cancelled &&
                            checkIn < b.CheckOutDate && checkOut > b.CheckInDate)
                .Select(b => b.RoomId)
                .Distinct()
                .ToListAsync();

            var availableRooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => !bookedRoomIds.Contains(r.Id) && r.Status == RoomStatus.Available)
                .ToListAsync();

            // Group by RoomType to show options
            var roomTypeOptions = availableRooms
                .GroupBy(r => r.RoomType)
                .Select(g => new RoomSearchViewModel
                {
                    RoomType = g.Key!,
                    AvailableRoomsCount = g.Count(),
                    FirstAvailableRoomId = g.First().Id
                })
                .ToList();

            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;

            return View(roomTypeOptions);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var room = await _context.Rooms.Include(r => r.RoomType).FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null) return NotFound();

            // Double check availability (Race condition prevention)
            var isAlreadyBooked = await _context.Bookings
                .AnyAsync(b => b.RoomId == roomId && b.Status != BookingStatus.Cancelled &&
                            checkIn < b.CheckOutDate && checkOut > b.CheckInDate);

            if (isAlreadyBooked)
            {
                TempData["Error"] = "Phòng này đã có người đặt trong khoảng thời gian này. Vui lòng chọn phòng khác.";
                return RedirectToAction("Search", new { checkIn, checkOut });
            }

            var days = (checkOut - checkIn).Days;
            var totalPrice = days * room.RoomType!.BasePrice;

            var booking = new Booking
            {
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                RoomId = roomId,
                GuestId = user.Id,
                TotalPrice = totalPrice,
                Status = BookingStatus.Pending
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { id = booking.Id });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .ThenInclude(r => r!.RoomType)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }
    }

    public class RoomSearchViewModel
    {
        public RoomType RoomType { get; set; } = null!;
        public int AvailableRoomsCount { get; set; }
        public int FirstAvailableRoomId { get; set; }
    }
}
