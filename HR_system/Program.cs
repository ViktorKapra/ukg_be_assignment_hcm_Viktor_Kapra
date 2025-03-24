using Data;
using Data.Account;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HR_system
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add logger
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Information()
              .WriteTo.File(".\\Logs\\log_all.txt", rollingInterval: RollingInterval.Day)
              .WriteTo.Logger(l => l
                  .Filter.ByIncludingOnly(x => x.Level == Serilog.Events.LogEventLevel.Information)
                  .WriteTo.File(".\\Logs\\log_info.txt"))
              .WriteTo.Logger(l => l
                  .Filter.ByIncludingOnly(x => x.Level == Serilog.Events.LogEventLevel.Warning)
                  .WriteTo.File(".\\Logs\\log_warn.txt", rollingInterval: RollingInterval.Day))
              .WriteTo.Logger(l => l
                  .Filter.ByIncludingOnly(x => x.Level == Serilog.Events.LogEventLevel.Fatal)
                  .WriteTo.File(".\\Logs\\log_fatal.txt", rollingInterval: RollingInterval.Day))
              .WriteTo.Logger(l => l
                  .Filter.ByIncludingOnly(x => x.Level == Serilog.Events.LogEventLevel.Fatal)
                  .WriteTo.File(".\\Logs\\log_fatal.txt", rollingInterval: RollingInterval.Day))
              .WriteTo.TestCorrelator()
              .CreateLogger();

            builder.Services.AddSerilog();

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
