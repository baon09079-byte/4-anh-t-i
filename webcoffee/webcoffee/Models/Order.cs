using System.ComponentModel.DataAnnotations;

namespace webcoffee.Models
{
    public enum OrderStatus
    {
        Moi = 0,
        DangChuanBi = 1,
        DaPhucVu = 2,
        DaThanhToan = 3,
        DaHuy = 4
    }

    public class Order
    {
        public int Id { get; set; }
        
        // Optional for in-store orders
        public string? CustomerName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        
        public decimal TotalAmount { get; set; }
        
        public OrderStatus Status { get; set; } = OrderStatus.Moi;
        
        public int? TableId { get; set; }
        public Table? Table { get; set; }
        
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
