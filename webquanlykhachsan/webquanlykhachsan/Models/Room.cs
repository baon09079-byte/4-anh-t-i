using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webquanlykhachsan.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Số Phòng")]
        public string Number { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Tầng")]
        public int Floor { get; set; }

        [Required]
        [Display(Name = "Trạng Thái")]
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        [Required]
        [Display(Name = "Loại Phòng")]
        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        public RoomType? RoomType { get; set; }
    }
}
