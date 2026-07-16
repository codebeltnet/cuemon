using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon
{
    /// <summary>
    /// Measures the format-validation members of <see cref="Validator"/> over their no-throw paths.
    /// All inputs are stored in instance fields (initialized in <see cref="Setup"/>) so the JIT cannot
    /// treat them as compile-time constants and elide the guard.
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class ValidatorFormatBenchmark
    {
        private const string ParamName = "argument";

        private string _nonNumeric = null!;
        private string _numeric = null!;
        private string _nonHex = null!;
        private string _hex = null!;
        private string _nonEmail = null!;
        private string _email = null!;
        private string _nonGuid = null!;
        private string _guid = null!;
        private string _nonUri = null!;
        private string _uri = null!;
        private string _nonEnum = null!;
        private string _enum = null!;
        private string _binary = null!;
        private string _base64 = null!;

        /// <summary>
        /// Initializes deterministic inputs used by the benchmarks.
        /// </summary>
        [GlobalSetup]
        public void Setup()
        {
            _nonNumeric = "Cuemon";
            _numeric = "42";
            _nonHex = "Cuemon";
            _hex = "C0DE";
            _nonEmail = "Cuemon";
            _email = "benchmark@cuemon.net";
            _nonGuid = "Cuemon";
            _guid = "8C929A0D-5534-4D33-AE8E-12B2E8B80B9B";
            _nonUri = "not a URI";
            _uri = "https://www.cuemon.net/";
            _nonEnum = "NotADay";
            _enum = nameof(DayOfWeek.Monday);
            _binary = "10101010";
            _base64 = "Q3VlbW9u";
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfNumber"/> with a non-numeric value (group baseline).
        /// </summary>
        [Benchmark(Baseline = true, Description = "ThrowIfNumber")]
        [BenchmarkCategory("Numeric")]
        public void ThrowIfNumber()
        {
            Validator.ThrowIfNumber(_nonNumeric);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfNotNumber"/> with a numeric value.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotNumber")]
        [BenchmarkCategory("Numeric")]
        public void ThrowIfNotNumber()
        {
            Validator.ThrowIfNotNumber(_numeric);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfHex"/> with non-hexadecimal text (group baseline).
        /// </summary>
        [Benchmark(Baseline = true, Description = "ThrowIfHex")]
        [BenchmarkCategory("Hexadecimal")]
        public void ThrowIfHex()
        {
            Validator.ThrowIfHex(_nonHex);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfNotHex"/> with hexadecimal text.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotHex")]
        [BenchmarkCategory("Hexadecimal")]
        public void ThrowIfNotHex()
        {
            Validator.ThrowIfNotHex(_hex);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfEmailAddress"/> with non-email text (group baseline).
        /// </summary>
        [Benchmark(Baseline = true, Description = "ThrowIfEmailAddress")]
        [BenchmarkCategory("Email")]
        public void ThrowIfEmailAddress()
        {
            Validator.ThrowIfEmailAddress(_nonEmail);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfNotEmailAddress"/> with an email address.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotEmailAddress")]
        [BenchmarkCategory("Email")]
        public void ThrowIfNotEmailAddress()
        {
            Validator.ThrowIfNotEmailAddress(_email);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfGuid"/> with non-GUID text (group baseline).
        /// </summary>
        [Benchmark(Baseline = true, Description = "ThrowIfGuid")]
        [BenchmarkCategory("Guid")]
        public void ThrowIfGuid()
        {
            Validator.ThrowIfGuid(_nonGuid);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfNotGuid"/> with a D-format GUID.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotGuid")]
        [BenchmarkCategory("Guid")]
        public void ThrowIfNotGuid()
        {
            Validator.ThrowIfNotGuid(_guid);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfUri"/> with invalid URI text (group baseline).
        /// </summary>
        [Benchmark(Baseline = true, Description = "ThrowIfUri")]
        [BenchmarkCategory("Uri")]
        public void ThrowIfUri()
        {
            Validator.ThrowIfUri(_nonUri);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfNotUri"/> with an absolute URI.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotUri")]
        [BenchmarkCategory("Uri")]
        public void ThrowIfNotUri()
        {
            Validator.ThrowIfNotUri(_uri);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfEnum{TEnum}"/> with text outside the enumeration (group baseline).
        /// </summary>
        [Benchmark(Baseline = true, Description = "ThrowIfEnum")]
        [BenchmarkCategory("Enumeration")]
        public void ThrowIfEnum()
        {
            Validator.ThrowIfEnum<DayOfWeek>(_nonEnum);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfNotEnum{TEnum}"/> with an enumeration value.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotEnum")]
        [BenchmarkCategory("Enumeration")]
        public void ThrowIfNotEnum()
        {
            Validator.ThrowIfNotEnum<DayOfWeek>(_enum);
        }

        /// <summary>
        /// Measures the type overload of <see cref="Validator.ThrowIfEnumType(Type,string,string)"/>.
        /// </summary>
        [Benchmark(Description = "ThrowIfEnumType - type")]
        [BenchmarkCategory("Enumeration")]
        public void ThrowIfEnumType_Type()
        {
            Validator.ThrowIfEnumType(typeof(string));
        }

        /// <summary>
        /// Measures the generic overload of <see cref="Validator.ThrowIfEnumType{TEnum}"/>.
        /// </summary>
        [Benchmark(Description = "ThrowIfEnumType - generic")]
        [BenchmarkCategory("Enumeration")]
        public void ThrowIfEnumType_Generic()
        {
            Validator.ThrowIfEnumType<DateTime>(ParamName);
        }

        /// <summary>
        /// Measures the generic overload of <see cref="Validator.ThrowIfNotEnumType{TEnum}"/>.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotEnumType - generic")]
        [BenchmarkCategory("Enumeration")]
        public void ThrowIfNotEnumType_Generic()
        {
            Validator.ThrowIfNotEnumType<DayOfWeek>(ParamName);
        }

        /// <summary>
        /// Measures the type overload of <see cref="Validator.ThrowIfNotEnumType(Type,string,string)"/>.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotEnumType - type")]
        [BenchmarkCategory("Enumeration")]
        public void ThrowIfNotEnumType_Type()
        {
            Validator.ThrowIfNotEnumType(typeof(DayOfWeek));
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfNotBinaryDigits"/> with binary digits.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotBinaryDigits")]
        [BenchmarkCategory("Binary")]
        public void ThrowIfNotBinaryDigits()
        {
            Validator.ThrowIfNotBinaryDigits(_binary);
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowIfNotBase64String"/> with Base64 text.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotBase64String")]
        [BenchmarkCategory("Base64")]
        public void ThrowIfNotBase64String()
        {
            Validator.ThrowIfNotBase64String(_base64);
        }
    }
}
