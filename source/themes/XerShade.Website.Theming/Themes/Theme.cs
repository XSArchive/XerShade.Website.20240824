﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using XerShade.Website.Theming.Themes.Interfaces;

namespace XerShade.Website.Theming.Themes;

public class Theme : ITheme
{
    private static readonly HashSet<Assembly> RegisteredProviderAssemblies = [];

    public virtual void RegisterProviders(WebApplication app)
    {
        Assembly assembly = this.GetType().Assembly;

        if (!RegisteredProviderAssemblies.Contains(assembly))
        {
            try
            {
                _ = app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new ManifestEmbeddedFileProvider(assembly)
                });
                _ = RegisteredProviderAssemblies.Add(assembly);
            }
            catch
            {
                // Do nothing if there are no embedded files to be loaded.
            }
        }
    }
}
