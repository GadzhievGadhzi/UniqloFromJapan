using Microsoft.EntityFrameworkCore;
using UniqloFromJapan.Data;
using UniqloFromJapan.Middlewares;
using UniqloFromJapan.Services;

namespace UniqloFromJapan {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddDbContext<DataRepository>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("dbUniqloFromJapan")!));

            // Middlewares
            builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

            builder.Services.AddTransient<CachingService>();
            builder.Services.AddMemoryCache();

            var app = builder.Build();
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

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