using Data.Account;
using HR_system.BLogic.DTOs;
using HR_system.BLogic.Services.Interfaces;
using HR_system.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace HR_system.BLogic.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(SignInManager<ApplicationUser> signInManager,
             UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> LoginAsync(UserCredentialsDTO credentials)
        {
            var result = await _signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                Log.Logger.Information("User logged in.");
            }
            return result;
        }

        public async Task<IdentityResult> RegisterAsync(UserDTO userDTO)
        {
            var user = Activator.CreateInstance<ApplicationUser>();
            user.UserName = userDTO.Email;
            user.Email = userDTO.Email;
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.JobTitle = userDTO.JobTitle;
            user.Department = userDTO.Department;
            user.Salary = userDTO.Salary;
            user.EmailConfirmed = true;// remove this line when email confirmation must be used
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (result.Succeeded)
            {
                Log.Logger.Information("User created a new account with password.");
                var createdUser = await _userManager.FindByEmailAsync(user.Email);
                await _userManager.AddToRoleAsync(createdUser!, DefaultValuesConsnts.EMPLOYEE_ROLE);
            }
            return result;
        }

        [HttpGet]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
