using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using XerShade.Website.Core.Data.Models;

namespace XerShade.Website.Core.Data;

public class GeneralDbContext : DbContext
{
    private readonly string connectionString;

    public DbSet<Option> Options { get; private set; }

    public GeneralDbContext()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddUserSecrets<GeneralDbContext>()
            .AddEnvironmentVariables();

        IConfiguration configuration = builder.Build();

        this.connectionString = configuration["XS_CONNECTION_STRING_GENERAL_DBCONTEXT"] ??
            $"Server=localhost;Port=3306;Database=DEFAULT_DATABASE;Uid=DEFAULT_USERNAME;Pwd=DEFAULT_PASSWORD;";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            _ = optionsBuilder.UseMySql(this.connectionString, ServerVersion.Create(new Version("11.1.1"), ServerType.MariaDb));
        }
    }
}
