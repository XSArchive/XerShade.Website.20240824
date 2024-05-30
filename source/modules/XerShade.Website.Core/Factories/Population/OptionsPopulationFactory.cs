using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Core.Factories.Population;

public class OptionsPopulationFactory(IOptionsService service) : PopulationFactory
{
    private readonly IOptionsService service = service;

    public override async Task PopulateAsync()
    {
        await this.PopulateOption("Core.Website.Name", "XerShade's Corner");
        await this.PopulateOption("Core.Website.Description", "My little corner of the internet.");

        await this.PopulateOption("Core.Authentication.RequiredLength", 8);
        await this.PopulateOption("Core.Authentication.RequireNonAlphanumeric", false);
        await this.PopulateOption("Core.Authentication.RequireDigit", true);
        await this.PopulateOption("Core.Authentication.RequireLowercase", true);
        await this.PopulateOption("Core.Authentication.RequireUppercase", true);

        await base.PopulateAsync();
    }

    protected async Task PopulateOption<TValue>(string optionName, TValue optionValue)
    {
        bool result = await this.service.HasAsync(optionName, checkCache: false);

        if (!result)
        {
            await this.service.WriteAsync(optionName, optionValue, true);
        }
    }
}
