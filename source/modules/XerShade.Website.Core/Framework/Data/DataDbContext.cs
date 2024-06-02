using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using XerShade.Website.Core.Components.Options.Models;

namespace XerShade.Website.Core.Framework.Data;

public class DataDbContext(IConfiguration configuration) : DbContext
{
    private readonly IConfiguration configuration = configuration;

    public DbSet<Option> Options { get; private set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            DbContextConfiguration contextConfiguration = new(configuration.GetSection("Data:DbContext"));

            string? connectionString = configuration["Core.Data.DbContext.ConnectionString"] ?? contextConfiguration.GenerateConnectionString();
            string? contextVersion = configuration["Core.Data.DbContext.Version"] ?? contextConfiguration.Version ?? string.Empty;

            _ = optionsBuilder.UseMySql(connectionString, ServerVersion.Create(new Version(contextVersion), ServerType.MariaDb));
        }
    }
}
