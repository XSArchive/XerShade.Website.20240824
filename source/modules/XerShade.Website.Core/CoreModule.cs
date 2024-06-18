using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using XerShade.Website.Core.Areas.User.Data;
using XerShade.Website.Core.Areas.User.Data.Models;
using XerShade.Website.Core.Components.Options.Factories;
using XerShade.Website.Core.Components.Options.Services;
using XerShade.Website.Core.Components.Options.Services.Interfaces;
using XerShade.Website.Core.Framework.Data;
using XerShade.Website.Core.Framework.Factories.Population.Interfaces;
using XerShade.Website.Core.Framework.Modules;
using XerShade.Website.Core.Framework.Services;
using XerShade.Website.Core.Framework.Services.Interfaces;

namespace XerShade.Website.Core;

public class CoreModule : Module
{
    public override void ConfigureEnvironment(WebApplication app)
    {
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
    }

    public override void ConfigureIdentity(IServiceProvider services)
    {
        IOptionsService? optionsService = services.GetService<IOptionsService>();
        IdentityOptions? identityOptions = services.GetService<IOptions<IdentityOptions>>()?.Value;

        if (optionsService != null && identityOptions != null)
        {
            identityOptions.Password.RequiredLength = optionsService.Read("Core.Authentication.RequiredLength", 8);
            identityOptions.Password.RequireNonAlphanumeric = optionsService.Read("Core.Authentication.RequireNonAlphanumeric", false);
            identityOptions.Password.RequireDigit = optionsService.Read("Core.Authentication.RequireDigit", true);
            identityOptions.Password.RequireLowercase = optionsService.Read("Core.Authentication.RequireLowercase", true);
            identityOptions.Password.RequireUppercase = optionsService.Read("Core.Authentication.RequireUppercase", true);
        }
    }

    public override void ConfigureRouting(WebApplication app)
    {
        _ = app.UseRouting();

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        _ = app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        _ = app.MapControllerRoute(
            name: "default",
            pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}");
    }

    public override void MigrateDbContexts(IServiceProvider services)
    {
        services.GetRequiredService<DataDbContext>().Database.Migrate();
        services.GetRequiredService<AuthenticationDbContext>().Database.Migrate();
    }

    public override void PopulateDbContexts(IServiceProvider services)
    {
        IPopulationService? populationService = services.GetService<IPopulationService>();

        populationService?.PopulateFactories();
        _ = Task.Run(async () => await (populationService?.PopulateFactoriesAsync() ?? throw new NullReferenceException()));

        base.PopulateDbContexts(services);
    }

    public override void RegisterDbContexts(IServiceCollection services)
    {
        _ = services.AddDbContextFactory<DataDbContext>();
        _ = services.AddDbContext<AuthenticationDbContext>();
    }

    public override void RegisterIdentity(IServiceCollection services) => _ = services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<AuthenticationDbContext>().AddDefaultTokenProviders();

    public override void RegisterLogging(IServiceCollection services)
    {
        _ = services.AddSerilog();

        base.RegisterLogging(services);
    }

    public override void RegisterProviders(WebApplication app)
    {
        _ = app.UseStaticFiles(new StaticFileOptions());

        base.RegisterProviders(app);
    }

    public override void RegisterServices(IServiceCollection services)
    {
        _ = services.AddTransient<IPopulationService, PopulationService>();
        _ = services.AddTransient<IPopulationFactory, OptionsPopulationFactory>();
        _ = services.AddSingleton<IOptionsService, OptionsService>();

        _ = services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto);

        _ = services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        _ = services.AddScoped(x =>
        {
            ActionContext? actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
            IUrlHelperFactory factory = x.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(context: actionContext ?? throw new NullReferenceException());
        });
    }
}
