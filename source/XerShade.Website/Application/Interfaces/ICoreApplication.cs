using XerShade.Website.Core.Services.Interfaces;

namespace XerShade.Website.Application.Interfaces;

public interface ICoreApplication : IDisposable, IAsyncDisposable
{
    IOptionsService? Options { get; }
    ICoreApplication Build(string[] args);
}