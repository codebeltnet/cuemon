using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon.IO;
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class StreamDecoratorExtensionsBenchmark
{
    [Params(256, 4096, 65536)]
    public int Count { get; set; }

    private byte[] _payload;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(1337);
        _payload = new byte[Count];
        random.NextBytes(_payload);
    }

    [Benchmark(Baseline = true, Description = "CopyStream (changePosition=true)")]
    [BenchmarkCategory("CopyStream")]
    public int CopyStream_ChangePositionTrue()
    {
        using (var source = new MemoryStream(_payload, writable: false))
        using (var destination = new MemoryStream(_payload.Length))
        {
            Decorator.Enclose(source).CopyStream(destination, bufferSize: 81920, changePosition: true);
            return (int)destination.Length;
        }
    }

    [Benchmark(Description = "CopyStream (changePosition=false)")]
    [BenchmarkCategory("CopyStream")]
    public int CopyStream_ChangePositionFalse()
    {
        using (var source = new MemoryStream(_payload, writable: false))
        using (var destination = new MemoryStream(_payload.Length))
        {
            Decorator.Enclose(source).CopyStream(destination, bufferSize: 81920, changePosition: false);
            return (int)destination.Length;
        }
    }

    [Benchmark(Baseline = true, Description = "InvokeToByteArray (MemoryStream)")]
    [BenchmarkCategory("InvokeToByteArray")]
    public int InvokeToByteArray_MemoryStream()
    {
        using (var source = new MemoryStream(_payload, writable: false))
        {
            var result = Decorator.Enclose(source).InvokeToByteArray(bufferSize: 81920, leaveOpen: true);
            return result.Length;
        }
    }

    [Benchmark(Description = "InvokeToByteArray (BufferedStream)")]
    [BenchmarkCategory("InvokeToByteArray")]
    public int InvokeToByteArray_BufferedStream()
    {
        using (var memory = new MemoryStream(_payload, writable: false))
        using (var source = new BufferedStream(memory, bufferSize: 16384))
        {
            var result = Decorator.Enclose(source).InvokeToByteArray(bufferSize: 81920, leaveOpen: true);
            return result.Length;
        }
    }
}
