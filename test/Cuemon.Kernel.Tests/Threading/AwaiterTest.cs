using System;
using System.Collections.Generic;
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
            Assert.Same(TimeProvider.System, sut.TimeProvider);
            Assert.False(sut.CancellationToken.CanBeCanceled);
        }

        [Fact]
        public void ValidateOptions_ShouldAllowZeroTimeout()
        {
            var sut = new AsyncRunOptions
            {
                Timeout = TimeSpan.Zero
            };

            Validator.ThrowIfInvalidOptions(sut);

            Assert.Equal(TimeSpan.Zero, sut.Timeout);
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

            Assert.Equal(TimeSpan.Zero, sut.Delay);
            Assert.Equal(1, sut.MaximumAttempts);
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
        public void ValidateOptions_ShouldThrowArgumentException_WhenTimeProviderIsNull()
        {
            var sut = new AsyncRunOptions
            {
                TimeProvider = null
            };

            var ex = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut));

            Assert.Equal("sut", ex.ParamName);
            Assert.IsType<InvalidOperationException>(ex.InnerException);
            Assert.Contains("TimeProvider cannot be null.", ex.InnerException.Message);
        }


        [Fact]
        public void ValidateOptions_ShouldThrowArgumentException_WhenMaximumAttemptsIsNotPositive()
        {
            var sut = new AsyncRunOptions
            {
                MaximumAttempts = -1
            };

            var ex = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut));

            Assert.Equal("sut", ex.ParamName);
            Assert.IsType<InvalidOperationException>(ex.InnerException);
            Assert.Contains("MaximumAttempts must be greater than zero when specified.", ex.InnerException.Message);
        }

        [Fact]
        public void ValidateOptions_ShouldThrowArgumentException_WhenDelayIsZeroAndMaximumAttemptsIsNotSpecified()
        {
            var sut = new AsyncRunOptions
            {
                Delay = TimeSpan.Zero
            };

            var ex = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut));

            Assert.Equal("sut", ex.ParamName);
            Assert.IsType<InvalidOperationException>(ex.InnerException);
            Assert.Contains("MaximumAttempts must be specified when Delay is TimeSpan.Zero", ex.InnerException.Message);
        }
    }

    public class AwaiterTest : Test
    {
        public AwaiterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RunUntilSuccessfulOrTimeoutAsync_ShouldThrowArgumentNullException_WhenMethodIsNullBeforeSetupIsEvaluated()
        {
            var setupCalls = 0;

            Assert.Throws<ArgumentNullException>((Action)(() => Awaiter.RunUntilSuccessfulOrTimeoutAsync((Func<Task<ConditionalValue>>)null, o => { setupCalls++; })));
            Assert.Throws<ArgumentNullException>((Action)(() => Awaiter.RunUntilSuccessfulOrTimeoutAsync((Func<CancellationToken, Task<ConditionalValue>>)null, o => { setupCalls++; })));

            Assert.Equal(0, setupCalls);
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

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100)));

            Assert.Equal(1, callCount);
            Assert.Contains("null ConditionalValue", ex.Message);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldThrowInvalidOperationException_WhenTokenAwareDelegateReturnsNullConditionalValue()
        {
            var callCount = 0;

            Task<ConditionalValue> Method(CancellationToken cancellationToken)
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(null);
            }

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100)));

            Assert.Equal(1, callCount);
            Assert.Contains("null ConditionalValue", ex.Message);
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
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnOriginalConditionalValue_WhenFirstAttemptSucceeds()
        {
            var timeProvider = CreateTimeProvider();
            var expected = new SuccessfulValue();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(expected);
            }

            var result = await Run(Method, timeProvider, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

            Assert.Same(expected, result);
            Assert.Equal(1, callCount);

            await AdvanceAsync(timeProvider, TimeSpan.FromSeconds(5));

            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldUseConfiguredCancellationToken_WhenNoProviderIsConfigured()
        {
            var source = new CancellationTokenSource();
            var expected = new SuccessfulValue();
            var observed = CancellationToken.None;

            Task<ConditionalValue> Method(CancellationToken cancellationToken)
            {
                observed = cancellationToken;
                return Task.FromResult<ConditionalValue>(expected);
            }

            var result = await Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100), cancellationToken: source.Token);

            Assert.Same(expected, result);
            Assert.Equal(source.Token, observed);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldPreferCancellationTokenProviderOverConfiguredToken()
        {
            var configuredSource = new CancellationTokenSource();
            var providerSource = new CancellationTokenSource();
            var observed = CancellationToken.None;

            Task<ConditionalValue> Method(CancellationToken cancellationToken)
            {
                observed = cancellationToken;
                return Task.FromResult<ConditionalValue>(new SuccessfulValue());
            }

            var result = await Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100), cancellationToken: configuredSource.Token, cancellationTokenProvider: () => providerSource.Token);

            Assert.True(result.Succeeded);
            Assert.Equal(providerSource.Token, observed);
            Assert.NotEqual(configuredSource.Token, observed);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldResolveProviderBeforeEveryAttemptAndDelayAndPassAttemptTokens()
        {
            var timeProvider = CreateTimeProvider();
            var firstAttemptSource = new CancellationTokenSource();
            var delaySource = new CancellationTokenSource();
            var secondAttemptSource = new CancellationTokenSource();
            var resolvedTokens = new Queue<CancellationToken>(new[] { firstAttemptSource.Token, delaySource.Token, secondAttemptSource.Token });
            var observedAttemptTokens = new List<CancellationToken>();
            var providerCalls = 0;
            var callCount = 0;

            Task<ConditionalValue> Method(CancellationToken cancellationToken)
            {
                observedAttemptTokens.Add(cancellationToken);
                callCount++;
                return Task.FromResult<ConditionalValue>(callCount == 1 ? new UnsuccessfulValue() : new SuccessfulValue());
            }

            var task = Run(Method, timeProvider, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100), cancellationTokenProvider: () =>
            {
                providerCalls++;
                return resolvedTokens.Dequeue();
            });

            Assert.False(task.IsCompleted);
            Assert.Equal(2, providerCalls);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(100));

            var result = await task;

            Assert.True(result.Succeeded);
            Assert.Equal(2, callCount);
            Assert.Equal(3, providerCalls);
            Assert.Equal(2, observedAttemptTokens.Count);
            Assert.Equal(firstAttemptSource.Token, observedAttemptTokens[0]);
            Assert.Equal(secondAttemptSource.Token, observedAttemptTokens[1]);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldRetryAfterUnsuccessfulResultsUntilSuccess()
        {
            var timeProvider = CreateTimeProvider();
            var expected = new SuccessfulValue();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(callCount < 3 ? new UnsuccessfulValue() : expected);
            }

            var task = Run(Method, timeProvider, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100));

            Assert.False(task.IsCompleted);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(100));
            Assert.False(task.IsCompleted);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(100));

            var result = await task;

            Assert.Same(expected, result);
            Assert.Equal(3, callCount);

            await AdvanceAsync(timeProvider, TimeSpan.FromSeconds(1));

            Assert.Equal(3, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldRetryAfterExceptionsUntilSuccess()
        {
            var timeProvider = CreateTimeProvider();
            var expected = new SuccessfulValue();
            var failure = new InvalidOperationException("fail");
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                if (callCount == 1) { throw failure; }
                return Task.FromResult<ConditionalValue>(expected);
            }

            var task = Run(Method, timeProvider, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100));

            Assert.False(task.IsCompleted);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(100));

            var result = await task;

            Assert.Same(expected, result);
            Assert.Null(result.Failure);
            Assert.Equal(2, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldDiscardRetainedExceptions_WhenLaterAttemptSucceeds()
        {
            var timeProvider = CreateTimeProvider();
            var expected = new SuccessfulValue();
            var failure = new InvalidOperationException("fail");
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                if (callCount == 1) { return Task.FromResult<ConditionalValue>(new UnsuccessfulValue()); }
                if (callCount == 2) { throw failure; }
                return Task.FromResult<ConditionalValue>(expected);
            }

            var task = Run(Method, timeProvider, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100));

            Assert.False(task.IsCompleted);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(100));
            Assert.False(task.IsCompleted);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(100));

            var result = await task;

            Assert.Same(expected, result);
            Assert.Null(result.Failure);
            Assert.Equal(3, callCount);

            await AdvanceAsync(timeProvider, TimeSpan.FromSeconds(1));

            Assert.Equal(3, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldAllowExactlyOneAttempt_WhenTimeoutIsZeroAndAttemptIsUnsuccessful()
        {
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var result = await Run(Method, CreateTimeProvider(), TimeSpan.Zero, TimeSpan.FromMilliseconds(100));

            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldPreserveException_WhenDelegateThrowsSynchronouslyAtZeroTimeout()
        {
            var expected = new InvalidOperationException("fail");
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                throw expected;
            }

            var result = await Run(Method, CreateTimeProvider(), TimeSpan.Zero, TimeSpan.FromMilliseconds(100));

            Assert.False(result.Succeeded);
            Assert.Same(expected, result.Failure);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldPreserveException_WhenDelegateReturnsFaultedTaskAtZeroTimeout()
        {
            var expected = new InvalidOperationException("fail");
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromException<ConditionalValue>(expected);
            }

            var result = await Run(Method, CreateTimeProvider(), TimeSpan.Zero, TimeSpan.FromMilliseconds(100));

            Assert.False(result.Succeeded);
            Assert.Same(expected, result.Failure);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnDefaultUnsuccessful_WhenTimeoutExpiresWithoutExceptions()
        {
            var timeProvider = CreateTimeProvider();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var task = Run(Method, timeProvider, TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(100));

            Assert.False(task.IsCompleted);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(100));
            Assert.False(task.IsCompleted);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(100));
            Assert.False(task.IsCompleted);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(50));

            var result = await task;

            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
            Assert.Equal(3, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnAggregateExceptionInEncounterOrder_WhenTimeoutExpiresWithMultipleExceptions()
        {
            var timeProvider = CreateTimeProvider();
            var first = new InvalidOperationException("first");
            var second = new ArgumentException("second");
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                if (callCount == 1)
                {
                    timeProvider.Advance(TimeSpan.FromMilliseconds(2));
                    throw first;
                }

                timeProvider.Advance(TimeSpan.FromMilliseconds(2));
                throw second;
            }

            var task = Run(Method, timeProvider, TimeSpan.FromMilliseconds(5), TimeSpan.FromMilliseconds(1));

            Assert.False(task.IsCompleted);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(1));

            var result = await task;
            var aggregate = Assert.IsType<AggregateException>(result.Failure);

            Assert.False(result.Succeeded);
            Assert.Equal(2, callCount);
            Assert.Equal(2, aggregate.InnerExceptions.Count);
            Assert.Same(first, aggregate.InnerExceptions[0]);
            Assert.Same(second, aggregate.InnerExceptions[1]);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldAggregateOnlyCaughtExceptions_WhenTimeoutIncludesUnsuccessfulResults()
        {
            var timeProvider = CreateTimeProvider();
            var expected = new InvalidOperationException("fail");
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                if (callCount == 1) { return Task.FromResult<ConditionalValue>(new UnsuccessfulValue()); }
                timeProvider.Advance(TimeSpan.FromMilliseconds(4));
                throw expected;
            }

            var task = Run(Method, timeProvider, TimeSpan.FromMilliseconds(5), TimeSpan.FromMilliseconds(1));

            Assert.False(task.IsCompleted);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(1));

            var result = await task;

            Assert.False(result.Succeeded);
            Assert.Same(expected, result.Failure);
            Assert.Equal(2, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldCapDelayToRemainingTimeoutWithoutStartingAnotherAttempt()
        {
            var timeProvider = CreateTimeProvider();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var task = Run(Method, timeProvider, TimeSpan.FromMilliseconds(5), TimeSpan.FromMilliseconds(10));

            Assert.False(task.IsCompleted);
            Assert.Equal(1, callCount);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(4));

            Assert.False(task.IsCompleted);
            Assert.Equal(1, callCount);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(1));

            var result = await task;

            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldNotDelayOrRetry_WhenAttemptReachesDeadline()
        {
            var timeProvider = CreateTimeProvider();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                timeProvider.Advance(TimeSpan.FromMilliseconds(5));
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var result = await Run(Method, timeProvider, TimeSpan.FromMilliseconds(5), TimeSpan.FromMilliseconds(10));

            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldNotDelayOrRetry_WhenFinalAttemptPassesDeadline()
        {
            var timeProvider = CreateTimeProvider();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                if (callCount == 1) { return Task.FromResult<ConditionalValue>(new UnsuccessfulValue()); }
                timeProvider.Advance(TimeSpan.FromMilliseconds(10));
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var task = Run(Method, timeProvider, TimeSpan.FromMilliseconds(6), TimeSpan.FromMilliseconds(2));

            Assert.False(task.IsCompleted);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(2));

            var result = await task;

            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
            Assert.Equal(2, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldCancelBeforeFirstAttempt_WhenResolvedTokenIsAlreadyCanceled()
        {
            var canceledSource = new CancellationTokenSource();
            canceledSource.Cancel();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new SuccessfulValue());
            }

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100), cancellationTokenProvider: () => canceledSource.Token));

            Assert.Equal(0, callCount);
            Assert.Equal(canceledSource.Token, ex.CancellationToken);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldPropagateCancellation_WhenTokenAwareDelegateObservesAttemptToken()
        {
            var cancellationSource = new CancellationTokenSource();
            var callCount = 0;

            async Task<ConditionalValue> Method(CancellationToken cancellationToken)
            {
                callCount++;
                await Task.Delay(Timeout.Infinite, cancellationToken);
                return new SuccessfulValue();
            }

            var task = Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100), cancellationToken: cancellationSource.Token);

            cancellationSource.Cancel();

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);

            Assert.Equal(1, callCount);
            Assert.Equal(cancellationSource.Token, ex.CancellationToken);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldPropagateCancellation_WhenNonTokenAwareDelegateThrowsOperationCanceledException()
        {
            var delegateSource = new CancellationTokenSource();
            var callCount = 0;

            async Task<ConditionalValue> Method()
            {
                callCount++;
                await Task.Delay(Timeout.Infinite, delegateSource.Token);
                return new SuccessfulValue();
            }

            var task = Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100));

            delegateSource.Cancel();

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);

            Assert.Equal(1, callCount);
            Assert.Equal(delegateSource.Token, ex.CancellationToken);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldCancelAfterNonTokenAwareAttemptCompletes_WhenOuterTokenWasCanceledDuringAttempt()
        {
            var outerSource = new CancellationTokenSource();
            var completion = new TaskCompletionSource<ConditionalValue>(TaskCreationOptions.RunContinuationsAsynchronously);
            var callCount = 0;

            async Task<ConditionalValue> Method()
            {
                callCount++;
                return await completion.Task;
            }

            var task = Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.Zero, 3, outerSource.Token);

            outerSource.Cancel();
            completion.SetResult(new SuccessfulValue());

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);

            Assert.Equal(1, callCount);
            Assert.Equal(outerSource.Token, ex.CancellationToken);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldCancelBeforeDelay_WhenProviderChangesToCanceledToken()
        {
            var activeSource = new CancellationTokenSource();
            var canceledSource = new CancellationTokenSource();
            canceledSource.Cancel();
            var resolvedTokens = new Queue<CancellationToken>(new[] { activeSource.Token, canceledSource.Token });
            var providerCalls = 0;
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100), cancellationTokenProvider: () =>
            {
                providerCalls++;
                return resolvedTokens.Dequeue();
            }));

            Assert.Equal(1, callCount);
            Assert.Equal(2, providerCalls);
            Assert.Equal(canceledSource.Token, ex.CancellationToken);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldCancelDuringDelay_WithoutFurtherAttempts()
        {
            var timeProvider = CreateTimeProvider();
            var cancellationSource = new CancellationTokenSource();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var task = Run(Method, timeProvider, TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1), cancellationToken: cancellationSource.Token);

            Assert.False(task.IsCompleted);

            cancellationSource.Cancel();

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);

            Assert.Equal(1, callCount);
            Assert.Equal(cancellationSource.Token, ex.CancellationToken);

            await AdvanceAsync(timeProvider, TimeSpan.FromMinutes(1));

            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldCancelBeforeNextAttempt_WhenProviderChangesAfterDelay()
        {
            var timeProvider = CreateTimeProvider();
            var firstAttemptSource = new CancellationTokenSource();
            var delaySource = new CancellationTokenSource();
            var canceledSource = new CancellationTokenSource();
            canceledSource.Cancel();
            var resolvedTokens = new Queue<CancellationToken>(new[] { firstAttemptSource.Token, delaySource.Token, canceledSource.Token });
            var providerCalls = 0;
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var task = Run(Method, timeProvider, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100), cancellationTokenProvider: () =>
            {
                providerCalls++;
                return resolvedTokens.Dequeue();
            });

            Assert.False(task.IsCompleted);
            Assert.Equal(2, providerCalls);

            await AdvanceAsync(timeProvider, TimeSpan.FromMilliseconds(100));

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);

            Assert.Equal(1, callCount);
            Assert.Equal(3, providerCalls);
            Assert.Equal(canceledSource.Token, ex.CancellationToken);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldCancelBeforeNextAttempt_WhenDelayIsZeroAndProviderChanges()
        {
            var firstAttemptSource = new CancellationTokenSource();
            var canceledSource = new CancellationTokenSource();
            canceledSource.Cancel();
            var resolvedTokens = new Queue<CancellationToken>(new[] { firstAttemptSource.Token, canceledSource.Token });
            var providerCalls = 0;
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.Zero, 2, cancellationTokenProvider: () =>
            {
                providerCalls++;
                return resolvedTokens.Dequeue();
            }));

            Assert.Equal(1, callCount);
            Assert.Equal(2, providerCalls);
            Assert.Equal(canceledSource.Token, ex.CancellationToken);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldCancelInsteadOfAggregatingExceptions_WhenCancellationOccursAfterRetainedException()
        {
            var timeProvider = CreateTimeProvider();
            var cancellationSource = new CancellationTokenSource();
            var retained = new InvalidOperationException("fail");
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                if (callCount == 1) { throw retained; }
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var task = Run(Method, timeProvider, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100), cancellationToken: cancellationSource.Token);

            Assert.False(task.IsCompleted);

            cancellationSource.Cancel();

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);

            Assert.Equal(1, callCount);
            Assert.Equal(cancellationSource.Token, ex.CancellationToken);

            await AdvanceAsync(timeProvider, TimeSpan.FromSeconds(1));

            Assert.Equal(1, callCount);
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

            var result = await Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.Zero, 3);

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

            var result = await Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.Zero, 3);

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

            var result = await Run(Method, CreateTimeProvider(), TimeSpan.FromSeconds(1), TimeSpan.Zero, 3);
            var aggregate = Assert.IsType<AggregateException>(result.Failure);

            Assert.False(result.Succeeded);
            Assert.Equal(3, callCount);
            Assert.Equal(3, aggregate.InnerExceptions.Count);
            Assert.Same(first, aggregate.InnerExceptions[0]);
            Assert.Same(second, aggregate.InnerExceptions[1]);
            Assert.Same(third, aggregate.InnerExceptions[2]);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldUseIdenticalRetrySemanticsAcrossOverloads()
        {
            var existingTimeProvider = CreateTimeProvider();
            var tokenAwareTimeProvider = CreateTimeProvider();
            var existingCalls = 0;
            var tokenAwareCalls = 0;

            Task<ConditionalValue> ExistingMethod()
            {
                existingCalls++;
                return Task.FromResult<ConditionalValue>(existingCalls == 1 ? new UnsuccessfulValue() : new SuccessfulValue());
            }

            Task<ConditionalValue> TokenAwareMethod(CancellationToken cancellationToken)
            {
                tokenAwareCalls++;
                return Task.FromResult<ConditionalValue>(tokenAwareCalls == 1 ? new UnsuccessfulValue() : new SuccessfulValue());
            }

            var existingTask = Run(ExistingMethod, existingTimeProvider, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100));
            var tokenAwareTask = Run(TokenAwareMethod, tokenAwareTimeProvider, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100));

            Assert.False(existingTask.IsCompleted);
            Assert.False(tokenAwareTask.IsCompleted);

            await AdvanceAsync(existingTimeProvider, TimeSpan.FromMilliseconds(100));
            await AdvanceAsync(tokenAwareTimeProvider, TimeSpan.FromMilliseconds(100));

            var existingResult = await existingTask;
            var tokenAwareResult = await tokenAwareTask;

            Assert.True(existingResult.Succeeded);
            Assert.True(tokenAwareResult.Succeeded);
            Assert.Equal(2, existingCalls);
            Assert.Equal(existingCalls, tokenAwareCalls);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldUseSystemTimeProviderDelayBranch_WhenDelayIsCanceled()
        {
            var cancellationSource = new CancellationTokenSource();
            var callCount = 0;

            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            var task = Run(Method, TimeProvider.System, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), cancellationToken: cancellationSource.Token);

            Assert.False(task.IsCompleted);

            cancellationSource.Cancel();

            var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);

            Assert.Equal(1, callCount);
            Assert.Equal(cancellationSource.Token, ex.CancellationToken);
        }

        private static ManualTimeProvider CreateTimeProvider()
        {
            return new ManualTimeProvider(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        }

        private static async Task AdvanceAsync(ManualTimeProvider timeProvider, TimeSpan delay)
        {
            timeProvider.Advance(delay);
            await Task.Yield();
            timeProvider.Advance(TimeSpan.Zero);
            await Task.Yield();
        }

        private static Task<ConditionalValue> Run(Func<Task<ConditionalValue>> method, TimeProvider timeProvider, TimeSpan timeout, TimeSpan delay, int? maximumAttempts = null, CancellationToken? cancellationToken = null, Func<CancellationToken> cancellationTokenProvider = null)
        {
            return Awaiter.RunUntilSuccessfulOrTimeoutAsync(method, Configure(timeProvider, timeout, delay, maximumAttempts, cancellationToken, cancellationTokenProvider));
        }

        private static Task<ConditionalValue> Run(Func<CancellationToken, Task<ConditionalValue>> method, TimeProvider timeProvider, TimeSpan timeout, TimeSpan delay, int? maximumAttempts = null, CancellationToken? cancellationToken = null, Func<CancellationToken> cancellationTokenProvider = null)
        {
            return Awaiter.RunUntilSuccessfulOrTimeoutAsync(method, Configure(timeProvider, timeout, delay, maximumAttempts, cancellationToken, cancellationTokenProvider));
        }

        private static Action<AsyncRunOptions> Configure(TimeProvider timeProvider, TimeSpan timeout, TimeSpan delay, int? maximumAttempts = null, CancellationToken? cancellationToken = null, Func<CancellationToken> cancellationTokenProvider = null)
        {
            return o =>
            {
                o.TimeProvider = timeProvider;
                o.Timeout = timeout;
                o.Delay = delay;
                o.MaximumAttempts = maximumAttempts ?? 0;
                if (cancellationToken.HasValue) { o.CancellationToken = cancellationToken.Value; }
                if (cancellationTokenProvider != null) { o.CancellationTokenProvider = cancellationTokenProvider; }
            };
        }

        private sealed class ManualTimeProvider : TimeProvider
        {
            private readonly List<ManualTimer> _timers = new List<ManualTimer>();
            private DateTimeOffset _utcNow;

            public ManualTimeProvider(DateTimeOffset utcNow)
            {
                _utcNow = utcNow;
            }

            public override DateTimeOffset GetUtcNow()
            {
                return _utcNow;
            }

            public void Advance(TimeSpan delay)
            {
                _utcNow = _utcNow.Add(delay);
                ProcessTimers();
            }

            public override ITimer CreateTimer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
            {
                var timer = new ManualTimer(this, callback, state, dueTime, period);
                _timers.Add(timer);
                return timer;
            }

            private void ProcessTimers()
            {
                while (true)
                {
                    ManualTimer timerToFire = null;
                    foreach (var timer in _timers)
                    {
                        if (!timer.Disposed && timer.NextRun <= _utcNow && (timerToFire == null || timer.NextRun < timerToFire.NextRun))
                        {
                            timerToFire = timer;
                        }
                    }

                    if (timerToFire == null) { break; }

                    if (timerToFire.IsRecurring)
                    {
                        timerToFire.NextRun = timerToFire.NextRun.Add(timerToFire.Period);
                    }
                    else
                    {
                        timerToFire.Dispose();
                    }

                    timerToFire.Callback(timerToFire.State);
                }

                _timers.RemoveAll(timer => timer.Disposed);
            }

            private sealed class ManualTimer : ITimer
            {
                private readonly ManualTimeProvider _provider;

                public ManualTimer(ManualTimeProvider provider, TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
                {
                    _provider = provider;
                    Callback = callback;
                    State = state;
                    Change(dueTime, period);
                }

                public TimerCallback Callback { get; }

                public object State { get; }

                public TimeSpan Period { get; private set; }

                public DateTimeOffset NextRun { get; set; }

                public bool Disposed { get; private set; }

                public bool IsRecurring => Period != Timeout.InfiniteTimeSpan && Period > TimeSpan.Zero;

                public bool Change(TimeSpan dueTime, TimeSpan period)
                {
                    if (Disposed) { return false; }
                    Period = period;
                    NextRun = dueTime == Timeout.InfiniteTimeSpan ? DateTimeOffset.MaxValue : _provider._utcNow.Add(dueTime);
                    return true;
                }

                public void Dispose()
                {
                    Disposed = true;
                }

                public ValueTask DisposeAsync()
                {
                    Dispose();
                    return default;
                }
            }
        }
    }
}
