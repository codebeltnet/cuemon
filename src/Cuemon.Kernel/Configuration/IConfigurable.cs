namespace Cuemon.Configuration;

/// <summary>
/// Provides a generic way to support the options pattern on a class level.
/// </summary>
/// <typeparam name="TOptions">The type of the configured options.</typeparam>
public interface IConfigurable<out TOptions> where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Gets the configured options of this instance.
    /// </summary>
    /// <value>The configured options of this instance.</value>
    TOptions Options { get; }
}
