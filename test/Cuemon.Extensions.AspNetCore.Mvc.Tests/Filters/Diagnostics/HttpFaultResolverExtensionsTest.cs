using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics;
public class HttpFaultResolverExtensionsTest : Test
{
    public HttpFaultResolverExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddHttpFaultResolver_ShouldAddHttpFaultResolver()
    {
        var statusCode = StatusCodes.Status400BadRequest;
        var message = "Client related error; please insure valid payload.";
        var helpLink = new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400");

        var sut1 = new List<HttpFaultResolver>();
        var sut2 = new List<HttpFaultResolver>(sut1);
        var sut3 = sut2.AddHttpFaultResolver<ArgumentException>(statusCode, message: message, helpLink: helpLink, exceptionValidator: ex => ex.GetType().HasTypes(typeof(ArgumentException))).Single();
        var sut4 = sut3.TryResolveFault(new ArgumentException(), out var sut7);
        var sut5 = sut3.TryResolveFault(new InvalidCastException(), out _);
        var sut6 = sut3.TryResolveFault(new ArgumentOutOfRangeException(), out _);

        TestOutput.WriteLine(sut7.ToString());

        Assert.True(sut1.Count == 0, "sut1.Count == 0");
        Assert.True(sut2.Count == 1, "sut2.Count == 1");
        Assert.IsType<HttpFaultResolver>(sut3);
        Assert.True(sut4);
        Assert.False(sut5);
        Assert.True(sut6);
        Assert.Equal(statusCode, sut7.StatusCode);
        Assert.Equal(message, sut7.Message);
        Assert.Equal(helpLink, sut7.HelpLink);
        Assert.Equal("BadRequest", sut7.Code);
    }

    [Fact]
    public void AddHttpFaultResolver_ShouldAddHttpFaultResolver_AllowingOnlyExceptionOfHttpStatusCodeException()
    {
        var statusCode = StatusCodes.Status400BadRequest;
        var message = "Client related error; please insure valid payload.";
        var helpLink = new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400");

        var sut1 = new List<HttpFaultResolver>();
        var sut2 = new List<HttpFaultResolver>(sut1);
        var sut3 = sut2.AddHttpFaultResolver<BadRequestException>(message, helpLink).Single();
        var sut4 = sut3.TryResolveFault(new BadRequestException(), out var sut7);
        var sut5 = sut3.TryResolveFault(new ConflictException(), out _);

        TestOutput.WriteLine(sut7.ToString());

        Assert.True(sut1.Count == 0, "sut1.Count == 0");
        Assert.True(sut2.Count == 1, "sut2.Count == 1");
        Assert.IsType<HttpFaultResolver>(sut3);
        Assert.True(sut4);
        Assert.False(sut5);
        Assert.Equal(statusCode, sut7.StatusCode);
        Assert.Equal(message, sut7.Message);
        Assert.Equal(helpLink, sut7.HelpLink);
        Assert.Equal("BadRequest", sut7.Code);
        Assert.IsAssignableFrom<HttpStatusCodeException>(sut7.Failure);
    }

    [Fact]
    public void AddHttpFaultResolver_ShouldAddHttpFaultResolver_UsingFunctionDelegates()
    {
        var statusCode = StatusCodes.Status429TooManyRequests;
        var message = "The allowed number of requests has been exceeded.";
        var helpLink = new Uri("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/429");

        var sut1 = new List<HttpFaultResolver>();
        var sut2 = new List<HttpFaultResolver>(sut1);
        var sut3 = sut2.AddHttpFaultResolver<TooManyRequestsException>(e => new HttpExceptionDescriptor(e, e.StatusCode, e.ReasonPhrase, e.Message, helpLink), e => e is TooManyRequestsException).Single();
        var sut4 = sut3.TryResolveFault(new TooManyRequestsException(), out var sut7);
        var sut5 = sut3.TryResolveFault(new ConflictException(), out _);

        TestOutput.WriteLine(sut7.ToString());

        Assert.True(sut1.Count == 0, "sut1.Count == 0");
        Assert.True(sut2.Count == 1, "sut2.Count == 1");
        Assert.IsType<HttpFaultResolver>(sut3);
        Assert.True(sut4);
        Assert.False(sut5);
        Assert.Equal(statusCode, sut7.StatusCode);
        Assert.Equal(message, sut7.Message);
        Assert.Equal(helpLink, sut7.HelpLink);
        Assert.Equal("TooManyRequests", sut7.Code);
        Assert.IsAssignableFrom<HttpStatusCodeException>(sut7.Failure);
    }

    [Fact]
    public void AddHttpFaultResolver_ShouldAddHttpFaultResolver_UsingStandardValues_FromHttpStatusCodeDerivedException()
    {
        var statusCode = StatusCodes.Status429TooManyRequests;
        var message = "The allowed number of requests has been exceeded.";

        var sut1 = new List<HttpFaultResolver>();
        var sut2 = new List<HttpFaultResolver>(sut1);
        var sut3 = sut2.AddHttpFaultResolver<TooManyRequestsException>(e => new HttpExceptionDescriptor(e), e => e is TooManyRequestsException).Single();
        var sut4 = sut3.TryResolveFault(new TooManyRequestsException(), out var sut7);
        var sut5 = sut3.TryResolveFault(new ConflictException(), out _);

        TestOutput.WriteLine(sut7.ToString());

        Assert.True(sut1.Count == 0, "sut1.Count == 0");
        Assert.True(sut2.Count == 1, "sut2.Count == 1");
        Assert.IsType<HttpFaultResolver>(sut3);
        Assert.True(sut4);
        Assert.False(sut5);
        Assert.Equal(statusCode, sut7.StatusCode);
        Assert.Equal(message, sut7.Message);
        Assert.Equal("TooManyRequests", sut7.Code);
        Assert.IsAssignableFrom<HttpStatusCodeException>(sut7.Failure);
    }

    [Fact]
    public void AddHttpFaultResolver_ShouldAddHttpFaultResolver_UsingStandardValues_FromNonHttpStatusCodeDerivedException()
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var message = "Insufficient memory to continue the execution of the program.";

        var sut1 = new List<HttpFaultResolver>();
        var sut2 = new List<HttpFaultResolver>(sut1);
        var sut3 = sut2.AddHttpFaultResolver<OutOfMemoryException>(e => new HttpExceptionDescriptor(e), e => e is OutOfMemoryException).Single();
        var sut4 = sut3.TryResolveFault(new OutOfMemoryException(), out var sut7);
        var sut5 = sut3.TryResolveFault(new ConflictException(), out _);

        TestOutput.WriteLine(sut7.ToString());

        Assert.True(sut1.Count == 0, "sut1.Count == 0");
        Assert.True(sut2.Count == 1, "sut2.Count == 1");
        Assert.IsType<HttpFaultResolver>(sut3);
        Assert.True(sut4);
        Assert.False(sut5);
        Assert.Equal(statusCode, sut7.StatusCode);
        Assert.Equal(message, sut7.Message);
        Assert.Equal("InternalServerError", sut7.Code);
        Assert.IsAssignableFrom<SystemException>(sut7.Failure);
    }
}
