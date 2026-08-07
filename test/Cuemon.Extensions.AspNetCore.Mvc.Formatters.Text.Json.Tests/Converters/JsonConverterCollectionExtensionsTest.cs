using System;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Collections.Generic;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Json.Converters;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Text.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.Converters;
public class JsonConverterCollectionExtensionsTest : Test
{
    public JsonConverterCollectionExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.None)]
    public void AddHttpExceptionDescriptorConverter_ShouldAddHttpExceptionDescriptorToConverterCollection(FaultSensitivityDetails sensitivityDetails)
    {
        OutOfMemoryException oome = null;
        try
        {
            throw new OutOfMemoryException();
        }
        catch (OutOfMemoryException e)
        {
            oome = e;
        }

        using (var middleware = WebHostTestFactory.Create(services => { services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton); }))
        {
            var context = middleware.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var correlationId = Guid.NewGuid().ToString("N");
            var requestId = Guid.NewGuid().ToString("N");

            var sut1 = new HttpExceptionDescriptor(oome, message: "Custom non-revealing message.")
            {
                CorrelationId = correlationId,
                RequestId = requestId,
                HelpLink = new Uri("https://docs.microsoft.com/en-us/dotnet/api/system.outofmemoryexception")
            };

            sut1.AddEvidence("Request", context.Request, request => new HttpRequestEvidence(request));

            var sut2 = new JsonFormatterOptions()
            {
                SensitivityDetails = sensitivityDetails
            };

            var dc = sut2.Settings.Converters.SingleOrDefault(jc => jc.CanConvert(typeof(HttpExceptionDescriptor)));
            if (dc != null) { sut2.Settings.Converters.Remove(dc); }
            sut2.Settings.Converters.AddHttpExceptionDescriptorConverter(o =>
            {
                o.SensitivityDetails = sensitivityDetails;
            });

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(typeof(HttpExceptionDescriptor))).ToList(), jc =>
            {
                var jf = new JsonFormatter(sut2);

                var result = jf.Serialize(sut1);

                var json = result.ToEncodedString(o => o.LeaveOpen = true);

                Assert.True(jc.CanConvert(typeof(HttpExceptionDescriptor)));

                Assert.Contains("\"error\":", json);
                Assert.Contains("\"status\": 500", json);
                Assert.Contains("\"code\": \"InternalServerError\"", json);
                Assert.Contains("\"message\": \"Custom non-revealing message.\"", json);
                Assert.Contains("\"helpLink\": \"https://docs.microsoft.com/en-us/dotnet/api/system.outofmemoryexception\"", json);

                Assert.Contains($"\"correlationId\": \"{correlationId}\"", json);
                Assert.Contains($"\"requestId\": \"{requestId}\"", json);

                Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.Failure), () =>
                {
                    Assert.Contains("\"failure\":", json);
                    Assert.Contains("\"type\": \"System.OutOfMemoryException\"", json);
                    Assert.Contains("\"source\": \"Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.Tests\"", json);
                    Assert.Contains("\"message\": \"Insufficient memory to continue the execution of the program.\"", json);
                }, () =>
                {
                    Assert.DoesNotContain("\"failure\":", json);
                    Assert.DoesNotContain("\"type\": \"System.OutOfMemoryException\"", json);
                    Assert.DoesNotContain("\"source\": \"Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.Tests\"", json);
                    Assert.DoesNotContain("\"message\": \"Insufficient memory to continue the execution of the program.\"", json);
                });

                Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), () =>
                {
                    Assert.Contains("\"stack\":", json);
                    Assert.Contains("\"at Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.Converters.JsonConverterCollectionExtensionsTest.AddHttpExceptionDescriptorConverter_ShouldAddHttpExceptionDescriptorToConverterCollection", json);
                }, () =>
                {
                    Assert.DoesNotContain("\"stack\":", json);
                    Assert.DoesNotContain("\"at Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.Converters.JsonConverterCollectionExtensionsTest.AddHttpExceptionDescriptorConverter_ShouldAddHttpExceptionDescriptorToConverterCollection", json);
                });

                Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence), () =>
                {
                    Assert.Contains("\"evidence\":", json);
                    Assert.Contains("\"request\":", json);
                    Assert.Contains("\"location\": \"http:///\"", json);
                    Assert.Contains("\"method\": \"GET\"", json);
                    Assert.Contains("\"headers\":", json);
                    Assert.Contains("\"query\":", json);
                    Assert.Contains("\"cookies\":", json);
                    Assert.Contains("\"body\":", json);
                }, () =>
                {
                    Assert.DoesNotContain("\"evidence\":", json);
                });

                TestOutput.WriteLine(json);
            });
        }
    }

    [Fact]
    public void AddStringValuesConverter_ShouldAddStringValuesConverterToConverterCollection()
    {
        var sut1 = new StringValues(Arguments.ToArrayOf("This", "is", "a", "test", "!"));

        var sut2 = new JsonFormatterOptions();

        var dc = sut2.Settings.Converters.SingleOrDefault(jc => jc.CanConvert(typeof(StringValues)));
        if (dc != null) { sut2.Settings.Converters.Remove(dc); }
        sut2.Settings.Converters.AddStringValuesConverter();

        Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(typeof(StringValues))).ToList(), jc =>
        {
            var jf = new JsonFormatter(sut2);

            var result = jf.Serialize(sut1);

            var json = result.ToEncodedString(o => o.LeaveOpen = true);

            Assert.True(jc.CanConvert(typeof(StringValues)));

            Assert.Contains("[", json);
            Assert.Contains("\"This\",", json);
            Assert.Contains("\"is\"", json);
            Assert.Contains("\"a\"", json);
            Assert.Contains("\"test\"", json);
            Assert.Contains("\"!\"", json);
            Assert.Contains("]", json);

            TestOutput.WriteLine(json);
        });
    }

    [Fact]
    public void AddStringValuesConverter_ShouldAddStringValuesConverterToConverterCollection_OneValue()
    {
        var sut1 = new StringValues(Arguments.ToArrayOf("This"));

        var sut2 = new JsonFormatterOptions();

        var dc = sut2.Settings.Converters.SingleOrDefault(jc => jc.CanConvert(typeof(StringValues)));
        if (dc != null) { sut2.Settings.Converters.Remove(dc); }
        sut2.Settings.Converters.AddStringValuesConverter();

        Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(typeof(StringValues))).ToList(), jc =>
        {
            var jf = new JsonFormatter(sut2);

            var result = jf.Serialize(sut1);

            var json = result.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(json);

            Assert.True(jc.CanConvert(typeof(StringValues)));

            Assert.Equal("\"This\"", json);
        });
    }
}
