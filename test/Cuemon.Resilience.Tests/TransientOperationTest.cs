using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Diagnostics;
using Codebelt.Extensions.Xunit;
using Cuemon.Resilience.Assets;
using Xunit;

namespace Cuemon.Resilience
{
    public class TransientOperationTest : Test
    {
        private readonly ConcurrentDictionary<Guid, int> _retryTracker = new ConcurrentDictionary<Guid, int>();
        private readonly ConcurrentDictionary<Guid, TransientFaultEvidence> _transientFaultTracker = new ConcurrentDictionary<Guid, TransientFaultEvidence>();

        private const string ExpectedResult = "OK";
        private const int ExpectedRetryAttempts = 2;
        private static readonly TimeSpan Jitter = TimeSpan.FromSeconds(Generate.RandomNumber(7, 15));
        private const int NormalRunIncrement = 1;
        private const int DescriptiveExceptionCauseIncrement = 1;
        private static readonly TimeSpan ExpectedRecoveryWaitTime = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan ExpectedMaximumAllowedLatency = TimeSpan.FromMilliseconds(500);

        public TransientOperationTest(ITestOutputHelper output) : base(output)
        {
            TransientOperation.FaultCallback = evidence => RetryTrackerCallback(evidence, _transientFaultTracker);
            TransientOperationOptionsCallback = o =>
            {
                o.DetectionStrategy = DetectionStrategyCallback;
                o.RetryAttempts = ExpectedRetryAttempts;
                o.RetryStrategy = RetryStrategyCallback;
                o.MaximumAllowedLatency = ExpectedMaximumAllowedLatency;
            };
        }

        private static void RetryTrackerCallback(TransientFaultEvidence tfe, ConcurrentDictionary<Guid, TransientFaultEvidence> transientFaultTracker)
        {
            var indexOfId = Array.FindIndex(tfe.Descriptor.Parameters, name => name.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (tfe.Descriptor.Arguments[indexOfId] is Guid oId)
            {
                transientFaultTracker.TryAdd(oId, tfe);
            }
        }

        private bool DetectionStrategyCallback(Exception ex)
        {
            return ex is HttpRequestException;
        }

        private TimeSpan RetryStrategyCallback(int retry)
        {
            return ExpectedRecoveryWaitTime;
        }

        private Action<TransientOperationOptions> TransientOperationOptionsCallback { get; }

        [Fact]
        public void WithFunc_ShouldBypassTransientFaultHandling()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = TimeMeasure.WithFunc(() => TransientOperation.WithFunc(FuncTransientOperation.MethodThatReturnsOkString, id, _retryTracker, TransientOperationOptionsCallback));

            Assert.Equal(0, (int)profiler.Elapsed.TotalSeconds);
            Assert.Equal(ExpectedResult, profiler.Result);
            Assert.Equal(0, _retryTracker[id]);
        }

        [Fact]
        public void WithFunc_ShouldTriggerRetryAndSucceed()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = TimeMeasure.WithFunc(() => TransientOperation.WithFunc(FuncTransientOperation.FailUntilExpectedRetryAttemptsIsReached, id, ExpectedRetryAttempts, _retryTracker, TransientOperationOptionsCallback));

            Assert.Equal(ExpectedResult, profiler.Result);
            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
        }

        [Fact]
        public void WithFunc_ShouldTriggerTransientFaultException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = TimeMeasure.WithAction(() =>
            {
                var aex = Assert.Throws<AggregateException>(() => TransientOperation.WithFunc(FuncTransientOperation.TriggerTransientFaultException, id, _retryTracker, TransientOperationOptionsCallback));
                Assert.IsType<TransientFaultException>(aex.InnerExceptions.First());
                Assert.Equal(NormalRunIncrement + ExpectedRetryAttempts + DescriptiveExceptionCauseIncrement, aex.InnerExceptions.Count);
            });

            var tfe = _transientFaultTracker.Single(pair => pair.Key == id).Value;
            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
            Assert.Equal((int)TimeSpan.FromSeconds(ExpectedRetryAttempts).TotalSeconds, (int)tfe.TotalRecoveryWaitTime.TotalSeconds);
            Assert.Equal((int)ExpectedRecoveryWaitTime.TotalSeconds, (int)tfe.RecoveryWaitTime.TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, tfe.Attempts);

            TestOutput.WriteLine(tfe.ToString());
        }

        [Fact]
        public void WithFunc_ShouldTriggerLatencyException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = TimeMeasure.WithAction(() =>
            {
                var aex = Assert.Throws<AggregateException>(() => TransientOperation.WithFunc(FuncTransientOperation.TriggerLatencyException, id, _retryTracker, TransientOperationOptionsCallback));
                Assert.IsType<LatencyException>(aex.InnerExceptions.First());
                Assert.Equal(NormalRunIncrement + DescriptiveExceptionCauseIncrement, aex.InnerExceptions.Count);
                TestOutput.WriteLine(aex.ToString());
            });

            Assert.True(ExpectedMaximumAllowedLatency < profiler.Elapsed, "ExpectedMaximumAllowedLatency < profiler.Elapsed");
        }

        [Fact]
        public void WithFunc_ShouldTriggerInvalidOperationException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = TimeMeasure.WithAction(() =>
            {
                var aex = Assert.Throws<AggregateException>(() => TransientOperation.WithFunc(FuncTransientOperation.FailWithNonTransientFaultException, id, ExpectedRetryAttempts, _retryTracker, TransientOperationOptionsCallback));
                Assert.IsType<InvalidOperationException>(aex.InnerExceptions.First());

                TestOutput.WriteLine(aex.ToString());
            });

            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
        }

        [Fact]
        public void WithAction_ShouldBypassTransientFaultHandling()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = TimeMeasure.WithAction(() => TransientOperation.WithAction(ActionTransientOperation.MethodThatReturnsOkString, id, _retryTracker, TransientOperationOptionsCallback));

            Assert.Equal(0, (int)profiler.Elapsed.TotalSeconds);
            Assert.Equal(0, _retryTracker[id]);
        }

        [Fact]
        public void WithAction_ShouldTriggerRetryAndSucceed()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = TimeMeasure.WithAction(() => TransientOperation.WithAction(ActionTransientOperation.FailUntilExpectedRetryAttemptsIsReached, id, ExpectedRetryAttempts, _retryTracker, TransientOperationOptionsCallback));

            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
        }

        [Fact]
        public void WithAction_ShouldTriggerTransientFaultException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = TimeMeasure.WithAction(() =>
            {
                var aex = Assert.Throws<AggregateException>(() => TransientOperation.WithAction(ActionTransientOperation.TriggerTransientFaultException, id, _retryTracker, TransientOperationOptionsCallback));
                Assert.IsType<TransientFaultException>(aex.InnerExceptions.First());
                Assert.Equal(NormalRunIncrement + ExpectedRetryAttempts + DescriptiveExceptionCauseIncrement, aex.InnerExceptions.Count);
            });

            var tfe = _transientFaultTracker.Single(pair => pair.Key == id).Value;
            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
            Assert.Equal((int)TimeSpan.FromSeconds(ExpectedRetryAttempts).TotalSeconds, (int)tfe.TotalRecoveryWaitTime.TotalSeconds);
            Assert.Equal((int)ExpectedRecoveryWaitTime.TotalSeconds, (int)tfe.RecoveryWaitTime.TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, tfe.Attempts);

            TestOutput.WriteLine(tfe.ToString());
        }

        [Fact]
        public void WithAction_ShouldTriggerLatencyException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = TimeMeasure.WithAction(() =>
            {
                var aex = Assert.Throws<AggregateException>(() => TransientOperation.WithAction(ActionTransientOperation.TriggerLatencyException, id, _retryTracker, TransientOperationOptionsCallback));
                Assert.IsType<LatencyException>(aex.InnerExceptions.First());
                Assert.Equal(NormalRunIncrement + DescriptiveExceptionCauseIncrement, aex.InnerExceptions.Count);
                TestOutput.WriteLine(aex.ToString());
            });

            Assert.True(ExpectedMaximumAllowedLatency < profiler.Elapsed, "ExpectedMaximumAllowedLatency < profiler.Elapsed");
        }

        [Fact]
        public void WithAction_ShouldTriggerInvalidOperationException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = TimeMeasure.WithAction(() =>
            {
                var aex = Assert.Throws<AggregateException>(() => TransientOperation.WithAction(ActionTransientOperation.FailWithNonTransientFaultException, id, ExpectedRetryAttempts, _retryTracker, TransientOperationOptionsCallback));
                Assert.IsType<InvalidOperationException>(aex.InnerExceptions.First());

                TestOutput.WriteLine(aex.ToString());
            });

            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
        }

        [Fact]
        public async Task WithActionAsync_ShouldBypassTransientFaultHandling()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = await TimeMeasure.WithActionAsync(ct => TransientOperation.WithActionAsync(AsyncActionTransientOperation.MethodThatReturnsOkStringAsync, id, _retryTracker, TransientOperationOptionsCallback));

            Assert.Equal(0, (int)profiler.Elapsed.TotalSeconds);
            Assert.Equal(0, _retryTracker[id]);
        }

        [Fact]
        public async Task WithActionAsync_ShouldTriggerRetryAndSucceed()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = await TimeMeasure.WithActionAsync(ct => TransientOperation.WithActionAsync(AsyncActionTransientOperation.FailUntilExpectedRetryAttemptsIsReachedAsync, id, ExpectedRetryAttempts, _retryTracker, TransientOperationOptionsCallback));

            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
        }

        [Fact]
        public async Task WithActionAsync_ShouldTriggerTransientFaultException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = await TimeMeasure.WithActionAsync(async ct =>
            {
                var aex = await Assert.ThrowsAsync<AggregateException>(() => TransientOperation.WithActionAsync(AsyncActionTransientOperation.TriggerTransientFaultExceptionAsync, id, _retryTracker, TransientOperationOptionsCallback));
                Assert.IsType<TransientFaultException>(aex.InnerExceptions.First());
                Assert.Equal(NormalRunIncrement + ExpectedRetryAttempts + DescriptiveExceptionCauseIncrement, aex.InnerExceptions.Count);
            });

            var tfe = _transientFaultTracker.Single(pair => pair.Key == id).Value;
            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
            Assert.Equal((int)TimeSpan.FromSeconds(ExpectedRetryAttempts).TotalSeconds, (int)tfe.TotalRecoveryWaitTime.TotalSeconds);
            Assert.Equal((int)ExpectedRecoveryWaitTime.TotalSeconds, (int)tfe.RecoveryWaitTime.TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, tfe.Attempts);

            TestOutput.WriteLine(tfe.ToString());
        }

        [Fact]
        public async Task WithActionAsync_ShouldTriggerLatencyException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = await TimeMeasure.WithActionAsync(async ct =>
            {
                var aex = await Assert.ThrowsAsync<AggregateException>(() => TransientOperation.WithActionAsync(AsyncActionTransientOperation.TriggerLatencyExceptionAsync, id, _retryTracker, TransientOperationOptionsCallback));

                TestOutput.WriteLine(aex.ToString());

                Assert.IsType<LatencyException>(aex.InnerExceptions.First());

                var low = NormalRunIncrement + DescriptiveExceptionCauseIncrement;
                Assert.InRange(aex.InnerExceptions.Count, low, low + 1); // expect 2 - allow 3 in rare cases
                TestOutput.WriteLine(aex.ToString());
            });

            Assert.True(ExpectedMaximumAllowedLatency < profiler.Elapsed, "ExpectedMaximumAllowedLatency < profiler.Elapsed");
        }

        [Fact]
        public async Task WithActionAsync_ShouldTriggerInvalidOperationException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = await TimeMeasure.WithActionAsync(async ct =>
            {
                var aex = await Assert.ThrowsAsync<AggregateException>(() => TransientOperation.WithActionAsync(AsyncActionTransientOperation.FailWithNonTransientFaultExceptionAsync, id, ExpectedRetryAttempts, _retryTracker, TransientOperationOptionsCallback));
                Assert.IsType<InvalidOperationException>(aex.InnerExceptions.First());

                TestOutput.WriteLine(aex.ToString());
            });

            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
        }

        [Fact]
        public async Task WithFuncAsync_ShouldTriggerRetryAndSucceedAsync()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = await TimeMeasure.WithFuncAsync(ct => TransientOperation.WithFuncAsync(AsyncFuncTransientOperation.FailUntilExpectedRetryAttemptsIsReachedAsync, id, ExpectedRetryAttempts, _retryTracker, TransientOperationOptionsCallback));

            Assert.Equal(ExpectedResult, profiler.Result);
            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
        }

        [Fact]
        public async Task WithFuncAsync_ShouldTriggerTransientFaultException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = await TimeMeasure.WithActionAsync(async ct =>
            {
                var aex = await Assert.ThrowsAsync<AggregateException>(() => TransientOperation.WithFuncAsync(AsyncFuncTransientOperation.TriggerTransientFaultExceptionAsync, id, _retryTracker, TransientOperationOptionsCallback));
                Assert.IsType<TransientFaultException>(aex.InnerExceptions.First());
                Assert.Equal(NormalRunIncrement + ExpectedRetryAttempts + DescriptiveExceptionCauseIncrement, aex.InnerExceptions.Count);
            });

            var tfe = _transientFaultTracker.Single(pair => pair.Key == id).Value;
            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
            Assert.Equal((int)TimeSpan.FromSeconds(ExpectedRetryAttempts).TotalSeconds, (int)tfe.TotalRecoveryWaitTime.TotalSeconds);
            Assert.Equal((int)ExpectedRecoveryWaitTime.TotalSeconds, (int)tfe.RecoveryWaitTime.TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, tfe.Attempts);

            TestOutput.WriteLine(tfe.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_ShouldTriggerLatencyException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = await TimeMeasure.WithActionAsync(async ct =>
            {
                var aex = await Assert.ThrowsAsync<AggregateException>(() => TransientOperation.WithFuncAsync(AsyncFuncTransientOperation.TriggerLatencyExceptionAsync, id, _retryTracker, TransientOperationOptionsCallback));
                Assert.IsType<LatencyException>(aex.InnerExceptions.First());
                Assert.Equal(NormalRunIncrement + DescriptiveExceptionCauseIncrement, aex.InnerExceptions.Count);
                TestOutput.WriteLine(aex.ToString());
            });

            Assert.True(ExpectedMaximumAllowedLatency < profiler.Elapsed, "ExpectedMaximumAllowedLatency < profiler.Elapsed");
        }

        [Fact]
        public async Task WithFuncAsync_ShouldTriggerInvalidOperationException()
        {
            var id = Guid.NewGuid();
            _retryTracker.TryAdd(id, -1);

            var profiler = await TimeMeasure.WithActionAsync(async ct =>
            {
                var aex = await Assert.ThrowsAsync<AggregateException>(() => TransientOperation.WithFuncAsync(AsyncFuncTransientOperation.FailWithNonTransientFaultExceptionAsync, id, ExpectedRetryAttempts, _retryTracker, TransientOperationOptionsCallback));
                Assert.IsType<InvalidOperationException>(aex.InnerExceptions.First());

                TestOutput.WriteLine(aex.ToString());
            });

            TestOutput.WriteLine($"Profiler: {profiler.Elapsed.TotalSeconds} seconds.");

            Assert.InRange(profiler.Elapsed.TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) - Jitter).TotalSeconds, (TimeSpan.FromSeconds(ExpectedRetryAttempts) + Jitter).TotalSeconds);
            Assert.Equal(ExpectedRetryAttempts, _retryTracker[id]);
        }
    }
}
