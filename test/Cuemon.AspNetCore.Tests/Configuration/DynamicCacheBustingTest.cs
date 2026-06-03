using System;
using System.Threading;
using Codebelt.Extensions.Xunit;
using Cuemon.Configuration;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.AspNetCore.Configuration
{
    public class DynamicCacheBustingTest : Test
    {
        public DynamicCacheBustingTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DynamicCacheBustingOptions_ShouldHaveDefaultValues()
        {
            var sut = new DynamicCacheBustingOptions();

            Assert.Equal(CasingMethod.LowerCase, sut.PreferredCasing);
            Assert.Equal(8, sut.PreferredLength);
            Assert.Equal(Alphanumeric.LettersAndNumbers, sut.PreferredCharacters);
            Assert.Equal(TimeSpan.FromHours(12), sut.TimeToLive);
        }

        [Fact]
        public void Version_ShouldReuseGeneratedValue_WhenTimeToLiveHasNotExpired()
        {
            var sut = new DynamicCacheBusting(Options.Create(new DynamicCacheBustingOptions
            {
                PreferredCasing = CasingMethod.UpperCase,
                PreferredCharacters = Alphanumeric.Letters,
                PreferredLength = 4,
                TimeToLive = TimeSpan.FromMinutes(5)
            }));

            var first = sut.Version;
            var changed = sut.UtcChanged;
            var second = sut.Version;

            Assert.Equal(6, first.Length);
            Assert.Equal(first, second);
            Assert.Equal(changed, sut.UtcChanged);
            Assert.Equal(first.ToUpperInvariant(), first);
        }

        [Fact]
        public void Version_ShouldRefreshGeneratedValue_WhenTimeToLiveHasExpired()
        {
            var sut = new DynamicCacheBusting(Options.Create(new DynamicCacheBustingOptions
            {
                PreferredLength = 6,
                TimeToLive = TimeSpan.Zero
            }));

            var first = sut.Version;
            var firstChanged = sut.UtcChanged;
            var second = first;
            for (var i = 0; i < 5 && second == first; i++)
            {
                Thread.Sleep(20);
                second = sut.Version;
            }

            Assert.NotEqual(first, second);
            Assert.True(sut.UtcChanged > firstChanged);
        }
    }
}
