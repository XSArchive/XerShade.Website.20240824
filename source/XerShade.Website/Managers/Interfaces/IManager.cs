using System.Reflection;

namespace XerShade.Website.Managers.Interfaces;

public interface IManager<ObjectType>
{
    IManager<ObjectType> Discover();
    IManager<ObjectType> Execute(Action<ObjectType> action);
    IManager<ObjectType> Execute<ParameterType>(Action<ObjectType, ParameterType> action, ParameterType parameter);
    IManager<ObjectType> ExecuteOnAssemblies(Action<Assembly> action);
}
