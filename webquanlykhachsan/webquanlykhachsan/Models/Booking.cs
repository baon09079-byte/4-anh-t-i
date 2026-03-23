using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webquanlykhachsan.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ngày Nhận Phòng")]
        public DateTime CheckInDate { get; set; }

        [Required]
        [Display(Name = "Ngày Trả Phòng")]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Display(Name = "Tổng Tiền")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Display(Name = "Trạng Thái")]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [Required]
        [Display(Name = "Khách Hàng")]
        public string GuestId { get; set; } = string.Empty;

        [ForeignKey("GuestId")]
        public ApplicationUser? Guest { get; set; }

        [Required]
        [Display(Name = "Phòng")]
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room? Room { get; set; }

        public ICollection<BookingService>? BookingServices { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
    }
}
