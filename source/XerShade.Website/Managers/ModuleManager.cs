using System.Reflection;
using XerShade.Website.Core.Modules.Interfaces;
using XerShade.Website.Managers.Interfaces;

namespace XerShade.Website.Managers;

public class ModuleManager(Assembly assembly) : Manager<IModule>(assembly), IModuleManager
{
}
