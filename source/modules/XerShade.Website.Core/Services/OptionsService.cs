using Newtonsoft.Json;
using XerShade.Website.Core.Data.Models;
using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Core.Services;

public class OptionsService : IOptionsService
{
    private readonly List<Option> optionsCache;

    public OptionsService()
    {
        using RWDService<Option> dbService = new(new());

        this.optionsCache = dbService.ReadRange(o => o.AutoLoad) ?? [];
    }

    public Dictionary<string, object?> ToDictionary() => this.optionsCache.ToDictionary(option => option.OptionName, option => JsonConvert.DeserializeObject(option.OptionValue));

    public bool Has(string optionName, bool checkCache = true)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        if (checkCache)
        {
            return this.optionsCache.Any(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
        }

        using RWDService<Option> dbService = new(new());

        Option? dbOption = dbService.Read(option => option.OptionName.ToLower().Equals(optionName.ToLower()));

        return dbOption is not null;
    }
    public TValue? Read<TValue>(string optionName, TValue? defaultValue = default)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        Option? cacheOption = (from option in optionsCache
                               where option.OptionName.ToLower().Equals(optionName.ToLower())
                               select option as Option).FirstOrDefault();

        if (cacheOption is null)
        {
            using RWDService<Option> dbService = new(new());

            Option? dbOption = dbService.Read(option => option.OptionName.ToLower().Equals(optionName.ToLower()));

            if (dbOption is null)
            {
                this.optionsCache.Add(new Option()
                {
                    OptionName = optionName,
                    OptionValue = JsonConvert.SerializeObject(defaultValue)
                });

                return defaultValue;
            }

            this.optionsCache.Add(dbOption);

            return JsonConvert.DeserializeObject<TValue>(dbOption.OptionValue);
        }

        return JsonConvert.DeserializeObject<TValue>(cacheOption.OptionValue);
    }
    public void Write<TValue>(string optionName, TValue? value)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        Option? cacheOption = (from option in optionsCache
                               where option.OptionName.ToLower().Equals(optionName.ToLower())
                               select option as Option).FirstOrDefault();

        if (cacheOption is null)
        {
            cacheOption = new Option()
            {
                OptionName = optionName
            };

            this.optionsCache.Add(cacheOption);
        }

        cacheOption.OptionValue = JsonConvert.SerializeObject(value);

        using RWDService<Option> dbService = new(new());

        dbService.Write(option => option.OptionName.ToLower().Equals(optionName.ToLower()), (option) =>
        {
            option.OptionName = cacheOption.OptionName;
            option.OptionValue = cacheOption.OptionValue;
        });
    }
    public void Write<TValue>(string optionName, TValue? value, bool autoLoad = false)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        Option? cacheOption = (from option in optionsCache
                               where option.OptionName.ToLower().Equals(optionName.ToLower())
                               select option as Option).FirstOrDefault();

        if (cacheOption is null)
        {
            cacheOption = new Option()
            {
                OptionName = optionName
            };

            this.optionsCache.Add(cacheOption);
        }

        cacheOption.OptionValue = JsonConvert.SerializeObject(value);
        cacheOption.AutoLoad = autoLoad;

        using RWDService<Option> dbService = new(new());

        dbService.Write(option => option.OptionName.ToLower().Equals(optionName.ToLower()), (option) =>
        {
            option.OptionName = cacheOption.OptionName;
            option.OptionValue = cacheOption.OptionValue;
            option.AutoLoad = cacheOption.AutoLoad;
        });
    }
    public void Delete(string optionName)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        using RWDService<Option> dbService = new(new());

        _ = this.optionsCache.RemoveAll(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
        dbService.Delete(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
    }
    public virtual async Task<bool> HasAsync(string optionName, bool checkCache = true)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        if (checkCache)
        {
            return this.optionsCache.Any(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
        }

        using RWDService<Option> dbService = new(new());

        return await dbService.HasAsync(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
    }
    public virtual async Task<TValue?> ReadAsync<TValue>(string optionName, TValue? defaultValue = default)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        Option? cacheOption = (from option in optionsCache
                               where option.OptionName.ToLower().Equals(optionName.ToLower())
                               select option as Option).FirstOrDefault();

        if (cacheOption is null)
        {
            using RWDService<Option> dbService = new(new());

            Option? dbOption = await dbService.ReadAsync(option => option.OptionName.ToLower().Equals(optionName.ToLower()));

            if (dbOption is null)
            {
                this.optionsCache.Add(new Option()
                {
                    OptionName = optionName,
                    OptionValue = JsonConvert.SerializeObject(defaultValue)
                });

                return defaultValue;
            }

            this.optionsCache.Add(dbOption);

            return JsonConvert.DeserializeObject<TValue>(dbOption.OptionValue);
        }

        return JsonConvert.DeserializeObject<TValue>(cacheOption.OptionValue);
    }
    public virtual async Task WriteAsync<TValue>(string optionName, TValue? value)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        Option? cacheOption = (from option in optionsCache
                               where option.OptionName.ToLower().Equals(optionName.ToLower())
                               select option as Option).FirstOrDefault();

        if (cacheOption is null)
        {
            cacheOption = new Option()
            {
                OptionName = optionName
            };

            this.optionsCache.Add(cacheOption);
        }

        cacheOption.OptionValue = JsonConvert.SerializeObject(value);

        using RWDService<Option> dbService = new(new());

        await dbService.WriteAsync(option => option.OptionName.ToLower().Equals(optionName.ToLower()), (option) =>
        {
            option.OptionName = cacheOption.OptionName;
            option.OptionValue = cacheOption.OptionValue;
        });
    }
    public virtual async Task WriteAsync<TValue>(string optionName, TValue? value, bool autoLoad = false)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        Option? cacheOption = (from option in optionsCache
                               where option.OptionName.ToLower().Equals(optionName.ToLower())
                               select option as Option).FirstOrDefault();

        if (cacheOption is null)
        {
            cacheOption = new Option()
            {
                OptionName = optionName
            };

            this.optionsCache.Add(cacheOption);
        }

        cacheOption.OptionValue = JsonConvert.SerializeObject(value);
        cacheOption.AutoLoad = autoLoad;

        using RWDService<Option> dbService = new(new());

        await dbService.WriteAsync(option => option.OptionName.ToLower().Equals(optionName.ToLower()), (option) =>
        {
            option.OptionName = cacheOption.OptionName;
            option.OptionValue = cacheOption.OptionValue;
            option.AutoLoad = cacheOption.AutoLoad;
        });
    }
    public virtual async Task DeleteAsync(string optionName)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        using RWDService<Option> dbService = new(new());

        await dbService.DeleteAsync(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
        _ = this.optionsCache.RemoveAll(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
    }
}
