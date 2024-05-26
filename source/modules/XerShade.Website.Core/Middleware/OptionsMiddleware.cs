using Microsoft.AspNetCore.Http;
using XerShade.Website.Core.Factories.Population.Interfaces;

namespace XerShade.Website.Core.Middleware;

public class OptionsMiddleware(RequestDelegate next, IOptionsPopulationFactory populationFactory)
{
    private readonly RequestDelegate next = next ?? throw new ArgumentNullException(nameof(next));
    private readonly IOptionsPopulationFactory populationFactory = populationFactory ?? throw new ArgumentNullException(nameof(populationFactory));

    public async Task Invoke(HttpContext context)
    {
        await this.populationFactory.Populate();

        await this.next(context);
    }
}
