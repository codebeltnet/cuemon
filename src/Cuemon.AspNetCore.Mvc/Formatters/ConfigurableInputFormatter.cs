using Cuemon.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Cuemon.AspNetCore.Mvc.Formatters;
/// <summary>
/// Provides an alternate way to read an object from a request body with a text format.
/// </summary>
/// <typeparam name="TOptions">The type of the configured options.</typeparam>
/// <seealso cref="TextInputFormatter" />
/// <seealso cref="IConfigurable{TOptions}" />
public abstract class ConfigurableInputFormatter<TOptions> : TextInputFormatter, IConfigurable<TOptions> where TOptions : class, IParameterObject, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableInputFormatter{TOptions}"/> class.
    /// </summary>
    /// <param name="options">The <typeparamref name="TOptions"/> which need to be configured.</param>
    protected ConfigurableInputFormatter(TOptions options)
    {
        Options = options;
    }

    /// <summary>
    /// Gets the configured options of this instance.
    /// </summary>
    /// <value>The configured options of this instance.</value>
    public TOptions Options { get; }
}
