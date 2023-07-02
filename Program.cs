using CarService.Hubs;
using CarService.Models;
using CarService.Reposatories;
using CarService.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSignalR();
            builder.Services.AddDbContext<CarServiceEntities>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Cs"));
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                //options.User.
            }

           ).AddEntityFrameworkStores<CarServiceEntities>();


            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });


            builder.Services.AddScoped<IRulesRepository, RulesRepository>();
            builder.Services.AddScoped<IUserInfoRepsatories, UserInfoRepsatories>();
            builder.Services.AddScoped<ICityRepository, CityRepository>();
            builder.Services.AddScoped<IData, Data>();
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<AddOrderHub>("/AddOrder");
            app.MapHub<AddEmployeeHub>("/AddEmployee");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=sytemIndex}/{id?}");

            app.Run();



    }
    }
}