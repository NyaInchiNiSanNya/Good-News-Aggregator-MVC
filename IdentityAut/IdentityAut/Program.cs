
using Entities_Context;

using IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MVC.ControllerFactory;
using MVC.Filters.Validation;
using Repositores;
using Services.Account;
using UserConfigRepositores;
using MVC.Middlware;
using Serilog;
using Services.Article;
using System.Data;
using Abstract;
using AspNetSamples.Abstractions.Data.Repositories;
using Entities_Context.Entities.UserNews;
using IServices.Repositories;
using AspNetSamples.Repositories;
using Repositories.Implementations;

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

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<ISourceRepository, SourceRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUsersRolesRepository, UsersRolesRepository>();
            builder.Services.AddScoped<IUserInterfaceThemeRepository, UserInterfaceThemeRepository>();

            builder.Services.AddTransient<IServiceFactory,ServiceFactory>();
            builder.Services.AddScoped<ISourceService, SourceService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserInfoAndSettingsService, UserInfoAndSettingsService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUiThemeService, UiThemeService>();
            builder.Services.AddTransient<SettingsValidationFilterAttribute>();
            builder.Services.AddScoped<LoginValidationFilterAttribute>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");

                    options.AccessDeniedPath = new PathString("/Account/Login");

                });
            //builder.Services.AddMvc(options =>
            //{
            //    options.Filters.Add<CustomExceptionFilter>();
            //});

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
                pattern: "{controller=Article}/{action=GetArticlesByPage}/{id?}");
            app.Run();
        }
    }
}