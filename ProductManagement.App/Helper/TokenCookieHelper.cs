using Microsoft.AspNetCore.Http;
using System;

namespace ProductManagement.App.Helper
{
    public class TokenCookieHelper
    {
        public static void AppendTokens(HttpContext context, string accessToken, string refreshToken)
        {
            context.Response.Cookies.Append("token", accessToken, CookieOptions());
            context.Response.Cookies.Append("refreshToken", refreshToken, CookieOptions());
        }

        public static void AppendAccessToken(HttpContext context, string accessToken)
        {
            context.Response.Cookies.Append("token", accessToken, CookieOptions());
        }

        private static CookieOptions CookieOptions() =>
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };
    }
}
