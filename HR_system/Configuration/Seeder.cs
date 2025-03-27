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
                await AddRoles(roleManager, roles);

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                await AddUser(userManager, roleManager, "HR_Admin", "Admin@HR.com", "Test_123", "Tom", "Sawyer", "Admin", "Main", 1000);
                await AddUser(userManager, roleManager, "Manager", "Test@mail.com", "Test_123", "Piter", "Pan", "Manager", "Main", 1000);
                await AddUser(userManager, roleManager, "Employee", "Test2@mail.com", "Test_123", "Piter", "Pan", "Manager", "Finance", 1000);

            }
        }

        private static async Task AddRoles(RoleManager<ApplicationRole> roleManager, string[] roles)
        {
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole(role));
                }
            }
        }

        private static async Task AddUser(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
                             string role, string userName, string password, string firstName, string lastName, string jobTitle,
                             string department, decimal salary)
        {
            var employeeExist = await userManager.Users.AnyAsync(u => u.Email == userName);

            if (!employeeExist)
            {
                var user = Activator.CreateInstance<ApplicationUser>();
                user.UserName = userName;
                user.Email = user.UserName;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.JobTitle = jobTitle;
                user.Department = department;
                user.Salary = salary;
                user.EmailConfirmed = true;
                var result = await userManager.CreateAsync(user, password);
                user = await userManager.FindByEmailAsync(user.Email);
                await userManager.AddToRoleAsync(user!, role);
            }
        }
    }
}
