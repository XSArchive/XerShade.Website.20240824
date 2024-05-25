using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using XerShade.Website.Core.Modules.Interfaces;

namespace XerShade.Website.Core.Modules;

public abstract class Module : IModule
{
    private static readonly HashSet<Assembly> RegisteredControllerAssemblies = [];
    private static readonly HashSet<Assembly> RegisteredProviderAssemblies = [];

    public virtual void ConfigureEnvironment(WebApplication app) { }

    public virtual void ConfigureIdentity(IServiceProvider services) { }

    public virtual void ConfigureRouting(WebApplication app) { }

    public virtual void MigrateDbContexts(IServiceProvider services) { }

    public virtual void RegisterControllers(IMvcBuilder builder)
    {
        Assembly assembly = this.GetType().Assembly;

        if (!RegisteredControllerAssemblies.Contains(assembly))
        {
            _ = builder.AddApplicationPart(assembly);
            _ = RegisteredControllerAssemblies.Add(assembly);
        }
    }

    public virtual void RegisterDbContexts(IServiceCollection services) { }

    public virtual void RegisterIdentity(IServiceCollection services) { }

    public virtual void RegisterMiddleware(WebApplication app) { }

    public virtual void RegisterProviders(WebApplication app)
    {
        Assembly assembly = this.GetType().Assembly;

        if (!RegisteredProviderAssemblies.Contains(assembly))
        {
            if (assembly.GetManifestResourceNames().Length != 0)
            {
                _ = app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new ManifestEmbeddedFileProvider(assembly)
                });
                _ = RegisteredProviderAssemblies.Add(assembly);
            }
        }
    }

    public virtual void RegisterServices(IServiceCollection services) { }
}
