using XerShade.Website.Core.Data.Models;

namespace XerShade.Website.Core.Services.Interfaces;

public interface IOptionsService
{
    bool Has(string optionName, bool checkCache = true);
    TValue? Read<TValue>(string optionName, TValue? defaultValue = default);
    void Write<TValue>(string optionName, TValue? value);
    void Write<TValue>(string optionName, TValue? value, bool autoLoad = false);
    void Delete(string optionName);

    Task<bool> HasAsync(string optionName, bool checkCache = true);
    Task<TValue?> ReadAsync<TValue>(string optionName, TValue? defaultValue = default);
    Task WriteAsync<TValue>(string optionName, TValue? value);
    Task WriteAsync<TValue>(string optionName, TValue? value, bool autoLoad = false);
    Task DeleteAsync(string optionName);
}
