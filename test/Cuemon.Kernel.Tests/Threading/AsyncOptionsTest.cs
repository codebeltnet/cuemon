using System;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Threading
{
    public class AsyncOptionsTest : Test
    {
        public AsyncOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldInitializeCancellationTokenToDefault()
        {
            var sut = new AsyncOptions();

            Assert.Equal(sut.CancellationToken, CancellationToken.None);
        }

        [Fact]
        public async Task AsyncOptions_ShouldThrow_OperationCanceledException()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(250);
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await SomeMethod(o => o.CancellationToken = cts.Token));
        }

        private async Task SomeMethod(Action<AsyncOptions> setup)
        {
            var options = Patterns.Configure(setup);
            while (!options.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(50);
            }
            options.CancellationToken.ThrowIfCancellationRequested();
            TestOutput.WriteLine(options.CancellationToken.IsCancellationRequested.ToString());
        }
    }
}
