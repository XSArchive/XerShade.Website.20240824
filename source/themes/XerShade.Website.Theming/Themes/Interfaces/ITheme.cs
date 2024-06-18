using Microsoft.AspNetCore.Builder;

namespace XerShade.Website.Theming.Themes.Interfaces;

public interface ITheme
{
    void RegisterProviders(WebApplication app);
    void RegisterRazorPages(WebApplicationBuilder builder);
}
