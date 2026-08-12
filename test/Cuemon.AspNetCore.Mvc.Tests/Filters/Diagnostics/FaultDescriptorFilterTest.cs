using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
public class FaultDescriptorFilterTest : Test
{
    public FaultDescriptorFilterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task OnException_ShouldIncludeFailure_DifferentiateOnUseBaseException(bool useBaseException)
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services
                .AddControllers(o => { o.Filters.AddFaultDescriptor(); })
                .AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters()
                .AddFaultDescriptorOptions(o => o.UseBaseException = useBaseException);
            services.PostConfigureAllExceptionDescriptorOptions(o => o.SensitivityDetails = FaultSensitivityDetails.Failure);
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/400");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Condition.FlipFlop(useBaseException,
                () => Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/400"",
    ""status"": 400,
    ""code"": ""BadRequest"",
    ""message"": ""Value cannot be null."",
    ""failure"": {
      ""type"": ""System.ArgumentNullException"",
      ""message"": ""Value cannot be null.""
    }
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body, o => o.ThrowOnNoMatch = true)),
                () => Assert.True(Match("""
                                       {
                                         "error": {
                                           "instance": "http://localhost/statuscodes/400",
                                           "status": 400,
                                           "code": "BadRequest",
                                           "message": "The request could not be understood by the server due to malformed syntax.",
                                           "failure": {
                                             "type": "Cuemon.AspNetCore.Http.BadRequestException",
                                             "source": "Cuemon.AspNetCore.Mvc.Tests",
                                             "message": "The request could not be understood by the server due to malformed syntax.",
                                             "headers": {},
                                             "statusCode": 400,
                                             "reasonPhrase": "Bad Request",
                                             "inner": {
                                               "type": "System.ArgumentNullException",
                                               "message": "Value cannot be null."
                                             }
                                           }
                                         },
                                         "traceId": "*"
                                       }
                                       """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true)));

            Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureUserAgentException_BadRequestMessage()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
               {
                   services
                       .AddControllers(o =>
                       {
                           o.Filters.AddFaultDescriptor();
                           o.Filters.AddUserAgentSentinel();
                       })
                       .AddApplicationPart(typeof(FakeController).Assembly)
                       .AddJsonFormatters()
                       .AddUserAgentSentinelOptions(o => o.RequireUserAgentHeader = true);
               }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/fake/it");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/fake/it"",
    ""status"": 400,
    ""code"": ""BadRequest"",
    ""message"": ""The requirements of the request was not met.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(400), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureThrottlingException_TooManyRequests()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
               {
                   services.AddControllers(o =>
                       {
                           o.Filters.AddFaultDescriptor();
                           o.Filters.AddThrottlingSentinel();
                       }).AddApplicationPart(typeof(FakeController).Assembly)
                       .AddJsonFormatters()
                       .AddThrottlingSentinelOptions(o =>
                       {
                           o.ContextResolver = _ => "dummy";
                           o.Quota = new ThrottleQuota(0, TimeSpan.Zero);
                       });
                   services.AddMemoryThrottlingCache();
               }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/fake/it");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/fake/it"",
    ""status"": 429,
    ""code"": ""TooManyRequests"",
    ""message"": ""Throttling rate limit quota violation. Quota limit exceeded.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status429TooManyRequests, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(429), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureBadRequest()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/400");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/400"",
    ""status"": 400,
    ""code"": ""BadRequest"",
    ""message"": ""The request could not be understood by the server due to malformed syntax.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(400), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureConflict()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/409");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/409"",
    ""status"": 409,
    ""code"": ""Conflict"",
    ""message"": ""The request could not be completed due to a conflict with the current state of the resource.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status409Conflict, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(409), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureForbidden()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/403");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/403"",
    ""status"": 403,
    ""code"": ""Forbidden"",
    ""message"": ""The server understood the request, but is refusing to fulfill it.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status403Forbidden, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(403), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureGone()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/410");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/410"",
    ""status"": 410,
    ""code"": ""Gone"",
    ""message"": ""The requested resource is no longer available at the server and no forwarding address is known.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status410Gone, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(410), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureNotFound()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/404");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/404"",
    ""status"": 404,
    ""code"": ""NotFound"",
    ""message"": ""The server has not found anything matching the request URI.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status404NotFound, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(404), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCapturePayloadTooLarge()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/413");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/413"",
    ""status"": 413,
    ""code"": ""PayloadTooLarge"",
    ""message"": ""The server is refusing to process a request because the request entity is larger than the server is willing or able to process.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status413PayloadTooLarge, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(413), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCapturePreconditionFailed()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/412");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/412"",
    ""status"": 412,
    ""code"": ""PreconditionFailed"",
    ""message"": ""The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status412PreconditionFailed, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(412), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCapturePreconditionRequired()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/428");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/428"",
    ""status"": 428,
    ""code"": ""PreconditionRequired"",
    ""message"": ""No conditional request-header fields was supplied to the server.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status428PreconditionRequired, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(428), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureTooManyRequests()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/429");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/429"",
    ""status"": 429,
    ""code"": ""TooManyRequests"",
    ""message"": ""The allowed number of requests has been exceeded.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status429TooManyRequests, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(429), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureUnauthorized()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/401");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/401"",
    ""status"": 401,
    ""code"": ""Unauthorized"",
    ""message"": ""The request requires user authentication.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(401), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureMethodNotAllowed()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/405");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/405"",
    ""status"": 405,
    ""code"": ""MethodNotAllowed"",
    ""message"": ""The method specified in the request is not allowed for the resource identified by the request URI.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status405MethodNotAllowed, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(405), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureNotAcceptable()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/406");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/406"",
    ""status"": 406,
    ""code"": ""NotAcceptable"",
    ""message"": ""The resource identified by the request is only capable of generating response entities which have content characteristics not acceptable according to the accept headers sent in the request.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status406NotAcceptable, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(406), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureGeneric()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/XXX");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.Contains(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/XXX"",
    ""status"": 500,
    ""code"": ""InternalServerError"",
    ""message"": ""An unhandled exception was raised by ".ReplaceLineEndings(), body);
            Assert.Equal(StatusCodes.Status500InternalServerError, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(500), result.ReasonPhrase);
        }
    }

    [Fact]
    public async Task OnException_ShouldCaptureUnsupportedMediaType()
    {
        using (var filter = WebHostTestFactory.CreateWithHostBuilderContext((context, services) =>
        {
            services.AddControllers(o => { o.Filters.AddFaultDescriptor(); }).AddApplicationPart(typeof(FakeController).Assembly)
                .AddJsonFormatters();
        }, (context, app) =>
               {
                   app.UseRouting();
                   app.UseEndpoints(routes => { routes.MapControllers(); });
               }))
        {
            var client = filter.Host.GetTestClient();
            var result = await client.GetAsync("/statuscodes/415");
            var body = await result.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match(@"{
  ""error"": {
    ""instance"": ""http://localhost/statuscodes/415"",
    ""status"": 415,
    ""code"": ""UnsupportedMediaType"",
    ""message"": ""The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method.""
  },
  ""traceId"": ""*""
}".ReplaceLineEndings(), body));
            Assert.Equal(StatusCodes.Status415UnsupportedMediaType, (int)result.StatusCode);
            Assert.Equal(HttpStatusDescription.Get(415), result.ReasonPhrase);
        }
    }
}
