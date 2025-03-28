using AutoMapper;
using HR_system.BLogic.DTOs;
using HR_system.BLogic.Services.Interfaces;
using HR_system.Models;
using Microsoft.AspNetCore.Mvc;

namespace HR_system.Controllers
{

    /// <summary>
    /// Controller responsible for handling authentication-related actions.
    /// </summary>
    public class AuthenticationController : Controller
    {
        private IAuthService _authService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="authService">The authentication service.</param>
        public AuthenticationController(IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _mapper = mapper;
        }

        /// <summary>
        /// Displays the login view.
        /// </summary>
        /// <returns>The login view.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// Handles the login form submission.
        /// </summary>
        /// <param name="model">The login view model.</param>
        /// <returns>The result of the login attempt.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var loginResult = await _authService.LoginAsync(_mapper.Map<UserCredentialsDTO>(model));
            if (loginResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("InvalidLogin", "Invalid login attempt.");
            return View(model);
        }

        /// <summary>
        /// Displays the register view.
        /// </summary>
        /// <returns>The register view.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// Handles the register form submission.
        /// </summary>
        /// <param name="model">The register view model.</param>
        /// <returns>The result of the register attempt.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var registerResult = await _authService.RegisterAsync(_mapper.Map<UserDTO>(model));
            if (registerResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("InvalidRegister", "Invalid register attempt.");
            return View(model);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>A redirect to the home page.</returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
