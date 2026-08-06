using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Runtime;
using Xunit;

namespace Cuemon.Runtime.Caching;
public class CacheEntryTest : Test
{
    public CacheEntryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenKeyIsNull()
    {
        var sut = Assert.Throws<ArgumentNullException>(() => new CacheEntry(null, "value"));

        Assert.Equal("key", sut.ParamName);
    }

    [Fact]
    public void ToString_ShouldIncludeKeyAndValue_WhenCalled()
    {
        var cache = new SlimMemoryCache();
        var sut = new CacheEntry("key", "value", "ns");

        cache.Add(sut, new CacheInvalidation((IEnumerable<IDependency>)null));

        var result = sut.ToString();

        Assert.StartsWith("Cuemon.Runtime.Caching.CacheEntry", result);
        Assert.Contains("Key=key", result);
        Assert.Contains("Namespace=ns", result);
    }

    [Fact]
    public void CanExpire_ShouldReturnFalse_WhenInvalidationHasNoExpirationDetails()
    {
        var cache = new SlimMemoryCache();
        var sut = new CacheEntry("key", "value");

        cache.Add(sut, new CacheInvalidation((IEnumerable<IDependency>)null));

        Assert.False(sut.CanExpire);
        Assert.False(sut.HasExpired(DateTime.UtcNow));
    }

    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, true)]
    public void HasExpired_ShouldResolveAbsoluteExpiration(int tickOffset, bool expected)
    {
        var cache = new SlimMemoryCache();
        var sut = new CacheEntry("key", "value");
        var expiration = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        cache.Add(sut, new CacheInvalidation(expiration));

        var result = sut.HasExpired(expiration.AddTicks(tickOffset));

        Assert.True(sut.CanExpire);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, true)]
    public void HasExpired_ShouldResolveSlidingExpiration(long tickOffset, bool expected)
    {
        var cache = new SlimMemoryCache();
        var sut = new CacheEntry("key", "value");
        var slidingExpiration = TimeSpan.FromSeconds(30);

        cache.Add(sut, new CacheInvalidation(slidingExpiration));

        var result = sut.HasExpired(sut.Accessed.Add(slidingExpiration).AddTicks(tickOffset));

        Assert.True(sut.CanExpire);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void HasExpired_ShouldResolveDependencyState(bool hasChanged)
    {
        var cache = new SlimMemoryCache();
        var dependency = new DependencyStub(hasChanged);
        var sut = new CacheEntry("key", "value");

        cache.Add(sut, new CacheInvalidation(new[] { dependency }));

        var result = sut.HasExpired(DateTime.UtcNow);

        Assert.True(sut.CanExpire);
        Assert.Equal(hasChanged, result);
    }

    private sealed class DependencyStub : IDependency
    {
        public DependencyStub(bool hasChanged)
        {
            HasChanged = hasChanged;
        }

        public event EventHandler<DependencyEventArgs> DependencyChanged;

        public DateTime? UtcLastModified { get; private set; }

        public bool HasChanged { get; private set; }

        public void Start()
        {
        }

        public Task StartAsync()
        {
            return Task.CompletedTask;
        }

        public void SignalChanged()
        {
            UtcLastModified = DateTime.UtcNow;
            HasChanged = true;
            DependencyChanged?.Invoke(this, new DependencyEventArgs(UtcLastModified.Value));
        }
    }
}
