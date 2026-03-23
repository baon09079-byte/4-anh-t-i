using System.ComponentModel.DataAnnotations;

namespace webcoffee.Models
{
    public class Table
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public bool IsOccupied { get; set; } = false;
        
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
