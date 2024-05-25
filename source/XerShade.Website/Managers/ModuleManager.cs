using System.Reflection;
using XerShade.Website.Core.Modules.Interfaces;
using XerShade.Website.Managers.Interfaces;

namespace XerShade.Website.Managers;

public class ModuleManager(Assembly assembly) : Manager<IModule>(assembly), IModuleManager
{
    public override IManager<IModule> Discover()
    {
        _ = this.LoadCoreAssembly();

        return base.Discover();
    }

    private Assembly LoadCoreAssembly()
    {
        Assembly coreAssembly = Assembly.Load("XerShade.Website.Core");

        if (coreAssembly != null)
        {
            Type? coreModuleType = coreAssembly.GetTypes()
            .FirstOrDefault(t => t.Name == "CoreModule" && typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            if (coreModuleType != null)
            {
                if (Activator.CreateInstance(coreModuleType) is IModule coreModuleInstance)
                {
                    this.Objects.Add(coreModuleInstance);

                    IEnumerable<Type> objects = assembly.GetTypes().Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                    if (objects.Any())
                    {
                        foreach (Type? type in objects)
                        {
                            if (Activator.CreateInstance(type) is IModule objectInstance)
                            {
                                if (!this.Objects.Where(o => o?.GetType() != objectInstance.GetType()).Any())
                                {
                                    this.Objects.Add(objectInstance);
                                }
                            }
                        }
                    }

                    this.Assemblies.Add(coreModuleType.Assembly);

                    return coreAssembly;
                }
            }

            throw new NullReferenceException($"Unable to find the CoreModule class in the {coreAssembly.FullName} assembly.");
        }

        throw new NullReferenceException($"Unable to find the required assembly XerShade.Website.Core.");
    }

    public IManager<IModule> Execute(Action<IModule, IServiceCollection> action, IServiceCollection services)
    {
        foreach (IModule obj in this.Objects)
        {
            action(obj, services);
        }

        return this;
    }

    public IManager<IModule> Execute(Action<IModule, IMvcBuilder> action, IMvcBuilder builder)
    {
        foreach (IModule obj in this.Objects)
        {
            action(obj, builder);
        }

        return this;
    }

    public IManager<IModule> Execute(Action<IModule, IServiceProvider> action, IServiceProvider services)
    {
        foreach (IModule obj in this.Objects)
        {
            action(obj, services);
        }

        return this;
    }
}
