using Microsoft.AspNetCore.Http;
using System;

namespace ProductManagement.App.Helper
{
    public class TokenCookieHelper
    {
        public static void AppendTokens(HttpContext context, string accessToken, string refreshToken)
        {
            context.Response.Cookies.Append("token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(5)
            });

            context.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}
