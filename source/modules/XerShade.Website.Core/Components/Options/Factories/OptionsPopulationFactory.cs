using Microsoft.Extensions.Configuration;
using XerShade.Website.Core.Components.Options.Services.Interfaces;
using XerShade.Website.Core.Framework.Factories.Population;

namespace XerShade.Website.Core.Components.Options.Factories;

public class OptionsPopulationFactory(IOptionsService service, IConfiguration configuration) : PopulationFactory
{
    private readonly IOptionsService service = service;
    private readonly IConfiguration configuration = configuration;

    public override void Populate()
    {
        this.PopulateOption("Core.Website.Name", "XerShade's Corner");
        this.PopulateOption("Core.Website.Description", "My little corner of the internet.");

        this.PopulateOption("Core.Authentication.RequiredLength", 8);
        this.PopulateOption("Core.Authentication.RequireNonAlphanumeric", false);
        this.PopulateOption("Core.Authentication.RequireDigit", true);
        this.PopulateOption("Core.Authentication.RequireLowercase", true);
        this.PopulateOption("Core.Authentication.RequireUppercase", true);

        base.Populate();
    }

    protected void PopulateOption<TValue>(string optionName, TValue optionValue)
    {
        bool result = this.service.Has(optionName, checkCache: false);

        if (!result)
        {
            this.service.Write(optionName, optionValue, true);
        }
    }

    protected void PopulateConfigOption<TValue>(string optionName)
    {
        string? optionValue = this.configuration[optionName] ?? throw new NullReferenceException();

        this.PopulateOption(optionName, optionValue);
    }

    public override Task PopulateAsync() => base.PopulateAsync();

    protected async Task PopulateOptionAsync<TValue>(string optionName, TValue optionValue)
    {
        bool result = await this.service.HasAsync(optionName, checkCache: false);

        if (!result)
        {
            await this.service.WriteAsync(optionName, optionValue, true);
        }
    }

    protected async Task PopulateConfigOptionAsync<TValue>(string optionName)
    {
        string? optionValue = this.configuration[optionName] ?? throw new NullReferenceException();

        await this.PopulateOptionAsync(optionName, optionValue);
    }
}
