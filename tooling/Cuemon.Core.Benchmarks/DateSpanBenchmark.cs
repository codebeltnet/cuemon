using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class DateSpanBenchmark
    {
        private DateTime _now;
        private DateTime _shortEnd;
        private DateTime _mediumEnd;
        private DateTime _longEnd;

        private DateSpan _shortSpan;
        private DateSpan _mediumSpan;
        private DateSpan _longSpan;

        private string _shortStartString;
        private string _shortEndString;
        private string _longStartString;
        private string _longEndString;

        [GlobalSetup]
        public void Setup()
        {
            _now = DateTime.UtcNow;

            _shortEnd = _now.AddHours(36); // ~1.5 days
            _mediumEnd = _now.AddMonths(5).AddDays(12).AddHours(3);
            _longEnd = _now.AddYears(5).AddMonths(2).AddDays(10).AddHours(4).AddMilliseconds(123);

            _shortSpan = new DateSpan(_now, _shortEnd);
            _mediumSpan = new DateSpan(_now, _mediumEnd);
            _longSpan = new DateSpan(_now, _longEnd);

            // ISO sortable ("s") uses invariant culture, matches DateSpan.Parse overloads
            _shortStartString = _now.ToString("s", CultureInfo.InvariantCulture);
            _shortEndString = _shortEnd.ToString("s", CultureInfo.InvariantCulture);

            _longStartString = _now.ToString("s", CultureInfo.InvariantCulture);
            _longEndString = _longEnd.ToString("s", CultureInfo.InvariantCulture);
        }

        // Construction: common scenarios
        [Benchmark(Baseline = true, Description = "Ctor (short span)")]
        public DateSpan Construct_Short() => new DateSpan(_now, _shortEnd);

        [Benchmark(Description = "Ctor (medium span)")]
        public DateSpan Construct_Medium() => new DateSpan(_now, _mediumEnd);

        [Benchmark(Description = "Ctor (long span)")]
        public DateSpan Construct_Long() => new DateSpan(_now, _longEnd);

        // Single-argument ctor that uses DateTime.Today as upper bound
        [Benchmark(Description = "Ctor (single-date)")]
        public DateSpan Construct_SingleDate() => new DateSpan(_now);

        // Parsing from string (culture-aware overload)
        [Benchmark(Description = "Parse (short)")]
        public DateSpan Parse_Short() => DateSpan.Parse(_shortStartString, _shortEndString, CultureInfo.InvariantCulture);

        [Benchmark(Description = "Parse (long)")]
        public DateSpan Parse_Long() => DateSpan.Parse(_longStartString, _longEndString, CultureInfo.InvariantCulture);

        // Common instance operations
        [Benchmark(Description = "ToString (short)")]
        public string ToString_Short() => _shortSpan.ToString();

        [Benchmark(Description = "ToString (long)")]
        public string ToString_Long() => _longSpan.ToString();

        [Benchmark(Description = "GetWeeks (short)")]
        public int GetWeeks_Short() => _shortSpan.GetWeeks();

        [Benchmark(Description = "GetWeeks (long)")]
        public int GetWeeks_Long() => _longSpan.GetWeeks();

        [Benchmark(Description = "GetHashCode")]
        public int GetHashCode_Benchmark() => _longSpan.GetHashCode();

        [Benchmark(Description = "Equals (value vs same value)")]
        public bool Equals_Same() => _longSpan.Equals(new DateSpan(_now, _longEnd));

        [Benchmark(Description = "Operator == (same value)")]
        public bool OperatorEquality_Same()
        {
            var other = new DateSpan(_now, _longEnd);
            return _longSpan == other;
        }
    }
}
