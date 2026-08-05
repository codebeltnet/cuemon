using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Net.Http
{
    /// <summary>
    /// Provides ways for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class HttpManager : Disposable
    {
        private readonly Lazy<HttpClient> _httpClient;
        private const string HttpPatchVerb = "PATCH";

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpManager" /> class.
        /// </summary>
        /// <param name="setup">The <see cref="HttpManagerOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// The <see cref="HttpManagerOptions.HandlerFactory"/> of the <paramref name="setup"/> delegate cannot be null.
        /// </exception>
        public HttpManager(Action<HttpManagerOptions> setup = null) : this(() =>
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            var client = new HttpClient(options.HandlerFactory.Invoke(), options.DisposeHandler);
            foreach (var header in options.DefaultRequestHeaders)
            {
                if (client.DefaultRequestHeaders.Contains(header.Key)) { continue; }
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            client.Timeout = options.Timeout;
            return client;
        })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpManager"/> class.
        /// </summary>
        /// <param name="clientFactory">The function delegate that creates and configures an <see cref="HttpClient"/> instance.</param>
        public HttpManager(Func<HttpClient> clientFactory)
        {
            Validator.ThrowIfNull(clientFactory);
            _httpClient = new Lazy<HttpClient>(clientFactory.Invoke);
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Disposable.Dispose()" /> or <see cref="Disposable.Dispose(System.Boolean)" /> having <c>disposing</c> set to <c>true</c> and <see cref="Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            Client?.Dispose();
        }

        private HttpClient Client => _httpClient.Value;

        /// <summary>
        /// Gets the headers which should be sent with each request.
        /// </summary>
        /// <value>The headers which should be sent with each request.</value>
        public HttpRequestHeaders DefaultRequestHeaders => Client.DefaultRequestHeaders;

        /// <summary>
        /// Gets or sets the timespan to wait before the request times out.
        /// </summary>
        /// <value>The timespan to wait before the request times out.</value>
        public TimeSpan Timeout
        {
            get => Client.Timeout;
            set => Client.Timeout = value;
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpDeleteAsync(Uri location, CancellationToken ct = default)
        {
            return HttpAsync(location, o =>
            {
                o.Request.Method = HttpMethod.Delete;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpGetAsync(Uri location, CancellationToken ct = default)
        {
            return HttpAsync(location, o =>
            {
                o.Request.Method = HttpMethod.Get;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send a HEAD request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpHeadAsync(Uri location, CancellationToken ct = default)
        {
            return HttpAsync(location, o =>
            {
                o.Request.Method = HttpMethod.Head;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send an OPTIONS request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpOptionsAsync(Uri location, CancellationToken ct = default)
        {
            return HttpAsync(location, o =>
            {
                o.Request.Method = HttpMethod.Options;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send a TRACE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpTraceAsync(Uri location, CancellationToken ct = default)
        {
            return HttpAsync(location, o =>
            {
                o.Request.Method = HttpMethod.Trace;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contentType"/> cannot be null -or-
        /// <paramref name="content"/> cannot be null.
        /// </exception>
        public Task<HttpResponseMessage> HttpPostAsync(Uri location, string contentType, Stream content, CancellationToken ct = default)
        {
            return HttpAsync(HttpMethod.Post, location, contentType, content, ct);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contentType"/> cannot be null -or-
        /// <paramref name="content"/> cannot be null.
        /// </exception>
        public Task<HttpResponseMessage> HttpPostAsync(Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default)
        {
            return HttpAsync(HttpMethod.Post, location, contentType, content, ct);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contentType"/> cannot be null -or-
        /// <paramref name="content"/> cannot be null.
        /// </exception>
        public Task<HttpResponseMessage> HttpPutAsync(Uri location, string contentType, Stream content, CancellationToken ct = default)
        {
            return HttpAsync(HttpMethod.Put, location, contentType, content, ct);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contentType"/> cannot be null -or-
        /// <paramref name="content"/> cannot be null.
        /// </exception>
        public Task<HttpResponseMessage> HttpPutAsync(Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default)
        {
            return HttpAsync(HttpMethod.Put, location, contentType, content, ct);
        }

        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contentType"/> cannot be null -or-
        /// <paramref name="content"/> cannot be null.
        /// </exception>
        public Task<HttpResponseMessage> HttpPatchAsync(Uri location, string contentType, Stream content, CancellationToken ct = default)
        {
            return HttpAsync(new HttpMethod(HttpPatchVerb), location, contentType, content, ct);
        }

        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contentType"/> cannot be null -or-
        /// <paramref name="content"/> cannot be null.
        /// </exception>
        public Task<HttpResponseMessage> HttpPatchAsync(Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default)
        {
            return HttpAsync(new HttpMethod(HttpPatchVerb), location, contentType, content, ct);
        }

        /// <summary>
        /// Send a request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> cannot be null -or-
        /// <paramref name="location"/> cannot be null -or-
        /// <paramref name="contentType"/> cannot be null -or-
        /// <paramref name="content"/> cannot be null.
        /// </exception>
        public Task<HttpResponseMessage> HttpAsync(HttpMethod method, Uri location, string contentType, Stream content, CancellationToken ct = default)
        {
            Validator.ThrowIfNullOrEmpty(contentType);
            return HttpAsync(method, location, MediaTypeHeaderValue.Parse(contentType), content, ct);
        }

        /// <summary>
        /// Send a request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> cannot be null -or-
        /// <paramref name="location"/> cannot be null -or-
        /// <paramref name="contentType"/> cannot be null -or-
        /// <paramref name="content"/> cannot be null.
        /// </exception>
        public Task<HttpResponseMessage> HttpAsync(HttpMethod method, Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default)
        {
            Validator.ThrowIfNull(method);
            Validator.ThrowIfNull(contentType);
            Validator.ThrowIfNull(content);
            return HttpAsync(location, o =>
            {
                o.Request.Method = method;
                o.Request.Content = new StreamContent(content);
                o.Request.Content.Headers.ContentType = contentType;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send a request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="setup">The <see cref="HttpRequestOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="location"/> cannot be null -or-
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        public virtual Task<HttpResponseMessage> HttpAsync(Uri location, Action<HttpRequestOptions> setup)
        {
            Validator.ThrowIfNull(location);
            Validator.ThrowIfNull(setup);
            var options = Patterns.Configure(setup);
            options.Request.RequestUri = location;
            return Client.SendAsync(options.Request, options.CompletionOption, options.CancellationToken);
        }
    }
}