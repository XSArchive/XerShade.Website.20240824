using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Core.Factories.Population;

public class OptionsPopulationFactory(IOptionsService service) : PopulationFactory
{
    private readonly IOptionsService service = service;

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

    public override Task PopulateAsync() => base.PopulateAsync();

    protected async Task PopulateOptionAsync<TValue>(string optionName, TValue optionValue)
    {
        bool result = await this.service.HasAsync(optionName, checkCache: false);

        if (!result)
        {
            await this.service.WriteAsync(optionName, optionValue, true);
        }
    }
}
