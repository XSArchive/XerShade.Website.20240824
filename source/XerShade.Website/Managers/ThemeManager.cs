using System.Reflection;
using XerShade.Website.Managers.Interfaces;
using XerShade.Website.Theming.Themes.Interfaces;

namespace XerShade.Website.Managers;

public class ThemeManager(Assembly assembly) : Manager<ITheme>(assembly), IThemeManager
{
}
