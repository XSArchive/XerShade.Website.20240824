using System.Reflection;
using XerShade.Website.Managers.Interfaces;

namespace XerShade.Website.Managers;

public class Manager<ObjectType>(Assembly assembly) : IManager<ObjectType>
{
    private readonly Assembly Assembly = assembly;
    private readonly List<Assembly> Assemblies = [];
    private readonly List<ObjectType> Objects = [];

    public IManager<ObjectType> Discover()
    {
        Assembly[] referencedAssemblies = this.Assembly.GetReferencedAssemblies().Select(Assembly.Load).ToArray();

        foreach (Assembly? assembly in referencedAssemblies)
        {
            IEnumerable<Type> objects = assembly.GetTypes().Where(t => typeof(ObjectType).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            if (objects.Any())
            {
                foreach (Type? type in objects)
                {
                    if (!this.Assemblies.Where(a => !string.IsNullOrWhiteSpace(a.FullName) && a.FullName.Equals(assembly.FullName)).Any())
                    {
                        this.Assemblies.Add(assembly);
                    }

                    if (Activator.CreateInstance(type) is ObjectType objectInstance)
                    {
                        if(!this.Objects.Where(o => o?.GetType() != objectInstance.GetType()).Any())
                        {
                            this.Objects.Add(objectInstance);
                        }
                    }
                }
            }            
        }

        return this;
    }
}
