using System.Reflection;

namespace XerShade.Website.Managers.Interfaces;

public interface IManager<ObjectType>
{
    IManager<ObjectType> Discover();
    void Execute(Action<ObjectType> action);
    void ExecuteOnAssemblies(Action<Assembly> action);
}
