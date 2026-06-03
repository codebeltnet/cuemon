using System;
using System.Linq;
using System.Net;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Net.Http;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Http.Headers
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddApiKeySentinelOptions_ShouldThrowArgumentNullException_WhenServicesIsNull()
        {
            Assert.Throws<ArgumentNullException>("services", () => ServiceCollectionExtensions.AddApiKeySentinelOptions(null));
        }

        [Fact]
        public void AddApiKeySentinelOptions_ShouldRegisterApiKeySentinelOptions_WithDefaultValues()
        {
            var sut = new ServiceCollection();

            sut.AddApiKeySentinelOptions();

            var count = sut.Count(sd => sd.ServiceType == typeof(IConfigureOptions<ApiKeySentinelOptions>));

            TestOutput.WriteLine($"IConfigureOptions<ApiKeySentinelOptions> registrations: {count}");

            Assert.True(count >= 1);
        }

        [Fact]
        public void AddApiKeySentinelOptions_ShouldRegisterApiKeySentinelOptions_WithCustomValues()
        {
            var sut = new ServiceCollection();

            sut.AddApiKeySentinelOptions(o =>
            {
                o.AllowedKeys.Add("my-api-key");
                o.HeaderName = "X-My-Api-Key";
            });

            var count = sut.Count(sd => sd.ServiceType == typeof(IConfigureOptions<ApiKeySentinelOptions>));

            Assert.True(count >= 1);
        }

        [Fact]
        public void AddUserAgentSentinelOptions_ShouldThrowArgumentNullException_WhenServicesIsNull()
        {
            Assert.Throws<ArgumentNullException>("services", () => ServiceCollectionExtensions.AddUserAgentSentinelOptions(null));
        }

        [Fact]
        public void AddUserAgentSentinelOptions_ShouldRegisterUserAgentSentinelOptions_WithDefaultValues()
        {
            var sut = new ServiceCollection();

            sut.AddUserAgentSentinelOptions();

            var count = sut.Count(sd => sd.ServiceType == typeof(IConfigureOptions<UserAgentSentinelOptions>));

            TestOutput.WriteLine($"IConfigureOptions<UserAgentSentinelOptions> registrations: {count}");

            Assert.True(count >= 1);
        }

        [Fact]
        public void AddUserAgentSentinelOptions_ShouldRegisterUserAgentSentinelOptions_WithCustomValues()
        {
            var sut = new ServiceCollection();

            sut.AddUserAgentSentinelOptions(o =>
            {
                o.AllowedUserAgents.Add("MyApp/1.0");
                o.RequireUserAgentHeader = true;
            });

            var count = sut.Count(sd => sd.ServiceType == typeof(IConfigureOptions<UserAgentSentinelOptions>));

            Assert.True(count >= 1);
        }

        [Fact]
        public void AddApiKeySentinelOptions_ShouldResolveConfiguredOptions()
        {
            var services = new ServiceCollection();

            services.AddOptions();
            services.AddApiKeySentinelOptions(o =>
            {
                o.AllowedKeys.Add("known-key");
                o.HeaderName = "X-Test-Key";
                o.GenericClientStatusCode = HttpStatusCode.Unauthorized;
                o.GenericClientMessage = "custom";
                o.ForbiddenMessage = "forbidden";
                o.UseGenericResponse = true;
            });

            var options = services.BuildServiceProvider().GetRequiredService<IOptions<ApiKeySentinelOptions>>().Value;

            Assert.Contains("known-key", options.AllowedKeys);
            Assert.Equal("X-Test-Key", options.HeaderName);
            Assert.Equal(HttpStatusCode.Unauthorized, options.GenericClientStatusCode);
            Assert.Equal("custom", options.GenericClientMessage);
            Assert.Equal("forbidden", options.ForbiddenMessage);
            Assert.True(options.UseGenericResponse);
            Assert.NotNull(options.ResponseHandler);
        }

        [Fact]
        public void AddApiKeySentinelOptions_ShouldResolveDefaultOptions()
        {
            var services = new ServiceCollection();

            services.AddOptions();
            services.AddApiKeySentinelOptions();

            var options = services.BuildServiceProvider().GetRequiredService<IOptions<ApiKeySentinelOptions>>().Value;

            Assert.Equal(HttpHeaderNames.XApiKey, options.HeaderName);
            Assert.Equal(HttpStatusCode.BadRequest, options.GenericClientStatusCode);
            Assert.Equal("The requirements of the request was not met.", options.GenericClientMessage);
            Assert.Equal("The API key specified was rejected.", options.ForbiddenMessage);
            Assert.NotNull(options.AllowedKeys);
            Assert.NotNull(options.ResponseHandler);
        }

        [Fact]
        public void AddUserAgentSentinelOptions_ShouldResolveConfiguredOptions()
        {
            var services = new ServiceCollection();

            services.AddOptions();
            services.AddUserAgentSentinelOptions(o =>
            {
                o.AllowedUserAgents.Add("Cuemon-Agent");
                o.BadRequestMessage = "bad";
                o.ForbiddenMessage = "forbidden";
                o.RequireUserAgentHeader = true;
                o.ValidateUserAgentHeader = true;
                o.UseGenericResponse = true;
            });

            var options = services.BuildServiceProvider().GetRequiredService<IOptions<UserAgentSentinelOptions>>().Value;

            Assert.Contains("Cuemon-Agent", options.AllowedUserAgents);
            Assert.Equal("bad", options.BadRequestMessage);
            Assert.Equal("forbidden", options.ForbiddenMessage);
            Assert.True(options.RequireUserAgentHeader);
            Assert.True(options.ValidateUserAgentHeader);
            Assert.True(options.UseGenericResponse);
            Assert.NotNull(options.ResponseHandler);
        }

        [Fact]
        public void AddUserAgentSentinelOptions_ShouldResolveDefaultOptions()
        {
            var services = new ServiceCollection();

            services.AddOptions();
            services.AddUserAgentSentinelOptions();

            var options = services.BuildServiceProvider().GetRequiredService<IOptions<UserAgentSentinelOptions>>().Value;

            Assert.Equal("The requirements of the request was not met.", options.BadRequestMessage);
            Assert.Equal("The User-Agent specified was rejected.", options.ForbiddenMessage);
            Assert.NotNull(options.AllowedUserAgents);
            Assert.False(options.RequireUserAgentHeader);
            Assert.False(options.ValidateUserAgentHeader);
            Assert.False(options.UseGenericResponse);
            Assert.NotNull(options.ResponseHandler);
        }
    }
}
