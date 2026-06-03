using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Authentication
{
    public class NonceTrackerEntryTest : Test
    {
        public NonceTrackerEntryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldStoreCountAndCreatedTimestamp()
        {
            var created = DateTime.Parse("2024-01-01T00:00:00Z").ToUniversalTime();

            var sut = new NonceTrackerEntry(17, created);

            Assert.Equal(17, sut.Count);
            Assert.Equal(created, sut.Created);
        }
    }
}
