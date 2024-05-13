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
    public virtual ICoreApplication Build(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((builder, services) =>
        {
            _ = services.AddDbContext<GeneralDbContext>();

            _ = services.AddSingleton<IOptionsService, OptionsService>();
            _ = services.AddTransient<IOptionsPopulationFactory, OptionsPopulationFactory>();
            _ = services.AddTransient<IMvcApplication, MvcApplication>();
        })
        .Build();

        this.MigrateDatabases(host.Services);
        this.PopulateServices(host.Services);
        this.RunApplication(host.Services, args);

        return this;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        return;
    }

    public virtual ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    protected virtual void MigrateDatabases(IServiceProvider services) => services.GetRequiredService<GeneralDbContext>().Database.Migrate();
    protected virtual void PopulateServices(IServiceProvider services) => _ = services.GetRequiredService<IOptionsPopulationFactory>().Populate();
    protected virtual void RunApplication(IServiceProvider services, string[] args) => _ = services.GetRequiredService<IMvcApplication>().Build(args);
}