using XerShade.Website.Core.Factories.Population.Interfaces;

namespace XerShade.Website.Core.Factories.Population;
public abstract class AsyncPopulationFactory<ServiceClass>(ServiceClass service) : IAsyncPopulationFactory<ServiceClass> where ServiceClass : class
{
    protected readonly ServiceClass service = service;

    public abstract Task Populate();
}
