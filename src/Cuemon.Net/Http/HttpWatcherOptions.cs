using System;
using System.Net;
using System.Net.Http;
using Cuemon.Configuration;
using Cuemon.Runtime;
using Cuemon.Security;

namespace Cuemon.Net.Http;
/// <summary>
/// Configuration options for <see cref="HttpWatcher"/>.
/// </summary>
public class HttpWatcherOptions : WatcherOptions, IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpWatcherOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="HttpWatcherOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="ClientFactory"/></term>
    ///         <description><code>
    /// () => new HttpClient(new HttpClientHandler()
    /// {
    ///    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
    ///    MaxAutomaticRedirections = 10
    ///  }, false)
    /// </code></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="HashFactory"/></term>
    ///         <description><c>() => new CyclicRedundancyCheck64()</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="ReadResponseBody"/></term>
    ///         <description><c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public HttpWatcherOptions()
    {
        HashFactory = () => new CyclicRedundancyCheck64();
        ClientFactory = () => new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            MaxAutomaticRedirections = 10
        }, false);
    }

    /// <summary>
    /// Gets or sets the function delegate that will resolve an instance of <see cref="HttpClient"/>.
    /// </summary>
    /// <value>The function delegate that will resolve an instance of <see cref="HttpClient"/>.</value>
    public Func<HttpClient> ClientFactory { get; set; }

    /// <summary>
    /// Gets or sets the function delegate that will resolve an implementation of <see cref="Hash"/>.
    /// </summary>
    /// <value>The function delegate that will resolve an implementation of <see cref="Hash"/>.</value>
    public Func<Hash> HashFactory { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to compute a hash from the response data of the <see cref="HttpWatcher"/>.
    /// </summary>
    /// <value><c>true</c> to compute a hash from the response data of the <see cref="HttpWatcher"/>; otherwise, <c>false</c>.</value>
    public bool ReadResponseBody { get; set; }

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="ClientFactory"/> cannot be null - or -
    /// <see cref="HashFactory"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(ClientFactory == null);
        Validator.ThrowIfInvalidState(HashFactory == null);
    }
}
