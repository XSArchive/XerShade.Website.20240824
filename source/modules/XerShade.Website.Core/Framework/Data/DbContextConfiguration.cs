using Microsoft.Extensions.Configuration;

namespace XerShade.Website.Core.Framework.Data;

public class DbContextConfiguration(IConfigurationSection configuration) : IDisposable
{
    private readonly IConfigurationSection configuration = configuration;

    public readonly string? ConnectionString = configuration.GetValue<string>("ConnectionString");
    public readonly string? Host = configuration.GetValue<string>("Host");
    public readonly int? Port = configuration.GetValue<int>("Port");
    public readonly string? Database = configuration.GetValue<string>("Database");
    public readonly string? Username = configuration.GetValue<string>("Username");
    public readonly string? Password = configuration.GetValue<string>("Password");
    public readonly string? Version = configuration.GetValue<string>("Version");

    public string? GenerateConnectionString() => !string.IsNullOrWhiteSpace(ConnectionString) ? ConnectionString :
            $"Server={Host};Port={Port};Database={Database};Uid={Username};Pwd={Password};";

    public void Dispose() => GC.SuppressFinalize(this);
}
