using Microsoft.Extensions.Hosting;
using XerShade.Website.Core.Factories.Population.Interfaces;
using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Core.Services;

public class PopulationService(IEnumerable<IPopulationFactory> populationFactories) : IPopulationService, IHostedService
{
    protected readonly IEnumerable<IPopulationFactory> PopulationFactories = populationFactories;

    public virtual async Task StartAsync(CancellationToken cancellationToken) => await this.PopulateFactories();
    public virtual Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    protected virtual async Task PopulateFactories()
    {
        foreach(IPopulationFactory factory in this.PopulationFactories)
        {
            factory.Populate();

            await factory.PopulateAsync();
        }
    }
}
