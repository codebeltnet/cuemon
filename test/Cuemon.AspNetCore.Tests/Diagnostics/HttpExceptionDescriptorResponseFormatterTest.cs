using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.AspNetCore.Diagnostics
{
    public class HttpExceptionDescriptorResponseFormatterTest : Test
    {
        public HttpExceptionDescriptorResponseFormatterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldInitializeFromActionAndOptionsWrapper()
        {
            var actionFormatter = new HttpExceptionDescriptorResponseFormatter<JsonFormatterOptions>(_ =>
            {
            });
            var options = new JsonFormatterOptions();
            var optionsFormatter = new HttpExceptionDescriptorResponseFormatter<JsonFormatterOptions>(Options.Create(options));

            Assert.NotEmpty(actionFormatter.Options.SupportedMediaTypes);
            Assert.Same(options, optionsFormatter.Options);
        }

        [Fact]
        public void AdjustAndPopulate_ShouldAddResponseHandlersForEverySupportedMediaType()
        {
            var sut = new HttpExceptionDescriptorResponseFormatter<JsonFormatterOptions>(_ =>
            {
            });
            var handlers = new List<HttpExceptionDescriptorResponseHandler>();

            var returned = sut
                .Adjust(_ =>
                {
                })
                .Populate((descriptor, mediaType) => new StringContent(mediaType.MediaType), handlers);

            var descriptor = new HttpExceptionDescriptor(new InvalidOperationException("boom"), 418, "Teapot", "Short and stout");
            using var response = handlers.Last().ToHttpResponseMessage(descriptor);

            Assert.Same(sut, returned);
            Assert.Same(handlers, sut.ExceptionDescriptorHandlers);
            Assert.Equal(sut.Options.SupportedMediaTypes.Count, handlers.Count);
            Assert.Equal((HttpStatusCode)418, response.StatusCode);
            Assert.Equal(handlers.Last().ContentType.MediaType, response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
    }
}
