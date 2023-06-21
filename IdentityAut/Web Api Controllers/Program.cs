using Entities_Context.Entities.UserNews;
using IServices.Repositories;
using IServices.Services;
using IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementations;
using Repositories;
using Serilog;
using Services.Account;
using Services.Article;
using Web_Api_Controllers.ControllerFactory;
using Web_Api_Controllers.Filters.Errors;

namespace Web_Api_Controllers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
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
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<ISourceRepository, SourceRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUsersRolesRepository, UsersRolesRepository>();
            builder.Services.AddScoped<IArticleTagRepository, ArticleTagRepository>();
            builder.Services.AddScoped<IUserInterfaceThemeRepository, UserInterfaceThemeRepository>();
            builder.Services.AddTransient<IArticleTagService, ArticleTagService>();
            builder.Services.AddTransient<IServiceFactory, ServiceFactory>();
            builder.Services.AddScoped<ISourceService, SourceService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ISettingsService, SettingsService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUiThemeService, UiThemeService>();
            builder.Services.AddAutoMapper(typeof(Program));
            
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");

                    options.AccessDeniedPath = new PathString("/Account/Login");

                });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseAuthentication();
            app.MapControllers();

            app.Run();
        }
    }
}