using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Configuration;
using Cuemon.Runtime.Serialization.Formatters;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Formatters
{
    public class FormatterBaseTest : Test
    {
        public FormatterBaseTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ConfigurableFormatter_ShouldExposeConfiguredOptions()
        {
            var inputOptions = new FakeFormatterOptions() { Prefix = "in:" };
            var outputOptions = new FakeFormatterOptions() { Prefix = "out:" };
            var input = new FakeConfigurableInputFormatter(inputOptions);
            var output = new FakeConfigurableOutputFormatter(outputOptions);

            Assert.Same(inputOptions, input.Options);
            Assert.Same(outputOptions, output.Options);
        }

        [Fact]
        public async Task StreamInputFormatter_ShouldReadRequestBodyAndCaptureBodyStream()
        {
            var context = new DefaultHttpContext();
            var formatter = new FakeStreamInputFormatter(new FakeFormatterOptions() { Prefix = "in:" });
            var metadataProvider = new EmptyModelMetadataProvider();
            var input = "hello world";

            context.Request.ContentType = "text/plain";
            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(input));

            var formatterContext = new InputFormatterContext(
                context,
                string.Empty,
                new ModelStateDictionary(),
                metadataProvider.GetMetadataForType(typeof(string)),
                (stream, encoding) => new StreamReader(stream, encoding));

            Assert.True(formatter.CanRead(formatterContext));

            var result = await formatter.ReadRequestBodyAsync(formatterContext, Encoding.UTF8);

            Assert.True(result.IsModelSet);
            Assert.Equal("in:" + input, result.Model);
            Assert.True(context.Items.ContainsKey(HttpRequestEvidence.HttpContextItemsKeyForCapturedRequestBody));
            Assert.IsType<MemoryStream>(context.Items[HttpRequestEvidence.HttpContextItemsKeyForCapturedRequestBody]);
        }

        [Fact]
        public async Task StreamOutputFormatter_ShouldWriteResponseBody()
        {
            var context = new DefaultHttpContext();
            var formatter = new FakeStreamOutputFormatter(new FakeFormatterOptions() { Prefix = "out:" });

            context.Response.Body = new MemoryStream();

            var formatterContext = new OutputFormatterWriteContext(
                context,
                (stream, encoding) => new StreamWriter(stream, encoding, 1024, true),
                typeof(string),
                "payload");

            formatterContext.ContentType = new StringSegment("text/plain");

            Assert.True(formatter.CanWriteResult(formatterContext));

            await formatter.WriteResponseBodyAsync(formatterContext, Encoding.UTF8);

            context.Response.Body.Position = 0;
            using (var reader = new StreamReader(context.Response.Body, Encoding.UTF8, true, 1024, true))
            {
                Assert.Equal("out:payload", reader.ReadToEnd());
            }
        }

        private sealed class FakeFormatterOptions : IParameterObject
        {
            public string Prefix { get; set; }
        }

        private sealed class FakeConfigurableInputFormatter : ConfigurableInputFormatter<FakeFormatterOptions>
        {
            public FakeConfigurableInputFormatter(FakeFormatterOptions options) : base(options)
            {
                SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));
            }

            protected override bool CanReadType(Type type)
            {
                return type == typeof(string);
            }

            public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
            {
                return InputFormatterResult.SuccessAsync(context.ModelType.Name);
            }
        }

        private sealed class FakeConfigurableOutputFormatter : ConfigurableOutputFormatter<FakeFormatterOptions>
        {
            public FakeConfigurableOutputFormatter(FakeFormatterOptions options) : base(options)
            {
                SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));
            }

            protected override bool CanWriteType(Type type)
            {
                return type == typeof(string);
            }

            public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
            {
                return Task.CompletedTask;
            }
        }

        private sealed class FakeStreamInputFormatter : StreamInputFormatter<FakeStreamFormatter, FakeFormatterOptions>
        {
            public FakeStreamInputFormatter(FakeFormatterOptions options) : base(options)
            {
                SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));
            }

            protected override bool CanReadType(Type type)
            {
                return type == typeof(string);
            }
        }

        private sealed class FakeStreamOutputFormatter : StreamOutputFormatter<FakeStreamFormatter, FakeFormatterOptions>
        {
            public FakeStreamOutputFormatter(FakeFormatterOptions options) : base(options)
            {
                SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));
            }

            protected override bool CanWriteType(Type type)
            {
                return type == typeof(string);
            }
        }

        private sealed class FakeStreamFormatter : StreamFormatter<FakeFormatterOptions>
        {
            public FakeStreamFormatter(FakeFormatterOptions options) : base(options)
            {
            }

            public override object Deserialize(Stream value, Type objectType)
            {
                value.Position = 0;
                using (var reader = new StreamReader(value, Encoding.UTF8, true, 1024, true))
                {
                    return Options.Prefix + reader.ReadToEnd();
                }
            }

            public override Stream Serialize(object source, Type objectType)
            {
                return new MemoryStream(Encoding.UTF8.GetBytes(Options.Prefix + source));
            }
        }
    }
}
