using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using XerShade.Website.Core.Framework.Modules.Interfaces;

namespace XerShade.Website.Core.Framework.Modules;

public abstract class Module : IModule
{
    private static readonly Dictionary<string, HashSet<Assembly>> RegisteredAssemblies = [];

    private static bool HasRegisteredAssembly(string key, Assembly assembly)
    {
        if (!RegisteredAssemblies.TryGetValue(key, out HashSet<Assembly>? value))
        {
            value = ([]);

            RegisteredAssemblies.Add(key, value);

            _ = value.Add(assembly);

            return false;
        }

        if (!value.Contains(assembly))
        {
            _ = value.Add(assembly);

            return false;
        }

        return true;
    }

    public virtual void ConfigureEnvironment(WebApplication app) { }

    public virtual void ConfigureIdentity(IServiceProvider services) { }

    public virtual void ConfigureRouting(WebApplication app) { }

    public virtual void MigrateDbContexts(IServiceProvider services) { }

    public virtual void PopulateDbContexts(IServiceProvider services) { }

    public virtual void RegisterConfiguration(WebApplicationBuilder builder)
    {
        Assembly assembly = this.GetType().Assembly;

        if (!HasRegisteredAssembly("Configuration", assembly))
        {
            _ = builder.Configuration.AddUserSecrets(assembly);
        }
    }

    public virtual void RegisterControllers(IMvcBuilder builder)
    {
        Assembly assembly = this.GetType().Assembly;

        if (!HasRegisteredAssembly("Controllers", assembly))
        {
            _ = builder.AddApplicationPart(assembly);
        }
    }

    public virtual void RegisterDbContexts(IServiceCollection services) { }

    public virtual void RegisterIdentity(IServiceCollection services) { }

    public virtual void RegisterMiddleware(WebApplication app) { }

    public virtual void RegisterProviders(WebApplication app)
    {
        Assembly assembly = this.GetType().Assembly;

        if (!HasRegisteredAssembly("Providers", assembly))
        {
            if (assembly.GetManifestResourceNames().Length != 0)
            {
                _ = app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new ManifestEmbeddedFileProvider(assembly)
                });
            }
        }
    }

    public virtual void RegisterServices(IServiceCollection services) { }
}
