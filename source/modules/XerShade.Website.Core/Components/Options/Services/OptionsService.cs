using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using XerShade.Website.Core.Components.Options.Models;
using XerShade.Website.Core.Components.Options.Services.Interfaces;
using XerShade.Website.Core.Data;

namespace XerShade.Website.Core.Components.Options.Services;

public class OptionsService : DbStaticService<Option>, IOptionsService
{
    private readonly ConcurrentDictionary<string, Option> OptionsCache;

    private static string NormalizeOptionName(string optionName) => optionName.ToLower();

    public OptionsService(IDbContextFactory<GeneralDbContext> dbContextFactory) : base(dbContextFactory)
    {
        List<Option> options = base.ReadRange(o => o.AutoLoad) ?? [];
        OptionsCache = new ConcurrentDictionary<string, Option>(options.ToDictionary(opt => opt.OptionName.ToLower()));
    }

    public bool Has(string optionName, bool checkCache = true)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        return (checkCache && OptionsCache.ContainsKey(normalizedOptionName))
               || base.Has(option => option.OptionName.ToLower().Equals(normalizedOptionName));
    }

    public TValue Read<TValue>(string optionName, TValue defaultValue)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        if (!OptionsCache.TryGetValue(normalizedOptionName, out Option? cacheOption))
        {
            cacheOption = base.Read(option => option.OptionName.ToLower().Equals(normalizedOptionName));

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
        cacheOption.AutoLoad = autoLoad != cacheOption.AutoLoad ? autoLoad : cacheOption.AutoLoad;

        base.Write(option => option.OptionName.ToLower().Equals(normalizedOptionName), option =>
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
        base.Delete(option => option.OptionName.ToLower().Equals(normalizedOptionName));
    }

    public async Task<bool> HasAsync(string optionName, bool checkCache = true)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        return (checkCache && OptionsCache.ContainsKey(normalizedOptionName))
               || await base.HasAsync(option => option.OptionName.ToLower().Equals(normalizedOptionName));
    }

    public async Task<TValue> ReadAsync<TValue>(string optionName, TValue defaultValue)
    {
        if (string.IsNullOrWhiteSpace(optionName))
            throw new ArgumentNullException(nameof(optionName));

        string normalizedOptionName = NormalizeOptionName(optionName);

        if (!OptionsCache.TryGetValue(normalizedOptionName, out Option? cacheOption))
        {
            cacheOption = await base.ReadAsync(option => option.OptionName.ToLower().Equals(normalizedOptionName));

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
        cacheOption.AutoLoad = autoLoad != cacheOption.AutoLoad ? autoLoad : cacheOption.AutoLoad;

        await base.WriteAsync(option => option.OptionName.ToLower().Equals(normalizedOptionName), option =>
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
        await base.DeleteAsync(option => option.OptionName.ToLower().Equals(normalizedOptionName));
    }
}
