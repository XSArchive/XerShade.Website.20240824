using Newtonsoft.Json;
using XerShade.Website.Core.Data.Models;
using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Core.Services;

public class OptionsService : RWDService<Option>, IOptionsService
{
    private readonly List<Option> optionsCache;

    public OptionsService() : base() => this.optionsCache = this.ReadRange(o => o.AutoLoad) ?? [];

    public TValue? Read<TValue>(string optionName, TValue? defaultValue = default)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        Option? cacheOption = (from option in optionsCache
                               where option.OptionName.ToLower().Equals(optionName.ToLower())
                               select option as Option).FirstOrDefault();

        if (cacheOption is null)
        {
            Option? dbOption = this.Read(option => option.OptionName.ToLower().Equals(optionName.ToLower()));

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

        bool dbAutoLoad = this.Read(option => option.OptionName.ToLower().Equals(optionName.ToLower()))?.AutoLoad ?? false;
        Option? cacheOption = (from option in optionsCache
                               where option.OptionName.ToLower().Equals(optionName.ToLower())
                               select option as Option).FirstOrDefault() ?? new Option()
                               {
                                   OptionName = optionName,
                                   AutoLoad = dbAutoLoad
                               };

        cacheOption.OptionValue = JsonConvert.SerializeObject(value);

        this.Write(option => option.OptionName.ToLower().Equals(optionName.ToLower()), (option) =>
        {
            option.OptionName = cacheOption.OptionName;
            option.OptionValue = cacheOption.OptionValue;
            option.AutoLoad = cacheOption.AutoLoad;
        });
    }
    public void Write<TValue>(string optionName, TValue? value, bool autoLoad = false)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        Option? cacheOption = (from option in optionsCache
                               where option.OptionName.ToLower().Equals(optionName.ToLower())
                               select option as Option).FirstOrDefault() ?? new Option()
                               {
                                   OptionName = optionName
                               };

        cacheOption.OptionValue = JsonConvert.SerializeObject(value);
        cacheOption.AutoLoad = autoLoad;

        this.Write(option => option.OptionName.ToLower().Equals(optionName.ToLower()), (option) =>
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

        _ = this.optionsCache.RemoveAll(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
        this.Delete(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
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
            Option? dbOption = await this.ReadAsync(option => option.OptionName.ToLower().Equals(optionName.ToLower()));

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

        Option? dbOption = await this.ReadAsync(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
        bool dbAutoLoad = dbOption?.AutoLoad ?? false;

        Option? cacheOption = (from option in optionsCache
                               where option.OptionName.ToLower().Equals(optionName.ToLower())
                               select option as Option).FirstOrDefault() ?? new Option()
                               {
                                   OptionName = optionName,
                                   AutoLoad = dbAutoLoad
                               };

        cacheOption.OptionValue = JsonConvert.SerializeObject(value);

        await this.WriteAsync(option => option.OptionName.ToLower().Equals(optionName.ToLower()), (option) =>
        {
            option.OptionName = cacheOption.OptionName;
            option.OptionValue = cacheOption.OptionValue;
            option.AutoLoad = cacheOption.AutoLoad;
        });
    }
    public virtual async Task WriteAsync<TValue>(string optionName, TValue? value, bool autoLoad = false)
    {
        if (string.IsNullOrWhiteSpace(optionName))
        { throw new ArgumentNullException(nameof(optionName)); }

        Option? cacheOption = (from option in optionsCache
                               where option.OptionName.ToLower().Equals(optionName.ToLower())
                               select option as Option).FirstOrDefault() ?? new Option()
                               {
                                   OptionName = optionName
                               };

        cacheOption.OptionValue = JsonConvert.SerializeObject(value);
        cacheOption.AutoLoad = autoLoad;

        await this.WriteAsync(option => option.OptionName.ToLower().Equals(optionName.ToLower()), (option) =>
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

        _ = this.optionsCache.RemoveAll(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
        await this.DeleteAsync(option => option.OptionName.ToLower().Equals(optionName.ToLower()));
    }
}
