---
uid: Cuemon.Extensions.Net.Http.UriExtensions
example:
- *content
---

`UriExtensions` provides HTTP extension methods directly on `Uri` covering GET, POST, PUT, DELETE, HEAD, OPTIONS, PATCH, and TRACE with support for custom headers, content types, and cancellation. This example assigns a stub `IHttpClientFactory` that returns a fixed `200 OK` response, then calls each HTTP method variant including `HttpGetAsync`, `HttpDeleteAsync`, `HttpPostAsync` with JSON content, `HttpPutAsync` with a stream body, `HttpPatchAsync`, `HttpTraceAsync`, and the generic `HttpAsync` overload for full `HttpRequestOptions` control with custom headers. It also demonstrates passing a `CancellationToken` with a timeout that triggers a `TaskCanceledException`. Console output confirms that each request returns `200 OK` and that cancellation is handled gracefully.

```csharp
using System.Text;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Net.Http;

namespace MyApp.Http
{
    public class UriExtensionsExample
    {
        public async Task DemonstrateAsync()
        {
            // Configure a test HTTP client factory that returns predictable responses
            UriExtensions.DefaultHttpClientFactory = new StubHttpClientFactory(HttpStatusCode.OK);

            var location = new Uri("https://example.com/api/items");

            // GET request
            using (var response = await location.HttpGetAsync())
            {
                Console.WriteLine($"GET: {response.StatusCode}"); // 200
            }

            // DELETE request
            using (var response = await location.HttpDeleteAsync())
            {
                Console.WriteLine($"DELETE: {response.StatusCode}"); // 200
            }

            // HEAD request
            using (var response = await location.HttpHeadAsync())
            {
                Console.WriteLine($"HEAD: {response.StatusCode}"); // 200
            }

            // OPTIONS request
            using (var response = await location.HttpOptionsAsync())
            {
                Console.WriteLine($"OPTIONS: {response.StatusCode}"); // 200
            }

            // POST with string content type
            using (var response = await location.HttpPostAsync(
                "application/json", ToStream("{\"name\":\"New Item\"}")))
            {
                Console.WriteLine($"POST: {response.StatusCode}"); // 200
            }

            // POST with MediaTypeHeaderValue
            using (var response = await location.HttpPostAsync(
                MediaTypeHeaderValue.Parse("application/json; charset=utf-8"),
                ToStream("{\"name\":\"Another\"}")))
            {
                Console.WriteLine($"POST (typed): {response.StatusCode}"); // 200
            }

            // PUT
            using (var response = await location.HttpPutAsync(
                "application/json", ToStream("{\"name\":\"Updated\"}")))
            {
                Console.WriteLine($"PUT: {response.StatusCode}"); // 200
            }

            // PATCH
            using (var response = await location.HttpPatchAsync(
                "application/json-patch+json", ToStream("[{\"op\":\"replace\",\"path\":\"/name\",\"value\":\"Patched\"}]")))
            {
                Console.WriteLine($"PATCH: {response.StatusCode}"); // 200
            }

            // TRACE
            using (var response = await location.HttpTraceAsync())
            {
                Console.WriteLine($"TRACE: {response.StatusCode}"); // 200
            }

            // Generic HTTP method with string content type
            using (var response = await location.HttpAsync(
                HttpMethod.Post, "text/plain", ToStream("Hello")))
            {
                Console.WriteLine($"Generic POST: {response.StatusCode}"); // 200
            }

            // Generic HTTP method with typed content type
            using (var response = await location.HttpAsync(
                HttpMethod.Put,
                MediaTypeHeaderValue.Parse("application/octet-stream"),
                ToStream("binary")))
            {
                Console.WriteLine($"Generic PUT: {response.StatusCode}"); // 200
            }

            // Full control via HttpRequestOptions
            using (var response = await location.HttpAsync(o =>
            {
                o.Request.Method = HttpMethod.Get;
                o.Request.Headers.Add("X-Custom", "my-value");
                o.CancellationToken = CancellationToken.None;
            }))
            {
                Console.WriteLine($"Custom GET: {response.StatusCode}"); // 200
            }

            // Use cancellation token
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                using var response = await location.HttpGetAsync(cts.Token);
                Console.WriteLine($"Cancellable GET: {response.StatusCode}");
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Request was cancelled.");
            }
        }

        private static Stream ToStream(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            return new MemoryStream(bytes);
        }
    }

    /// <summary>
    /// A stub factory that returns an HttpClient backed by a handler
    /// that returns a fixed status code for all requests.
    /// </summary>
    internal class StubHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpStatusCode _statusCode;

        public StubHttpClientFactory(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }

        public HttpClient CreateClient(string name)
        {
            return new HttpClient(new StubHttpMessageHandler(_statusCode));
        }
    }

    internal class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;

        public StubHttpMessageHandler(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken ct)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent("OK"),
                RequestMessage = request
            };
            return Task.FromResult(response);
        }
    }
}

```
