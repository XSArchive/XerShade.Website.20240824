using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using XerShade.Website.Core.Areas.Account.Data;
using XerShade.Website.Core.Areas.Account.Data.Models;
using XerShade.Website.Core.Controllers;
using XerShade.Website.Core.Data;

namespace XerShade.Website;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        _ = builder.Services.AddDbContext<GeneralDbContext>();

        _ = builder.Services.AddDbContext<AuthenticationDbContext>();
        _ = builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
        })
            .AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddDefaultTokenProviders();

        _ = builder.Services.AddControllersWithViews()
            .AddApplicationPart(typeof(HomeController).Assembly);

        _ = builder.Services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto);

        _ = builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        _ = builder.Services.AddScoped<IUrlHelper>(x =>
        {
            ActionContext? actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
            IUrlHelperFactory factory = x.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(context: actionContext ?? throw new NullReferenceException());
        });

        WebApplication app = builder.Build();

        using (IServiceScope scope = app.Services.CreateScope())
        {
            IServiceProvider services = scope.ServiceProvider;

            services.GetRequiredService<GeneralDbContext>().Database.Migrate();
            services.GetRequiredService<AuthenticationDbContext>().Database.Migrate();
        }

        if (!app.Environment.IsDevelopment())
        {
            _ = app.UseExceptionHandler("/Home/Error");
            _ = app.UseForwardedHeaders();
            _ = app.UseHsts();
        }
        else
        {
            _ = app.UseDeveloperExceptionPage();
            _ = app.UseForwardedHeaders();
        }

        _ = app.UseHttpsRedirection();
        _ = app.UseStaticFiles();

        _ = app.UseRouting();

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        _ = app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        _ = app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
