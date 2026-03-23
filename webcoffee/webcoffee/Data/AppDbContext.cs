using Microsoft.EntityFrameworkCore;
using webcoffee.Models;

namespace webcoffee.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Username = "admin", 
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), 
                    FullName = "Quản trị viên", 
                    Role = "Admin" 
                }
            );

            // Seed Tables
            modelBuilder.Entity<Table>().HasData(
                new Table { Id = 1, Name = "Bàn 1" },
                new Table { Id = 2, Name = "Bàn 2" },
                new Table { Id = 3, Name = "Bàn 3" },
                new Table { Id = 4, Name = "Bàn 4" },
                new Table { Id = 5, Name = "Bàn 5" },
                new Table { Id = 6, Name = "Bàn 6" },
                new Table { Id = 7, Name = "Bàn 7" },
                new Table { Id = 8, Name = "Bàn 8" },
                new Table { Id = 9, Name = "VIP 1" },
                new Table { Id = 10, Name = "VIP 2" }
            );

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Cà Phê" },
                new Category { Id = 2, Name = "Trà" },
                new Category { Id = 3, Name = "Sinh Tố" },
                new Category { Id = 4, Name = "Đồ Ăn Nhẹ" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                // Coffee
                new Product { Id = 1, Name = "Espresso", Description = "Cà phê đậm đà nguyên chất", Price = 35000m, CategoryId = 1, ImageUrl = "https://placehold.co/400x300?text=Espresso" },
                new Product { Id = 2, Name = "Cappuccino", Description = "Espresso kết hợp với bọt sữa mịn", Price = 45000m, CategoryId = 1, ImageUrl = "https://placehold.co/400x300?text=Cappuccino" },
                new Product { Id = 3, Name = "Latte", Description = "Cà phê sữa kiểu Ý dịu nhẹ", Price = 47000m, CategoryId = 1, ImageUrl = "https://placehold.co/400x300?text=Latte" },
                new Product { Id = 4, Name = "Cold Brew", Description = "Cà phê ủ lạnh thanh mát", Price = 50000m, CategoryId = 1, ImageUrl = "https://placehold.co/400x300?text=Cold+Brew" },

                // Tea
                new Product { Id = 5, Name = "Trà Xanh", Description = "Trà xanh Nhật Bản tươi mát", Price = 30000m, CategoryId = 2, ImageUrl = "https://placehold.co/400x300?text=Tra+Xanh" },
                new Product { Id = 6, Name = "Trà Sữa Thái", Description = "Trà sữa thơm ngon kiểu Thái", Price = 45000m, CategoryId = 2, ImageUrl = "https://placehold.co/400x300?text=Tra+Sua+Thai" },

                // Smoothies
                new Product { Id = 7, Name = "Sinh Tố Dâu", Description = "Dâu tây tươi kết hợp sữa chua", Price = 60000m, CategoryId = 3, ImageUrl = "https://placehold.co/400x300?text=Sinh+To+Dau" },
                new Product { Id = 8, Name = "Sinh Tố Xoài", Description = "Xoài chín mọng và nước cốt dừa", Price = 60000m, CategoryId = 3, ImageUrl = "https://placehold.co/400x300?text=Sinh+To+Xoai" },

                // Snacks
                new Product { Id = 9, Name = "Bánh Sừng Bò", Description = "Bánh bơ thơm giòn tan", Price = 35000m, CategoryId = 4, ImageUrl = "https://placehold.co/400x300?text=Banh+Sung+Bo" },
                new Product { Id = 10, Name = "Muffin Socola", Description = "Bánh muffin đậm vị socola", Price = 37000m, CategoryId = 4, ImageUrl = "https://placehold.co/400x300?text=Muffin+Socola" }
            );
        }
    }
}
