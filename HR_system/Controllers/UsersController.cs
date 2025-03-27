using AutoMapper;
using Data;
using HR_sustem.BLogic.Services.Interfaces;
using HR_system.BLogic.DTOs;
using HR_system.BLogic.Services.Interfaces;
using HR_system.Constants;
using HR_system.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HR_system.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private const string managementRoles = DefaultValuesConsnts.ADMIN_ROLE + "," + DefaultValuesConsnts.MANAGER_ROLE;

        public UsersController(ApplicationDbContext context, IUserService userService,
                                IMapper mapper, IAuthService authService)
        {
            _userService = userService;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            UserFilterDTO userFilterDTO = new UserFilterDTO();
            List<UserDTO> listedUsers = new List<UserDTO>();
            var currentUser = await _userService.GetUserByClaimAsync(User);

            if (User.IsInRole(DefaultValuesConsnts.ADMIN_ROLE))
            {
                listedUsers.AddRange(await _userService.FilterAsync(userFilterDTO));
            }
            else if (User.IsInRole(DefaultValuesConsnts.MANAGER_ROLE))
            {
                userFilterDTO.Department = currentUser.Department;
                listedUsers.AddRange(await _userService.FilterAsync(userFilterDTO));
            }
            else
            {
                listedUsers.Add(currentUser);
            }

            return View(listedUsers.Select(x => _mapper.Map<UserViewModel>(x)).ToList());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDTO applicationUser = await _userService.GetUserByIdAsync(id.Value);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<UserViewModel>(applicationUser));
        }

        [Authorize(Roles = managementRoles)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDTO searchedUser = await _userService.GetUserByIdAsync(id.Value);
            if (searchedUser == null)
            {
                return NotFound();
            }
            var currentUser = await _userService.GetUserByClaimAsync(User);
            if (currentUser.Id != searchedUser.Id && !User.IsInRole(DefaultValuesConsnts.ADMIN_ROLE)
                && (User.IsInRole(DefaultValuesConsnts.MANAGER_ROLE) && currentUser.Department != searchedUser.Department))
            {
                return Unauthorized();
            }
            return View(_mapper.Map<UserViewModel>(searchedUser));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = managementRoles)]
        public async Task<IActionResult> Edit(UserViewModel editedUser)
        {
            if (ModelState.IsValid && editedUser.Id != null)
            {
                UserDTO currentUser = await _userService.GetUserByClaimAsync(User);

                if (currentUser.Id != editedUser.Id && !User.IsInRole(DefaultValuesConsnts.ADMIN_ROLE)
                 && (User.IsInRole(DefaultValuesConsnts.MANAGER_ROLE) && currentUser.Department != editedUser.Department))
                {
                    return Unauthorized();
                }
                var result = await _userService.EditUserAsync(_mapper.Map<UserDTO>(editedUser));
                if (!result.Succeeded)
                {
                    return RedirectToAction("Error", "Home");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(editedUser);
        }

        [Authorize(Roles = managementRoles)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var searchedUser = await _userService.GetUserByIdAsync(id.Value);
            if (searchedUser == null)
            {
                return NotFound();
            }
            var currentUser = await _userService.GetUserByClaimAsync(User);
            if (currentUser.Id != searchedUser.Id && !User.IsInRole(DefaultValuesConsnts.ADMIN_ROLE)
                && (User.IsInRole(DefaultValuesConsnts.MANAGER_ROLE) && currentUser.Department != searchedUser.Department))
            {
                return Unauthorized();
            }

            return View(_mapper.Map<UserViewModel>(searchedUser));
        }

        [Authorize(Roles = managementRoles)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            UserDTO searchedUser = await _userService.GetUserByIdAsync(id);
            if (searchedUser == null)
            {
                return NotFound();
            }
            UserDTO currentUser = await _userService.GetUserByClaimAsync(User);
            if (currentUser.Id != searchedUser.Id && !User.IsInRole(DefaultValuesConsnts.ADMIN_ROLE)
                && (User.IsInRole(DefaultValuesConsnts.MANAGER_ROLE) && currentUser.Department != searchedUser.Department))
            {
                return Unauthorized();
            }

            if (currentUser.Id == searchedUser.Id)
            {
                await _authService.Logout();
            }


            var result = await _userService.DeleteUserAsync(id);

            if (!result.Succeeded)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
