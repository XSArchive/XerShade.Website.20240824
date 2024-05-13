using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using XerShade.Website.Application.Interfaces;
using XerShade.Website.Core.Areas.Account.Data;
using XerShade.Website.Core.Areas.Account.Data.Models;
using XerShade.Website.Core.Controllers;
using XerShade.Website.Core.Data;
using XerShade.Website.Core.Factories.Population;
using XerShade.Website.Core.Factories.Population.Interfaces;
using XerShade.Website.Core.Middleware;
using XerShade.Website.Core.Services;
using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Application;

public class MvcApplication(IOptionsService optionsService) : IMvcApplication
{
    public readonly IOptionsService OptionsService = optionsService;

    public virtual IMvcApplication Build(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        _ = builder.Services.AddDbContext<GeneralDbContext>();
        _ = builder.Services.AddTransient<IOptionsPopulationFactory, OptionsPopulationFactory>();
        _ = builder.Services.AddSingleton<IOptionsService, OptionsService>();

        _ = builder.Services.AddDbContext<AuthenticationDbContext>();
        _ = builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                if (this.OptionsService != null)
                {
                    options.Password.RequiredLength = this.OptionsService.Read("Core.Authentication.RequiredLength", 8);
                    options.Password.RequireNonAlphanumeric = this.OptionsService.Read("Core.Authentication.RequireNonAlphanumeric", false);
                    options.Password.RequireDigit = this.OptionsService.Read("Core.Authentication.RequireDigit", true);
                    options.Password.RequireLowercase = this.OptionsService.Read("Core.Authentication.RequireLowercase", true);
                    options.Password.RequireUppercase = this.OptionsService.Read("Core.Authentication.RequireUppercase", true);
                }
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

        _ = app.UseMiddleware<OptionsMiddleware>();

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
            pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}");

        app.Run();

        return this;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        return;
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}
