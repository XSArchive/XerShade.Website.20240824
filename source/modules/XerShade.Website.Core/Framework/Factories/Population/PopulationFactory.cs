using XerShade.Website.Core.Framework.Factories.Population.Interfaces;

namespace XerShade.Website.Core.Framework.Factories.Population;
public abstract class PopulationFactory() : IPopulationFactory
{
    public virtual void Populate() { }
    public virtual Task PopulateAsync() => Task.CompletedTask;
}
