using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Configuration;
using Cuemon.Extensions.AspNetCore.Mvc.Assets;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable;
public class CacheableAsyncResultFilterExtensionsTest : Test
{
    public CacheableAsyncResultFilterExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddFilter_ShouldAddFilter()
    {
        var sut1 = new List<ICacheableAsyncResultFilter>();
        var sut2 = new List<ICacheableAsyncResultFilter>(sut1);

        sut2.AddFilter<FakeCacheableFilter>();

        Assert.True(sut1.Count == 0, "sut1.Count == 0");
        Assert.True(sut2.Count == 1, "sut2.Count == 1");
        Assert.IsAssignableFrom<ICacheableAsyncResultFilter>(sut2.Single());
        Assert.ThrowsAsync<NotImplementedException>(() => sut2.Single().OnResultExecutionAsync(null, null));
    }

    [Fact]
    public void AddFilter_ShouldAddFilterWithOptions()
    {
        var sut1 = new List<ICacheableAsyncResultFilter>();
        var sut2 = new List<ICacheableAsyncResultFilter>(sut1);

        sut2.AddFilter<ConfigurableFakeCacheableFilter, FakeCacheableOptions>();

        Assert.True(sut1.Count == 0, "sut1.Count == 0");
        Assert.True(sut2.Count == 1, "sut2.Count == 1");
        Assert.IsAssignableFrom<ICacheableAsyncResultFilter>(sut2.Single());
        Assert.IsAssignableFrom<IConfigurable<FakeCacheableOptions>>(sut2.Single());
        Assert.ThrowsAsync<NotImplementedException>(() => sut2.Single().OnResultExecutionAsync(null, null));
        Assert.Equal("Hello!", sut2.Single().As<IConfigurable<FakeCacheableOptions>>()?.Options.Greeting);
    }

    [Fact]
    public void AddFilter_ShouldFailWhenAddingFilterWithOptions()
    {
        var sut1 = new List<ICacheableAsyncResultFilter>();

        Assert.Throws<MissingMethodException>(() => sut1.AddFilter<FakeCacheableFilter, FakeCacheableOptions>());
    }

    [Fact]
    public void InsertFilter_ShouldInsertFilter()
    {
        var sut1 = new List<ICacheableAsyncResultFilter>();
        var sut2 = new List<ICacheableAsyncResultFilter>(sut1);

        sut2.AddFilter<ConfigurableFakeCacheableFilter, FakeCacheableOptions>();
        sut2.InsertFilter<FakeCacheableFilter>(0);

        Assert.True(sut1.Count == 0, "sut1.Count == 0");
        Assert.True(sut2.Count == 2, "sut2.Count == 2");
        Assert.IsType<FakeCacheableFilter>(sut2.First());
        Assert.IsType<ConfigurableFakeCacheableFilter>(sut2.Last());
    }

    [Fact]
    public void InsertFilter_ShouldInsertFilterWithOptions()
    {
        var sut1 = new List<ICacheableAsyncResultFilter>();
        var sut2 = new List<ICacheableAsyncResultFilter>(sut1);

        sut2.AddFilter<FakeCacheableFilter>();
        sut2.InsertFilter<ConfigurableFakeCacheableFilter, FakeCacheableOptions>(0);

        Assert.True(sut1.Count == 0, "sut1.Count == 0");
        Assert.True(sut2.Count == 2, "sut2.Count == 2");
        Assert.IsType<ConfigurableFakeCacheableFilter>(sut2.First());
        Assert.IsType<FakeCacheableFilter>(sut2.Last());
    }

    [Fact]
    public void AddEntityTagHeader_ShouldAddFilterWithDefaultOptions()
    {
        var sut1 = new List<ICacheableAsyncResultFilter>();
        var sut2 = new List<ICacheableAsyncResultFilter>(sut1);

        sut2.AddEntityTagHeader();

        Assert.True(sut1.Count == 0, "sut1.Count == 0");
        Assert.True(sut2.Count == 1, "sut2.Count == 1");
        Assert.IsAssignableFrom<ICacheableAsyncResultFilter>(sut2.Single());
        Assert.IsType<HttpEntityTagHeaderFilter>(sut2.Single());
        Assert.IsAssignableFrom<IConfigurable<HttpEntityTagHeaderOptions>>(sut2.Single());
        Assert.False(sut2.Single().As<IConfigurable<HttpEntityTagHeaderOptions>>()?.Options.UseEntityTagResponseParser);
        Assert.True(sut2.Single().As<IConfigurable<HttpEntityTagHeaderOptions>>()?.Options.HasEntityTagProvider);
        Assert.True(sut2.Single().As<IConfigurable<HttpEntityTagHeaderOptions>>()?.Options.HasEntityTagResponseParser);
    }

    [Fact]
    public void AddLastModifiedHeader_ShouldAddFilterWithDefaultOptions()
    {
        var sut1 = new List<ICacheableAsyncResultFilter>();
        var sut2 = new List<ICacheableAsyncResultFilter>(sut1);

        sut2.AddLastModifiedHeader();

        Assert.True(sut1.Count == 0, "sut1.Count == 0");
        Assert.True(sut2.Count == 1, "sut2.Count == 1");
        Assert.IsAssignableFrom<ICacheableAsyncResultFilter>(sut2.Single());
        Assert.IsType<HttpLastModifiedHeaderFilter>(sut2.Single());
        Assert.IsAssignableFrom<IConfigurable<HttpLastModifiedHeaderOptions>>(sut2.Single());
        Assert.True(sut2.Single().As<IConfigurable<HttpLastModifiedHeaderOptions>>()?.Options.HasLastModifiedProvider);
    }
}
