using Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HR_system.Configuration
{
    public class Seeder
    {
        public static async void Seed(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var roles = new[] { "Employee", "Manager", "HR_Admin" };
                foreach (var role in roles)
                {
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new ApplicationRole(role));
                        }

                    }
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var adminExist = await userManager.Users.AnyAsync(u => u.Email == "Admin@HR.com");

                if (!adminExist)
                {
                    var user = Activator.CreateInstance<ApplicationUser>();
                    user.UserName = "Admin@HR.com";
                    user.Email = user.UserName;
                    user.FirstName = "Admin";
                    user.LastName = "HR";
                    user.JobTitle = "HR";
                    user.Department = "General Management";
                    user.Salary = 1000;
                    user.EmailConfirmed = true;
                    var result = await userManager.CreateAsync(user, "Test_123");
                    user = await userManager.FindByEmailAsync(user.Email);
                    await userManager.AddToRoleAsync(user!, "HR_Admin");
                }
            }
        }
    }
}
