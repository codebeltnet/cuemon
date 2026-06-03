using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Runtime;
using Cuemon.Security;

namespace Cuemon.Net.Http
{
    /// <summary>
    /// Provides a watcher implementation designed to monitor and signal changes applied to an URI resource by raising the <see cref="Watcher.Changed"/> event.
    /// </summary>
    /// <seealso cref="Watcher" />
    public class HttpWatcher : Watcher
    {
        private readonly SemaphoreSlim _asyncLocker = new(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWatcher"/> class.
        /// </summary>
        /// <param name="location">The URI to monitor.</param>
        /// <param name="setup">The <see cref="HttpWatcherOptions" /> which may be configured.</param>
        public HttpWatcher(Uri location, Action<HttpWatcherOptions> setup = null) : base(Patterns.ConfigureExchange<HttpWatcherOptions, WatcherOptions>(setup))
        {
            Validator.ThrowIfNull(location);
            var options = Patterns.Configure(setup);
            Location = location;
            ClientFactory = options.ClientFactory;
            HashFactory = options.HashFactory;
            ReadResponseBody = options.ReadResponseBody;
            Checksum = null;
            EntityTag = null;
        }

        /// <summary>
        /// Gets the URI of the resource to watch.
        /// </summary>
        /// <value>The URI to monitor.</value>
        public Uri Location { get; }

        /// <summary>
        /// Gets the function delegate that will resolve an instance of <see cref="HttpClient"/>.
        /// </summary>
        /// <value>The function delegate that will resolve an instance of <see cref="HttpClient"/>.</value>
        public Func<HttpClient> ClientFactory { get; }

        /// <summary>
        /// Gets the function delegate that will resolve an implementation of <see cref="Hash"/>.
        /// </summary>
        /// <value>The function delegate that will resolve an implementation of <see cref="Hash"/>.</value>
        public Func<Hash> HashFactory { get; }

        /// <summary>
        /// Gets a value indicating whether to compute a hash from the response body.
        /// </summary>
        /// <value><c>true</c> to compute a hash from the response body; otherwise, <c>false</c>.</value>
        public bool ReadResponseBody { get; }

        /// <summary>
        /// Gets the checksum that is associated with the URI specified in <see cref="Location"/>.
        /// </summary>
        /// <value>The checksum that is associated with the URI specified in <see cref="Location"/>.</value>
        /// <remarks>If <see cref="ReadResponseBody"/> is <c>false</c> this property will remain <c>null</c>.</remarks>
        public string Checksum { get; private set; }

        private string EntityTag { get; set; }

        /// <summary>
        /// Handles the signaling of this <see cref="HttpWatcher"/>.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        protected override async Task HandleSignalingAsync()
        {
            await _asyncLocker.WaitAsync();
            try
            {
                var listenerHeader = $"Cuemon.Net.Http.HttpWatcher; Interval={Period.TotalSeconds} seconds";
                using (var manager = new HttpManager(ClientFactory))
                {
                    manager.DefaultRequestHeaders.Add("Listener-Object", listenerHeader);
                    if (ReadResponseBody)
                    {
                        await FetchUsingHttpGetAsync(manager).ConfigureAwait(false);
                    }
                    else
                    {
                        await FetchUsingHttpHeadAsync(manager).ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                _asyncLocker.Release();
            }
        }

        private async Task FetchUsingHttpGetAsync(HttpManager manager)
        {
            using (var response = await manager.HttpGetAsync(Location).ConfigureAwait(false))
            {
                var currentChecksum = HashFactory().ComputeHash(await response.Content.ReadAsStreamAsync().ConfigureAwait(false)).ToHexadecimalString();

                Checksum ??= currentChecksum;
                if (!Checksum.Equals(currentChecksum, StringComparison.OrdinalIgnoreCase))
                {
                    SetUtcLastModified(DateTime.UtcNow);
                    OnChangedRaised();
                }
                Checksum = currentChecksum;
            }
        }

        private async Task FetchUsingHttpHeadAsync(HttpManager manager)
        {
            using (var response = await manager.HttpHeadAsync(Location))
            {
                var utcLastModified = response.Content.Headers.LastModified?.UtcDateTime;
                var etag = response.Headers.ETag;
                var hasUtcLastModified = utcLastModified.HasValue;
                var hasEntityTag = !string.IsNullOrEmpty(etag?.Tag);
                var invalidState = !hasUtcLastModified && !hasEntityTag;
                if (invalidState) { throw new InvalidOperationException("Neither Last-Modified or ETag header was available doing the request. Unable to proceed."); }
                if (hasUtcLastModified)
                {
                    SetUtcLastModified(utcLastModified.Value);
                    OnChangedRaised();
                }
                else
                {
                    var currentEntityTag = etag.Tag;
                    EntityTag ??= currentEntityTag;
                    if (!EntityTag.Equals(currentEntityTag, StringComparison.OrdinalIgnoreCase))
                    {
                        SetUtcLastModified(DateTime.UtcNow);
                        OnChangedRaised();
                    }
                    EntityTag = currentEntityTag;
                }
            }
        }
    }
}