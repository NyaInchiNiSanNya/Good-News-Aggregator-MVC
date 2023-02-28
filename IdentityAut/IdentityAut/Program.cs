using CustomIdentityApp.Controllers;
using Entities_Context.Entities.Identity;
using Entities_Context.Entities.UserNews;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Domain.DBContext;
using Repositores;
using Services.Account;

namespace IdentityAut
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            
            builder.Services.AddDbContext<IdentityContext.IdentityContext>(opt =>
            {
                var connString = builder.Configuration
                    .GetConnectionString("IdentityConnection");
                opt.UseSqlServer(connString);

            });

            builder.Services.AddDbContext<UserNewsContext>(opt =>
            {
                var connString = builder.Configuration
                    .GetConnectionString("DefaultConnection");
                opt.UseSqlServer(connString);

            });

            

            builder.Services.AddScoped<IdentityRepository, IdentityService>();
            builder.Services.AddScoped<UserConfigRepositores.GetSetUserConfigRepositore, UserConfigService>();



            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext.IdentityContext>();
            builder.Services.Configure<IdentityOptions>(opts => {
                opts.User.RequireUniqueEmail = true;
            });

            
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();    
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Start}/{id?}");

            app.Run();
        }
    }
}