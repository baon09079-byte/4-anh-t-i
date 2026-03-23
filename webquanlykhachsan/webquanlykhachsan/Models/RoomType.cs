using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webquanlykhachsan.Models
{
    public class RoomType
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên Loại Phòng")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Giá Cơ Bản")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }

        [Display(Name = "Mô Tả")]
        public string? Description { get; set; }

        [Display(Name = "Sức Chứa")]
        public int Capacity { get; set; }

        [Display(Name = "Ảnh")]
        public string? ImageUrl { get; set; }
        
        // Navigation property
        public ICollection<Room>? Rooms { get; set; }
    }
}
