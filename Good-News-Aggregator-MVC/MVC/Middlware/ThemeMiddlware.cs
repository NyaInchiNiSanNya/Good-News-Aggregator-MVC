using Azure;
using IServices.Services;
using Microsoft.IdentityModel.Tokens;
using MVC.ControllerFactory;

namespace MVC.Middlware
{
    public class CookieExpirationMiddleware
    {
        private readonly RequestDelegate _next;



        public CookieExpirationMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task InvokeAsync(HttpContext context, ISettingsService userSettingsService, IUiThemeService themeService)
        {


            var themeCookie = context.Request.Cookies["theme"];

            if (context.User.Identity is { IsAuthenticated: true }
                && !context.User.Identity.Name.IsNullOrEmpty() && themeCookie == null)
            {
                var theme = await userSettingsService.GetUserInformationAsync(context.User.Identity.Name!);


                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                };

                if (theme != null && !theme.Theme.IsNullOrEmpty())
                {
                    context.Response.Cookies.Append("theme", theme.Theme, cookieOptions);
                }
                else
                {
                    context.Response.Cookies.Append("theme", await themeService.GetThemeNameByIdAsync(
                        await themeService.GetIdDefaultThemeAsync()), cookieOptions);
                }
            }
            await _next(context);
        }
    }
}
