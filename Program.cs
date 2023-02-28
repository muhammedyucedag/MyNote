using EFCoreWithMvcCore;
using MyNoteSampleApp.Models.Context;

namespace MyNoteSampleApp
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;
            AppSettings appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            DatabaseInitializer databaseInitializer = new DatabaseInitializer(new DatabaseContext(), appSettings);
            databaseInitializer.Seed();

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "notes.session";
                options.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession(); // sessionu pipeline alanýna aktifleþitiryoruz
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

    }
}