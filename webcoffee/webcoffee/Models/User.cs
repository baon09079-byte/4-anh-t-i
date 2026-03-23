using System.ComponentModel.DataAnnotations;

namespace webcoffee.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        public string FullName { get; set; }
        
        [Required]
        public string Role { get; set; } // "Admin" or "Staff"
    }
}
