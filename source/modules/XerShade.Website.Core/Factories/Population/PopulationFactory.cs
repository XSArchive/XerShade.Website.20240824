using XerShade.Website.Core.Factories.Population.Interfaces;

namespace XerShade.Website.Core.Factories.Population;
public abstract class PopulationFactory() : IPopulationFactory
{
    public virtual void Populate() { }
    public virtual Task PopulateAsync() => Task.CompletedTask;
}
