using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon
{
    /// <summary>
    /// The set of set-difference scenarios exercised by <see cref="HasDifferenceBenchmark"/>.
    /// </summary>
    public enum DifferenceScenario
    {
        /// <summary>Both strings are identical (no difference, low cardinality).</summary>
        Equivalent,

        /// <summary>Same character set, different ordering (no difference).</summary>
        Reordered,

        /// <summary>Same character set with heavy duplication (no difference).</summary>
        DuplicateHeavy,

        /// <summary>A single differing character at the beginning of <c>second</c>.</summary>
        DiffAtStart,

        /// <summary>A single differing character in the middle of <c>second</c>.</summary>
        DiffAtMiddle,

        /// <summary>A single differing character at the end of <c>second</c>.</summary>
        DiffAtEnd,

        /// <summary>High-cardinality strings that share the same set (no difference).</summary>
        MostlyUnique
    }

    /// <summary>
    /// Measures <see cref="Condition.HasDifference"/> directly (without the exception overhead of
    /// <see cref="Validator.ThrowIfDifferent"/>) across representative set-difference scenarios and lengths.
    /// </summary>
    [MemoryDiagnoser]
    [WarmupCount(3)]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class HasDifferenceBenchmark
    {
        private string _first = null!;
        private string _second = null!;

        /// <summary>
        /// Gets the length of the strings used in the benchmark.
        /// </summary>
        [Params(16, 256, 4096)]
        public int Length { get; set; }

        /// <summary>
        /// Gets the set-difference scenario used in the benchmark.
        /// </summary>
        [Params(
            DifferenceScenario.Equivalent,
            DifferenceScenario.Reordered,
            DifferenceScenario.DuplicateHeavy,
            DifferenceScenario.DiffAtStart,
            DifferenceScenario.DiffAtMiddle,
            DifferenceScenario.DiffAtEnd,
            DifferenceScenario.MostlyUnique)]
        public DifferenceScenario Scenario { get; set; }

        /// <summary>
        /// Builds deterministic inputs for the current scenario and length outside the measured operation.
        /// </summary>
        [GlobalSetup]
        public void Setup()
        {
            switch (Scenario)
            {
                case DifferenceScenario.Equivalent:
                    _first = new string('a', Length);
                    _second = new string('a', Length);
                    break;
                case DifferenceScenario.Reordered:
                    _first = BuildCycle(Length, "abcd");
                    _second = BuildCycle(Length, "dcba");
                    break;
                case DifferenceScenario.DuplicateHeavy:
                    _first = "abc";
                    _second = BuildBlocks(Length);
                    break;
                case DifferenceScenario.DiffAtStart:
                    _first = new string('a', Length);
                    _second = "Z" + new string('a', Length - 1);
                    break;
                case DifferenceScenario.DiffAtMiddle:
                    _first = new string('a', Length);
                    _second = new string('a', Length / 2) + "Z" + new string('a', Length - (Length / 2) - 1);
                    break;
                case DifferenceScenario.DiffAtEnd:
                    _first = new string('a', Length);
                    _second = new string('a', Length - 1) + "Z";
                    break;
                case DifferenceScenario.MostlyUnique:
                    _first = BuildUnique(Length);
                    _second = BuildUnique(Length);
                    break;
            }
        }

        /// <summary>
        /// Measures the set-difference guard.
        /// </summary>
        [Benchmark(Description = "HasDifference")]
        [BenchmarkCategory("Difference")]
        public bool HasDifference()
        {
            return Condition.HasDifference(_first, _second, out _);
        }

        private static string BuildCycle(int length, string alphabet)
        {
            var buffer = new char[length];
            for (var i = 0; i < length; i++) { buffer[i] = alphabet[i % alphabet.Length]; }
            return new string(buffer);
        }

        private static string BuildBlocks(int length)
        {
            var buffer = new char[length];
            var third = length / 3;
            for (var i = 0; i < length; i++)
            {
                buffer[i] = i < third ? 'a' : i < third * 2 ? 'b' : 'c';
            }
            return new string(buffer);
        }

        private static string BuildUnique(int length)
        {
            var buffer = new char[length];
            for (var i = 0; i < length; i++) { buffer[i] = (char)(0x100 + (i % 4000)); }
            return new string(buffer);
        }
    }
}
