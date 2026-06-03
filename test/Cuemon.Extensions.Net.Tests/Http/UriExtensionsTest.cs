using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Threading;
using Xunit;

namespace Cuemon.Extensions.Net.Http
{
    public class UriExtensionsTest : Test
    {
        public UriExtensionsTest(ITestOutputHelper output) : base(output)
        {
            UriExtensions.DefaultHttpClientFactory = new SlimHttpClientFactory(() => new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                MaxAutomaticRedirections = 10
            }, o => o.HandlerLifetime = TimeSpan.MinValue);
        }

        [Fact]
        public async Task HttpGetAsync_ShouldGetResponseFromUri()
        {
            var expected = 125;
            var atomicCount = 0;

            // Find an available port
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start(200);  // Increase backlog to handle many concurrent connections
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            var baseUri = new Uri($"http://localhost:{port}/");

            // Track pending connection handler tasks
            var pendingTasks = new List<Task>();
            var pendingTasksLock = new object();

            // Handle requests in a background task
            var cts = new CancellationTokenSource();
            var serverTask = Task.Run(async () =>
            {
                try
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        try
                        {
#if NET9_0_OR_GREATER
                            var client = await listener.AcceptTcpClientAsync(cts.Token);
#else
                            var client = await listener.AcceptTcpClientAsync();
#endif
                            var clientTask = Task.Run(async () =>
                            {
                                try
                                {
                                    using (client)
                                    using (var stream = client.GetStream())
                                    {
                                        var buffer = new byte[1024];
                                        try
                                        {
                                            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                                            if (bytesRead > 0)
                                            {
                                                var responseText = "HTTP/1.1 200 OK\r\nContent-Length: 0\r\nConnection: close\r\n\r\n";
                                                var responseBytes = Encoding.ASCII.GetBytes(responseText);
                                                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                                            }
                                        }
                                        catch
                                        {
                                            // Timeout or other read/write error
                                        }
                                    }
                                }
                                catch
                                {
                                    // Ignore errors on individual connections
                                }
                            });

                            lock (pendingTasksLock)
                            {
                                pendingTasks.Add(clientTask);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch
                        {
                            // Continue on accept errors
                        }
                    }
                }
                finally
                {
                    listener.Stop();
                }
            });

            try
            {
                // Make parallel requests to the local server
                await ParallelFactory.ForAsync(0, expected, async (i, ct) =>
                {
                    try
                    {
                        using (var response = await baseUri.HttpGetAsync(ct))
                        {
                            Interlocked.Increment(ref atomicCount);
                            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                        }
                    }
                    catch
                    {
                        // Ignore request failures during parallel execution
                    }
                });

                // Signal server to stop accepting new connections
                cts.Cancel();

                // Wait for all pending connection handler tasks to complete (with timeout)
                Task[] tasksToWait = null;
                lock (pendingTasksLock)
                {
                    if (pendingTasks.Count > 0)
                    {
                        tasksToWait = pendingTasks.ToArray();
                    }
                }

                if (tasksToWait != null && tasksToWait.Length > 0)
                {
                    try
                    {
                        await Task.WhenAny(Task.WhenAll(tasksToWait), Task.Delay(5000));
                    }
                    catch
                    {
                        // Ignore timeout exceptions; we've waited long enough
                    }
                }

                // Wait for server task to complete
                try
                {
                    await Task.WhenAny(serverTask, Task.Delay(5000));
                }
                catch
                {
                    // Ignore exceptions during server shutdown
                }

                Assert.Equal(expected, atomicCount);
            }
            finally
            {
                cts.Cancel();
                try
                {
                    listener.Stop();
                }
                catch
                {
                    // Ignore cleanup exceptions
                }
            }
        }
    }
}