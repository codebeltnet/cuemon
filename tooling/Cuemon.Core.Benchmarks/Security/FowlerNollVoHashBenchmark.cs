using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Cuemon;
using Cuemon.Security;

namespace Cuemon.Security
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class FowlerNollVoHashBenchmark
    {
        private byte[] _smallPayload;
        private byte[] _largePayload;
        private FowlerNollVo32 _fnv32 = null!;
        private FowlerNollVo64 _fnv64 = null!;
        private FowlerNollVo128 _fnv128 = null!;
        private FowlerNollVo256 _fnv256 = null!;
        private FowlerNollVo512 _fnv512 = null!;
        private FowlerNollVo1024 _fnv1024 = null!;



        [GlobalSetup]
        public void Setup()
        {
            _smallPayload = "The quick brown fox jumps over the lazy dog"u8.ToArray();
            var sb = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                sb.Append("The quick brown fox jumps over the lazy dog");
            }
            _largePayload = Encoding.UTF8.GetBytes(sb.ToString());

            _fnv32 = new FowlerNollVo32(o => o.Algorithm = Algorithm);
            _fnv64 = new FowlerNollVo64(o => o.Algorithm = Algorithm);
            _fnv128 = new FowlerNollVo128(o => o.Algorithm = Algorithm);
            _fnv256 = new FowlerNollVo256(o => o.Algorithm = Algorithm);
            _fnv512 = new FowlerNollVo512(o => o.Algorithm = Algorithm);
            _fnv1024 = new FowlerNollVo1024(o => o.Algorithm = Algorithm);
        }

        [Params(FowlerNollVoAlgorithm.Fnv1, FowlerNollVoAlgorithm.Fnv1a)]
        public FowlerNollVoAlgorithm Algorithm { get; set; }

        [Benchmark(Description = "ComputeHash32 (small)")]
        public HashResult ComputeHash32_Small()
            => _fnv32.ComputeHash(_smallPayload);

        [Benchmark(Description = "ComputeHash32 (large)")]
        public HashResult ComputeHash32_Large()
            => _fnv32.ComputeHash(_largePayload);

        [Benchmark(Description = "ComputeHash64 (small)")]
        public HashResult ComputeHash64_Small()
            => _fnv64.ComputeHash(_smallPayload);

        [Benchmark(Description = "ComputeHash64 (large)")]
        public HashResult ComputeHash64_Large()
            => _fnv64.ComputeHash(_largePayload);

        [Benchmark(Description = "ComputeHash128 (small)")]
        public HashResult ComputeHash128_Small()
            => _fnv128.ComputeHash(_smallPayload);

        [Benchmark(Description = "ComputeHash128 (large)")]
        public HashResult ComputeHash128_Large()
            => _fnv128.ComputeHash(_largePayload);

        [Benchmark(Description = "ComputeHash256 (small)")]
        public HashResult ComputeHash256_Small()
            => _fnv256.ComputeHash(_smallPayload);

        [Benchmark(Description = "ComputeHash256 (large)")]
        public HashResult ComputeHash256_Large()
            => _fnv256.ComputeHash(_largePayload);

        [Benchmark(Description = "ComputeHash512 (small)")]
        public HashResult ComputeHash512_Small()
            => _fnv512.ComputeHash(_smallPayload);

        [Benchmark(Description = "ComputeHash512 (large)")]
        public HashResult ComputeHash512_Large()
            => _fnv512.ComputeHash(_largePayload);

        [Benchmark(Description = "ComputeHash1024 (small)")]
        public HashResult ComputeHash1024_Small()
            => _fnv1024.ComputeHash(_smallPayload);

        [Benchmark(Description = "ComputeHash1024 (large)")]
        public HashResult ComputeHash1024_Large()
            => _fnv1024.ComputeHash(_largePayload);
    }
}
