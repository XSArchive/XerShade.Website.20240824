using Microsoft.EntityFrameworkCore;
using XerShade.Website.Application.Interfaces;
using XerShade.Website.Core.Data;
using XerShade.Website.Core.Factories.Population;
using XerShade.Website.Core.Factories.Population.Interfaces;
using XerShade.Website.Core.Services;
using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Application;

public class CoreApplication : ICoreApplication
{
    public IOptionsService? Options { get; private set; }

    protected virtual void MigrateDatabases(IServiceProvider services) => services.GetRequiredService<GeneralDbContext>().Database.Migrate();
    protected virtual void PopulateServices(IServiceProvider services) => _ = services.GetRequiredService<IOptionsPopulationFactory>().Populate();
    protected virtual void AssignServices(IServiceProvider services) => this.Options = services.GetRequiredService<IOptionsService>();

    public virtual ICoreApplication Build(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((builder, services) =>
        {
            _ = services.AddDbContext<GeneralDbContext>();

            _ = services.AddSingleton<IOptionsService, OptionsService>();
            _ = services.AddTransient<IOptionsPopulationFactory, OptionsPopulationFactory>();
        })
        .Build();

        this.MigrateDatabases(host.Services);
        this.PopulateServices(host.Services);
        this.AssignServices(host.Services);

        return this;
    }

    public void Dispose()
    {
        this.Options = null;

        GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
        this.Options = null;

        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}