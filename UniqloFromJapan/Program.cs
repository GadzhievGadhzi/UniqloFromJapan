using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using UniqloFromJapan.Data;
using UniqloFromJapan.Models;

namespace UniqloFromJapan {
    public class Program {
        public static void Main(string[] args) {

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<DataRepository>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("dbUniqloFromJapan")!));

            var app = builder.Build();
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}