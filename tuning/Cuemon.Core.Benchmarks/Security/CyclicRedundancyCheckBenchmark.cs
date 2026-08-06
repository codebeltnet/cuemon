using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon.Security;
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class CyclicRedundancyCheckBenchmark
{
    [Params(64, 4096, 1048576)]
    public int Size { get; set; }

    private byte[] _payload;
    private CyclicRedundancyCheck32 _crc32;
    private CyclicRedundancyCheck64 _crc64;

    [GlobalSetup]
    public void Setup()
    {
        _payload = new byte[Size];
        var rnd = new Random(42);
        rnd.NextBytes(_payload);

        _crc32 = new CyclicRedundancyCheck32();
        _crc64 = new CyclicRedundancyCheck64();

        // Warm-up to ensure lazy lookup tables are initialized outside measured runs
        _crc32.ComputeHash(new byte[] { 0x0 });
        _crc64.ComputeHash(new byte[] { 0x0 });
    }

    [Benchmark(Baseline = true, Description = "CRC32 - byte[]")]
    public HashResult ComputeHash_Crc32_Bytes() => _crc32.ComputeHash(_payload);

    [Benchmark(Description = "CRC64 - byte[]")]
    public HashResult ComputeHash_Crc64_Bytes() => _crc64.ComputeHash(_payload);

    [Benchmark(Description = "CRC32 - Stream (includes copy)")]
    public HashResult ComputeHash_Crc32_Stream()
    {
        using var ms = new MemoryStream(_payload, writable: false);
        return _crc32.ComputeHash(ms);
    }

    [Benchmark(Description = "CRC64 - Stream (includes copy)")]
    public HashResult ComputeHash_Crc64_Stream()
    {
        using var ms = new MemoryStream(_payload, writable: false);
        return _crc64.ComputeHash(ms);
    }
}
