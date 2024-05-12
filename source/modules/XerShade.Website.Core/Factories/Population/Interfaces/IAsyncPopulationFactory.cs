namespace XerShade.Website.Core.Factories.Population.Interfaces;

public interface IAsyncPopulationFactory<ServiceClass> where ServiceClass : class
{
    Task Populate();
}
