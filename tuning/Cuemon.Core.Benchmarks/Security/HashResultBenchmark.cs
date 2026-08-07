using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon.Security;
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class HashResultBenchmark
{
    [Params(0, 8, 32, 256, 1024)]
    public int Size { get; set; }

    private byte[] _data;
    private HashResult _hashResult;

    [GlobalSetup]
    public void Setup()
    {
        _data = new byte[Size];
        var rng = new Random(42);
        rng.NextBytes(_data);
        _hashResult = new HashResult(_data);
    }

    [Benchmark(Description = "HashResult.GetBytes - copy bytes")]
    public byte[] GetBytes_Copy()
    {
        return _hashResult.GetBytes();
    }

    [Benchmark(Description = "HashResult.ToHexadecimalString")]
    public string ToHexadecimalString()
    {
        return _hashResult.ToHexadecimalString();
    }

    [Benchmark(Description = "HashResult.ToBase64String")]
    public string ToBase64String()
    {
        return _hashResult.ToBase64String();
    }

    [Benchmark(Description = "HashResult.ToUrlEncodedBase64String")]
    public string ToUrlEncodedBase64String()
    {
        return _hashResult.ToUrlEncodedBase64String();
    }

    [Benchmark(Description = "HashResult.ToBinaryString")]
    public string ToBinaryString()
    {
        return _hashResult.ToBinaryString();
    }

    [Benchmark(Description = "HashResult.To<string> (converter)")]
    public string ToWithConverter()
    {
        return _hashResult.To(Convert.ToBase64String);
    }
}
