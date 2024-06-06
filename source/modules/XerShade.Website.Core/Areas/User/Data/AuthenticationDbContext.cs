using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using XerShade.Website.Core.Areas.User.Data.Models;
using XerShade.Website.Core.Framework.Data;

namespace XerShade.Website.Core.Areas.User.Data;

public class AuthenticationDbContext(IConfiguration configuration) : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly IConfiguration configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            DbContextConfiguration contextConfiguration = new(this.configuration.GetSection("Authentication:DbContext"));

            string? connectionString = this.configuration["Core.Authentication.DbContext.ConnectionString"] ?? contextConfiguration.GenerateConnectionString();
            string? contextVersion = this.configuration["Core.Authentication.DbContext.Version"] ?? contextConfiguration.Version ?? string.Empty;

            _ = optionsBuilder.UseMySql(connectionString, ServerVersion.Create(new Version(contextVersion), ServerType.MariaDb));
        }
    }
}
