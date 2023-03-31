using System.Text;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MVC.Filters.Validation;
using Repositores;
using Services.Account;
using UserConfigRepositores;
using MVC.Middlware;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Telegram;
using Telegram.Bot;
using System.Diagnostics;

namespace Business_Logic
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<UserArticleContext>(opt =>
            {
                var connString = builder.Configuration
                    .GetConnectionString("DefaultConnection");
                opt.UseSqlServer(connString);

            });

            builder.Services.AddHttpContextAccessor();

            builder.Host.UseSerilog((ctx, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(ctx.Configuration);

            });

            builder.Services.AddTransient<IIdentityService, IdentityService>();
            builder.Services.AddTransient<IUserInfoAndSettingsService, UserInfoAndSettingsService>();
            builder.Services.AddTransient<IRoleService, RoleService>();
            builder.Services.AddScoped<LoginValidationFilterAttribute>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");

                    options.AccessDeniedPath = new PathString("/Account/Login");

                });
            builder.Services.AddMvc(options =>
            {
                options.Filters.Add<CustomExceptionFilter>();
            });

            builder.Services.AddSwaggerGen();
            
            
            var app = builder.Build();
            
            //if (!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            
            app.UseMiddleware<ErrorMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Start}/{id?}");
            app.Run();
        }
    }
}