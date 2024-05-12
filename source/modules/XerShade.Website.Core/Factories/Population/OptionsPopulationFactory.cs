using XerShade.Website.Core.Factories.Population.Interfaces;
using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Core.Factories.Population;

public class OptionsPopulationFactory(IOptionsService service) : AsyncPopulationFactory<IOptionsService>(service), IOptionsPopulationFactory
{
    public override async Task Populate()
    {
        await this.PopulateOption("Core.Website.Name", "XerShade's Corner");
        await this.PopulateOption("Core.Website.Description", "My little corner of the internet.");
    }

    protected async Task PopulateOption<TValue>(string optionName, TValue optionValue) where TValue : class
    {
        bool result = await this.service.HasAsync(optionName, checkCache: false);

        if (!result)
        {
            await this.service.WriteAsync(optionName, optionValue, true);
        }
    }
}
