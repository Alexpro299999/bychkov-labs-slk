using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WatchShop.DataAccess.Models;

namespace WatchShop.DataAccess
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            string adminRole = "Admin";
            string userRole = "User";

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }
            if (!await roleManager.RoleExistsAsync(userRole))
            {
                await roleManager.CreateAsync(new IdentityRole(userRole));
            }

            string adminEmail = "admin@watchshop.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Adminov",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }

            if (!context.Categories.Any())
            {
                await context.Categories.AddRangeAsync(
                    new Category { Name = "Классика" },
                    new Category { Name = "Спорт" },
                    new Category { Name = "Смарт-часы" }
                );
                await context.SaveChangesAsync();
            }

            if (!context.Manufacturers.Any())
            {
                await context.Manufacturers.AddRangeAsync(
                    new Manufacturer { Name = "Rolex", Country = "Швейцария" },
                    new Manufacturer { Name = "Tissot", Country = "Швейцария" },
                    new Manufacturer { Name = "Casio", Country = "Япония" }
                );
                await context.SaveChangesAsync();
            }

            if (!context.Watches.Any())
            {
                var classicCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Классика");
                var sportCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Спорт");

                var rolex = await context.Manufacturers.FirstOrDefaultAsync(m => m.Name == "Rolex");
                var tissot = await context.Manufacturers.FirstOrDefaultAsync(m => m.Name == "Tissot");
                var casio = await context.Manufacturers.FirstOrDefaultAsync(m => m.Name == "Casio");

                await context.Watches.AddRangeAsync(
                    new Watch
                    {
                        Name = "Rolex Submariner",
                        Description = "Легендарные дайверские часы.",
                        Price = 15000,
                        ImageUrl = "/images/submariner.jpg",
                        CategoryId = classicCategory.Id,
                        ManufacturerId = rolex.Id
                    },
                    new Watch
                    {
                        Name = "Tissot T-Classic",
                        Description = "Швейцарская элегантность.",
                        Price = 500,
                        ImageUrl = "/images/tclassic.jpg",
                        CategoryId = classicCategory.Id,
                        ManufacturerId = tissot.Id
                    },
                    new Watch
                    {
                        Name = "Casio G-Shock",
                        Description = "Неубиваемые спортивные часы.",
                        Price = 150,
                        ImageUrl = "/images/gshock.jpg",
                        CategoryId = sportCategory.Id,
                        ManufacturerId = casio.Id
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}