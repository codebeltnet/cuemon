using Codebelt.Extensions.Xunit;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace Cuemon
{
    public class DateSpanTest : Test
    {
        public DateSpanTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void GetHashCode_IsStableForSameInstance()
        {
            var span = new DateSpan();

            var h1 = span.GetHashCode();
            var h2 = span.GetHashCode();
            var h3 = span.GetHashCode();

            Assert.Equal(h1, h2);
            Assert.Equal(h1, h3);
        }

        [Fact]
        public void GetHashCode_EqualInstances_HaveSameHashCode()
        {
            var a = new DateSpan();
            var b = new DateSpan();

            Assert.True(a.Equals(b));
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ChangingUpperBoundary_UsuallyChangesHashCode()
        {
            var a = new DateSpan();
            var b = new DateSpan(DateTime.UtcNow, DateTime.UtcNow.Add(TimeSpan.FromDays(1)), new ChineseLunisolarCalendar());

            Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void GetHashCode_TwoSameTypeCalendars_MustBeEqual()
        {
            var utcNow = DateTime.UtcNow;

            var a = new DateSpan(utcNow, utcNow.Add(TimeSpan.FromDays(1)), new GregorianCalendar());
            var b = new DateSpan(utcNow, utcNow.Add(TimeSpan.FromDays(1)), new GregorianCalendar());

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void GetHashCode_TwoDifferentTypeCalendars_MustBeNotEqual()
        {
            var utcNow = DateTime.UtcNow;

            var a = new DateSpan(utcNow, utcNow.Add(TimeSpan.FromDays(1)), new JulianCalendar());
            var b = new DateSpan(utcNow, utcNow.Add(TimeSpan.FromDays(1)), new GregorianCalendar());

            Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void Parse_ShouldGetPastDecadeDifference_UsingIso8601String()
        {
            var start = new DateTime(2010, 1, 15, 22, 10, 28, 256).ToString("O");
            var end = new DateTime(2020, 3, 15, 17, 17, 17, 512).ToString("O");

            var span = DateSpan.Parse(start, end);

            Assert.Equal("10:121:3711:19:06:49.256", span.ToString());
            Assert.Equal(10, span.Years);
            Assert.Equal(121, span.Months);
            Assert.Equal(3711, span.Days);
            Assert.Equal(19, span.Hours);
            Assert.Equal(6, span.Minutes);
            Assert.Equal(49, span.Seconds);
            Assert.Equal(256, span.Milliseconds);

            Assert.Equal(10.163736044430246, span.TotalYears);
            Assert.Equal(121.02596734425681, span.TotalMonths);
            Assert.Equal(3711.7964034259257, span.TotalDays);
            Assert.Equal(89083.11368222222, span.TotalHours);
            Assert.Equal(5344986.820933334, span.TotalMinutes);
            Assert.Equal(320699209.256, span.TotalSeconds);
            Assert.Equal(320699209256, span.TotalMilliseconds);

            Assert.Equal(531, span.GetWeeks());

            TestOutput.WriteLine(span.ToString());
        }

        [Fact]
        public void Parse_ShouldGetOneMonthOfDifference_UsingIso8601String()
        {
            var start = new DateTime(2021, 3, 5).ToString("O");
            var end = new DateTime(2021, 4, 5).ToString("O");

            var span = DateSpan.Parse(start, end);

            Assert.Equal("0:01:31:00:00:00.0", span.ToString());
            Assert.Equal(0, span.Years);
            Assert.Equal(1, span.Months);
            Assert.Equal(31, span.Days);
            Assert.Equal(0, span.Hours);
            Assert.Equal(0, span.Minutes);
            Assert.Equal(0, span.Seconds);
            Assert.Equal(0, span.Milliseconds);

            Assert.Equal(0.08493150684931507, span.TotalYears);
            Assert.Equal(1, span.TotalMonths);
            Assert.Equal(31, span.TotalDays);
            Assert.Equal(744, span.TotalHours);
            Assert.Equal(44640, span.TotalMinutes);
            Assert.Equal(2678400, span.TotalSeconds);
            Assert.Equal(2678400000, span.TotalMilliseconds);

            Assert.Equal(6, span.GetWeeks());

            TestOutput.WriteLine(span.ToString());
        }

        [Fact]
        public void Parse_ShouldGetThreeMonthOfDifference_UsingIso8601String()
        {
            var start = new DateTime(2021, 3, 5).ToString("O");
            var end = new DateTime(2021, 6, 5).ToString("O");

            var span = DateSpan.Parse(start, end);

            Assert.Equal("0:03:92:00:00:00.0", span.ToString());
            Assert.Equal(0, span.Years);
            Assert.Equal(3, span.Months);
            Assert.Equal(92, span.Days);
            Assert.Equal(0, span.Hours);
            Assert.Equal(0, span.Minutes);
            Assert.Equal(0, span.Seconds);
            Assert.Equal(0, span.Milliseconds);

            Assert.Equal(0.25205479452054796, span.TotalYears);
            Assert.Equal(3, span.TotalMonths);
            Assert.Equal(92, span.TotalDays);
            Assert.Equal(2208, span.TotalHours);
            Assert.Equal(132480, span.TotalMinutes);
            Assert.Equal(7948800, span.TotalSeconds);
            Assert.Equal(7948800000, span.TotalMilliseconds);

            Assert.Equal(14, span.GetWeeks());

            TestOutput.WriteLine(span.ToString());
        }

        [Fact]
        public void Parse_ShouldGetSixMonthOfDifference_UsingIso8601String()
        {
            var start = new DateTime(2021, 3, 5).ToString("O");
            var end = new DateTime(2021, 9, 5).ToString("O");

            var span = DateSpan.Parse(start, end);

            Assert.Equal("0:06:184:00:00:00.0", span.ToString());
            Assert.Equal(0, span.Years);
            Assert.Equal(6, span.Months);
            Assert.Equal(184, span.Days);
            Assert.Equal(0, span.Hours);
            Assert.Equal(0, span.Minutes);
            Assert.Equal(0, span.Seconds);
            Assert.Equal(0, span.Milliseconds);

            Assert.Equal(0.5041095890410959, span.TotalYears);
            Assert.Equal(6, span.TotalMonths);
            Assert.Equal(184, span.TotalDays);
            Assert.Equal(4416, span.TotalHours);
            Assert.Equal(264960, span.TotalMinutes);
            Assert.Equal(15897600, span.TotalSeconds);
            Assert.Equal(15897600000, span.TotalMilliseconds);

            Assert.Equal(27, span.GetWeeks());

            TestOutput.WriteLine(span.ToString());
        }

        [Fact]
        public void Parse_ShouldGetNineMonthOfDifference_UsingIso8601String()
        {
            var start = new DateTime(2021, 3, 5).ToString("O");
            var end = new DateTime(2021, 12, 5).ToString("O");

            var span = DateSpan.Parse(start, end);

            Assert.Equal("0:09:275:00:00:00.0", span.ToString());
            Assert.Equal(0, span.Years);
            Assert.Equal(9, span.Months);
            Assert.Equal(275, span.Days);
            Assert.Equal(0, span.Hours);
            Assert.Equal(0, span.Minutes);
            Assert.Equal(0, span.Seconds);
            Assert.Equal(0, span.Milliseconds);

            Assert.Equal(0.7534246575342466, span.TotalYears);
            Assert.Equal(9, span.TotalMonths);
            Assert.Equal(275, span.TotalDays);
            Assert.Equal(6600, span.TotalHours);
            Assert.Equal(396000, span.TotalMinutes);
            Assert.Equal(23760000, span.TotalSeconds);
            Assert.Equal(23760000000, span.TotalMilliseconds);

            Assert.Equal(40, span.GetWeeks());

            TestOutput.WriteLine(span.ToString());
        }

        [Fact]
        public void DateSpan_ShouldHandleOverlapInMonthAndDays()
        {
            var sut0 = new DateTime(2020, 5, 12);
            var sut1 = new[]
            {
                new DateTime(2021, 5, 10),
                new DateTime(2021, 5, 11),
                new DateTime(2021, 5, 12),
                new DateTime(2021, 5, 13),
                new DateTime(2021, 5, 14)
            };

            Assert.Collection(sut1,
                dt => Assert.Equal("0:11:363:00:00:00.0", new DateSpan(sut0, dt).ToString()),
                dt => Assert.Equal("0:11:364:00:00:00.0", new DateSpan(sut0, dt).ToString()),
                dt => Assert.Equal("1:12:365:00:00:00.0", new DateSpan(sut0, dt).ToString()),
                dt => Assert.Equal("1:12:366:00:00:00.0", new DateSpan(sut0, dt).ToString()),
                dt => Assert.Equal("1:12:367:00:00:00.0", new DateSpan(sut0, dt).ToString()));
        }

        [Fact]
        public void DateSpan_ShouldGetNineMonthOfDifference_UsingChineseLunisolarCalendar()
        {
            var span = new DateSpan(new DateTime(2021, 3, 5), new DateTime(2021, 12, 5), new ChineseLunisolarCalendar());

            Assert.Equal("0:09:266:00:00:00.0", span.ToString());
            Assert.Equal(0, span.Years);
            Assert.Equal(9, span.Months);
            Assert.Equal(266, span.Days);
            Assert.Equal(0, span.Hours);
            Assert.Equal(0, span.Minutes);
            Assert.Equal(0, span.Seconds);
            Assert.Equal(0, span.Milliseconds);

            Assert.Equal(0.751412429378531, span.TotalYears);
            Assert.Equal(9, span.TotalMonths);
            Assert.Equal(266, span.TotalDays);
            Assert.Equal(6384, span.TotalHours);
            Assert.Equal(383040, span.TotalMinutes);
            Assert.Equal(22982400, span.TotalSeconds);
            Assert.Equal(22982400000, span.TotalMilliseconds);

            Assert.Equal(40, span.GetWeeks());

            TestOutput.WriteLine(span.ToString());
        }

        [Fact]
        public void Parse_ShouldGetTwelveMonthOfDifference_UsingIso8601String()
        {
            var start = new DateTime(2021, 3, 5).ToString("O");
            var end = new DateTime(2022, 3, 5).ToString("O");

            var span = DateSpan.Parse(start, end);

            Assert.Equal("1:12:365:00:00:00.0", span.ToString());
            Assert.Equal(1, span.Years);
            Assert.Equal(12, span.Months);
            Assert.Equal(365, span.Days);
            Assert.Equal(0, span.Hours);
            Assert.Equal(0, span.Minutes);
            Assert.Equal(0, span.Seconds);
            Assert.Equal(0, span.Milliseconds);

            Assert.Equal(1, span.TotalYears);
            Assert.Equal(12, span.TotalMonths);
            Assert.Equal(365, span.TotalDays);
            Assert.Equal(8760, span.TotalHours);
            Assert.Equal(525600, span.TotalMinutes);
            Assert.Equal(31536000, span.TotalSeconds);
            Assert.Equal(31536000000, span.TotalMilliseconds);

            Assert.Equal(53, span.GetWeeks());

            TestOutput.WriteLine(span.ToString());
        }

        [Fact]
        public void Parse_ShouldGetLeapYear_UsingIso8601String()
        {
            var start = new DateTime(2020, 1, 1).ToString("O");
            var end = new DateTime(2020, 12, 31).ToString("O");

            var span = DateSpan.Parse(start, end);

            Assert.Equal("0:11:365:00:00:00.0", span.ToString());
            Assert.Equal(0, span.Years);
            Assert.Equal(11, span.Months);
            Assert.Equal(365, span.Days);
            Assert.Equal(0, span.Hours);
            Assert.Equal(0, span.Minutes);
            Assert.Equal(0, span.Seconds);
            Assert.Equal(0, span.Milliseconds);

            Assert.Equal(0.9972677595628415, span.TotalYears);
            Assert.Equal(11, span.TotalMonths);
            Assert.Equal(365, span.TotalDays);
            Assert.Equal(8760, span.TotalHours);
            Assert.Equal(525600, span.TotalMinutes);
            Assert.Equal(31536000, span.TotalSeconds);
            Assert.Equal(31536000000, span.TotalMilliseconds);

            Assert.Equal(53, span.GetWeeks());

            TestOutput.WriteLine(span.ToString());
        }

        [Fact]
        public void Parse_ShouldGetTwelveMonthOfDifferenceWithinLeapYear_UsingIso8601String()
        {
            var start = new DateTime(2020, 1, 1).ToString("O");
            var end = new DateTime(2021, 1, 1).ToString("O");

            var span = DateSpan.Parse(start, end);

            Assert.Equal("1:12:366:00:00:00.0", span.ToString());
            Assert.Equal(1, span.Years);
            Assert.Equal(12, span.Months);
            Assert.Equal(366, span.Days);
            Assert.Equal(0, span.Hours);
            Assert.Equal(0, span.Minutes);
            Assert.Equal(0, span.Seconds);
            Assert.Equal(0, span.Milliseconds);

            Assert.Equal(1, span.TotalYears);
            Assert.Equal(12, span.TotalMonths);
            Assert.Equal(366, span.TotalDays);
            Assert.Equal(8784, span.TotalHours);
            Assert.Equal(527040, span.TotalMinutes);
            Assert.Equal(31622400, span.TotalSeconds);
            Assert.Equal(31622400000, span.TotalMilliseconds);

            Assert.Equal(53, span.GetWeeks());

            TestOutput.WriteLine(span.ToString());
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullExceptionWhenCalendarIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new DateSpan(DateTime.UtcNow, DateTime.UtcNow, null));
        }

        [Fact]
        public void Constructor_WithSingleDate_ShouldUseTodayAsUpperBoundary()
        {
            var today = DateTime.Today;
            var sut = new DateSpan(today);
            var expected = new DateSpan(today, today);

            Assert.Equal(expected, sut);
            Assert.Equal(0, sut.Years);
            Assert.Equal(0, sut.Months);
            Assert.Equal(0, sut.Days);
        }

        [Fact]
        public void Constructor_ShouldNormalizeReversedRange()
        {
            var earlier = new DateTime(2021, 3, 5);
            var later = new DateTime(2021, 4, 5);

            var sut = new DateSpan(later, earlier);
            var expected = new DateSpan(earlier, later);

            Assert.Equal(expected, sut);
            Assert.True(sut == expected);
            Assert.False(sut != expected);
        }

        [Fact]
        public void Constructor_ShouldAdjustYearWhenFullYearHasNotElapsed()
        {
            var sut = new DateSpan(new DateTime(2020, 3, 1), new DateTime(2021, 2, 28));

            Assert.Equal(0, sut.Years);
            Assert.Equal(11, sut.Months);
            Assert.Equal(364, sut.Days);
        }

        [Fact]
        public void Constructor_ShouldCapturePartialMonthTimeDifference()
        {
            var sut = new DateSpan(new DateTime(2021, 1, 31, 23, 0, 0, 0), new DateTime(2021, 2, 1, 22, 0, 0, 500));

            Assert.Equal(0, sut.Years);
            Assert.Equal(0, sut.Months);
            Assert.Equal(0, sut.Days);
            Assert.Equal(23, sut.Hours);
            Assert.Equal(0, sut.Minutes);
            Assert.Equal(0, sut.Seconds);
            Assert.Equal(500, sut.Milliseconds);
            Assert.Equal(TimeSpan.FromHours(23).Add(TimeSpan.FromMilliseconds(500)).Ticks, sut.Ticks);
        }

        [Fact]
        public void Parse_WithSingleDate_ShouldUseTodayAsUpperBoundary()
        {
            var today = DateTime.Today;
            var sut = DateSpan.Parse(today.ToString("s", CultureInfo.InvariantCulture));
            var expected = new DateSpan(today, today);

            Assert.Equal(expected, sut);
        }

        [Fact]
        public void Parse_WithCulture_ShouldUseProvidedCultureCalendar()
        {
            var culture = CultureInfo.InvariantCulture;
            var start = new DateTime(2021, 3, 5).ToString("s", culture);
            var end = new DateTime(2021, 4, 5).ToString("s", culture);

            var sut = DateSpan.Parse(start, end, culture);
            var expected = new DateSpan(new DateTime(2021, 3, 5), new DateTime(2021, 4, 5), culture.Calendar);

            Assert.Equal(expected, sut);
        }

        [Fact]
        public void Equals_ShouldReturnFalseForNonDateSpanObject()
        {
            Assert.False(new DateSpan().Equals("DateSpan"));
        }

        [Fact]
        public void Equals_ShouldReturnFalseForDifferentCalendarTypes()
        {
            var start = new DateTime(2021, 3, 5);
            var end = new DateTime(2021, 4, 5);
            var left = new DateSpan(start, end, new GregorianCalendar());
            var right = new DateSpan(start, end, new JulianCalendar());

            Assert.False(left.Equals(right));
            Assert.True(left != right);
        }

        [Theory]
        [InlineData(2021, 3, 1, 2021, 3, 3, 2)]
        [InlineData(2021, 3, 5, 2021, 3, 7, 2)]
        public void GetWeeks_ShouldHandleShortRanges(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay, int expected)
        {
            var sut = new DateSpan(new DateTime(startYear, startMonth, startDay), new DateTime(endYear, endMonth, endDay));

            Assert.Equal(expected, sut.GetWeeks());
        }

        [Theory]
        [MemberData(nameof(GetSupportedCalendars))]
        public void Constructor_ShouldSupportCalendarImplementations(DateTime start, DateTime end, Func<Calendar> calendarFactory)
        {
            var sut = new DateSpan(start, end, calendarFactory());
            var expected = new DateSpan(start, end, calendarFactory());

            Assert.Equal(expected, sut);
            Assert.Equal(1, sut.Days);
        }

        public static IEnumerable<object[]> GetSupportedCalendars()
        {
            yield return new object[] { new DateTime(2021, 3, 5), new DateTime(2021, 3, 6), (Func<Calendar>)(() => new ChineseLunisolarCalendar()) };
            yield return new object[] { new DateTime(31, 3, 5), new DateTime(31, 3, 6), (Func<Calendar>)(() => new JapaneseLunisolarCalendar()) };
            yield return new object[] { new DateTime(2021, 3, 5), new DateTime(2021, 3, 6), (Func<Calendar>)(() => new KoreanLunisolarCalendar()) };
            yield return new object[] { new DateTime(100, 3, 5), new DateTime(100, 3, 6), (Func<Calendar>)(() => new TaiwanLunisolarCalendar()) };
            yield return new object[] { new DateTime(2021, 3, 5), new DateTime(2021, 3, 6), (Func<Calendar>)(() => new GregorianCalendar()) };
            yield return new object[] { new DateTime(5343, 3, 5), new DateTime(5343, 3, 6), (Func<Calendar>)(() => new HebrewCalendar()) };
            yield return new object[] { new DateTime(2021, 3, 5), new DateTime(2021, 3, 6), (Func<Calendar>)(() => new HijriCalendar()) };
            yield return new object[] { new DateTime(2021, 3, 5), new DateTime(2021, 3, 6), (Func<Calendar>)(() => new JapaneseCalendar()) };
            yield return new object[] { new DateTime(2021, 3, 5), new DateTime(2021, 3, 6), (Func<Calendar>)(() => new JulianCalendar()) };
            yield return new object[] { new DateTime(2334, 3, 5), new DateTime(2334, 3, 6), (Func<Calendar>)(() => new KoreanCalendar()) };
            yield return new object[] { new DateTime(2021, 3, 5), new DateTime(2021, 3, 6), (Func<Calendar>)(() => new PersianCalendar()) };
            yield return new object[] { new DateTime(2021, 3, 5), new DateTime(2021, 3, 6), (Func<Calendar>)(() => new TaiwanCalendar()) };
            yield return new object[] { new DateTime(2021, 3, 5), new DateTime(2021, 3, 6), (Func<Calendar>)(() => new ThaiBuddhistCalendar()) };
            yield return new object[] { new DateTime(1400, 3, 5), new DateTime(1400, 3, 6), (Func<Calendar>)(() => new UmAlQuraCalendar()) };
            yield return new object[] { new DateTime(2021, 3, 5), new DateTime(2021, 3, 6), (Func<Calendar>)(() => new DelegatingCalendar()) };
        }

        private sealed class DelegatingCalendar : Calendar
        {
            private readonly GregorianCalendar _inner = new GregorianCalendar();

            public override DateTime MinSupportedDateTime => _inner.MinSupportedDateTime;

            public override DateTime MaxSupportedDateTime => _inner.MaxSupportedDateTime;

            public override CalendarAlgorithmType AlgorithmType => _inner.AlgorithmType;

            public override int[] Eras => _inner.Eras;

            public override int TwoDigitYearMax
            {
                get => _inner.TwoDigitYearMax;
                set => _inner.TwoDigitYearMax = value;
            }

            public override DateTime AddMonths(DateTime time, int months)
            {
                return _inner.AddMonths(time, months);
            }

            public override DateTime AddYears(DateTime time, int years)
            {
                return _inner.AddYears(time, years);
            }

            public override int GetDayOfMonth(DateTime time)
            {
                return _inner.GetDayOfMonth(time);
            }

            public override DayOfWeek GetDayOfWeek(DateTime time)
            {
                return _inner.GetDayOfWeek(time);
            }

            public override int GetDayOfYear(DateTime time)
            {
                return _inner.GetDayOfYear(time);
            }

            public override int GetDaysInMonth(int year, int month, int era)
            {
                return _inner.GetDaysInMonth(year, month, era);
            }

            public override int GetDaysInYear(int year, int era)
            {
                return _inner.GetDaysInYear(year, era);
            }

            public override int GetEra(DateTime time)
            {
                return _inner.GetEra(time);
            }

            public override int GetMonth(DateTime time)
            {
                return _inner.GetMonth(time);
            }

            public override int GetMonthsInYear(int year, int era)
            {
                return _inner.GetMonthsInYear(year, era);
            }

            public override int GetYear(DateTime time)
            {
                return _inner.GetYear(time);
            }

            public override bool IsLeapDay(int year, int month, int day, int era)
            {
                return _inner.IsLeapDay(year, month, day, era);
            }

            public override bool IsLeapMonth(int year, int month, int era)
            {
                return _inner.IsLeapMonth(year, month, era);
            }

            public override bool IsLeapYear(int year, int era)
            {
                return _inner.IsLeapYear(year, era);
            }

            public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
            {
                return _inner.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
            }

            public override int ToFourDigitYear(int year)
            {
                return _inner.ToFourDigitYear(year);
            }
        }
    }
}
