using AutoMapper;
using Data;
using HR_sustem.BLogic.Services.Interfaces;
using HR_system.BLogic.DTOs;
using HR_system.BLogic.Services.Interfaces;
using HR_system.Constants;
using HR_system.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HR_system.Constants.DefaultValuesConsnts;

namespace HR_system.Controllers
{
    /// <summary>
    /// Controller for managing users in the HR system.
    /// </summary>
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private const string managementRoles = ADMIN_ROLE + "," + MANAGER_ROLE;

        public UsersController(ApplicationDbContext context, IUserService userService,
                                IMapper mapper, IAuthService authService)
        {
            _userService = userService;
            _mapper = mapper;
            _authService = authService;
        }

        /// <summary>
        /// Displays the list of users based on the user's role and department.
        /// </summary>
        /// <returns>The view containing the list of users.</returns>
        public async Task<IActionResult> Index()
        {
            UserFilterDTO userFilterDTO = new UserFilterDTO();
            List<UserDTO> listedUsers = new List<UserDTO>();
            var currentUser = await _userService.GetUserByClaimAsync(User);

            if (User.IsInRole(ADMIN_ROLE))
            {
                listedUsers.AddRange(await _userService.FilterAsync(userFilterDTO));
            }
            else if (User.IsInRole(MANAGER_ROLE))
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

        /// <summary>
        /// Displays the details of a specific user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The view containing the user details.</returns>
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

        /// <summary>
        /// Displays the edit view for a specific user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The edit view for the user.</returns>
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

        /// <summary>
        /// Handles the post request to edit a user. Can only be accessed by managers and admins.
        /// Only admins can change the role of a user.
        /// </summary>
        /// <param name="editedUser">The edited user data.</param>
        /// <returns>The appropriate action result based on the edit operation.</returns>
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

                if (User.IsInRole(ADMIN_ROLE))
                {
                    await _userService.ChangeRole(editedUser.Id.Value, editedUser.Role);
                }

                if (!result.Succeeded)
                {
                    return RedirectToAction("Error", "Home");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(editedUser);
        }

        /// <summary>
        /// Displays the delete view for a specific user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The delete view for the user.</returns>
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

        /// <summary>
        /// Handles the post request to delete a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>The appropriate action result based on the delete operation.</returns>
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
