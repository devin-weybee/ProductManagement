using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.App.Helper;
using ProductManagement.Data;
using ProductManagement.Domain.Entities;
using ProductManagement.Models;
using System;
using System.Threading.Tasks;

namespace ProductManagement.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtTokenHelper jwtTokenHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenHelper = jwtTokenHelper;
        }

        // Register Page
        [HttpGet]
        [Route("[action]")]
        public IActionResult Register() => View();

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FullName", user.FullName ?? ""));
                    var claims = await _userManager.GetClaimsAsync(user);
                    var token = _jwtTokenHelper.GenerateAccessToken(user, claims);
                    var refreshToken = RefreshTokenGenerator.GenerateRefreshToken();

                    user.RefreshToken = refreshToken;
                    await _userManager.UpdateAsync(user);
                    TokenCookieHelper.AppendTokens(HttpContext, token, refreshToken);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // Login Page
        [HttpGet]
        [Route("[action]")]
        public IActionResult Login() => View();

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    // Ensure the FullName claim exists
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var claims = await _userManager.GetClaimsAsync(user);

                    var token = _jwtTokenHelper.GenerateAccessToken(user, claims);
                    var refreshToken = user.RefreshToken;

                    await _userManager.UpdateAsync(user);
                    TokenCookieHelper.AppendTokens(HttpContext, token, refreshToken);

                    if (!claims.Any(c => c.Type == "FullName"))
                    {
                        await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FullName", user.FullName ?? ""));
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View(model);
        }

        // Logout
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            Response.Cookies.Delete("token");
            Response.Cookies.Delete("refreshToken");

            return RedirectToAction("Login", "Account");
        }

    }
}