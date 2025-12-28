using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.App.Helper;
using ProductManagement.Data;
using ProductManagement.Domain.Entities;
using ProductManagement.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductManagement.Controllers
{
    [AllowAnonymous]
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
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }

            var claims = await _userManager.GetClaimsAsync(user);
            if (!claims.Any(c => c.Type == "FullName"))
            {
                await _userManager.AddClaimAsync(
                    user,
                    new System.Security.Claims.Claim("FullName", user.FullName ?? "")
                );

                claims = await _userManager.GetClaimsAsync(user);
            }

            var token = _jwtTokenHelper.GenerateAccessToken(user, claims);
            var refreshToken = user.RefreshToken;
            await _userManager.UpdateAsync(user);

            TokenCookieHelper.AppendTokens(HttpContext, token, refreshToken);

            return RedirectToAction("Index", "Home");
        }


        // Logout
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("refreshToken");

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var user = _userManager.Users
                .FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (user == null)
                return Unauthorized();

            var claims = await _userManager.GetClaimsAsync(user);
            var newAccessToken = _jwtTokenHelper.GenerateAccessToken(user, claims);

            TokenCookieHelper.AppendAccessToken(HttpContext, newAccessToken);

            return Ok();
        }

        [HttpGet]
        [Route("google-login")]
        public IActionResult GoogleLogin(string returnUrl = "/")
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(
                "Google",
                Url.Action("GoogleCallback", "Account", new { returnUrl })
            );

            return Challenge(properties, "Google");
        }

        [HttpGet]
        [Route("google-callback")]
        public async Task<IActionResult> GoogleCallback(string returnUrl = "/")
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);

            if (email == null)
                return RedirectToAction("Login");

            // 1️⃣ Find or create user
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = name,
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(user);
                await _userManager.AddClaimAsync(user, new Claim("FullName", name ?? ""));
            }

            // 2️⃣ Link Google login if not linked
            var logins = await _userManager.GetLoginsAsync(user);
            if (!logins.Any(l => l.LoginProvider == info.LoginProvider))
            {
                await _userManager.AddLoginAsync(user, info);
            }

            // 3️⃣ Generate JWT + Refresh Token
            var claims = await _userManager.GetClaimsAsync(user);
            var accessToken = _jwtTokenHelper.GenerateAccessToken(user, claims);

            if (string.IsNullOrEmpty(user.RefreshToken))
            {
                user.RefreshToken = RefreshTokenGenerator.GenerateRefreshToken();
                await _userManager.UpdateAsync(user);
            }

            TokenCookieHelper.AppendTokens(HttpContext, accessToken, user.RefreshToken);

            return RedirectToAction("LoginSuccess");
            //return LocalRedirect(returnUrl);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult LoginSuccess()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}