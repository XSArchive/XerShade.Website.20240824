namespace XerShade.Website.Core.Factories.Population.Interfaces;

public interface IPopulationFactory<ServiceClass> where ServiceClass : class
{
    void Populate();
}
