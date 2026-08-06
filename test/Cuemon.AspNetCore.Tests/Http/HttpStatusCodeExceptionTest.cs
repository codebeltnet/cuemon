using System;
using System.Net;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Cuemon.AspNetCore.Http;
public class HttpStatusCodeExceptionTest : Test
{
    public HttpStatusCodeExceptionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(StatusCodes.Status400BadRequest, typeof(BadRequestException))]
    [InlineData(StatusCodes.Status401Unauthorized, typeof(UnauthorizedException))]
    [InlineData(StatusCodes.Status403Forbidden, typeof(ForbiddenException))]
    [InlineData(StatusCodes.Status404NotFound, typeof(NotFoundException))]
    [InlineData(StatusCodes.Status405MethodNotAllowed, typeof(MethodNotAllowedException))]
    [InlineData(StatusCodes.Status406NotAcceptable, typeof(NotAcceptableException))]
    [InlineData(StatusCodes.Status409Conflict, typeof(ConflictException))]
    [InlineData(StatusCodes.Status410Gone, typeof(GoneException))]
    [InlineData(StatusCodes.Status412PreconditionFailed, typeof(PreconditionFailedException))]
    [InlineData(StatusCodes.Status413PayloadTooLarge, typeof(PayloadTooLargeException))]
    [InlineData(StatusCodes.Status415UnsupportedMediaType, typeof(UnsupportedMediaTypeException))]
    [InlineData(StatusCodes.Status428PreconditionRequired, typeof(PreconditionRequiredException))]
    [InlineData(StatusCodes.Status429TooManyRequests, typeof(TooManyRequestsException))]
    public void TryParse_ShouldResolveKnownStatusCodes(int statusCode, Type expectedType)
    {
        var inner = new InvalidOperationException("inner");

        var result = HttpStatusCodeException.TryParse(statusCode, "custom", inner, out var sut);

        Assert.True(result);
        Assert.IsType(expectedType, sut);
        Assert.Equal(statusCode, sut.StatusCode);
        Assert.Equal("custom", sut.Message);
        Assert.Same(inner, sut.InnerException);
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForUnknownStatusCode()
    {
        var result = HttpStatusCodeException.TryParse(418, out var sut);

        Assert.False(result);
        Assert.Null(sut);
    }

    [Fact]
    public void Ctor_ShouldValidateRange_AndIncludeAdditionalInformationInToString()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FakeHttpStatusCodeException(99, "too-low", null));
        Assert.Throws<ArgumentOutOfRangeException>(() => new FakeHttpStatusCodeException(512, "too-high", null));

        var sut = new FakeHttpStatusCodeException((int)HttpStatusCode.InternalServerError, "boom", new InvalidOperationException("inner"));
        sut.Headers["X-Test"] = "1";

        var result = sut.ToString();

        Assert.Contains("Additional Information:", result);
        Assert.Contains("StatusCode: 500", result);
        Assert.Contains("ReasonPhrase: Internal Server Error", result);
        Assert.Contains("Headers:", result);
    }

    private sealed class FakeHttpStatusCodeException : HttpStatusCodeException
    {
        public FakeHttpStatusCodeException(int statusCode, string message, Exception innerException) : base(statusCode, message, innerException)
        {
        }
    }
}
