using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon
{
    /// <summary>
    /// Measures the character, reserved-keyword, and conditional members of <see cref="Validator"/>.
    /// No-throw and throwing scenarios are kept as separate benchmarks; throwing benchmarks swallow the
    /// expected <see cref="ArgumentException"/> so the run is not invalidated.
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class ValidatorMiscBenchmark
    {
        private const string ParamName = "argument";

        private string _argument = null!;
        private char[] _presentCharacters = null!;
        private char[] _absentCharacters = null!;
        private string[] _reservedKeywords = null!;
        private Action<ExceptionCondition<ArgumentException>> _emptyExceptionCondition = null!;

        /// <summary>
        /// Initializes deterministic inputs used by the benchmarks.
        /// </summary>
        [GlobalSetup]
        public void Setup()
        {
            _argument = "Cuemon";
            _presentCharacters = new[] { 'u' };            // 'u' occurs in "Cuemon"
            _absentCharacters = new[] { 'x', 'y', 'z' };   // none occur in "Cuemon"
            _reservedKeywords = new[] { "class", "namespace" };
            _emptyExceptionCondition = ConfigureWithoutException;
        }

        /// <summary>
        /// Measures the default-comparer reserved-keyword guard (group baseline).
        /// </summary>
        [Benchmark(Baseline = true, Description = "ThrowIfContainsReservedKeyword - default comparer")]
        [BenchmarkCategory("Reserved keywords")]
        public void ThrowIfContainsReservedKeyword_DefaultComparer()
        {
            Validator.ThrowIfContainsReservedKeyword(_argument, _reservedKeywords);
        }

        /// <summary>
        /// Measures the custom-comparer reserved-keyword guard.
        /// </summary>
        [Benchmark(Description = "ThrowIfContainsReservedKeyword - custom comparer")]
        [BenchmarkCategory("Reserved keywords")]
        public void ThrowIfContainsReservedKeyword_CustomComparer()
        {
            Validator.ThrowIfContainsReservedKeyword(_argument, _reservedKeywords, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Measures the no-throw path of <see cref="Validator.ThrowIfContainsAny"/> (no candidate occurs).
        /// </summary>
        [Benchmark(Baseline = true, Description = "ThrowIfContainsAny - no match")]
        [BenchmarkCategory("Character guards")]
        public void ThrowIfContainsAny_NoMatch()
        {
            Validator.ThrowIfContainsAny(_argument, _absentCharacters);
        }

        /// <summary>
        /// Measures the no-throw path of <see cref="Validator.ThrowIfNotContainsAny"/> (a candidate occurs).
        /// </summary>
        [Benchmark(Description = "ThrowIfNotContainsAny - match")]
        [BenchmarkCategory("Character guards")]
        public void ThrowIfNotContainsAny_Match()
        {
            Validator.ThrowIfNotContainsAny(_argument, _presentCharacters);
        }

        /// <summary>
        /// Measures the throwing path of <see cref="Validator.ThrowIfContainsAny"/> (a candidate occurs).
        /// </summary>
        [Benchmark(Description = "ThrowIfContainsAny - match (throws)")]
        [BenchmarkCategory("Character guards")]
        public void ThrowIfContainsAny_Match_Throws()
        {
            try
            {
                Validator.ThrowIfContainsAny(_argument, _presentCharacters);
            }
            catch (ArgumentException)
            {
            }
        }

        /// <summary>
        /// Measures the throwing path of <see cref="Validator.ThrowIfNotContainsAny"/> (no candidate occurs).
        /// </summary>
        [Benchmark(Description = "ThrowIfNotContainsAny - no match (throws)")]
        [BenchmarkCategory("Character guards")]
        public void ThrowIfNotContainsAny_NoMatch_Throws()
        {
            try
            {
                Validator.ThrowIfNotContainsAny(_argument, _absentCharacters);
            }
            catch (ArgumentException)
            {
            }
        }

        /// <summary>
        /// Measures <see cref="Validator.ThrowWhen"/> when the condition does not create an exception.
        /// </summary>
        [Benchmark(Description = "ThrowWhen")]
        [BenchmarkCategory("Conditional guards")]
        public void ThrowWhen()
        {
            Validator.ThrowWhen(_emptyExceptionCondition);
        }

        private static void ConfigureWithoutException(ExceptionCondition<ArgumentException> condition)
        {
        }
    }
}
