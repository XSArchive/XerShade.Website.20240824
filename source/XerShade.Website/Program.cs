using System.Reflection;
using XerShade.Website.Managers;
using XerShade.Website.Managers.Interfaces;

IModuleManager moduleManager = new ModuleManager(Assembly.GetExecutingAssembly()).Discover() as IModuleManager ?? throw new NullReferenceException();
IThemeManager themeManager = new ThemeManager(Assembly.GetExecutingAssembly()).Discover() as IThemeManager ?? throw new NullReferenceException();
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

_ = moduleManager.Execute((module, builder) => module.RegisterConfiguration(builder), builder);

_ = moduleManager.Execute((module, services) => module.RegisterDbContexts(services), builder.Services);

_ = moduleManager.Execute((module, services) => module.RegisterServices(services), builder.Services);

_ = moduleManager.Execute((module, services) => module.RegisterIdentity(services), builder.Services);

_ = moduleManager.Execute((module, builder) => module.RegisterControllers(builder), builder.Services.AddControllersWithViews());

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    _ = moduleManager.Execute((module, services) => module.MigrateDbContexts(services), services);
    _ = moduleManager.Execute((module, services) => module.PopulateDbContexts(services), services);

    _ = moduleManager.Execute((module, services) => module.ConfigureIdentity(services), services);
}

_ = moduleManager.Execute((module, app) => module.RegisterMiddleware(app), app);

_ = moduleManager.Execute((module, app) => module.ConfigureEnvironment(app), app);

_ = moduleManager.Execute((module, app) => module.RegisterProviders(app), app);
_ = themeManager.Execute((theme, app) => theme.RegisterProviders(app), app);

_ = moduleManager.Execute((module, app) => module.ConfigureRouting(app), app);

app.Run();
