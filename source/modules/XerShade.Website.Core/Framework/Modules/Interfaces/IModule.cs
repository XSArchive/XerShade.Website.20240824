using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace XerShade.Website.Core.Framework.Modules.Interfaces;

public interface IModule
{
    void ConfigureEnvironment(WebApplication app);
    void ConfigureIdentity(IServiceProvider services);
    void ConfigureRouting(WebApplication app);
    void MigrateDbContexts(IServiceProvider services);
    void PopulateDbContexts(IServiceProvider services);
    void RegisterConfiguration(WebApplicationBuilder builder);
    void RegisterControllers(IMvcBuilder builder);
    void RegisterDbContexts(IServiceCollection services);
    void RegisterIdentity(IServiceCollection services);
    void RegisterLogging(IServiceCollection services);
    void RegisterMiddleware(WebApplication app);
    void RegisterProviders(WebApplication app);
    void RegisterRazorPages(WebApplicationBuilder builder);
    void RegisterServices(IServiceCollection services);
}
