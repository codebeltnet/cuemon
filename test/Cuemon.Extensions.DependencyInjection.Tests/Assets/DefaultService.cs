namespace Cuemon.Extensions.DependencyInjection.Assets;
public class DefaultService : IService
{
    public DefaultService()
    {
        ServiceType = nameof(DefaultService);
    }

    public string ServiceType { get; protected set; }

    public override string ToString()
    {
        return ServiceType;
    }
}

public class DefaultService<TMarker> : DefaultService, IService<TMarker>
{
    public DefaultService()
    {
        ServiceType = $"{nameof(DefaultService)}:{typeof(TMarker).Name}";
    }
}
