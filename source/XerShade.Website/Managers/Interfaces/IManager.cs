using System.Reflection;

namespace XerShade.Website.Managers.Interfaces;

public interface IManager<ObjectType>
{
    IManager<ObjectType> Discover();
    IManager<ObjectType> Execute(Action<ObjectType> action);
    IManager<ObjectType> Execute(Action<ObjectType, WebApplication> action, WebApplication app);
    IManager<ObjectType> ExecuteOnAssemblies(Action<Assembly> action);
}
