namespace XerShade.Website.Application.Interfaces;

public interface IMvcApplication : IDisposable, IAsyncDisposable
{
    IMvcApplication Build(string[] args);
}
