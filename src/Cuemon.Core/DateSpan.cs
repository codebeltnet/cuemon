using System;
using System.Globalization;
using System.Linq;
using Cuemon.Collections.Generic;
using Cuemon.Security;

namespace Cuemon
{
    /// <summary>
    /// Represents a <see cref="DateTime"/> interval between two <see cref="DateTime"/> values.
    /// </summary>
    public readonly struct DateSpan : IEquatable<DateSpan>
    {
        private readonly DateTime _lower;
        private readonly DateTime _upper;
        private readonly Calendar _calendar;
        private readonly ulong _calendarId;
        private static readonly ulong Hash = (ulong)new FowlerNollVo64().OffsetBasis;
        private static readonly ulong Prime = (ulong)new FowlerNollVo64().Prime;

        /// <summary>
		/// Initializes a new instance of the <see cref="DateSpan"/> structure with a default <see cref="DateTime"/> value set to <see cref="DateTime.Today"/>.
		/// </summary>
		/// <param name="start">A <see cref="DateTime"/> value for the <see cref="DateSpan"/> calculation.</param>
		public DateSpan(DateTime start) : this(start, DateTime.Today)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateSpan"/> structure with a default <see cref="Calendar"/> value from the <see cref="CultureInfo.InvariantCulture"/> class.
        /// </summary>
        /// <param name="start">A <see cref="DateTime"/> value for the <see cref="DateSpan"/> calculation.</param>
        /// <param name="end">A <see cref="DateTime"/> value for the <see cref="DateSpan"/> calculation.</param>
        public DateSpan(DateTime start, DateTime end) : this(start, end, CultureInfo.InvariantCulture.Calendar)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateSpan"/> structure.
        /// </summary>
        /// <param name="start">A <see cref="DateTime"/> value for the <see cref="DateSpan"/> calculation.</param>
        /// <param name="end">A <see cref="DateTime"/> value for the <see cref="DateSpan"/> calculation.</param>
        /// <param name="calendar">The <see cref="Calendar"/> that applies to this <see cref="DateSpan"/>.</param>
        public DateSpan(DateTime start, DateTime end, Calendar calendar) : this()
        {
            Validator.ThrowIfNull(calendar);

            _lower = Arguments.ToEnumerableOf(start, end).Min();
            _upper = Arguments.ToEnumerableOf(start, end).Max();
            _calendar = calendar;

            _calendarId = _calendar switch
            {
                ChineseLunisolarCalendar => 1,
                JapaneseLunisolarCalendar => 2,
                KoreanLunisolarCalendar => 3,
                TaiwanLunisolarCalendar => 4,
                EastAsianLunisolarCalendar => 5,
                GregorianCalendar => 6,
                HebrewCalendar => 7,
                HijriCalendar => 8,
                JapaneseCalendar => 9,
                JulianCalendar => 10,
                KoreanCalendar => 11,
                PersianCalendar => 12,
                TaiwanCalendar => 13,
                ThaiBuddhistCalendar => 14,
                UmAlQuraCalendar => 15,
                _ => (ulong)_calendar.GetType().FullName!.GetHashCode()
            };

            var lower = _lower;
            var upper = _upper;

            var years = GetYears(upper, lower, out var adjustYearsMinusOne);

            var daysPerYearsAverage = CalculateAverageDaysPerYear(lower, upper, years);

            CalculateDifference(ref lower, upper, out var months, out var days, out var hours, out var milliseconds);

            var averageDaysPerMonth = months == 0 ? days : Convert.ToDouble(days) / Convert.ToDouble(months);
            var remainder = new TimeSpan(days, hours, 0, 0, milliseconds);

            Years = adjustYearsMinusOne ? --years : years;
            Months = months;
            Days = days;
            Hours = remainder.Hours;
            Minutes = remainder.Minutes;
            Seconds = remainder.Seconds;
            Milliseconds = remainder.Milliseconds;
            Ticks = remainder.Ticks;

            TotalYears = remainder.TotalDays / daysPerYearsAverage;
            TotalMonths = remainder.TotalDays / averageDaysPerMonth;
            TotalDays = remainder.TotalDays;
            TotalHours = remainder.TotalHours;
            TotalMinutes = remainder.TotalMinutes;
            TotalSeconds = remainder.TotalSeconds;
            TotalMilliseconds = remainder.TotalMilliseconds;
        }

        private static int GetYears(DateTime upper, DateTime lower, out bool adjustYearsMinusOne)
        {
            adjustYearsMinusOne = false;

            if (upper.Year == lower.Year) { return 0; }

            var years = 0;

            while (lower.Year < upper.Year)
            {
                lower = lower.AddYears(1);
                years++;
            }

            adjustYearsMinusOne = lower > upper;

            return years;
        }

        private double CalculateAverageDaysPerYear(DateTime lower, DateTime upper, int years)
        {
            var daysPerYears = 0;
            var y = lower.Year;
            do
            {
                daysPerYears += _calendar.GetDaysInYear(y);
                y++;
            } while (y < upper.Year);

            return years == 0 ? daysPerYears : Convert.ToDouble(daysPerYears) / Convert.ToDouble(years);
        }

        private void CalculateDifference(ref DateTime lower, DateTime upper, out int months, out int days, out int hours, out int milliseconds)
        {
            months = 0;
            days = 0;
            hours = 0;
            milliseconds = 0;

            while (!lower.Year.Equals(upper.Year) || !lower.Month.Equals(upper.Month))
            {
                var daysPerMonth = _calendar.GetDaysInMonth(lower.Year, lower.Month);
                var peekNextLower = lower.AddMonths(1);
                if (peekNextLower > upper)
                {
                    CalculatePartialMonthDifference(ref lower, upper, ref days, ref hours, ref milliseconds);
                }
                else
                {
                    days += daysPerMonth;
                    lower = lower.AddMonths(1);
                    months++;
                }
            }

            while (!lower.Day.Equals(upper.Day))
            {
                days++;
                lower = lower.AddDays(1);
            }
        }

        private static void CalculatePartialMonthDifference(ref DateTime lower, DateTime upper, ref int days, ref int hours, ref int milliseconds)
        {
            while (!lower.Month.Equals(upper.Month) || !lower.Day.Equals(upper.Day))
            {
                days++;
                lower = lower.AddDays(1);
            }

            if (lower > upper)
            {
                lower = lower.AddDays(-1);
                days--;

                CalculateTimeDifference(ref lower, upper, ref hours, ref milliseconds);
            }
        }

        private static void CalculateTimeDifference(ref DateTime lower, DateTime upper, ref int hours, ref int milliseconds)
        {
            while (!lower.Hour.Equals(upper.Hour))
            {
                hours++;
                lower = lower.AddHours(1);
            }

            while (!lower.Minute.Equals(upper.Minute) || !lower.Second.Equals(upper.Second) || !lower.Millisecond.Equals(upper.Millisecond))
            {
                milliseconds++;
                lower = lower.AddMilliseconds(1);
            }
        }

        /// <summary>
		/// Calculates the number of weeks represented by the current <see cref="DateSpan"/> structure.
		/// </summary>
		/// <value>Calculates the number of weeks represented by the current <see cref="DateSpan"/> structure.</value>
		public int GetWeeks()
        {
            var range = _upper.Subtract(_lower);
            int totalDays;
            if (range.Days <= 7)
            {
                totalDays = _lower.DayOfWeek > _upper.DayOfWeek ? 2 : 1;
            }
            else
            {
                totalDays = range.Days - 7 + (int)_lower.DayOfWeek;
            }
            var weeks = 1 + ((totalDays + 6) / 7);
            return weeks;
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = Hash;
                var prime = Prime;

                hash ^= (ulong)_upper.Ticks;
                hash *= prime;

                hash ^= (ulong)_lower.Ticks;
                hash *= prime;

                hash ^= _calendarId;
                hash *= prime;

                return (int)(hash ^ (hash >> 32));
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is not DateSpan span) { return false; }
            return Equals(span);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>. </returns>
        public bool Equals(DateSpan other)
        {
            if ((_upper != other._upper) || (_calendarId != other._calendarId)) { return false; }
            return (_lower == other._lower);
        }

        /// <summary>
        /// Indicates whether two <see cref="DateSpan"/> instances are equal.
        /// </summary>
        /// <param name="dateSpan1">The first date interval to compare.</param>
        /// <param name="dateSpan2">The second date interval to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="dateSpan1"/> and <paramref name="dateSpan2"/> are equal; otherwise, false. </returns>
        public static bool operator ==(DateSpan dateSpan1, DateSpan dateSpan2)
        {
            return dateSpan1.Equals(dateSpan2);
        }

        /// <summary>
        /// Indicates whether two <see cref="DateSpan"/> instances are not equal.
        /// </summary>
        /// <param name="dateSpan1">The first date interval to compare.</param>
        /// <param name="dateSpan2">The second date interval to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="dateSpan1"/> and <paramref name="dateSpan2"/> are not equal; otherwise, false.</returns>
        public static bool operator !=(DateSpan dateSpan1, DateSpan dateSpan2)
        {
            return !dateSpan1.Equals(dateSpan2);
        }

        /// <summary>
        /// Constructs a new <see cref="DateSpan"/> object from a date and time interval specified in a string.
        /// </summary>
        /// <param name="start">A string that specifies the starting date and time value for the <see cref="DateSpan"/> interval.</param>
        /// <returns>A <see cref="DateSpan"/> that corresponds to <paramref name="start"/> and <see cref="DateTime.Today"/> for the last part of the interval.</returns>
        public static DateSpan Parse(string start)
        {
            return Parse(start, DateTime.Today.ToString("s", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Constructs a new <see cref="DateSpan"/> object from a date and time interval specified in a string.
        /// </summary>
        /// <param name="start">A string that specifies the starting date and time value for the <see cref="DateSpan"/> interval.</param>
        /// <param name="end">A string that specifies the ending date and time value for the <see cref="DateSpan"/> interval.</param>
        /// <returns>A <see cref="DateSpan"/> that corresponds to <paramref name="start"/> and <paramref name="end"/> of the interval.</returns>
        public static DateSpan Parse(string start, string end)
        {
            return Parse(start, end, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Constructs a new <see cref="DateSpan"/> object from a date and time interval specified in a string.
        /// </summary>
        /// <param name="start">A string that specifies the starting date and time value for the <see cref="DateSpan"/> interval.</param>
        /// <param name="end">A string that specifies the ending date and time value for the <see cref="DateSpan"/> interval.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> to resolve a <see cref="Calendar"/> object from.</param>
        /// <returns>A <see cref="DateSpan"/> that corresponds to <paramref name="start"/> and <paramref name="end"/> of the interval.</returns>
        public static DateSpan Parse(string start, string end, CultureInfo culture)
        {
            return new DateSpan(DateTime.Parse(start, culture), DateTime.Parse(end, culture), culture.Calendar);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateSpan"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/> representation of the current <see cref="DateSpan"/> value. 
        /// </returns>
        /// <remarks>The returned string has the following format: y*:MM:dd:hh:mm:ss.f*, where y* is the actual calculated years and f* is the actual calculated milliseconds.</remarks>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1:D2}:{2:D2}:{3:D2}:{4:D2}:{5:D2}.{6}", Years, Months, Days, Hours, Minutes, Seconds, Milliseconds);
        }

        /// <summary>
        /// Gets the number of days represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of days represented by the current <see cref="DateSpan"/> structure.</value>
        public int Days { get; }

        /// <summary>
        /// Gets the total number of days represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of days represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalDays { get; }

        /// <summary>
        /// Gets the number of hours represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of hours represented by the current <see cref="DateSpan"/> structure.</value>
        public int Hours { get; }

        /// <summary>
        /// Gets the total number of hours represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of hours represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalHours { get; }

        /// <summary>
        /// Gets the number of milliseconds represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of milliseconds represented by the current <see cref="DateSpan"/> structure.</value>
        public int Milliseconds { get; }

        /// <summary>
        /// Gets the total number of milliseconds represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of milliseconds represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalMilliseconds { get; }

        /// <summary>
        /// Gets the number of minutes represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of minutes represented by the current <see cref="DateSpan"/> structure.</value>
        public int Minutes { get; }

        /// <summary>
        /// Gets the total number of minutes represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of minutes represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalMinutes { get; }

        /// <summary>
        /// Gets the number of months represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of months represented by the current <see cref="DateSpan"/> structure.</value>
        public int Months { get; }

        /// <summary>
        /// Gets the total number of months represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of months represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalMonths { get; }

        /// <summary>
        /// Gets the number of seconds represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of seconds represented by the current <see cref="DateSpan"/> structure.</value>
        public int Seconds { get; }

        /// <summary>
        /// Gets the total number of seconds represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of seconds represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalSeconds { get; }

        /// <summary>
        /// Gets the number of ticks represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of ticks represented by the current <see cref="DateSpan"/> structure.</value>
        public long Ticks { get; }

        /// <summary>
        /// Gets the number of years represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The number of years represented by the current <see cref="DateSpan"/> structure.</value>
        public int Years { get; }

        /// <summary>
        /// Gets the total number of years represented by the current <see cref="DateSpan"/> structure.
        /// </summary>
        /// <value>The total number of years represented by the current <see cref="DateSpan"/> structure.</value>
        public double TotalYears { get; }
    }
}
