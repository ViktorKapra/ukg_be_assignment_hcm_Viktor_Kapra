using HR_system.BLogic.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HR_system.BLogic.Services.Interfaces
{
    public interface IUserService
    {
        public Task<IdentityResult> ChangePasswordAsync(Guid id, string oldPassword, string newPassword);
        public Task<UserDTO> GetUserByIdAsync(Guid id);
        public Task<List<UserDTO>> FilterAsync(UserFilterDTO filterDTO);
        public Task<IdentityResult> EditUserAsync(UserDTO userDTO);
        public Task<UserDTO> GetUserByClaimAsync(ClaimsPrincipal userClaims);
        public Task<IdentityResult> DeleteUserAsync(Guid guid);
        public Task ChangeRole(Guid userId, string role);
    }
}
