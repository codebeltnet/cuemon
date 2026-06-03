using System;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Cuemon.AspNetCore.Http
{
    public class InternalServerErrorExceptionTest : Test
    {
        public InternalServerErrorExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldUseDefaultMessageAndStatusCode()
        {
            var sut = new InternalServerErrorException();

            Assert.Equal(StatusCodes.Status500InternalServerError, sut.StatusCode);
            Assert.Equal("Internal Server Error", sut.ReasonPhrase);
            Assert.Equal("The server has encountered a situation it does not know how to handle.", sut.Message);
        }

        [Fact]
        public void Ctor_ShouldUseDefaultMessage_WhenOnlyInnerExceptionIsProvided()
        {
            var inner = new InvalidOperationException("boom");
            var sut = new InternalServerErrorException(inner);

            Assert.Same(inner, sut.InnerException);
            Assert.Equal(StatusCodes.Status500InternalServerError, sut.StatusCode);
            Assert.Equal("The server has encountered a situation it does not know how to handle.", sut.Message);
        }

        [Fact]
        public void Ctor_ShouldUseProvidedMessageAndInnerException()
        {
            var inner = new InvalidOperationException("boom");
            var sut = new InternalServerErrorException("Something unexpected happened.", inner);

            Assert.Same(inner, sut.InnerException);
            Assert.Equal(StatusCodes.Status500InternalServerError, sut.StatusCode);
            Assert.Equal("Something unexpected happened.", sut.Message);
        }
    }
}
