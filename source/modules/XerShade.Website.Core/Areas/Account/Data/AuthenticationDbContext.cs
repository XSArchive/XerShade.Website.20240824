using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using XerShade.Website.Core.Areas.Account.Data.Models;

namespace XerShade.Website.Core.Areas.Account.Data;

public class AuthenticationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly string connectionString;

    public AuthenticationDbContext()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddUserSecrets<AuthenticationDbContext>()
            .AddEnvironmentVariables();

        IConfiguration configuration = builder.Build();

        this.connectionString = configuration["XS_CONNECTION_STRING_AUTHENTICATION_DBCONTEXT"] ?? 
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
