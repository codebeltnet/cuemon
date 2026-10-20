using System;
using System.Globalization;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
    public class RangeTest : Test
    {
        public RangeTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Equals_ShouldHandleReferenceAndNullCases()
        {
            var start = new DateTime(2021, 3, 5, 6, 7, 8);
            var end = new DateTime(2021, 3, 6, 8, 10, 12);
            var sut = new DateTimeRange(start, end);

            Assert.True(sut.Equals(sut, sut));
            Assert.False(sut.Equals(null, sut));
            Assert.False(sut.Equals(sut, null));
        }

        [Fact]
        public void Equals_ShouldReturnFalseForDifferentDerivedTypes()
        {
            var start = new DateTime(2021, 3, 5, 6, 7, 8);
            var end = new DateTime(2021, 3, 6, 8, 10, 12);
            Range<DateTime> left = new DateTimeRange(start, end);
            Range<DateTime> right = new DateTimeRangeStub(start, end);

            Assert.False(left.Equals(left, right));
        }

        [Fact]
        public void Equals_ShouldCompareBoundaries()
        {
            var start = new DateTime(2021, 3, 5, 6, 7, 8);
            var end = new DateTime(2021, 3, 6, 8, 10, 12);
            var comparer = new DateTimeRange(start, end);
            var left = new DateTimeRange(start, end);
            var equal = new DateTimeRange(start, end);
            var different = new DateTimeRange(start, end.AddSeconds(1));

            Assert.True(comparer.Equals(left, equal));
            Assert.False(comparer.Equals(left, different));
        }

        [Fact]
        public void GetHashCode_ShouldXorBoundaries()
        {
            var start = new DateTime(2021, 3, 5, 6, 7, 8);
            var end = new DateTime(2021, 3, 6, 8, 10, 12);
            var sut = new DateTimeRange(start, end);

            Assert.Equal(start.GetHashCode() ^ end.GetHashCode(), sut.GetHashCode(sut));
        }

        [Fact]
        public void DateTimeRange_ShouldExposeDurationAndFormatUsingSortablePattern()
        {
            var start = new DateTime(2021, 3, 5, 6, 7, 8);
            var end = new DateTime(2021, 3, 6, 8, 10, 12);
            var sut = new DateTimeRange(start, end);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
            Assert.Equal(new TimeSpan(1, 2, 3, 4), sut.Duration);
            Assert.Equal("A duration of 01.02:03:04 between 2021-03-05T06:07:08 and 2021-03-06T08:10:12.", sut.ToString("s", CultureInfo.InvariantCulture));
            Assert.Equal("A duration of 01.02:03:04 between 2021-03-05T06:07:08 and 2021-03-06T08:10:12.", sut.ToString());
        }

        [Fact]
        public void TimeRange_ShouldUseDefaultDurationAndConstantPattern()
        {
            var start = new TimeSpan(6, 7, 8);
            var end = new TimeSpan(8, 10, 12);
            var sut = new TimeRange(start, end);

            Assert.Equal(start, sut.Start);
            Assert.Equal(end, sut.End);
            Assert.Equal(new TimeSpan(2, 3, 4), sut.Duration);
            Assert.Equal("A duration of 00.02:03:04 between 06:07:08 and 08:10:12.", sut.ToString("c", CultureInfo.InvariantCulture));
            Assert.Equal("A duration of 00.02:03:04 between 06:07:08 and 08:10:12.", sut.ToString());
        }

        [Fact]
        public void TimeRange_ShouldUseCustomDurationResolverWhenProvided()
        {
            var sut = new TimeRange(TimeSpan.FromHours(1), TimeSpan.FromHours(2), () => TimeSpan.FromHours(3));

            Assert.Equal(TimeSpan.FromHours(3), sut.Duration);
        }

        private sealed class DateTimeRangeStub : Range<DateTime>
        {
            public DateTimeRangeStub(DateTime start, DateTime end) : base(start, end, () => end.Subtract(start))
            {
            }
        }
    }
}
