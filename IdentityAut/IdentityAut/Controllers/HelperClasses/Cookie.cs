using Core.DTOs;
using Microsoft.AspNetCore.Mvc;

static class Cookie
{
    internal static async Task SettingsAndInfoPutIn(IHttpContextAccessor a, GetUserInfoWithSettingsDTO modelDTO)
    {
        a.HttpContext.Response.Cookies.Append("name", modelDTO.Name);
        a.HttpContext.Response.Cookies.Append("email", modelDTO.Email);
        a.HttpContext.Response.Cookies.Append("theme", modelDTO.Theme);
    }
}