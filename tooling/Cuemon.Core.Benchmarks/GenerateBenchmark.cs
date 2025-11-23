using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class GenerateBenchmark
    {
        // Parameterized input sizes to exercise micro / mid / macro scenarios
        [Params(8, 256, 4096)]
        public int Count { get; set; }

        private object _sampleObject;
        private string[] _randomStringValues;
        private IEnumerable<IConvertible> _hashConvertibles;

        [GlobalSetup]
        public void Setup()
        {
            _sampleObject = new Sample
            {
                Id = 42,
                Name = "BenchmarkSample",
                Created = DateTime.UtcNow,
                Tags = new[] { "alpha", "beta", "gamma" }
            };

            _randomStringValues = new[] { "abcdefghijklmnopqrstuvwxyz", "0123456789", "ABCDEFGHIJKLMNOPQRSTUVWXYZ" };

            _hashConvertibles = new IConvertible[] { 1, "two", 3.0, (short)4, 5L, 6.5m };
        }

        // ---------------------
        // RangeOf
        // ---------------------
        private int _sink;

        [Benchmark(Description = "RangeOf - enumerate")]
        public void RangeOf_Enumerate()
        {
            _sink = 0;

            foreach (var value in Generate.RangeOf(Count, i => i))
            {
                _sink += value;
            }
        }

        // ---------------------
        // RandomNumber
        // ---------------------
        [Benchmark(Description = "RandomNumber - default")]
        public int RandomNumber_Default() => Generate.RandomNumber();

        [Benchmark(Description = "RandomNumber - bounded")]
        public int RandomNumber_Bounded() => Generate.RandomNumber(0, Math.Max(1, Count));

        // ---------------------
        // FixedString
        // ---------------------
        [Benchmark(Description = "FixedString")]
        public string FixedString_Benchmark() => Generate.FixedString('x', Count);

        // ---------------------
        // RandomString
        // ---------------------
        [Benchmark(Description = "RandomString - letters/numbers")]
        public string RandomString_Benchmark() => Generate.RandomString(Count, _randomStringValues);

        // ---------------------
        // ObjectPortrayal
        // ---------------------
        [Benchmark(Description = "ObjectPortrayal - basic object")]
        public string ObjectPortrayal_Basic() => Generate.ObjectPortrayal(_sampleObject);

        // ---------------------
        // HashCode32 / HashCode64
        // ---------------------
        [Benchmark(Description = "HashCode32 - params")]
        public int HashCode32_Params() => Generate.HashCode32(1, "a", 3.14);

        [Benchmark(Description = "HashCode32 - enumerable")]
        public int HashCode32_Enumerable() => Generate.HashCode32(_hashConvertibles);

        [Benchmark(Description = "HashCode64 - params")]
        public long HashCode64_Params() => Generate.HashCode64(1, "a", 3.14);

        [Benchmark(Description = "HashCode64 - enumerable")]
        public long HashCode64_Enumerable() => Generate.HashCode64(_hashConvertibles);

        // ---------------------
        // Helpers / sample types
        // ---------------------
        private sealed class Sample
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime Created { get; set; }
            public string[] Tags { get; set; }
        }
    }
}
