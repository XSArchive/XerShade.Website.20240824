namespace XerShade.Website.Core.Services.Interfaces;

public interface IOptionsService
{
    TValue? Read<TValue>(string optionName, TValue? defaultValue = default);
    void Write<TValue>(string optionName, TValue? value);
    void Write<TValue>(string optionName, TValue? value, bool autoLoad = false);
    void Delete(string optionName);

    Task<TValue?> ReadAsync<TValue>(string optionName, TValue? defaultValue = default);
    Task WriteAsync<TValue>(string optionName, TValue? value);
    Task WriteAsync<TValue>(string optionName, TValue? value, bool autoLoad = false);
    Task DeleteAsync(string optionName);
}
