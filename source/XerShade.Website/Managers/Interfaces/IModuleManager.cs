using XerShade.Website.Core.Modules.Interfaces;

namespace XerShade.Website.Managers.Interfaces;

public interface IModuleManager : IManager<IModule>
{
    IManager<IModule> Execute(Action<IModule, IServiceCollection> action, IServiceCollection services);
    IManager<IModule> Execute(Action<IModule, IMvcBuilder> action, IMvcBuilder builder);
    IManager<IModule> Execute(Action<IModule, IServiceProvider> action, IServiceProvider services);
}
