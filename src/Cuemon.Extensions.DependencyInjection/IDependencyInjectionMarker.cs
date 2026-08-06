namespace Cuemon.Extensions.DependencyInjection;
/// <summary>
/// Defines a generic way to support multiple implementations of a given service for Microsoft Dependency Injection.
/// </summary>
/// <typeparam name="TMarker">The type used to uniquely mark the implementation that this service represents.</typeparam>
/// <remarks>Inspiration gathered from: https://blog.rsuter.com/dotnet-dependency-injection-way-to-work-around-missing-named-registrations/</remarks>
public interface IDependencyInjectionMarker<TMarker>
{
}
