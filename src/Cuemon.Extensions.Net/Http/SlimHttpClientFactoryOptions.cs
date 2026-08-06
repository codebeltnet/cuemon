using System;
using System.Net.Http;
using Cuemon.Configuration;

namespace Cuemon.Extensions.Net.Http;
/// <summary>
/// Configuration options for <see cref="SlimHttpClientFactory"/>.
/// </summary>
/// <seealso cref="IParameterObject"/>
public class SlimHttpClientFactoryOptions : IParameterObject
{
    private TimeSpan _handlerLifetime;

    /// <summary>
    /// Initializes a new instance of the <see cref="SlimHttpClientFactoryOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="SlimHttpClientFactoryOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="HandlerLifetime"/></term>
    ///         <description><c>TimeSpan.FromMinutes(5);</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public SlimHttpClientFactoryOptions()
    {
        HandlerLifetime = TimeSpan.FromMinutes(5);
    }

    /// <summary>
    /// Gets or sets the lifetime of the <see cref="HttpMessageHandler"/>.
    /// </summary>
    /// <value>The lifetime of the <see cref="HttpMessageHandler"/>.</value>
    public TimeSpan HandlerLifetime
    {
        get => _handlerLifetime;
        set
        {
            if (value < SlimHttpClientFactory.ExpirationTimerDueTime) { value = SlimHttpClientFactory.ExpirationTimerDueTime; }
            _handlerLifetime = value;
        }
    }
}
