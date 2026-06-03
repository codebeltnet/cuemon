using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Cuemon.AspNetCore.Mvc.Filters.Throttling;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Mvc.Filters;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters
{
    public class FilterCollectionExtensionsTest : Test
    {
        public FilterCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddHttpCacheable_ShouldAddOneFilterToCollection()
        {
            var sut = new FilterCollection();

            sut.AddHttpCacheable();

            Assert.Equal(1, sut.Count);
        }

        [Fact]
        public void AddFaultDescriptor_ShouldAddOneFilterToCollection()
        {
            var sut = new FilterCollection();

            sut.AddFaultDescriptor();

            Assert.Equal(1, sut.Count);
        }

        [Fact]
        public void AddServerTiming_ShouldAddOneFilterToCollection()
        {
            var sut = new FilterCollection();

            sut.AddServerTiming();

            Assert.Equal(1, sut.Count);
        }

        [Fact]
        public void AddUserAgentSentinel_ShouldAddOneFilterToCollection()
        {
            var sut = new FilterCollection();

            sut.AddUserAgentSentinel();

            Assert.Equal(1, sut.Count);
        }

        [Fact]
        public void AddThrottlingSentinel_ShouldAddOneFilterToCollection()
        {
            var sut = new FilterCollection();

            sut.AddThrottlingSentinel();

            Assert.Equal(1, sut.Count);
        }

        [Fact]
        public void AddApiKeySentinel_ShouldAddOneFilterToCollection()
        {
            var sut = new FilterCollection();

            sut.AddApiKeySentinel();

            Assert.Equal(1, sut.Count);
        }
    }
}
