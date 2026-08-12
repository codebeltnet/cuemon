using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Diagnostics;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters;
public class MvcBuilderExtensionsTest : Test
{
    public MvcBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddApiKeySentinelOptions_ShouldThrowArgumentNullException_WhenBuilderIsNull()
    {
        Assert.Throws<ArgumentNullException>("builder", () => MvcBuilderExtensions.AddApiKeySentinelOptions(null));
    }

    [Fact]
    public void AddThrottlingSentinelOptions_ShouldThrowArgumentNullException_WhenBuilderIsNull()
    {
        Assert.Throws<ArgumentNullException>("builder", () => MvcBuilderExtensions.AddThrottlingSentinelOptions(null));
    }

    [Fact]
    public void AddUserAgentSentinelOptions_ShouldThrowArgumentNullException_WhenBuilderIsNull()
    {
        Assert.Throws<ArgumentNullException>("builder", () => MvcBuilderExtensions.AddUserAgentSentinelOptions(null));
    }

    [Fact]
    public void AddFaultDescriptorOptions_ShouldThrowArgumentNullException_WhenBuilderIsNull()
    {
        Assert.Throws<ArgumentNullException>("builder", () => MvcBuilderExtensions.AddFaultDescriptorOptions(null));
    }

    [Fact]
    public void AddHttpCacheableOptions_ShouldThrowArgumentNullException_WhenBuilderIsNull()
    {
        Assert.Throws<ArgumentNullException>("builder", () => MvcBuilderExtensions.AddHttpCacheableOptions(null));
    }

    [Fact]
    public void AddApiKeySentinelOptions_ShouldReturnBuilder_WithDefaultOptions()
    {
        var services = new ServiceCollection();
        var builder = services.AddMvc();

        var result = builder.AddApiKeySentinelOptions();

        Assert.Same(builder, result);
    }

    [Fact]
    public void AddThrottlingSentinelOptions_ShouldReturnBuilder_WithDefaultOptions()
    {
        var services = new ServiceCollection();
        var builder = services.AddMvc();

        var result = builder.AddThrottlingSentinelOptions();

        Assert.Same(builder, result);
    }

    [Fact]
    public void AddUserAgentSentinelOptions_ShouldReturnBuilder_WithDefaultOptions()
    {
        var services = new ServiceCollection();
        var builder = services.AddMvc();

        var result = builder.AddUserAgentSentinelOptions();

        Assert.Same(builder, result);
    }

    [Fact]
    public void AddFaultDescriptorOptions_ShouldReturnBuilder_WithDefaultOptions()
    {
        var services = new ServiceCollection();
        var builder = services.AddMvc();

        var result = builder.AddFaultDescriptorOptions();

        Assert.Same(builder, result);
    }

    [Fact]
    public void AddHttpCacheableOptions_ShouldReturnBuilder_WithDefaultOptions()
    {
        var services = new ServiceCollection();
        var builder = services.AddMvc();

        var result = builder.AddHttpCacheableOptions();

        Assert.Same(builder, result);
    }

    [Fact]
    public void AddFaultDescriptorOptions_ShouldReturnBuilder_WithCustomSensitivityDetails()
    {
        var services = new ServiceCollection();
        var builder = services.AddMvc();

        var result = builder.AddFaultDescriptorOptions(o =>
        {
            o.SensitivityDetails = FaultSensitivityDetails.All;
        });

        Assert.Same(builder, result);
    }

    [Fact]
    public void AddHttpCacheableOptions_ShouldReturnBuilder_WithCustomCacheControl()
    {
        var services = new ServiceCollection();
        var builder = services.AddMvc();

        var result = builder.AddHttpCacheableOptions(o =>
        {
            o.CacheControl.MaxAge = TimeSpan.FromMinutes(5);
        });

        Assert.Same(builder, result);
    }
}
