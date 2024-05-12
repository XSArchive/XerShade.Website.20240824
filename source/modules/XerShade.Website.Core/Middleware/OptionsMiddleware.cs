using Microsoft.AspNetCore.Http;
using XerShade.Website.Core.Factories.Population.Interfaces;
using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Core.Middleware;

public class OptionsMiddleware(RequestDelegate next, IOptionsService options, IOptionsPopulationFactory populationFactory)
{
    private readonly RequestDelegate next = next ?? throw new ArgumentNullException(nameof(next));
    private readonly IOptionsService options = options ?? throw new ArgumentNullException(nameof(options));
    private readonly IOptionsPopulationFactory populationFactory = populationFactory ?? throw new ArgumentNullException(nameof(populationFactory));

    public async Task Invoke(HttpContext context)
    {
        await this.populationFactory.Populate();
        await this.ReadOptionsData(context);

        await this.next(context);
    }

    protected Task ReadOptionsData(HttpContext context)
    {
        _ = context.Items.TryAdd("Options", this.options.ToDictionary());

        return Task.CompletedTask;
    }
}
