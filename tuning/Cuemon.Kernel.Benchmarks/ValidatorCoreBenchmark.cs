using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Cuemon.Configuration;

namespace Cuemon;
/// <summary>
/// Measures the common guard, state, and comparison members of <see cref="Validator"/>.
/// </summary>
[MemoryDiagnoser]
[WarmupCount(3)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ValidatorCoreBenchmark
{
    private const string ParamName = "argument";

    private object _firstInstance = null!;
    private object _secondInstance = null!;
    private Decorator<object> _decorator = null!;
    private List<int> _sequence = null!;
    private BenchmarkOptions _options = null!;
    private Action _checkAction = null!;
    private Func<int> _checkFunction = null!;
    private Func<bool> _truePredicate = null!;
    private Action<BenchmarkOptions> _configureOptions = null!;

    /// <summary>
    /// Initializes deterministic arguments used by the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        _firstInstance = new object();
        _secondInstance = new object();
        _decorator = Decorator.Enclose(_firstInstance);
        _sequence = new List<int> { 42 };
        _options = new BenchmarkOptions();
        _checkAction = DoNothing;
        _checkFunction = ReturnFortyTwo;
        _truePredicate = ReturnTrue;
        _configureOptions = ConfigureOptions;
    }

    /// <summary>
    /// Measures construction of a <see cref="Validator"/> instance.
    /// </summary>
    [Benchmark(Baseline = true, Description = "Constructor")]
    [BenchmarkCategory("Core")]
    public Validator Constructor()
    {
        return new Validator();
    }

    /// <summary>
    /// Measures access to the <see cref="Validator.ThrowIf"/> singleton.
    /// </summary>
    [Benchmark(Description = "ThrowIf property")]
    [BenchmarkCategory("Core")]
    public Validator ThrowIf_Property()
    {
        return Validator.ThrowIf;
    }

    /// <summary>
    /// Measures the action overload of <see cref="Validator.CheckParameter{T}(T,Action)"/>.
    /// </summary>
    [Benchmark(Description = "CheckParameter - action")]
    [BenchmarkCategory("Core")]
    public object CheckParameter_Action()
    {
        return Validator.CheckParameter(_firstInstance, _checkAction);
    }

    /// <summary>
    /// Measures the function overload of <see cref="Validator.CheckParameter{TResult}(Func{TResult})"/>.
    /// </summary>
    [Benchmark(Description = "CheckParameter - function")]
    [BenchmarkCategory("Core")]
    public int CheckParameter_Function()
    {
        return Validator.CheckParameter(_checkFunction);
    }

    /// <summary>
    /// Measures valid configuration through <see cref="Validator.ThrowIfInvalidConfigurator{TOptions}"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfInvalidConfigurator")]
    [BenchmarkCategory("Options")]
    public void ThrowIfInvalidConfigurator()
    {
        Validator.ThrowIfInvalidConfigurator(_configureOptions, out BenchmarkOptions _);
    }

    /// <summary>
    /// Measures the valid-options path of <see cref="Validator.ThrowIfInvalidOptions{TOptions}"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfInvalidOptions")]
    [BenchmarkCategory("Options")]
    public void ThrowIfInvalidOptions()
    {
        Validator.ThrowIfInvalidOptions(_options);
    }

    /// <summary>
    /// Measures the valid state path of <see cref="Validator.ThrowIfInvalidState"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfInvalidState - valid")]
    [BenchmarkCategory("State")]
    public void ThrowIfInvalidState_Valid()
    {
        Validator.ThrowIfInvalidState(false);
    }

    /// <summary>
    /// Measures the object overload of <see cref="Validator.ThrowIfDisposed(bool,object,string)"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfDisposed - object")]
    [BenchmarkCategory("State")]
    public void ThrowIfDisposed_Object()
    {
        Validator.ThrowIfDisposed(false, _firstInstance);
    }

    /// <summary>
    /// Measures the type overload of <see cref="Validator.ThrowIfDisposed(bool,Type,string)"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfDisposed - type")]
    [BenchmarkCategory("State")]
    public void ThrowIfDisposed_Type()
    {
        Validator.ThrowIfDisposed(false, typeof(ValidatorCoreBenchmark));
    }

    /// <summary>
    /// Measures the decorator overload of <see cref="Validator.ThrowIfNull{T}"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfNull - decorator")]
    [BenchmarkCategory("Null guards")]
    public void ThrowIfNull_Decorator()
    {
        Validator.ThrowIfNull(_decorator, out object _);
    }

    /// <summary>
    /// Measures the object overload of <see cref="Validator.ThrowIfNull(object,string,string)"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfNull - object")]
    [BenchmarkCategory("Null guards")]
    public void ThrowIfNull_Object()
    {
        Validator.ThrowIfNull(_firstInstance);
    }

    /// <summary>
    /// Measures the Boolean overload of <see cref="Validator.ThrowIfFalse(bool,string,string,string)"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfFalse - Boolean")]
    [BenchmarkCategory("Boolean guards")]
    public void ThrowIfFalse_Boolean()
    {
        Validator.ThrowIfFalse(true, ParamName);
    }

    /// <summary>
    /// Measures the predicate overload of <see cref="Validator.ThrowIfFalse(Func{bool},string,string,string)"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfFalse - predicate")]
    [BenchmarkCategory("Boolean guards")]
    public void ThrowIfFalse_Predicate()
    {
        Validator.ThrowIfFalse(_truePredicate, ParamName);
    }

    /// <summary>
    /// Measures the Boolean overload of <see cref="Validator.ThrowIfTrue(bool,string,string,string)"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfTrue - Boolean")]
    [BenchmarkCategory("Boolean guards")]
    public void ThrowIfTrue_Boolean()
    {
        Validator.ThrowIfTrue(false, ParamName);
    }

    /// <summary>
    /// Measures the predicate overload of <see cref="Validator.ThrowIfTrue(Func{bool},string,string,string)"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfTrue - predicate")]
    [BenchmarkCategory("Boolean guards")]
    public void ThrowIfTrue_Predicate()
    {
        Validator.ThrowIfTrue(ReturnFalse, ParamName);
    }

    /// <summary>
    /// Measures <see cref="Validator.ThrowIfSequenceEmpty{T}"/> with a populated sequence.
    /// </summary>
    [Benchmark(Description = "ThrowIfSequenceEmpty")]
    [BenchmarkCategory("Collection guards")]
    public void ThrowIfSequenceEmpty()
    {
        Validator.ThrowIfSequenceEmpty(_sequence);
    }

    /// <summary>
    /// Measures <see cref="Validator.ThrowIfSequenceNullOrEmpty{T}"/> with a populated sequence.
    /// </summary>
    [Benchmark(Description = "ThrowIfSequenceNullOrEmpty")]
    [BenchmarkCategory("Collection guards")]
    public void ThrowIfSequenceNullOrEmpty()
    {
        Validator.ThrowIfSequenceNullOrEmpty(_sequence);
    }

    /// <summary>
    /// Measures the valid path of <see cref="Validator.ThrowIfEmpty"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfEmpty")]
    [BenchmarkCategory("String guards")]
    public void ThrowIfEmpty()
    {
        Validator.ThrowIfEmpty("Cuemon");
    }

    /// <summary>
    /// Measures the valid path of <see cref="Validator.ThrowIfWhiteSpace"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfWhiteSpace")]
    [BenchmarkCategory("String guards")]
    public void ThrowIfWhiteSpace()
    {
        Validator.ThrowIfWhiteSpace("Cuemon");
    }

    /// <summary>
    /// Measures the valid path of <see cref="Validator.ThrowIfNullOrEmpty"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfNullOrEmpty")]
    [BenchmarkCategory("String guards")]
    public void ThrowIfNullOrEmpty()
    {
        Validator.ThrowIfNullOrEmpty("Cuemon");
    }

    /// <summary>
    /// Measures the valid path of <see cref="Validator.ThrowIfNullOrWhitespace"/>.
    /// </summary>
    [Benchmark(Description = "ThrowIfNullOrWhitespace")]
    [BenchmarkCategory("String guards")]
    public void ThrowIfNullOrWhitespace()
    {
        Validator.ThrowIfNullOrWhitespace("Cuemon");
    }

    /// <summary>
    /// Measures <see cref="Validator.ThrowIfSame{T}"/> with different instances.
    /// </summary>
    [Benchmark(Description = "ThrowIfSame")]
    [BenchmarkCategory("Comparison guards")]
    public void ThrowIfSame()
    {
        Validator.ThrowIfSame(_firstInstance, _secondInstance, ParamName);
    }

    /// <summary>
    /// Measures <see cref="Validator.ThrowIfNotSame{T}"/> with the same instance.
    /// </summary>
    [Benchmark(Description = "ThrowIfNotSame")]
    [BenchmarkCategory("Comparison guards")]
    public void ThrowIfNotSame()
    {
        Validator.ThrowIfNotSame(_firstInstance, _firstInstance, ParamName);
    }

    /// <summary>
    /// Measures <see cref="Validator.ThrowIfEqual{T}"/> with different values.
    /// </summary>
    [Benchmark(Description = "ThrowIfEqual")]
    [BenchmarkCategory("Comparison guards")]
    public void ThrowIfEqual()
    {
        Validator.ThrowIfEqual(1, 2, ParamName);
    }

    /// <summary>
    /// Measures <see cref="Validator.ThrowIfNotEqual{T}"/> with equal values.
    /// </summary>
    [Benchmark(Description = "ThrowIfNotEqual")]
    [BenchmarkCategory("Comparison guards")]
    public void ThrowIfNotEqual()
    {
        Validator.ThrowIfNotEqual(1, 1, ParamName);
    }

    /// <summary>
    /// Measures <see cref="Validator.ThrowIfGreaterThan{T}"/> with an in-range value.
    /// </summary>
    [Benchmark(Description = "ThrowIfGreaterThan")]
    [BenchmarkCategory("Range guards")]
    public void ThrowIfGreaterThan()
    {
        Validator.ThrowIfGreaterThan(1, 2, ParamName);
    }

    /// <summary>
    /// Measures <see cref="Validator.ThrowIfGreaterThanOrEqual{T}"/> with an in-range value.
    /// </summary>
    [Benchmark(Description = "ThrowIfGreaterThanOrEqual")]
    [BenchmarkCategory("Range guards")]
    public void ThrowIfGreaterThanOrEqual()
    {
        Validator.ThrowIfGreaterThanOrEqual(1, 2, ParamName);
    }

    /// <summary>
    /// Measures <see cref="Validator.ThrowIfLowerThan{T}"/> with an in-range value.
    /// </summary>
    [Benchmark(Description = "ThrowIfLowerThan")]
    [BenchmarkCategory("Range guards")]
    public void ThrowIfLowerThan()
    {
        Validator.ThrowIfLowerThan(2, 1, ParamName);
    }

    /// <summary>
    /// Measures <see cref="Validator.ThrowIfLowerThanOrEqual{T}"/> with an in-range value.
    /// </summary>
    [Benchmark(Description = "ThrowIfLowerThanOrEqual")]
    [BenchmarkCategory("Range guards")]
    public void ThrowIfLowerThanOrEqual()
    {
        Validator.ThrowIfLowerThanOrEqual(2, 1, ParamName);
    }

    private static void ConfigureOptions(BenchmarkOptions options)
    {
        options.Value = 42;
    }

    private static void DoNothing()
    {
    }

    private static int ReturnFortyTwo()
    {
        return 42;
    }

    private static bool ReturnFalse()
    {
        return false;
    }

    private static bool ReturnTrue()
    {
        return true;
    }

    private sealed class BenchmarkOptions : IParameterObject
    {
        public int Value { get; set; }
    }
}
