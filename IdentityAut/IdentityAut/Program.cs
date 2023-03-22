using System.Text;
using Entities_Context;
using Entities_Context.Entities.UserNews;
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

namespace Business_Logic
{
    public class Program
    {
        public static void Main(string[] args)
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

            builder.Services.AddScoped<IIdentityService, IdentityService>();
            builder.Services.AddScoped<IUserInfoAndSettingsService, UserInfoAndSettingsService>();
            builder.Services.AddScoped<LoginValidationFilterAttribute>();
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddSwaggerGen();
            
            
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Start}/{id?}");
            app.Run();
        }
    }
}