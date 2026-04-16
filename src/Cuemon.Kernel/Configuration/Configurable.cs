using System;

namespace Cuemon.Configuration;

/// <summary>
/// Provides a generic way to support the options pattern on a class level.
/// </summary>
/// <typeparam name="TOptions">The type of the configured options.</typeparam>
/// <seealso cref="IConfigurable{TOptions}" />
public abstract class Configurable<TOptions> : IConfigurable<TOptions> where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Configurable{TOptions}"/> class.
    /// </summary>
    /// <param name="options">The configured options of this instance.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="options"/> are not in a valid state.
    /// </exception>
    protected Configurable(TOptions options)
    {
        Validator.ThrowIfInvalidOptions(options);
        Options = options;
    }

    /// <summary>
    /// Gets the configured options of this instance.
    /// </summary>
    /// <value>The configured options of this instance.</value>
    public TOptions Options { get; }
}
