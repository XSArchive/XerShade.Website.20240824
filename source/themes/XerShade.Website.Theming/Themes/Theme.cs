using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using XerShade.Website.Theming.Themes.Interfaces;

namespace XerShade.Website.Theming.Themes;

public class Theme : ITheme
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

    public virtual void RegisterRazorPages(WebApplicationBuilder builder)
    {
        if (builder.Configuration.GetValue<bool>("EnableRazorRuntimeCompilation"))
        {
            Assembly assembly = this.GetType().Assembly;

            if (!HasRegisteredAssembly("RazorPageProviders", assembly))
            {
                _ = builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
                {
                    string libraryPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "themes", assembly.GetName().Name ?? throw new NullReferenceException()));
                    options.FileProviders.Add(new PhysicalFileProvider(libraryPath));
                });
            }
        }
    }
}
