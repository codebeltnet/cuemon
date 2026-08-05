using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Threading
{
    public class AsyncRunOptionsTest : Test
    {
        public AsyncRunOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldInitializeDefaults()
        {
            var sut = new AsyncRunOptions();

            Assert.Equal(TimeSpan.FromSeconds(5), sut.Timeout);
            Assert.Equal(TimeSpan.FromMilliseconds(100), sut.Delay);
            Assert.Equal(0, sut.MaximumAttempts);
            Assert.False(sut.CancellationToken.CanBeCanceled);
        }

        [Fact]
        public void ValidateOptions_ShouldThrowArgumentException_WhenTimeoutIsNegative()
        {
            var sut = new AsyncRunOptions
            {
                Timeout = TimeSpan.FromMilliseconds(-1)
            };

            var ex = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut));

            Assert.Equal("sut", ex.ParamName);
            Assert.IsType<InvalidOperationException>(ex.InnerException);
            Assert.Contains("Timeout cannot be negative.", ex.InnerException.Message);
        }

        [Fact]
        public void ValidateOptions_ShouldThrowArgumentException_WhenDelayIsNegative()
        {
            var sut = new AsyncRunOptions
            {
                Delay = TimeSpan.FromMilliseconds(-1)
            };

            var ex = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut));

            Assert.Equal("sut", ex.ParamName);
            Assert.IsType<InvalidOperationException>(ex.InnerException);
            Assert.Contains("Delay cannot be negative.", ex.InnerException.Message);
        }

        [Fact]
        public void ValidateOptions_ShouldAllowZeroDelay_WhenMaximumAttemptsIsPositive()
        {
            var sut = new AsyncRunOptions
            {
                Delay = TimeSpan.Zero,
                MaximumAttempts = 1
            };

            Validator.ThrowIfInvalidOptions(sut);

            Assert.Equal(1, sut.MaximumAttempts);
        }

        [Fact]
        public void ValidateOptions_ShouldThrowArgumentException_WhenMaximumAttemptsIsNegative()
        {
            var sut = new AsyncRunOptions
            {
                MaximumAttempts = -1
            };

            var ex = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut));

            Assert.Equal("sut", ex.ParamName);
            Assert.IsType<InvalidOperationException>(ex.InnerException);
            Assert.Contains("MaximumAttempts cannot be negative.", ex.InnerException.Message);
        }

        [Fact]
        public void ValidateOptions_ShouldThrowArgumentException_WhenDelayIsZeroAndMaximumAttemptsIsNotPositive()
        {
            var sut = new AsyncRunOptions
            {
                Delay = TimeSpan.Zero
            };

            var ex = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut));

            Assert.Equal("sut", ex.ParamName);
            Assert.IsType<InvalidOperationException>(ex.InnerException);
            Assert.Contains("MaximumAttempts must be configured with a positive value", ex.InnerException.Message);
        }
    }

    public class AwaiterTest : Test
    {
        private static readonly TimeSpan AttemptDuration = TimeSpan.FromMilliseconds(20);
        private static readonly TimeSpan RetryDelay = TimeSpan.FromMilliseconds(20);

        public AwaiterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RunUntilSuccessfulOrTimeoutAsync_ShouldThrowArgumentNullException_WhenMethodIsNullBeforeSetupIsEvaluated()
        {
            var setupCalls = 0;

            Assert.Throws<ArgumentNullException>((Action)(() => Awaiter.RunUntilSuccessfulOrTimeoutAsync(null, o => { setupCalls++; })));

            Assert.Equal(0, setupCalls);
        }

        [Fact]
        public void RunUntilSuccessfulOrTimeoutAsync_ShouldThrowArgumentException_WhenSetupConfiguresInvalidOptions()
        {
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new SuccessfulValue());
            }

            var ex = Assert.Throws<ArgumentException>((Action)(() => Awaiter.RunUntilSuccessfulOrTimeoutAsync(Method, o => { o.Timeout = TimeSpan.FromMilliseconds(-1); })));

            Assert.Equal("setup", ex.ParamName);
            Assert.Equal(0, callCount);
            Assert.IsType<InvalidOperationException>(ex.InnerException);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldThrowInvalidOperationException_WhenDelegateReturnsNullConditionalValue()
        {
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(null);
            }

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => Run(Method, TimeSpan.FromSeconds(1), RetryDelay));

            Assert.Equal(1, callCount);
            Assert.Contains("null ConditionalValue", ex.Message);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnOriginalConditionalValue_WhenFirstAttemptSucceeds()
        {
            var expected = new SuccessfulValue();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(expected);
            }

            var result = await Run(Method, TimeSpan.FromSeconds(1), RetryDelay);

            Assert.Same(expected, result);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldRetryAfterUnsuccessfulResultUntilSuccess()
        {
            var expected = new SuccessfulValue();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(callCount == 1 ? new UnsuccessfulValue() : expected);
            }

            var result = await Run(Method, TimeSpan.FromSeconds(1), RetryDelay);

            Assert.Same(expected, result);
            Assert.Equal(2, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnDefaultUnsuccessful_WhenRepeatedResultsRemainUnsuccessfulUntilTimeout()
        {
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var result = await Run(Method, TimeSpan.FromMilliseconds(80), RetryDelay);

            TestOutput.WriteLine("Call-count: " + callCount);

            Assert.IsType<UnsuccessfulValue>(result);
            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
            Assert.True(callCount >= 2);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldRetryAfterExceptionUntilSuccess()
        {
            var expected = new SuccessfulValue();
            var failure = new InvalidOperationException("fail");
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                if (callCount == 1) { throw failure; }
                return Task.FromResult<ConditionalValue>(expected);
            }

            var result = await Run(Method, TimeSpan.FromSeconds(1), RetryDelay);

            Assert.Same(expected, result);
            Assert.Null(result.Failure);
            Assert.Equal(2, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnSingleRetainedException_WhenTimeoutElapsesAfterCaughtException()
        {
            var expected = new InvalidOperationException("fail");
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return ThrowAfterAsync(expected, AttemptDuration);
            }

            var result = await Run(Method, TimeSpan.FromMilliseconds(5), RetryDelay);

            Assert.False(result.Succeeded);
            Assert.Same(expected, result.Failure);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnAggregateExceptionInEncounterOrder_WhenTimeoutElapsesAfterMultipleCaughtExceptions()
        {
            var first = new InvalidOperationException("first");
            var second = new ArgumentException("second");
            var third = new ApplicationException("third");
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                if (callCount == 1) { throw first; }
                if (callCount == 2) { throw second; }
                return ThrowAfterAsync(third, AttemptDuration);
            }

            var result = await Run(Method, TimeSpan.FromMilliseconds(10), TimeSpan.Zero, maximumAttempts: 3);
            var aggregate = Assert.IsType<AggregateException>(result.Failure);

            Assert.False(result.Succeeded);
            Assert.Equal(3, callCount);
            Assert.Equal(3, aggregate.InnerExceptions.Count);
            Assert.Same(first, aggregate.InnerExceptions[0]);
            Assert.Same(second, aggregate.InnerExceptions[1]);
            Assert.Same(third, aggregate.InnerExceptions[2]);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldThrowOperationCanceledException_WhenCancellationIsRequestedBeforeInitialAttempt()
        {
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.Cancel();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new SuccessfulValue());
            }

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Run(Method, TimeSpan.FromSeconds(1), RetryDelay, cancellationToken: cancellationSource.Token));

            Assert.Equal(0, callCount);
            Assert.Equal(cancellationSource.Token, ex.CancellationToken);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldPropagateOperationCanceledException_WhenDelegateCancels()
        {
            var delegateSource = new CancellationTokenSource();
            delegateSource.Cancel();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromCanceled<ConditionalValue>(delegateSource.Token);
            }

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Run(Method, TimeSpan.FromSeconds(1), RetryDelay));

            Assert.Equal(1, callCount);
            Assert.Equal(delegateSource.Token, ex.CancellationToken);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldUseResolvedCancellationTokenForRetryDelay()
        {
            var attemptSource = new CancellationTokenSource();
            var delaySource = new CancellationTokenSource();
            var resolvedTokens = new Queue<CancellationToken>(new[] { attemptSource.Token, delaySource.Token });
            var attemptCompleted = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                attemptCompleted.TrySetResult(null);
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var task = Run(Method, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), cancellationTokenProvider: () => resolvedTokens.Dequeue());

            await attemptCompleted.Task;
            await Task.Delay(TimeSpan.FromMilliseconds(30));
            delaySource.Cancel();

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);

            Assert.Equal(1, callCount);
            Assert.Equal(delaySource.Token, ex.CancellationToken);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldResolveCancellationTokenProviderBeforeEveryAttempt()
        {
            var activeSource = new CancellationTokenSource();
            var canceledSource = new CancellationTokenSource();
            canceledSource.Cancel();
            var resolvedTokens = new Queue<CancellationToken>(new[] { activeSource.Token, canceledSource.Token });
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Run(Method, TimeSpan.FromSeconds(1), TimeSpan.Zero, maximumAttempts: 2, cancellationTokenProvider: () => resolvedTokens.Dequeue()));

            Assert.Equal(1, callCount);
            Assert.Equal(canceledSource.Token, ex.CancellationToken);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldSucceedOnFinalAllowedAttempt_WhenDelayIsZero()
        {
            var expected = new SuccessfulValue();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(callCount < 3 ? new UnsuccessfulValue() : expected);
            }

            var result = await Run(Method, TimeSpan.FromSeconds(1), TimeSpan.Zero, maximumAttempts: 3);

            Assert.Same(expected, result);
            Assert.Equal(3, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnUnsuccessfulAfterMaximumAttempts_WhenDelayIsZero()
        {
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var result = await Run(Method, TimeSpan.FromSeconds(1), TimeSpan.Zero, maximumAttempts: 3);

            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
            Assert.Equal(3, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnAggregateExceptionAfterMaximumAttempts_WhenDelayIsZeroAndMultipleExceptionsAreCaught()
        {
            var first = new InvalidOperationException("first");
            var second = new ArgumentException("second");
            var third = new ApplicationException("third");
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                if (callCount == 1) { throw first; }
                if (callCount == 2) { throw second; }
                throw third;
            }

            var result = await Run(Method, TimeSpan.FromSeconds(1), TimeSpan.Zero, maximumAttempts: 3);
            var aggregate = Assert.IsType<AggregateException>(result.Failure);

            Assert.False(result.Succeeded);
            Assert.Equal(3, callCount);
            Assert.Equal(3, aggregate.InnerExceptions.Count);
            Assert.Same(first, aggregate.InnerExceptions[0]);
            Assert.Same(second, aggregate.InnerExceptions[1]);
            Assert.Same(third, aggregate.InnerExceptions[2]);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldAllowExactlyOneAttempt_WhenTimeoutIsZero()
        {
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var result = await Run(Method, TimeSpan.Zero, RetryDelay);

            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldNotRetryAfterTimeoutElapses()
        {
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return ReturnAfterAsync(new UnsuccessfulValue(), AttemptDuration);
            }

            var result = await Run(Method, TimeSpan.FromMilliseconds(5), TimeSpan.FromMilliseconds(200));

            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldCapDelayToRemainingTimeoutWindow()
        {
            var stopwatch = Stopwatch.StartNew();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var result = await Run(Method, TimeSpan.FromMilliseconds(40), TimeSpan.FromMilliseconds(200));

            stopwatch.Stop();

            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
            Assert.Equal(1, callCount);
            Assert.True(stopwatch.Elapsed < TimeSpan.FromMilliseconds(150), $"Expected capped delay, but elapsed was {stopwatch.Elapsed}.");
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnSuccess_WhenInFlightAttemptCompletesAfterTimeout()
        {
            var expected = new SuccessfulValue();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return ReturnAfterAsync(expected, AttemptDuration);
            }

            var result = await Run(Method, TimeSpan.FromMilliseconds(5), RetryDelay);

            Assert.Same(expected, result);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnUnsuccessful_WhenInFlightAttemptCompletesAfterTimeout()
        {
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return ReturnAfterAsync(new UnsuccessfulValue(), AttemptDuration);
            }

            var result = await Run(Method, TimeSpan.FromMilliseconds(5), RetryDelay);

            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
            Assert.Equal(1, callCount);
        }

        private static Task<ConditionalValue> Run(Func<Task<ConditionalValue>> method, TimeSpan timeout, TimeSpan delay, int maximumAttempts = 0, CancellationToken? cancellationToken = null, Func<CancellationToken> cancellationTokenProvider = null)
        {
            return Awaiter.RunUntilSuccessfulOrTimeoutAsync(method, Configure(timeout, delay, maximumAttempts, cancellationToken, cancellationTokenProvider));
        }

        private static Action<AsyncRunOptions> Configure(TimeSpan timeout, TimeSpan delay, int maximumAttempts = 0, CancellationToken? cancellationToken = null, Func<CancellationToken> cancellationTokenProvider = null)
        {
            return o =>
            {
                o.Timeout = timeout;
                o.Delay = delay;
                o.MaximumAttempts = maximumAttempts;
                if (cancellationToken.HasValue) { o.CancellationToken = cancellationToken.Value; }
                if (cancellationTokenProvider != null) { o.CancellationTokenProvider = cancellationTokenProvider; }
            };
        }

        private static async Task<ConditionalValue> ReturnAfterAsync(ConditionalValue result, TimeSpan delay)
        {
            await Task.Delay(delay).ConfigureAwait(false);
            return result;
        }

        private static async Task<ConditionalValue> ThrowAfterAsync(Exception exception, TimeSpan delay)
        {
            await Task.Delay(delay).ConfigureAwait(false);
            throw exception;
        }
    }
}
