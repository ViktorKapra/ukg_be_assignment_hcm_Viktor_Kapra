using AutoMapper;
using Data;
using Data.Account;
using HR_system.BLogic.DTOs;
using HR_system.BLogic.Services.Interfaces;
using HR_system.BLogic.Templates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HR_system.BLogic.Services.User
{
    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(SignInManager<ApplicationUser> signInManager,
             UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _context = context;
        }
        public async Task<IdentityResult> ChangePasswordAsync(Guid id, string oldPassword, string newPassword)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            }
            return IdentityResult.Failed();
        }
        public async Task<UserDTO> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserByClaimAsync(ClaimsPrincipal userClaims)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userClaims);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<List<UserDTO>> FilterAsync(UserFilterDTO filterDTO)
        {
            var query = _mapper.Map<Query<ApplicationUser>>(filterDTO);
            var entities = await _context.Users.Where(query.Expression).ToListAsync();
            var userDTOs = entities.Select(x => _mapper.Map<UserDTO>(x)).ToList();
            return userDTOs;
        }
        public async Task<IdentityResult> EditUserAsync(UserDTO userDTO)
        {
            var user = await _userManager.FindByIdAsync(userDTO.Id!.Value.ToString());
            _mapper.Map(userDTO, user);
            return await _userManager.UpdateAsync(user!);
        }
        public async Task<IdentityResult> DeleteUserAsync(Guid guid)
        {
            var user = await _context.Users.FindAsync(guid);
            if (user != null)
            {
                return await _userManager.DeleteAsync(user);
            }
            return IdentityResult.Failed();

        }
    }
}
