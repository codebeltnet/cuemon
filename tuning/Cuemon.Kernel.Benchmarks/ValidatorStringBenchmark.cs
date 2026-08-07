using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon;
/// <summary>
/// Measures the successful (no-throw) string validation paths over representative input lengths.
/// Inputs are stored in instance fields initialized in <see cref="Setup"/> so the JIT cannot fold
/// the guards away as compile-time constants.
/// </summary>
[MemoryDiagnoser]
[WarmupCount(3)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ValidatorStringBenchmark
{
    private const string ParamName = "argument";

    private readonly char[] _absentCharacters = { 'x', 'y', 'z' };

    private string _value = null!;
    private string _equivalentValue = null!;
    private string _hexadecimalValue = null!;
    private string _base64Value = null!;

    /// <summary>
    /// Gets the length of the strings used in the benchmark.
    /// </summary>
    [Params(16, 256, 4096)]
    public int StringLength { get; set; }

    /// <summary>
    /// Initializes deterministic strings outside the measured operations.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        _value = new string('a', StringLength);
        _equivalentValue = new string('a', StringLength);
        _hexadecimalValue = new string('A', StringLength);       // hexadecimal digits, even length
        _base64Value = new string('A', StringLength);            // valid base-64 (length is a multiple of four)
    }

    /// <summary>
    /// Measures the successful null, empty, and whitespace guard (group baseline).
    /// </summary>
    [Benchmark(Baseline = true, Description = "ThrowIfNullOrWhitespace - text")]
    [BenchmarkCategory("String guards")]
    public void ThrowIfNullOrWhitespace_Text()
    {
        Validator.ThrowIfNullOrWhitespace(_value);
    }

    /// <summary>
    /// Measures a successful character exclusion guard using ordinal comparison.
    /// </summary>
    [Benchmark(Description = "ThrowIfContainsAny - no match (Ordinal)")]
    [BenchmarkCategory("String guards")]
    public void ThrowIfContainsAny_NoMatch_Ordinal()
    {
        Validator.ThrowIfContainsAny(_value, _absentCharacters, StringComparison.Ordinal);
    }

    /// <summary>
    /// Measures a successful character exclusion guard using ordinal, case-insensitive comparison.
    /// </summary>
    [Benchmark(Description = "ThrowIfContainsAny - no match (OrdinalIgnoreCase)")]
    [BenchmarkCategory("String guards")]
    public void ThrowIfContainsAny_NoMatch_OrdinalIgnoreCase()
    {
        Validator.ThrowIfContainsAny(_value, _absentCharacters, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Measures a successful set-difference guard for equivalent values.
    /// </summary>
    [Benchmark(Description = "ThrowIfDifferent - equivalent values")]
    [BenchmarkCategory("String guards")]
    public void ThrowIfDifferent_EquivalentValues()
    {
        Validator.ThrowIfDifferent(_value, _equivalentValue, ParamName);
    }

    /// <summary>
    /// Measures the successful hexadecimal-format guard.
    /// </summary>
    [Benchmark(Description = "ThrowIfNotHex - hexadecimal text")]
    [BenchmarkCategory("String guards")]
    public void ThrowIfNotHex_HexadecimalText()
    {
        Validator.ThrowIfNotHex(_hexadecimalValue);
    }

    /// <summary>
    /// Measures the successful base-64 format guard.
    /// </summary>
    [Benchmark(Description = "ThrowIfNotBase64String - base-64 text")]
    [BenchmarkCategory("String guards")]
    public void ThrowIfNotBase64String_Base64Text()
    {
        Validator.ThrowIfNotBase64String(_base64Value);
    }
}
