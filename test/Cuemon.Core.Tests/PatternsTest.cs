using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Codebelt.Extensions.Xunit;
using Cuemon.Text;
using Cuemon.Threading;
using Xunit;

namespace Cuemon
{
    public class PatternsTest : Test
    {
        public PatternsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Configure_ShouldInitializeDefaultInstance()
        {
            Action<AsyncOptions> sut = null;
            var ao = new AsyncOptions();

            var options = Patterns.Configure(sut);

            Assert.NotNull(options);
            Assert.IsType<AsyncOptions>(options);
            Assert.Equal(ao.CancellationToken, options.CancellationToken);
        }

        [Fact]
        public void ConfigureExchange_ShouldSwapOptions_VerifyDefaultValues()
        {
            Action<AsyncEncodingOptions> sut1 = null;
            var sut2 = Patterns.ConfigureExchange<AsyncEncodingOptions, EncodingOptions>(sut1);

            var o1 = Patterns.Configure(sut1);
            var o2 = Patterns.Configure(sut2);

            Assert.NotNull(o1);
            Assert.NotNull(o2);
            Assert.IsType<AsyncEncodingOptions>(o1);
            Assert.IsType<EncodingOptions>(o2);
            Assert.Equal(o1.Encoding, o2.Encoding);
            Assert.Equal(o1.Preamble, o2.Preamble);
        }

        [Fact]
        public void ConfigureRevert_ShouldReturnDelegateThatProducesEquivalentOptions()
        {
            var original = Patterns.Configure<AsyncEncodingOptions>(o => o.Encoding = Encoding.UTF32);
            var revertDelegate = Patterns.ConfigureRevert(original);
            var reverted = Patterns.Configure(revertDelegate);

            Assert.Equal(original.Encoding, reverted.Encoding);
            Assert.Equal(original.Preamble, reverted.Preamble);
        }

        [Fact]
        public void ConfigureRevertExchange_ShouldExchangeAndRevertOptions()
        {
            var original = Patterns.Configure<AsyncEncodingOptions>(o => o.Encoding = Encoding.UTF32);
            var exchangeDelegate = Patterns.ConfigureRevertExchange<AsyncEncodingOptions, EncodingOptions>(original);
            var result = Patterns.Configure(exchangeDelegate);

            Assert.Equal(Encoding.UTF32, result.Encoding);
            Assert.Equal(original.Preamble, result.Preamble);
        }

        [Fact]
        public void CreateInstance_ShouldInitializeWithFactory()
        {
            var sut = Patterns.CreateInstance<AsyncEncodingOptions>(o => o.Encoding = Encoding.UTF32);

            Assert.NotNull(sut);
            Assert.Equal(Encoding.UTF32, sut.Encoding);
        }

        [Fact]
        public void CreateInstance_ShouldCreateDefaultInstance_WhenFactoryIsNull()
        {
            var defaults = new AsyncEncodingOptions();
            var sut = Patterns.CreateInstance<AsyncEncodingOptions>(null);

            Assert.NotNull(sut);
            Assert.Equal(defaults.Encoding, sut.Encoding);
            Assert.Equal(defaults.Preamble, sut.Preamble);
        }

        [Fact]
        public void TryInvoke_ShouldReturnTrue_WhenActionSucceeds()
        {
            var invoked = false;

            var result = Patterns.TryInvoke(() => { invoked = true; });

            Assert.True(result);
            Assert.True(invoked);
        }

        [Fact]
        public void TryInvoke_ShouldReturnFalse_WhenActionThrows()
        {
            var result = Patterns.TryInvoke(() => throw new InvalidOperationException());

            Assert.False(result);
        }

        [Fact]
        public void TryInvoke_ShouldReturnTrueAndResult_WhenFuncSucceeds()
        {
            var result = Patterns.TryInvoke(() => 42, out var value);

            Assert.True(result);
            Assert.Equal(42, value);
        }

        [Fact]
        public void TryInvoke_ShouldReturnFalseAndDefault_WhenFuncThrows()
        {
            var result = Patterns.TryInvoke<int>(() => throw new InvalidOperationException(), out var value);

            Assert.False(result);
            Assert.Equal(default, value);
        }

        [Fact]
        public void InvokeOrDefault_ShouldReturnResult_WhenMethodSucceeds()
        {
            var result = Patterns.InvokeOrDefault(() => 42);

            Assert.Equal(42, result);
        }

        [Fact]
        public void InvokeOrDefault_ShouldReturnFallback_WhenMethodThrows()
        {
            var result = Patterns.InvokeOrDefault<int>(() => throw new InvalidOperationException(), -1);

            Assert.Equal(-1, result);
        }

        [Fact]
        public void IsFatalException_ShouldReturnTrue_WhenExceptionIsFatal()
        {
            Assert.True(Patterns.IsFatalException(new OutOfMemoryException()));
            Assert.True(Patterns.IsFatalException(new StackOverflowException()));
            Assert.True(Patterns.IsFatalException(new AccessViolationException()));
            Assert.True(Patterns.IsFatalException(new SEHException()));
        }

        [Fact]
        public void IsFatalException_ShouldReturnFalse_WhenExceptionIsNotFatal()
        {
            Assert.False(Patterns.IsFatalException(new InvalidOperationException()));
            Assert.False(Patterns.IsFatalException(new ArgumentNullException()));
            Assert.False(Patterns.IsFatalException(new NotSupportedException()));
        }

        [Fact]
        public void IsRecoverableException_ShouldReturnTrue_WhenExceptionIsNotFatal()
        {
            Assert.True(Patterns.IsRecoverableException(new InvalidOperationException()));
            Assert.True(Patterns.IsRecoverableException(new ArgumentNullException()));
            Assert.True(Patterns.IsRecoverableException(new NotSupportedException()));
        }

        [Fact]
        public void IsRecoverableException_ShouldReturnFalse_WhenExceptionIsFatal()
        {
            Assert.False(Patterns.IsRecoverableException(new OutOfMemoryException()));
            Assert.False(Patterns.IsRecoverableException(new StackOverflowException()));
            Assert.False(Patterns.IsRecoverableException(new AccessViolationException()));
            Assert.False(Patterns.IsRecoverableException(new SEHException()));
        }

        [Fact]
        public void SafeInvoke_ShouldReturnResult_WhenTesterSucceeds()
        {
            using var result = Patterns.SafeInvoke(
                () => new MemoryStream(new byte[] { 1, 2, 3 }),
                ms => ms);

            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void SafeInvoke_ShouldReturnNull_AndInvokeCatcher_WhenTesterThrows()
        {
            Exception caught = null;

            var result = Patterns.SafeInvoke<MemoryStream>(
                () => new MemoryStream(),
                _ => throw new InvalidOperationException("tester failure"),
                ex => caught = ex);

            Assert.Null(result);
            Assert.NotNull(caught);
            Assert.IsType<InvalidOperationException>(caught);
            Assert.Equal("tester failure", caught.Message);
        }
    }
}
