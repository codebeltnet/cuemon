using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon.Threading
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class AwaiterBenchmark
    {
        // Fast-path comparison: direct await vs Awaiter wrapper
        [Benchmark(Baseline = true, Description = "Direct await - immediate success")]
        public Task<ConditionalValue> DirectAwait_ImmediateSuccess() => Task.FromResult<ConditionalValue>(new SuccessfulValue());

        [Benchmark(Description = "Awaiter - immediate success")]
        public Task<ConditionalValue> Awaiter_ImmediateSuccess() => Awaiter.RunUntilSuccessfulOrTimeoutAsync(() => Task.FromResult<ConditionalValue>(new SuccessfulValue()), o =>
        {
            o.Timeout = TimeSpan.Zero; // force single iteration
            o.Delay = TimeSpan.Zero;
        });

        // Retry scenarios: fail N times then succeed
        [Benchmark(Description = "Awaiter - fail 1 then success")]
        public Task<ConditionalValue> Awaiter_Fail1_ThenSuccess()
        {
            int call = 0;
            Task<ConditionalValue> Method()
            {
                call++;
                if (call <= 1) return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
                return Task.FromResult<ConditionalValue>(new SuccessfulValue());
            }

            return Awaiter.RunUntilSuccessfulOrTimeoutAsync(Method, o =>
            {
                o.Timeout = TimeSpan.FromSeconds(1);
                o.Delay = TimeSpan.Zero;
            });
        }

        [Benchmark(Description = "Awaiter - fail 10 then success")]
        public Task<ConditionalValue> Awaiter_Fail10_ThenSuccess()
        {
            int call = 0;
            Task<ConditionalValue> Method()
            {
                call++;
                if (call <= 10) return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
                return Task.FromResult<ConditionalValue>(new SuccessfulValue());
            }

            return Awaiter.RunUntilSuccessfulOrTimeoutAsync(Method, o =>
            {
                o.Timeout = TimeSpan.FromSeconds(5);
                o.Delay = TimeSpan.Zero;
            });
        }

        // Exception collection scenarios
        [Benchmark(Description = "Awaiter - 1 thrown exception then unsuccessful")]
        public Task<ConditionalValue> Awaiter_Throw1_ThenUnsuccessful()
        {
            var exceptions = new Exception[] { new InvalidOperationException("fail1") };
            int call = 0;
            Task<ConditionalValue> Method()
            {
                if (call < exceptions.Length)
                {
                    throw exceptions[call++];
                }
                // After throwing, return unsuccessful immediately
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            return Awaiter.RunUntilSuccessfulOrTimeoutAsync(Method, o =>
            {
                o.Timeout = TimeSpan.FromSeconds(1);
                o.Delay = TimeSpan.Zero;
            });
        }

        [Benchmark(Description = "Awaiter - 2 thrown exceptions then unsuccessful")]
        public Task<ConditionalValue> Awaiter_Throw2_ThenUnsuccessful()
        {
            var exceptions = new Exception[] { new InvalidOperationException("fail1"), new ArgumentException("fail2") };
            int call = 0;
            Task<ConditionalValue> Method()
            {
                if (call < exceptions.Length)
                {
                    throw exceptions[call++];
                }
                // After throwing, return unsuccessful immediately
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            return Awaiter.RunUntilSuccessfulOrTimeoutAsync(Method, o =>
            {
                o.Timeout = TimeSpan.FromSeconds(1);
                o.Delay = TimeSpan.Zero;
            });
        }
    }
}
