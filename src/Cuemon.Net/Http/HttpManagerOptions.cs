using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Cuemon.Configuration;

namespace Cuemon.Net.Http;
/// <summary>
/// Specifies options that is related to the <see cref="HttpManager"/> class.
/// </summary>
/// <seealso cref="IValidatableParameterObject"/>
public class HttpManagerOptions : IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpManagerOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="HttpManagerOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="DisposeHandler"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="DefaultRequestHeaders"/></term>
    ///         <description>Connection: Keep-Alive</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="HandlerFactory"/></term>
    ///         <description><see cref="HttpClientHandler"/> initialized with <see cref="HttpClientHandler.AutomaticDecompression"/> for GZip|Deflate and <see cref="HttpClientHandler.MaxAutomaticRedirections"/> to 10.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Timeout"/></term>
    ///         <description>2 minutes</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public HttpManagerOptions()
    {
        HandlerFactory = () => new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            MaxAutomaticRedirections = 10
        };
        DisposeHandler = false;
        DefaultRequestHeaders = new Dictionary<string, string>()
        {
            { "Connection", "Keep-Alive" }
        };
        Timeout = TimeSpan.FromMinutes(2);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the inner handler should be disposed of by Dispose().
    /// </summary>
    /// <value><c>true</c> if if the inner handler should be disposed of by Dispose(); otherwise, <c>false</c> if you intend to reuse the inner handler.</value>
    public bool DisposeHandler { get; set; }

    /// <summary>
    /// Gets or sets the default headers which should be sent with each request.
    /// </summary>
    /// <value>The default headers which should be sent with each request.</value>
    public Dictionary<string, string> DefaultRequestHeaders { get; set; }

    /// <summary>
    /// Gets or sets the HTTP handler stack to use for sending requests.
    /// </summary>
    /// <value>The HTTP handler stack to use for sending requests.</value>
    public Func<HttpMessageHandler> HandlerFactory { get; set; }

    /// <summary>
    /// Gets or sets the timespan to wait before the request times out. Default is 2 minutes.
    /// </summary>
    /// <value>The timespan to wait before the request times out.</value>
    public TimeSpan Timeout { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="HandlerFactory"/> cannot be null - or -
    /// <see cref="DefaultRequestHeaders"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(HandlerFactory == null);
        Validator.ThrowIfInvalidState(DefaultRequestHeaders == null);
    }
}
