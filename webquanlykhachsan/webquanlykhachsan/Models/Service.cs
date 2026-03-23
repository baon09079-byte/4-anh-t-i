using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webquanlykhachsan.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên Dịch Vụ")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Đơn Giá")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Mô Tả")]
        public string? Description { get; set; }
    }
}
