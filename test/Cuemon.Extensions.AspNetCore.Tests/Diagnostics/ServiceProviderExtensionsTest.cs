using System;
using System.Linq;
using System.Net.Http;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Json.Formatters;
using Cuemon.Extensions.AspNetCore.Xml.Formatters;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Xml.Serialization.Formatters;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Diagnostics
{
    public class ServiceProviderExtensionsTest : Test
    {
        public ServiceProviderExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void GetExceptionResponseFormatters_ShouldGetAllRegisteredServicesOf_IExceptionResponseFormatter()
        {
            var services = new ServiceCollection();

            services.AddOptions();
            services.AddXmlExceptionResponseFormatter();
            services.AddJsonExceptionResponseFormatter();

            var serviceProvider = services.BuildServiceProvider();

            var formatters = serviceProvider.GetExceptionResponseFormatters().ToList();

            var formattersAndResponseHandlers = formatters.SelectMany(formatter => formatter.ExceptionDescriptorHandlers.Select(handler => $"{formatter.GetType().GenericTypeArguments[0].Name} -> {handler.ContentType}")).ToList();

            TestOutput.WriteLine(formattersAndResponseHandlers.ToDelimitedString(o => o.Delimiter = Environment.NewLine));

            Assert.Equal(6, formattersAndResponseHandlers.Count);
            Assert.Equal("""
                         XmlFormatterOptions -> application/xml
                         XmlFormatterOptions -> text/xml
                         XmlFormatterOptions -> application/problem+xml
                         JsonFormatterOptions -> application/json
                         JsonFormatterOptions -> text/json
                         JsonFormatterOptions -> application/problem+json
                         """.ReplaceLineEndings(), formattersAndResponseHandlers.ToDelimitedString(o => o.Delimiter = Environment.NewLine));
        }

        [Fact]
        public void GetExceptionResponseFormatters_ShouldSupportImplementationInstanceAndFactoryRegistrations()
        {
            var services = new ServiceCollection();
            var instance = new HttpExceptionDescriptorResponseFormatter<JsonFormatterOptions>(Options.Create(new JsonFormatterOptions()))
                .Populate((_, mediaType) => new StringContent(mediaType.MediaType));

            services.AddSingleton(instance.GetType(), instance);
            services.AddSingleton(typeof(HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions>), _ =>
                new HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions>(Options.Create(new XmlFormatterOptions()))
                    .Populate((_, mediaType) => new StringContent(mediaType.MediaType)));

            var serviceProvider = services.BuildServiceProvider();

            var formatters = serviceProvider.GetExceptionResponseFormatters().ToList();

            Assert.Equal(2, formatters.Count);
            Assert.Same(instance, formatters.Single(formatter => formatter.GetType() == instance.GetType()));
            Assert.Contains(formatters, formatter => formatter.GetType() == typeof(HttpExceptionDescriptorResponseFormatter<XmlFormatterOptions>));
        }
    }
}
