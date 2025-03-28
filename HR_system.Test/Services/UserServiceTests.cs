using AutoMapper;
using Data;
using Data.Account;
using FakeItEasy;
using HR_system.BLogic.DTOs;
using HR_system.BLogic.Services.User;
using Microsoft.AspNetCore.Identity;

namespace HR_system.Test.Unit
{
    public class UserServiceTests
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private UserService _userService;
        public UserServiceTests()
        {
            _userManager = A.Fake<UserManager<ApplicationUser>>();
            _signInManager = A.Fake<SignInManager<ApplicationUser>>();
            _context = A.Fake<ApplicationDbContext>();
            _mapper = A.Fake<IMapper>();
            _userService = new UserService(_signInManager, _userManager, _mapper, _context);
        }

        [Fact]
        public async Task GetUserByIdAsync_Invokes_UserManager()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            // Act
            await _userService.GetUserByIdAsync(guid);
            // Assert
            A.CallTo(() => _userManager.FindByIdAsync(guid.ToString())).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task EditUserAsync_Invokes_UserManager()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            UserDTO userDTO = A.Fake<UserDTO>();
            ApplicationUser user = A.Fake<ApplicationUser>();
            userDTO.Id = guid;
            A.CallTo(() => _userManager.FindByIdAsync(userDTO.Id.ToString())).Returns(Task.FromResult<ApplicationUser?>(user));
            // Act
            await _userService.EditUserAsync(userDTO);
            // Assert
            A.CallTo(() => _userManager.UpdateAsync(user)).MustHaveHappenedOnceExactly();
        }
    }
}
