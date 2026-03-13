using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Threading
{
    public class AwaiterTest : Test
    {
        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnOnImmediateSuccess()
        {
            // Arrange
            var callCount = 0;
            Task<ConditionalValue> Method()
            {
                callCount++;
                return Task.FromResult<ConditionalValue>(new SuccessfulValue());
            }

            // Act
            var result = await Awaiter.RunUntilSuccessfulOrTimeoutAsync(Method);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldRetryUntilSuccess()
        {
            // Arrange
            var callCount = 0;
            Task<ConditionalValue> Method()
            {
                callCount++;
                if (callCount < 3)
                    return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
                return Task.FromResult<ConditionalValue>(new SuccessfulValue());
            }

            // Act
            var result = await Awaiter.RunUntilSuccessfulOrTimeoutAsync(Method, o =>
            {
                o.Timeout = TimeSpan.FromSeconds(2);
                o.Delay = TimeSpan.FromMilliseconds(10);
            });

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(3, callCount);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnUnsuccessfulOnTimeout_NoExceptions()
        {
            // Arrange
            Task<ConditionalValue> Method() => Task.FromResult<ConditionalValue>(new UnsuccessfulValue());

            // Act
            var result = await Awaiter.RunUntilSuccessfulOrTimeoutAsync(Method, o =>
            {
                o.Timeout = TimeSpan.FromMilliseconds(50);
                o.Delay = TimeSpan.FromMilliseconds(10);
            });

            // Assert
            Assert.False(result.Succeeded);
            Assert.Null(result.Failure);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnUnsuccessfulWithSingleException()
        {
            // Arrange
            var cts = new CancellationTokenSource();

            cts.Cancel();

            var ct = cts.Token;

            // Act
            var result = await Awaiter.RunUntilSuccessfulOrTimeoutAsync(() => Task.FromResult<ConditionalValue>(new UnsuccessfulValue()), o =>
            {
                o.Timeout = TimeSpan.FromMilliseconds(10);
                o.Delay = TimeSpan.FromMilliseconds(100);
                o.CancellationToken = ct;
            });

            // Assert
            Assert.False(result.Succeeded);
            Assert.IsType<OperationCanceledException>(result.Failure);
        }

        [Fact]
        public async Task RunUntilSuccessfulOrTimeoutAsync_ShouldReturnUnsuccessfulWithAggregateException()
        {
            // Arrange
            var exceptions = new List<Exception>
            {
                new InvalidOperationException("fail1"),
                new ArgumentException("fail2")
            };
            var callCount = 0;
            Task<ConditionalValue> Method()
            {
                if (callCount < exceptions.Count)
                {
                    throw exceptions[callCount++];
                }
                // After throwing both exceptions, always return unsuccessful
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }

            // Act
            var result = await Awaiter.RunUntilSuccessfulOrTimeoutAsync(Method, o =>
            {
                o.Timeout = TimeSpan.FromSeconds(5); // Significantly longer to ensure both exceptions are thrown (CI is slow in GHA)
                o.Delay = TimeSpan.FromMilliseconds(10);
            });

            // Assert
            Assert.False(result.Succeeded);
            Assert.IsType<AggregateException>(result.Failure);
            var agg = (AggregateException)result.Failure;
            Assert.Contains(exceptions[0], agg.InnerExceptions);
            Assert.Contains(exceptions[1], agg.InnerExceptions);
        }
    }
}
