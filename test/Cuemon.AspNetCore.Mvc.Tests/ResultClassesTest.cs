using System;
using Cuemon.AspNetCore.Diagnostics;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Cuemon.AspNetCore.Mvc
{
    public class ExceptionDescriptorResultTest : Test
    {
        public ExceptionDescriptorResultTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldWrapProblemDetails()
        {
            var problemDetails = new ProblemDetails() { Title = "Broken" };

            var sut = new ExceptionDescriptorResult(problemDetails);

            var wrapper = Assert.IsAssignableFrom<IDecorator<ProblemDetails>>(sut.Value);
            Assert.Same(problemDetails, wrapper.Inner);
        }

        [Fact]
        public void Constructor_ShouldStoreHttpExceptionDescriptor()
        {
            var descriptor = new HttpExceptionDescriptor(new InvalidOperationException("fail"));

            var sut = new ExceptionDescriptorResult(descriptor);

            Assert.Same(descriptor, sut.Value);
        }
    }

    public class ForbiddenResultTest : Test
    {
        public ForbiddenResultTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldDefaultToStatusCode403()
        {
            var sut = new ForbiddenResult();

            Assert.Equal(StatusCodes.Status403Forbidden, sut.StatusCode);
        }

        [Theory]
        [InlineData(StatusCodes.Status400BadRequest)]
        [InlineData(StatusCodes.Status404NotFound)]
        public void Constructor_ShouldAcceptClientErrorStatusCodes(int statusCode)
        {
            var sut = new ForbiddenResult(statusCode);

            Assert.Equal(statusCode, sut.StatusCode);
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK)]
        [InlineData(StatusCodes.Status500InternalServerError)]
        public void Constructor_ShouldThrowArgumentException_WhenStatusCodeIsNotClientError(int statusCode)
        {
            Assert.ThrowsAny<ArgumentException>(() => new ForbiddenResult(statusCode));
        }
    }

    public class ForbiddenObjectResultTest : Test
    {
        public ForbiddenObjectResultTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldDefaultToStatusCode403()
        {
            var payload = new { Message = "denied" };

            var sut = new ForbiddenObjectResult(payload);

            Assert.Equal(StatusCodes.Status403Forbidden, sut.StatusCode);
            Assert.Same(payload, sut.Value);
        }

        [Theory]
        [InlineData(StatusCodes.Status401Unauthorized)]
        [InlineData(StatusCodes.Status429TooManyRequests)]
        public void Constructor_ShouldAcceptClientErrorStatusCodes(int statusCode)
        {
            var sut = new ForbiddenObjectResult("payload", statusCode);

            Assert.Equal(statusCode, sut.StatusCode);
            Assert.Equal("payload", sut.Value);
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK)]
        [InlineData(StatusCodes.Status500InternalServerError)]
        public void Constructor_ShouldThrowArgumentException_WhenStatusCodeIsNotClientError(int statusCode)
        {
            Assert.ThrowsAny<ArgumentException>(() => new ForbiddenObjectResult("payload", statusCode));
        }
    }

    public class TooManyRequestsResultTest : Test
    {
        public TooManyRequestsResultTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldDefaultToStatusCode429()
        {
            var sut = new TooManyRequestsResult();

            Assert.Equal(StatusCodes.Status429TooManyRequests, sut.StatusCode);
        }
    }

    public class TooManyRequestsObjectResultTest : Test
    {
        public TooManyRequestsObjectResultTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldDefaultToStatusCode429()
        {
            var payload = new { Message = "slow down" };

            var sut = new TooManyRequestsObjectResult(payload);

            Assert.Equal(StatusCodes.Status429TooManyRequests, sut.StatusCode);
            Assert.Same(payload, sut.Value);
        }
    }
}
