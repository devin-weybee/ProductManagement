using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProductManagement.App.Helper;
using ProductManagement.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

public class JwtSilentRefreshMiddleware
{
    private readonly RequestDelegate _next;

    public JwtSilentRefreshMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        UserManager<ApplicationUser> userManager,
        JwtTokenHelper jwtHelper)
    {
        var accessToken = context.Request.Cookies["token"];

        if (string.IsNullOrEmpty(accessToken))
        {
            await _next(context);
            return;
        }

        JwtSecurityToken jwt;
        var handler = new JwtSecurityTokenHandler();

        try
        {
            jwt = handler.ReadJwtToken(accessToken);
        }
        catch
        {
            await _next(context);
            return;
        }

        // ✅ CHECK EXPIRY MANUALLY
        if (jwt.ValidTo < DateTime.UtcNow)
        {
            var refreshToken = context.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                await _next(context);
                return;
            }

            var user = userManager.Users
                .FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (user == null)
            {
                context.Response.Cookies.Delete("token");
                context.Response.Cookies.Delete("refreshToken");
                await _next(context);
                return;
            }

            // 🔁 Generate new token
            var claims = await userManager.GetClaimsAsync(user);
            var newAccessToken = jwtHelper.GenerateAccessToken(user, claims);

            // 🍪 Store in cookie
            TokenCookieHelper.AppendAccessToken(context, newAccessToken);
        }

        await _next(context);
    }
}
