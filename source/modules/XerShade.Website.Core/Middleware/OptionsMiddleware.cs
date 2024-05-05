using Microsoft.AspNetCore.Http;
using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Core.Middleware;

public class OptionsMiddleware(RequestDelegate next, IOptionsService options)
{
    private readonly RequestDelegate next = next ?? throw new ArgumentNullException(nameof(next));
    private readonly IOptionsService options = options ?? throw new ArgumentNullException(nameof(options));

    public async Task Invoke(HttpContext context)
    {
        await this.PopulateOptionsTable();
        await this.ReadOptionsData(context);

        await this.next(context);
    }

    protected Task ReadOptionsData(HttpContext context)
    {
        _ = context.Items.TryAdd("Options", this.options.ToDictionary());

        return Task.CompletedTask;
    }

    protected async Task PopulateOptionsTable()
    {
        await this.PopulateOption("Core.Website.Name", "XerShade's Corner");
        await this.PopulateOption("Core.Website.Description", "My little corner of the internet.");
    }

    protected async Task PopulateOption<TValue>(string optionName, TValue optionValue) where TValue : class
    {
        bool result = await this.options.HasAsync(optionName, checkCache: false);

        if(!result)
        {
            await this.options.WriteAsync(optionName, optionValue, true);
        }
    }
}
