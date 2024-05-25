using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace XerShade.Website.Core.Modules.Interfaces;

public interface IModule
{
    void ConfigureEnvironment(WebApplication app);
    void ConfigureIdentity(IServiceProvider services);
    void ConfigureRouting(WebApplication app);
    void MigrateDbContexts(IServiceProvider services);
    void RegisterControllers(IMvcBuilder builder);
    void RegisterDbContexts(IServiceCollection services);
    void RegisterIdentity(IServiceCollection services);
    void RegisterMiddleware(WebApplication app);
    void RegisterProviders(WebApplication app);
    void RegisterServices(IServiceCollection services);
}
