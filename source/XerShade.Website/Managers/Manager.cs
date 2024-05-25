using System.Reflection;
using XerShade.Website.Managers.Interfaces;

namespace XerShade.Website.Managers;

public class Manager<ObjectType>(Assembly assembly) : IManager<ObjectType>
{
    protected readonly Assembly Assembly = assembly ?? throw new ArgumentNullException();
    protected readonly List<Assembly> Assemblies = [];
    protected readonly List<ObjectType> Objects = [];

    public virtual IManager<ObjectType> Discover()
    {
        Assembly[] referencedAssemblies = this.Assembly.GetReferencedAssemblies().Select(Assembly.Load).ToArray();

        foreach (Assembly? assembly in referencedAssemblies)
        {
            if (this.Assemblies.Where(a => !string.IsNullOrWhiteSpace(a.FullName) && a.FullName.Equals(assembly.FullName)).Any())
            {
                continue;
            }

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
                        if (!this.Objects.Where(o => o?.GetType() != objectInstance.GetType()).Any())
                        {
                            this.Objects.Add(objectInstance);
                        }
                    }
                }
            }
        }

        return this;
    }

    public virtual IManager<ObjectType> Execute(Action<ObjectType> action)
    {
        foreach (ObjectType obj in this.Objects)
        {
            action(obj);
        }

        return this;
    }

    public virtual IManager<ObjectType> Execute(Action<ObjectType, WebApplication> action, WebApplication app)
    {
        foreach (ObjectType obj in this.Objects)
        {
            action(obj, app);
        }

        return this;
    }

    public virtual IManager<ObjectType> ExecuteOnAssemblies(Action<Assembly> action)
    {
        foreach (Assembly assembly in this.Assemblies)
        {
            action(assembly);
        }

        return this;
    }
}
