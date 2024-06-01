namespace XerShade.Website.Core.Components.Options.Services.Interfaces;

public interface IOptionsService
{
    bool Has(string optionName, bool checkCache = true);
    TValue Read<TValue>(string optionName, TValue defaultValue);
    void Write<TValue>(string optionName, TValue value, bool autoLoad = false);
    void Delete(string optionName);

    Task<bool> HasAsync(string optionName, bool checkCache = true);
    Task<TValue> ReadAsync<TValue>(string optionName, TValue defaultValue);
    Task WriteAsync<TValue>(string optionName, TValue value, bool autoLoad = false);
    Task DeleteAsync(string optionName);
}
