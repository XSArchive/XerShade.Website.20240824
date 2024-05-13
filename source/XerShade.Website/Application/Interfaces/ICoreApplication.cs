namespace XerShade.Website.Application.Interfaces;

public interface ICoreApplication : IDisposable, IAsyncDisposable
{
    ICoreApplication Build(string[] args);
}