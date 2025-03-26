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

                var managerExist = await userManager.Users.AnyAsync(u => u.Email == "Test@mail.com");

                if (!managerExist)
                {
                    var user = Activator.CreateInstance<ApplicationUser>();
                    user.UserName = "Test@mail.com";
                    user.Email = user.UserName;
                    user.FirstName = "Manager";
                    user.LastName = "Something";
                    user.JobTitle = "Manager";
                    user.Department = "General Management";
                    user.Salary = 1000;
                    user.EmailConfirmed = true;
                    var result = await userManager.CreateAsync(user, "Test_123");
                    user = await userManager.FindByEmailAsync(user.Email);
                    await userManager.AddToRoleAsync(user!, "Manager");
                }

                var employeeExist = await userManager.Users.AnyAsync(u => u.Email == "Test2@mail.com");

                if (!employeeExist)
                {
                    var user = Activator.CreateInstance<ApplicationUser>();
                    user.UserName = "Test2@mail.com";
                    user.Email = user.UserName;
                    user.FirstName = "Manager";
                    user.LastName = "Something";
                    user.JobTitle = "Employee";
                    user.Department = "Param";
                    user.Salary = 1000;
                    user.EmailConfirmed = true;
                    var result = await userManager.CreateAsync(user, "Test_123");
                    user = await userManager.FindByEmailAsync(user.Email);
                    await userManager.AddToRoleAsync(user!, "Employee");
                }
            }
        }
    }
}
