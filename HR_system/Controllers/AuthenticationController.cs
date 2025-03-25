using AutoMapper;
using HR_sustem.BLogic.Services.Interfaces;
using HR_system.BLogic.DTOs;
using HR_system.Models;
using Microsoft.AspNetCore.Mvc;

namespace HR_system.Controllers
{
    public class AuthenticationController : Controller
    {
        private IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthenticationController(IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
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
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
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
    }
}
