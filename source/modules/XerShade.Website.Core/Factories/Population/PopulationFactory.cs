using XerShade.Website.Core.Factories.Population.Interfaces;

namespace XerShade.Website.Core.Factories.Population;
public abstract class PopulationFactory<ServiceClass>(ServiceClass service) : IPopulationFactory<ServiceClass> where ServiceClass : class
{
    protected readonly ServiceClass service = service;

    public abstract void Populate();
}
