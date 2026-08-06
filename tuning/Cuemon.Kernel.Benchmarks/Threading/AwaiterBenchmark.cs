using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon.Threading;
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class AwaiterBenchmark
{
    private static readonly Task<ConditionalValue> SuccessfulTask = Task.FromResult<ConditionalValue>(new SuccessfulValue());
    private static readonly Task<ConditionalValue> UnsuccessfulTask = Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
    private static readonly Task<ConditionalValue> InvalidOperationTask = Task.FromException<ConditionalValue>(new InvalidOperationException("fail1"));
    private static readonly Task<ConditionalValue> ArgumentTask = Task.FromException<ConditionalValue>(new ArgumentException("fail2"));
    private static readonly Task<ConditionalValue>[] OneExceptionSequence = { InvalidOperationTask };
    private static readonly Task<ConditionalValue>[] TwoExceptionSequence = { InvalidOperationTask, ArgumentTask };
    private static readonly Task<ConditionalValue>[] TenExceptionSequence =
    {
        InvalidOperationTask,
        ArgumentTask,
        InvalidOperationTask,
        ArgumentTask,
        InvalidOperationTask,
        ArgumentTask,
        InvalidOperationTask,
        ArgumentTask,
        InvalidOperationTask,
        ArgumentTask
    };
    private static readonly Action<AsyncRunOptions> ImmediateSuccessSetup = CreateSetup(1);
    private static readonly Action<AsyncRunOptions> OneRetrySetup = CreateSetup(2);
    private static readonly Action<AsyncRunOptions> TwoRetrySetup = CreateSetup(3);
    private static readonly Action<AsyncRunOptions> TenRetrySetup = CreateSetup(11);

    private readonly Func<Task<ConditionalValue>> _immediateSuccessMethod;
    private readonly Func<Task<ConditionalValue>> _unsuccessfulThenSuccessMethod;
    private readonly Func<Task<ConditionalValue>> _exceptionsThenSuccessMethod;

    private int _attempt;
    private int _unsuccessfulAttemptsBeforeSuccess;
    private Task<ConditionalValue>[] _exceptionSequence;

    public AwaiterBenchmark()
    {
        _immediateSuccessMethod = ImmediateSuccessAsync;
        _unsuccessfulThenSuccessMethod = UnsuccessfulThenSuccessAsync;
        _exceptionsThenSuccessMethod = ExceptionsThenSuccessAsync;
    }

    [Benchmark(Baseline = true, Description = "Direct await - immediate success")]
    public Task<ConditionalValue> DirectAwait_ImmediateSuccess()
    {
        return SuccessfulTask;
    }

    [Benchmark(Description = "Awaiter - immediate success")]
    public Task<ConditionalValue> Awaiter_ImmediateSuccess()
    {
        return Awaiter.RunUntilSuccessfulOrTimeoutAsync(_immediateSuccessMethod, ImmediateSuccessSetup);
    }

    [Benchmark(Description = "Awaiter - 1 unsuccessful result then success")]
    public Task<ConditionalValue> Awaiter_Unsuccessful1_ThenSuccess()
    {
        _attempt = 0;
        _unsuccessfulAttemptsBeforeSuccess = 1;
        return Awaiter.RunUntilSuccessfulOrTimeoutAsync(_unsuccessfulThenSuccessMethod, OneRetrySetup);
    }

    [Benchmark(Description = "Awaiter - 10 unsuccessful results then success")]
    public Task<ConditionalValue> Awaiter_Unsuccessful10_ThenSuccess()
    {
        _attempt = 0;
        _unsuccessfulAttemptsBeforeSuccess = 10;
        return Awaiter.RunUntilSuccessfulOrTimeoutAsync(_unsuccessfulThenSuccessMethod, TenRetrySetup);
    }

    [Benchmark(Description = "Awaiter - 1 exception then success")]
    public Task<ConditionalValue> Awaiter_Exception1_ThenSuccess()
    {
        _attempt = 0;
        _exceptionSequence = OneExceptionSequence;
        return Awaiter.RunUntilSuccessfulOrTimeoutAsync(_exceptionsThenSuccessMethod, OneRetrySetup);
    }

    [Benchmark(Description = "Awaiter - 2 exceptions then success")]
    public Task<ConditionalValue> Awaiter_Exception2_ThenSuccess()
    {
        _attempt = 0;
        _exceptionSequence = TwoExceptionSequence;
        return Awaiter.RunUntilSuccessfulOrTimeoutAsync(_exceptionsThenSuccessMethod, TwoRetrySetup);
    }

    [Benchmark(Description = "Awaiter - 10 exceptions then success")]
    public Task<ConditionalValue> Awaiter_Exception10_ThenSuccess()
    {
        _attempt = 0;
        _exceptionSequence = TenExceptionSequence;
        return Awaiter.RunUntilSuccessfulOrTimeoutAsync(_exceptionsThenSuccessMethod, TenRetrySetup);
    }

    private static Action<AsyncRunOptions> CreateSetup(int maximumAttempts)
    {
        return o =>
        {
            o.Timeout = TimeSpan.FromSeconds(1);
            o.Delay = TimeSpan.Zero;
            o.MaximumAttempts = maximumAttempts;
        };
    }

    private Task<ConditionalValue> ImmediateSuccessAsync()
    {
        return SuccessfulTask;
    }

    private Task<ConditionalValue> UnsuccessfulThenSuccessAsync()
    {
        var currentAttempt = ++_attempt;
        return currentAttempt <= _unsuccessfulAttemptsBeforeSuccess ? UnsuccessfulTask : SuccessfulTask;
    }

    private Task<ConditionalValue> ExceptionsThenSuccessAsync()
    {
        var currentAttempt = _attempt++;
        return currentAttempt < _exceptionSequence.Length ? _exceptionSequence[currentAttempt] : SuccessfulTask;
    }
}
