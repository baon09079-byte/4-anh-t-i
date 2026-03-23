using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webquanlykhachsan.Models;

namespace webquanlykhachsan.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
              await context.Database.MigrateAsync();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Seed Roles
            string[] roleNames = { "Admin", "Staff", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Admin User
            var adminEmail = "admin@hotel.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Seed Room Types
            if (!context.RoomTypes.Any())
            {
                context.RoomTypes.AddRange(
                    new RoomType { Name = "Single", BasePrice = 500000, Capacity = 1, Description = "Phòng đơn tiêu chuẩn" },
                    new RoomType { Name = "Double", BasePrice = 800000, Capacity = 2, Description = "Phòng đôi tiện nghi" },
                    new RoomType { Name = "Suite", BasePrice = 2000000, Capacity = 4, Description = "Phòng VIP cao cấp" }
                );
                await context.SaveChangesAsync();
            }

            // Seed Rooms
            if (!context.Rooms.Any())
            {
                var single = await context.RoomTypes.FirstAsync(r => r.Name == "Single");
                var doubleRoom = await context.RoomTypes.FirstAsync(r => r.Name == "Double");
                
                context.Rooms.AddRange(
                    new Room { Number = "101", Floor = 1, Status = RoomStatus.Available, RoomTypeId = single.Id },
                    new Room { Number = "102", Floor = 1, Status = RoomStatus.Available, RoomTypeId = single.Id },
                    new Room { Number = "201", Floor = 2, Status = RoomStatus.Available, RoomTypeId = doubleRoom.Id }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
