using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Diagnostics
{
    public class TimeMeasureTest : Test
    {
        private static readonly TimeSpan ExpectedExecutionTime = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan Jitter = TimeSpan.FromMilliseconds(100);

        public TimeMeasureTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void WithAction_Use_0_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithAction(() => Thread.Sleep(expected));

            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.False(profiler.Member.HasParameters());
            Assert.Empty(profiler.Data);

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithAction_Use_1_Argument_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithAction(a1 => Thread.Sleep(expected), 1);

            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Contains(profiler.Data.Values, o => o is int i && i == 1);

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithAction_Use_2_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithAction((a1, a2) => Thread.Sleep(expected), 1, 2);

            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values, i => Assert.Equal(1, i), i => Assert.Equal(2, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithAction_Use_3_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithAction((a1, a2, a3) => Thread.Sleep(expected), 1, 2, 3);

            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                o1 => Assert.Equal(1, o1),
                o2 => Assert.Equal(2, o2),
                o3 => Assert.Equal(3, o3));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithAction_Use_4_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithAction((a1, a2, a3, a4) => Thread.Sleep(expected), 1, 2, 3, 4);

            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithAction_Use_5_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithAction((a1, a2, a3, a4, a5) => Thread.Sleep(expected), 1, 2, 3, 4, 5);

            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithAction_Use_6_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithAction((a1, a2, a3, a4, a5, a6) => Thread.Sleep(expected), 1, 2, 3, 4, 5, 6);

            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithAction_Use_7_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithAction((a1, a2, a3, a4, a5, a6, a7) => Thread.Sleep(expected), 1, 2, 3, 4, 5, 6, 7);

            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithAction_Use_8_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithAction((a1, a2, a3, a4, a5, a6, a7, a8) => Thread.Sleep(expected), 1, 2, 3, 4, 5, 6, 7, 8);

            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithAction_Use_9_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithAction((a1, a2, a3, a4, a5, a6, a7, a8, a9) => Thread.Sleep(expected), 1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i),
                i => Assert.Equal(9, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithAction_Use_10_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithAction((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => Thread.Sleep(expected), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i),
                i => Assert.Equal(9, i),
                i => Assert.Equal(10, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithFunc_Use_0_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;

            var profiler = TimeMeasure.WithFunc(() =>
            {
                Thread.Sleep(expected);
                return 42;
            });

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.False(profiler.Member.HasParameters());
            Assert.Empty(profiler.Data);

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithFunc_Use_1_Argument_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;

            var profiler = TimeMeasure.WithFunc((a) =>
            {
                Thread.Sleep(expected);
                return 42;
            }, 1);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithFunc_Use_2_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;

            var profiler = TimeMeasure.WithFunc((a1, a2) =>
            {
                Thread.Sleep(expected);
                return 42;
            }, 1, 2);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithFunc_Use_3_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;

            var profiler = TimeMeasure.WithFunc((a1, a2, a3) =>
            {
                Thread.Sleep(expected);
                return 42;
            }, 1, 2, 3);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithFunc_Use_4_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;

            var profiler = TimeMeasure.WithFunc((a1, a2, a3, a4) =>
            {
                Thread.Sleep(expected);
                return 42;
            }, 1, 2, 3, 4);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithFunc_Use_5_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;

            var profiler = TimeMeasure.WithFunc((a1, a2, a3, a4, a5) =>
            {
                Thread.Sleep(expected);
                return 42;
            }, 1, 2, 3, 4, 5);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithFunc_Use_6_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;

            var profiler = TimeMeasure.WithFunc((a1, a2, a3, a4, a5, a6) =>
            {
                Thread.Sleep(expected);
                return 42;
            }, 1, 2, 3, 4, 5, 6);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithFunc_Use_7_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;

            var profiler = TimeMeasure.WithFunc((a1, a2, a3, a4, a5, a6, a7) =>
            {
                Thread.Sleep(expected);
                return 42;
            }, 1, 2, 3, 4, 5, 6, 7);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithFunc_Use_8_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;

            var profiler = TimeMeasure.WithFunc((a1, a2, a3, a4, a5, a6, a7, a8) =>
            {
                Thread.Sleep(expected);
                return 42;
            }, 1, 2, 3, 4, 5, 6, 7, 8);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithFunc_Use_9_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;

            var profiler = TimeMeasure.WithFunc((a1, a2, a3, a4, a5, a6, a7, a8, a9) =>
            {
                Thread.Sleep(expected);
                return 42;
            }, 1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i),
                i => Assert.Equal(9, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithFunc_Use_10_Arguments_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var profiler = TimeMeasure.WithFunc((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) =>
            {
                Thread.Sleep(expected);
                return 42;
            }, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i),
                i => Assert.Equal(9, i),
                i => Assert.Equal(10, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithActionAsync_Use_0_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithActionAsync(token => Task.Delay(expected, token), o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithActionAsync(token => Task.Delay(expected, token), o => o.CancellationToken = ctsShouldPass.Token);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.Empty(profiler.Data);

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithActionAsync_Use_1_Argument_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithActionAsync((a, token) => Task.Delay(expected, token), 1, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithActionAsync((a, token) => Task.Delay(expected, token), 1, o => o.CancellationToken = ctsShouldPass.Token);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithActionAsync_Use_2_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithActionAsync((a1, a2, token) => Task.Delay(expected, token), 1, 2, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithActionAsync((a1, a2, token) => Task.Delay(expected, token), 1, 2, o => o.CancellationToken = ctsShouldPass.Token);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithActionAsync_Use_3_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithActionAsync((a1, a2, a3, token) => Task.Delay(expected, token), 1, 2, 3, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithActionAsync((a1, a2, a3, token) => Task.Delay(expected, token), 1, 2, 3, o => o.CancellationToken = ctsShouldPass.Token);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithActionAsync_Use_4_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithActionAsync((a1, a2, a3, a4, token) => Task.Delay(expected, token), 1, 2, 3, 4, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithActionAsync((a1, a2, a3, a4, token) => Task.Delay(expected, token), 1, 2, 3, 4, o => o.CancellationToken = ctsShouldPass.Token);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithActionAsync_Use_5_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, o => o.CancellationToken = ctsShouldPass.Token);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithActionAsync_Use_6_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, a6, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, 6, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, a6, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, 6, o => o.CancellationToken = ctsShouldPass.Token);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithActionAsync_Use_7_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, a6, a7, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, 6, 7, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, a6, a7, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, 6, 7, o => o.CancellationToken = ctsShouldPass.Token);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithActionAsync_Use_8_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, a6, a7, a8, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, 6, 7, 8, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, a6, a7, a8, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, 6, 7, 8, o => o.CancellationToken = ctsShouldPass.Token);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithActionAsync_Use_9_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, a6, a7, a8, a9, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, 6, 7, 8, 9, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, a6, a7, a8, a9, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, 6, 7, 8, 9, o => o.CancellationToken = ctsShouldPass.Token);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i),
                i => Assert.Equal(9, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithActionAsync_Use_10_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithActionAsync((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, token) => Task.Delay(expected, token), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, o => o.CancellationToken = ctsShouldPass.Token);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i),
                i => Assert.Equal(9, i),
                i => Assert.Equal(10, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_Use_0_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithFuncAsync(async token =>
                {
                    await Task.Delay(expected, token);
                    return 42;
                }, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithFuncAsync(async token =>
            {
                await Task.Delay(expected, token);
                return 42;
            }, o => o.CancellationToken = ctsShouldPass.Token);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.Empty(profiler.Data);

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_Use_1_Argument_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithFuncAsync(async (a, token) =>
                {
                    await Task.Delay(expected, token);
                    return 42;
                }, 1, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithFuncAsync(async (a, token) =>
            {
                await Task.Delay(expected, token);
                return 42;
            }, 1, o => o.CancellationToken = ctsShouldPass.Token);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_Use_2_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithFuncAsync(async (a1, a2, token) =>
                {
                    await Task.Delay(expected, token);
                    return 42;
                }, 1, 2, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithFuncAsync(async (a1, a2, token) =>
            {
                await Task.Delay(expected, token);
                return 42;
            }, 1, 2, o => o.CancellationToken = ctsShouldPass.Token);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_Use_3_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithFuncAsync(async (a1, a2, a3, token) =>
                {
                    await Task.Delay(expected, token);
                    return 42;
                }, 1, 2, 3, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithFuncAsync(async (a1, a2, a3, token) =>
            {
                await Task.Delay(expected, token);
                return 42;
            }, 1, 2, 3, o => o.CancellationToken = ctsShouldPass.Token);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_Use_4_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, token) =>
                {
                    await Task.Delay(expected, token);
                    return 42;
                }, 1, 2, 3, 4, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, token) =>
            {
                await Task.Delay(expected, token);
                return 42;
            }, 1, 2, 3, 4, o => o.CancellationToken = ctsShouldPass.Token);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_Use_5_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, token) =>
                {
                    await Task.Delay(expected, token);
                    return 42;
                }, 1, 2, 3, 4, 5, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, token) =>
            {
                await Task.Delay(expected, token);
                return 42;
            }, 1, 2, 3, 4, 5, o => o.CancellationToken = ctsShouldPass.Token);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_Use_6_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, a6, token) =>
                {
                    await Task.Delay(expected, token);
                    return 42;
                }, 1, 2, 3, 4, 5, 6, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, a6, token) =>
            {
                await Task.Delay(expected, token);
                return 42;
            }, 1, 2, 3, 4, 5, 6, o => o.CancellationToken = ctsShouldPass.Token);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_Use_7_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, a6, a7, token) =>
                {
                    await Task.Delay(expected, token);
                    return 42;
                }, 1, 2, 3, 4, 5, 6, 7, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, a6, a7, token) =>
            {
                await Task.Delay(expected, token);
                return 42;
            }, 1, 2, 3, 4, 5, 6, 7, o => o.CancellationToken = ctsShouldPass.Token);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_Use_8_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, a6, a7, a8, token) =>
                {
                    await Task.Delay(expected, token);
                    return 42;
                }, 1, 2, 3, 4, 5, 6, 7, 8, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, a6, a7, a8, token) =>
            {
                await Task.Delay(expected, token);
                return 42;
            }, 1, 2, 3, 4, 5, 6, 7, 8, o => o.CancellationToken = ctsShouldPass.Token);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_Use_9_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, a6, a7, a8, a9, token) =>
                {
                    await Task.Delay(expected, token);
                    return 42;
                }, 1, 2, 3, 4, 5, 6, 7, 8, 9, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, a6, a7, a8, a9, token) =>
            {
                await Task.Delay(expected, token);
                return 42;
            }, 1, 2, 3, 4, 5, 6, 7, 8, 9, o => o.CancellationToken = ctsShouldPass.Token);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i),
                i => Assert.Equal(9, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public async Task WithFuncAsync_Use_10_Arguments_And_CancellationToken_ShouldTakeAroundOneSecond()
        {
            var expected = ExpectedExecutionTime;
            var ctsShouldFail = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            var ctsShouldPass = new CancellationTokenSource();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, token) =>
                {
                    await Task.Delay(expected, token);
                    return 42;
                }, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, o => o.CancellationToken = ctsShouldFail.Token);
            });

            var profiler = await TimeMeasure.WithFuncAsync(async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, token) =>
            {
                await Task.Delay(expected, token);
                return 42;
            }, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, o => o.CancellationToken = ctsShouldPass.Token);

            Assert.Equal(42, profiler.Result);
            Assert.InRange(profiler.Elapsed, expected.Subtract(Jitter), expected.Add(Jitter));
            Assert.True(profiler.Member.HasParameters());
            Assert.Contains(profiler.Member.Parameters, item => item.ParameterName == "token" && item.ParameterType == typeof(CancellationToken));
            Assert.NotEmpty(profiler.Data);
            Assert.Collection(profiler.Data.Values,
                i => Assert.Equal(1, i),
                i => Assert.Equal(2, i),
                i => Assert.Equal(3, i),
                i => Assert.Equal(4, i),
                i => Assert.Equal(5, i),
                i => Assert.Equal(6, i),
                i => Assert.Equal(7, i),
                i => Assert.Equal(8, i),
                i => Assert.Equal(9, i),
                i => Assert.Equal(10, i));

            TestOutput.WriteLine(profiler.Elapsed.ToString());
            TestOutput.WriteLine(profiler.Member.ToString());
        }

        [Fact]
        public void WithAction_ShouldInvokeCompletedCallback_WhenThresholdIsMet()
        {
            var callbacks = new ConcurrentBag<TimeMeasureProfiler>();
            var previous = TimeMeasure.CompletedCallback;
            try
            {
                TimeMeasure.CompletedCallback = profiler =>
                {
                    callbacks.Add(profiler);
                    previous?.Invoke(profiler);
                };

                var measured = TimeMeasure.WithAction(() => Thread.Sleep(TimeSpan.FromMilliseconds(50)), o => o.TimeMeasureCompletedThreshold = TimeSpan.FromMilliseconds(10));

                Assert.Contains(callbacks, profiler => ReferenceEquals(profiler, measured));
            }
            finally
            {
                TimeMeasure.CompletedCallback = previous;
            }
        }

        [Fact]
        public void ToString_ShouldIncludeParameters_WhenProfilerHasData()
        {
            var profiler = TimeMeasure.WithAction((a1, a2) => Thread.Sleep(TimeSpan.FromMilliseconds(10)), 1, "two");

            var result = profiler.ToString();

            Assert.Contains("took", result);
            Assert.Contains("Parameters: {", result);
            foreach (var parameterName in profiler.Data.Keys)
            {
                Assert.Contains(parameterName + "=", result);
            }
        }

        [Fact]
        public void WithAction_ShouldHaveStoppedProfiler_WhenCompleted()
        {
            var profiler = TimeMeasure.WithAction(() => Thread.Sleep(TimeSpan.FromMilliseconds(10)));

            Assert.False(profiler.IsRunning);
            Assert.False(profiler.Timer.IsRunning);
        }
    }
}