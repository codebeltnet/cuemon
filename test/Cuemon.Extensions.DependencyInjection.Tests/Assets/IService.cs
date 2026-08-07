namespace Cuemon.Extensions.DependencyInjection.Assets;
public interface IService
{
    string ServiceType { get; }
}

public interface IService<TMarker> : IService, IDependencyInjectionMarker<TMarker>
{
}
