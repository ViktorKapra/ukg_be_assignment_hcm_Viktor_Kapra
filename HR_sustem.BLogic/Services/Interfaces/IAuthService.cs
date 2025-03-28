using HR_system.BLogic.DTOs;
using Microsoft.AspNetCore.Identity;

namespace HR_system.BLogic.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<SignInResult> LoginAsync(UserCredentialsDTO credentials);
        public Task<IdentityResult> RegisterAsync(UserDTO credentials);
        public Task Logout();

    }
}
