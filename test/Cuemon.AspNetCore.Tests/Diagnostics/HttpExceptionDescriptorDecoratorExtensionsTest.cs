using System;
using Codebelt.Extensions.Xunit;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Cuemon.AspNetCore.Diagnostics;
public class HttpExceptionDescriptorDecoratorExtensionsTest : Test
{
    public HttpExceptionDescriptorDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToProblemDetails_ShouldIncludeFailureEvidenceAndIdentifiers_WhenSensitivityIsAll()
    {
        var helpLink = new Uri("https://docs.cuemon.net/errors/teapot");
        var descriptor = new HttpExceptionDescriptor(new InvalidOperationException("boom"), 418, "Teapot", "Short and stout", helpLink)
        {
            CorrelationId = "cid-123",
            RequestId = "rid-123",
            TraceId = "tid-123",
            Instance = new Uri("urn:request:42")
        };
        descriptor.AddEvidence("request", new { Path = "/tea" }, evidence => evidence);

        var sut = Decorator.Enclose(descriptor).ToProblemDetails(FaultSensitivityDetails.All);

        Assert.Equal("Short and stout", sut.Detail);
        Assert.Equal(418, sut.Status);
        Assert.Equal("Teapot", sut.Title);
        Assert.Equal(helpLink.ToString(), sut.Type);
        Assert.Equal("urn:request:42", sut.Instance);
        Assert.Equal("cid-123", Assert.IsType<string>(sut.Extensions[nameof(HttpExceptionDescriptor.CorrelationId)]));
        Assert.Equal("rid-123", Assert.IsType<string>(sut.Extensions[nameof(HttpExceptionDescriptor.RequestId)]));
        Assert.Equal("tid-123", Assert.IsType<string>(sut.Extensions[nameof(HttpExceptionDescriptor.TraceId)]));
        Assert.True(sut.Extensions.ContainsKey(nameof(FaultSensitivityDetails.Failure)));
        Assert.True(sut.Extensions.ContainsKey("request"));
    }

    [Fact]
    public void ToProblemDetails_ShouldExcludeFailureAndEvidence_WhenSensitivityIsNone()
    {
        var descriptor = new HttpExceptionDescriptor(new InvalidOperationException("boom"), 500, "InternalServerError", "Unexpected failure");
        descriptor.AddEvidence("request", new { Path = "/tea" }, evidence => evidence);

        var sut = Decorator.Enclose(descriptor).ToProblemDetails(FaultSensitivityDetails.None);

        Assert.Equal("Unexpected failure", sut.Detail);
        Assert.False(sut.Extensions.ContainsKey(nameof(FaultSensitivityDetails.Failure)));
        Assert.False(sut.Extensions.ContainsKey("request"));
    }
}
