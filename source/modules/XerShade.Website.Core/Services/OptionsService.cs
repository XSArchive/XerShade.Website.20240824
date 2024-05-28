using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using XerShade.Website.Core.Data;
using XerShade.Website.Core.Data.Models;
using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Core.Services;

public class OptionsService : IOptionsService
{
    private readonly ConcurrentDictionary<string, Option> OptionsCache;
    private readonly IDbContextFactory<GeneralDbContext> DbContextFactory;

    private static string NormalizeOptionName(string optionName) => optionName.ToLower();
    private RWDService<Option> CreateDbService() => new(this.DbContextFactory.CreateDbContext());

    public OptionsService(IDbContextFactory<GeneralDbContext> dbContextFactory)
    {
        this.DbContextFactory = dbContextFactory;
        List<Option> options = this.CreateDbService().ReadRange(o => o.AutoLoad) ?? [];
        OptionsCache = new ConcurrentDictionary<string, Option>(options.ToDictionary(opt => opt.OptionName.ToLower()));
    }

    public bool Has(string optionName, bool checkCache = true)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        if (checkCache && OptionsCache.ContainsKey(normalizedOptionName))
            return true;

        Option? dbOption = this.CreateDbService().Read(option => option.OptionName.ToLower().Equals(normalizedOptionName));
        return dbOption != null;
    }

    public TValue Read<TValue>(string optionName, TValue defaultValue)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        if (!OptionsCache.TryGetValue(normalizedOptionName, out Option? cacheOption))
        {
            cacheOption = this.CreateDbService().Read(option => option.OptionName.ToLower().Equals(normalizedOptionName));

            if (cacheOption == null)
            {
                cacheOption = new Option()
                {
                    OptionName = optionName,
                    OptionValue = JsonConvert.SerializeObject(defaultValue)
                };
                OptionsCache[normalizedOptionName] = cacheOption;
                return defaultValue;
            }

            OptionsCache[normalizedOptionName] = cacheOption;
        }

        return JsonConvert.DeserializeObject<TValue>(cacheOption.OptionValue) ?? defaultValue;
    }

    public void Write<TValue>(string optionName, TValue? value, bool autoLoad = false)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        Option cacheOption = OptionsCache.GetOrAdd(normalizedOptionName, _ => new Option() { OptionName = optionName });

        cacheOption.OptionValue = JsonConvert.SerializeObject(value);
        cacheOption.AutoLoad = (autoLoad != cacheOption.AutoLoad) ? autoLoad : cacheOption.AutoLoad;

        this.CreateDbService().Write(option => option.OptionName.ToLower().Equals(normalizedOptionName), option =>
        {
            option.OptionName = cacheOption.OptionName;
            option.OptionValue = cacheOption.OptionValue;
            option.AutoLoad = cacheOption.AutoLoad;
        });
    }

    public void Delete(string optionName)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        _ = OptionsCache.TryRemove(normalizedOptionName, out _);
        this.CreateDbService().Delete(option => option.OptionName.ToLower().Equals(normalizedOptionName));
    }

    public async Task<bool> HasAsync(string optionName, bool checkCache = true)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        return (checkCache && OptionsCache.ContainsKey(normalizedOptionName))
               || await this.CreateDbService().HasAsync(option => option.OptionName.ToLower().Equals(normalizedOptionName));
    }

    public async Task<TValue> ReadAsync<TValue>(string optionName, TValue defaultValue)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        if (!OptionsCache.TryGetValue(normalizedOptionName, out Option? cacheOption))
        {
            cacheOption = await this.CreateDbService().ReadAsync(option => option.OptionName.ToLower().Equals(normalizedOptionName));

            if (cacheOption == null)
            {
                cacheOption = new Option()
                {
                    OptionName = optionName,
                    OptionValue = JsonConvert.SerializeObject(defaultValue)
                };
                OptionsCache[normalizedOptionName] = cacheOption;
                return defaultValue;
            }

            OptionsCache[normalizedOptionName] = cacheOption;
        }

        return JsonConvert.DeserializeObject<TValue>(cacheOption.OptionValue) ?? defaultValue;
    }

    public async Task WriteAsync<TValue>(string optionName, TValue? value, bool autoLoad = false)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        Option cacheOption = OptionsCache.GetOrAdd(normalizedOptionName, _ => new Option() { OptionName = optionName });

        cacheOption.OptionValue = JsonConvert.SerializeObject(value);
        cacheOption.AutoLoad = (autoLoad != cacheOption.AutoLoad) ? autoLoad : cacheOption.AutoLoad;

        await this.CreateDbService().WriteAsync(option => option.OptionName.ToLower().Equals(normalizedOptionName), option =>
        {
            option.OptionName = cacheOption.OptionName;
            option.OptionValue = cacheOption.OptionValue;
            option.AutoLoad = cacheOption.AutoLoad;
        });
    }

    public async Task DeleteAsync(string optionName)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        _ = OptionsCache.TryRemove(normalizedOptionName, out _);
        await this.CreateDbService().DeleteAsync(option => option.OptionName.ToLower().Equals(normalizedOptionName));
    }
}
