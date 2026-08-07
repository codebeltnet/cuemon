using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon.Security.Cryptography;
// Group results by Params (makes comparing algorithm variants easy)
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByParams)]
public class Sha512256Benchmark
{
    // Expose algorithm variants so runs can be filtered/controlled via Params
    public enum AlgorithmVariant
    {
        CustomSHA512_256,
        SHA512_Truncated
    }

    // Allows switching algorithm variant at runtime via BenchmarkDotNet params
    [Params(AlgorithmVariant.CustomSHA512_256, AlgorithmVariant.SHA512_Truncated)]
    public AlgorithmVariant Variant { get; set; }

    // Prepared inputs (created once in GlobalSetup)
    private byte[] _smallInput;
    private byte[] _largeInput;

    // Factory map to create fresh HashAlgorithm instances per invocation
    private readonly Dictionary<AlgorithmVariant, Func<HashAlgorithm>> _factories = new();

    [GlobalSetup]
    public void GlobalSetup()
    {
        // Prepare deterministic payloads once
        var rng = new Random(42);

        _smallInput = new byte[64]; // small payload (~64 bytes)
        rng.NextBytes(_smallInput);

        _largeInput = new byte[1_048_576]; // large payload (~1 MB)
        rng.NextBytes(_largeInput);

        // Initialize algorithm factories (do not reuse instances across invocations)
        _factories[AlgorithmVariant.CustomSHA512_256] = () => new Cuemon.Security.Cryptography.SHA512256();

        // Built-in SHA-512 then truncate to 256 bits (first 32 bytes) � implemented by computing full SHA-512
        _factories[AlgorithmVariant.SHA512_Truncated] = () => SHA512.Create();
    }

    // ---- Explicit benchmark methods for each combination of implementation and input size ----
    // These methods return the algorithm result (byte[]), have descriptive names and are measured separately.

    [Benchmark(Baseline = true, Description = "Custom SHA-512/256 � small (64 bytes)")]
    public byte[] CustomSHA512256_Small()
    {
        using var alg = _factories[AlgorithmVariant.CustomSHA512_256]();
        return alg.ComputeHash(_smallInput);
    }

    [Benchmark(Description = "Custom SHA-512/256 � large (1 MB)")]
    public byte[] CustomSHA512256_Large()
    {
        using var alg = _factories[AlgorithmVariant.CustomSHA512_256]();
        return alg.ComputeHash(_largeInput);
    }

    [Benchmark(Description = "Built-in SHA-512 truncated -> 256 � small (64 bytes)")]
    public byte[] BuiltInSHA512_Truncated_Small()
    {
        using var alg = _factories[AlgorithmVariant.SHA512_Truncated]();
        var full = alg.ComputeHash(_smallInput);
        // Truncate to 256 bits (first 32 bytes) to mimic SHA-512/256 output length
        var truncated = new byte[32];
        Array.Copy(full, 0, truncated, 0, truncated.Length);
        return truncated;
    }

    [Benchmark(Description = "Built-in SHA-512 truncated -> 256 � large (1 MB)")]
    public byte[] BuiltInSHA512_Truncated_Large()
    {
        using var alg = _factories[AlgorithmVariant.SHA512_Truncated]();
        var full = alg.ComputeHash(_largeInput);
        var truncated = new byte[32];
        Array.Copy(full, 0, truncated, 0, truncated.Length);
        return truncated;
    }

    // ---- Generic method that uses the [Params] Variant (optional; useful for grouped runs) ----
    // Returns byte[] and chooses algorithm based on the Variant param.
    [Benchmark(Description = "Param-based: ComputeHash (selects algorithm by [Params] Variant)")]
    public byte[] ParamBased_ComputeHash()
    {
        if (Variant == AlgorithmVariant.CustomSHA512_256)
        {
            using var alg = _factories[AlgorithmVariant.CustomSHA512_256]();
            return alg.ComputeHash(_smallInput); // small input chosen for param-based path
        }

        using var sha = _factories[AlgorithmVariant.SHA512_Truncated]();
        var full = sha.ComputeHash(_smallInput);
        var truncated = new byte[32];
        Array.Copy(full, 0, truncated, 0, truncated.Length);
        return truncated;
    }
}
