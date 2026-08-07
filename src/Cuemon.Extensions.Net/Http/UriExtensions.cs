using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using HttpRequestOptions = Cuemon.Net.Http.HttpRequestOptions;

namespace Cuemon.Extensions.Net.Http;
/// <summary>
/// Extension methods for the <see cref="Uri"/> struct.
/// </summary>
public static class UriExtensions
{
    private static readonly string HandlerName = $"{nameof(UriExtensions)}.{nameof(DefaultHttpClientFactory)}";

    private static IHttpClientFactory _defaultHttpClientFactory = new SlimHttpClientFactory(() => new HttpClientHandler()
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        MaxAutomaticRedirections = 10
    });

    /// <summary>
    /// Gets or sets the default <see cref="IHttpClientFactory"/> implementation for the extensions methods on this class.
    /// </summary>
    /// <value>The default <see cref="IHttpClientFactory"/> implementation for the URI extensions methods on this class.</value>
    public static IHttpClientFactory DefaultHttpClientFactory
    {
        get => _defaultHttpClientFactory;
        set
        {
            if (value == null) { return; }
            _defaultHttpClientFactory = value;
        }
    }

    /// <summary>
    /// Send a DELETE request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpDeleteAsync(this Uri location, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpDeleteAsync(location, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a GET request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpGetAsync(this Uri location, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpGetAsync(location, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a HEAD request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpHeadAsync(this Uri location, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpHeadAsync(location, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send an OPTIONS request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpOptionsAsync(this Uri location, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpOptionsAsync(location, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a POST request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpPostAsync(this Uri location, string contentType, Stream content, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpPostAsync(location, contentType, content, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a POST request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpPostAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpPostAsync(location, contentType, content, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a PUT request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpPutAsync(this Uri location, string contentType, Stream content, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpPutAsync(location, contentType, content, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a PUT request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpPutAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpPutAsync(location, contentType, content, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a PATCH request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpPatchAsync(this Uri location, string contentType, Stream content, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpPatchAsync(location, contentType, content, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a PATCH request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpPatchAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default)
    {

        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpPatchAsync(location, contentType, content, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a TRACE request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpTraceAsync(this Uri location, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpTraceAsync(location, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a request as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="method">The HTTP method.</param>
    /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null -or-
    /// <paramref name="method"/> cannot be null -or-
    /// <paramref name="contentType"/> cannot be null -or-
    /// <paramref name="content"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpAsync(this Uri location, HttpMethod method, string contentType, Stream content, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpAsync(method, location, contentType, content, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a request as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="method">The HTTP method.</param>
    /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="ct">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null -or-
    /// <paramref name="method"/> cannot be null -or-
    /// <paramref name="contentType"/> cannot be null -or-
    /// <paramref name="content"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpAsync(this Uri location, HttpMethod method, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpAsync(method, location, contentType, content, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a request as an asynchronous operation.
    /// </summary>
    /// <param name="location">The <see cref="Uri"/> to extend.</param>
    /// <param name="setup">The <see cref="HttpRequestOptions"/> which need to be configured.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="location"/> cannot be null.
    /// </exception>
    public static async Task<HttpResponseMessage> HttpAsync(this Uri location, Action<HttpRequestOptions> setup)
    {
        return await HttpManagerFactory.CreateManager(DefaultHttpClientFactory, HandlerName).HttpAsync(location, setup).ConfigureAwait(false);
    }
}
