using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WatchShop.DataAccess.Models;

namespace WatchShop.Web.Controllers
{
    public class SetupController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public SetupController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = await _userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "Example",
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(newAdmin, "Password123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }

            return Content("Setup complete. Admin user created.");
        }
    }
}