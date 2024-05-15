using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using XerShade.Website.Core.Areas.Account.Data;
using XerShade.Website.Core.Areas.Account.Data.Models;
using XerShade.Website.Core.Controllers;
using XerShade.Website.Core.Data;
using XerShade.Website.Core.Factories.Population;
using XerShade.Website.Core.Factories.Population.Interfaces;
using XerShade.Website.Core.Middleware;
using XerShade.Website.Core.Services;
using XerShade.Website.Core.Services.Interfaces;
using XerShade.Website.Theming;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

_ = builder.Services.AddDbContext<GeneralDbContext>();
_ = builder.Services.AddTransient<IOptionsPopulationFactory, OptionsPopulationFactory>();
_ = builder.Services.AddSingleton<IOptionsService, OptionsService>();

_ = builder.Services.AddDbContext<AuthenticationDbContext>();
_ = builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
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

_ = app.UseStaticFiles(new StaticFileOptions());

_ = app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new ManifestEmbeddedFileProvider(typeof(DefaultTheme).Assembly)
});

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
