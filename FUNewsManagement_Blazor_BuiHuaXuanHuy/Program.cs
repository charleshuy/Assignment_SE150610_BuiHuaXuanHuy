using DAO;
using FUNewsManagement_Blazor_BuiHuaXuanHuy.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Repo;
using Microsoft.EntityFrameworkCore;
using Repo.Services;

namespace FUNewsManagement_Blazor_BuiHuaXuanHuy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddQuickGridEntityFrameworkAdapter();
            builder.Services.AddDbContext<FunewsManagementContext>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<INewsService, NewsService>();

            // Add authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.AccessDeniedPath = "/AccessDenied";
                });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAntiforgery(); // Keep this for general antiforgery support
            builder.Services.AddSignalR();

            // Add distributed in-memory cache for session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseMigrationsEndPoint();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting(); // Ensure routing is before authentication/authorization
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery(); // Only once, after routing but before endpoints
            app.UseSession();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
