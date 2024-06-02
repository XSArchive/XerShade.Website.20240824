using XerShade.Website.Core.Framework.Factories.Population.Interfaces;
using XerShade.Website.Core.Framework.Services.Interfaces;

namespace XerShade.Website.Core.Framework.Services;

public class PopulationService(IEnumerable<IPopulationFactory> populationFactories) : IPopulationService
{
    protected readonly IEnumerable<IPopulationFactory> PopulationFactories = populationFactories;

    public virtual void PopulateFactories()
    {
        foreach (IPopulationFactory factory in PopulationFactories)
        {
            factory.Populate();
        }
    }

    public virtual async Task PopulateFactoriesAsync()
    {
        foreach (IPopulationFactory factory in PopulationFactories)
        {
            await factory.PopulateAsync();
        }
    }
}
