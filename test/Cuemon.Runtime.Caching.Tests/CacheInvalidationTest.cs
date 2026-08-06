using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Runtime;
using Xunit;

namespace Cuemon.Runtime.Caching;
public class CacheInvalidationTest : Test
{
    public CacheInvalidationTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Constructor_ShouldInitializeAbsoluteExpiration_WhenDateTimeIsProvided()
    {
        var absoluteExpiration = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Local);

        var sut = new CacheInvalidation(absoluteExpiration);

        Assert.Equal(absoluteExpiration.ToUniversalTime(), sut.AbsoluteExpiration);
        Assert.True(sut.UseAbsoluteExpiration);
        Assert.False(sut.UseSlidingExpiration);
        Assert.False(sut.UseDependency);
        Assert.Null(sut.SlidingExpiration);
        Assert.Null(sut.Dependencies);
    }

    [Fact]
    public void Constructor_ShouldInitializeDependencies_WhenSequenceIsProvided()
    {
        var dependencies = new List<IDependency> { new DependencyStub(), new DependencyStub() };

        var sut = new CacheInvalidation(dependencies);

        Assert.False(sut.UseAbsoluteExpiration);
        Assert.False(sut.UseSlidingExpiration);
        Assert.True(sut.UseDependency);
        Assert.Equal(2, sut.Dependencies.Count());
    }

    [Fact]
    public void Constructor_ShouldUseEmptyDependencies_WhenSequenceIsNull()
    {
        var sut = new CacheInvalidation((IEnumerable<IDependency>)null);

        Assert.False(sut.UseAbsoluteExpiration);
        Assert.False(sut.UseSlidingExpiration);
        Assert.False(sut.UseDependency);
        Assert.Empty(sut.Dependencies);
    }

    [Fact]
    public void Constructor_ShouldInitializeSlidingExpiration_WhenTimeSpanIsProvided()
    {
        var slidingExpiration = TimeSpan.FromMinutes(5);

        var sut = new CacheInvalidation(slidingExpiration);

        Assert.Equal(slidingExpiration, sut.SlidingExpiration);
        Assert.False(sut.UseAbsoluteExpiration);
        Assert.True(sut.UseSlidingExpiration);
        Assert.False(sut.UseDependency);
        Assert.Null(sut.AbsoluteExpiration);
        Assert.Null(sut.Dependencies);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenSlidingExpirationIsLessThanOrEqualToZero(long ticks)
    {
        var sut = Assert.Throws<ArgumentOutOfRangeException>(() => new CacheInvalidation(TimeSpan.FromTicks(ticks)));

        Assert.Equal("slidingExpiration", sut.ParamName);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenSlidingExpirationExceedsOneYear()
    {
        var sut = Assert.Throws<ArgumentOutOfRangeException>(() => new CacheInvalidation(TimeSpan.FromDays(366)));

        Assert.Equal("slidingExpiration", sut.ParamName);
    }

    private sealed class DependencyStub : IDependency
    {
        public event EventHandler<DependencyEventArgs> DependencyChanged;

        public DateTime? UtcLastModified { get; }

        public bool HasChanged { get; }

        public void Start()
        {
        }

        public Task StartAsync()
        {
            return Task.CompletedTask;
        }
    }
}
