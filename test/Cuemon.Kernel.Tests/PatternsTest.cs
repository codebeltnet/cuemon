using Codebelt.Extensions.Xunit;
using Cuemon.Assets;
using Cuemon.Configuration;
using Cuemon.Text;
using Cuemon.Threading;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
        public void Configure_ShouldInvokeInitializerSetupAndValidatorInOrder()
        {
            var calls = new System.Collections.Generic.List<string>();

            var options = Patterns.Configure<AsyncOptions>(
                setup =>
                {
                    calls.Add("setup");
                    setup.CancellationTokenProvider = () => new System.Threading.CancellationToken(true);
                },
                initializer =>
                {
                    calls.Add("initializer");
                    initializer.CancellationToken = System.Threading.CancellationToken.None;
                },
                validator =>
                {
                    calls.Add("validator");
                    Assert.True(validator.CancellationToken.IsCancellationRequested);
                });

            Assert.Equal(new[] { "initializer", "setup", "validator" }, calls);
            Assert.True(options.CancellationToken.IsCancellationRequested);
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
        public void ConfigureExchange_ShouldUseCustomInitializer_AndThrowWhenNoMatchingProperties()
        {
            var exchange = Patterns.ConfigureExchange<AsyncEncodingOptions, PatternsExchangeProbe>(
                setup => setup.Encoding = Encoding.UTF32,
                (source, target) =>
                {
                    target.Name = source.Encoding.WebName;
                    target.Flag = source.Preamble == PreambleSequence.Remove;
                });

            var custom = new PatternsExchangeProbe();
            exchange(custom);

            Assert.Equal(Encoding.UTF32.WebName, custom.Name);
            Assert.True(custom.Flag);

            var failingExchange = Patterns.ConfigureExchange<AsyncOptions, PatternsNoMatchProbe>(setup => { });

            var ex = Assert.Throws<InvalidOperationException>(() => failingExchange(new PatternsNoMatchProbe()));
            Assert.StartsWith("Unable to use default converter for exchange of TSource", ex.Message);
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
        public void ConfigureRevert_ShouldThrowArgumentNullException_WhenOptionsIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Patterns.ConfigureRevert<AsyncEncodingOptions>(null));

            Assert.Equal("options", ex.ParamName);
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
        public void ConfigureRevertExchange_ShouldUseCustomInitializer_AndThrowArgumentNullException_WhenOptionsIsNull()
        {
            var original = Patterns.Configure<AsyncEncodingOptions>(o => o.Encoding = Encoding.Unicode);
            var exchangeDelegate = Patterns.ConfigureRevertExchange<AsyncEncodingOptions, PatternsExchangeProbe>(
                original,
                (source, target) => target.Name = source.Encoding.WebName);

            var result = new PatternsExchangeProbe();
            exchangeDelegate(result);

            Assert.Equal(Encoding.Unicode.WebName, result.Name);

            var ex = Assert.Throws<ArgumentNullException>(() => Patterns.ConfigureRevertExchange<AsyncEncodingOptions, EncodingOptions>(null));
            Assert.Equal("options", ex.ParamName);
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
        public void TryInvoke_ShouldReturnFalse_WhenActionIsNull_AndRethrowFatalExceptions()
        {
            Assert.False(Patterns.TryInvoke(null));
            Assert.Throws<OutOfMemoryException>(() => Patterns.TryInvoke(() => throw new OutOfMemoryException()));
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
        public void TryInvoke_ShouldHandleNullFunc_AndRethrowFatalExceptions()
        {
            var nullResult = Patterns.TryInvoke<int>(null, out var nullValue);

            Assert.False(nullResult);
            Assert.Equal(default, nullValue);

            Assert.Throws<OutOfMemoryException>(() => Patterns.TryInvoke<int>(() => throw new OutOfMemoryException(), out _));
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
        public void InvokeOrDefault_ShouldReturnFallback_WhenMethodIsNull()
        {
            Assert.Equal(-1, Patterns.InvokeOrDefault<int>(null, -1));
        }

        [Fact]
        public void IsFatalException_ShouldReturnTrue_WhenExceptionIsFatal()
        {
            Assert.True(Patterns.IsFatalException(new OutOfMemoryException()));
            Assert.True(Patterns.IsFatalException(new StackOverflowException()));
            Assert.True(Patterns.IsFatalException(new AccessViolationException()));
            Assert.True(Patterns.IsFatalException(new SEHException()));
            Assert.True(Patterns.IsFatalException(new ThreadInterruptedException()));
#pragma warning disable CS0618
            Assert.True(Patterns.IsFatalException(new ExecutionEngineException()));
#pragma warning restore CS0618
        }

        [Fact]
        public void IsFatalException_ShouldReturnFalse_WhenExceptionIsNotFatal()
        {
            Assert.False(Patterns.IsFatalException(new InvalidOperationException()));
            Assert.False(Patterns.IsFatalException(new ArgumentNullException()));
            Assert.False(Patterns.IsFatalException(new NotSupportedException()));
            Assert.False(Patterns.IsFatalException(null));
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
        public void Use_ShouldReturnSingletonInstance()
        {
            Assert.Same(Patterns.Use, Patterns.Use);
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

        [Fact]
        public void SafeInvoke_ShouldValidateDelegates_AndRethrowWithoutCatcher()
        {
            Assert.Throws<ArgumentNullException>(() => Patterns.SafeInvoke<MemoryStream>(null, stream => stream));
            Assert.Throws<ArgumentNullException>(() => Patterns.SafeInvoke(() => new MemoryStream(), (Func<MemoryStream, MemoryStream>)null));
            Assert.Throws<InvalidOperationException>(() => Patterns.SafeInvoke(() => new MemoryStream(), _ => throw new InvalidOperationException("boom")));
        }

        [Fact]
        public void SafeInvoke_ShouldCoverGenericOverloads_WithSuccessAndCatcherPaths()
        {
            using var one = Patterns.SafeInvoke(() => new MemoryStream(), (stream, factor) =>
            {
                stream.WriteByte((byte)factor);
                stream.Position = 0;
                return stream;
            }, 2);
            Assert.Equal(1, one.Length);

            using var two = Patterns.SafeInvoke(() => new MemoryStream(), (stream, a, b) =>
            {
                stream.WriteByte((byte)(a + b));
                stream.Position = 0;
                return stream;
            }, 2, 3);
            Assert.Equal(1, two.Length);

            using var three = Patterns.SafeInvoke(() => new MemoryStream(), (stream, a, b, c) =>
            {
                stream.WriteByte((byte)(a + b + c));
                stream.Position = 0;
                return stream;
            }, 1, 2, 3);
            Assert.Equal(1, three.Length);

            using var four = Patterns.SafeInvoke(() => new MemoryStream(), (stream, a, b, c, d) =>
            {
                stream.WriteByte((byte)(a + b + c + d));
                stream.Position = 0;
                return stream;
            }, 1, 2, 3, 4);
            Assert.Equal(1, four.Length);

            using var five = Patterns.SafeInvoke(() => new MemoryStream(), (stream, a, b, c, d, e) =>
            {
                stream.WriteByte((byte)(a + b + c + d + e));
                stream.Position = 0;
                return stream;
            }, 1, 2, 3, 4, 5);
            Assert.Equal(1, five.Length);

            Exception oneCaught = null;
            Exception twoCaught = null;
            Exception threeCaught = null;
            Exception fourCaught = null;
            Exception fiveCaught = null;

            Assert.Null(Patterns.SafeInvoke(() => new MemoryStream(), (MemoryStream stream, int arg) => throw new InvalidOperationException($"one:{arg}"), 7, (ex, arg) => oneCaught = new InvalidOperationException($"{ex.Message}:{arg}")));
            Assert.Null(Patterns.SafeInvoke(() => new MemoryStream(), (MemoryStream stream, int a, int b) => throw new InvalidOperationException($"two:{a + b}"), 2, 3, (ex, a, b) => twoCaught = new InvalidOperationException($"{ex.Message}:{a}:{b}")));
            Assert.Null(Patterns.SafeInvoke(() => new MemoryStream(), (MemoryStream stream, int a, int b, int c) => throw new InvalidOperationException($"three:{a + b + c}"), 1, 2, 3, (ex, a, b, c) => threeCaught = new InvalidOperationException($"{ex.Message}:{a}:{b}:{c}")));
            Assert.Null(Patterns.SafeInvoke(() => new MemoryStream(), (MemoryStream stream, int a, int b, int c, int d) => throw new InvalidOperationException($"four:{a + b + c + d}"), 1, 2, 3, 4, (ex, a, b, c, d) => fourCaught = new InvalidOperationException($"{ex.Message}:{a}:{b}:{c}:{d}")));
            Assert.Null(Patterns.SafeInvoke(() => new MemoryStream(), (MemoryStream stream, int a, int b, int c, int d, int e) => throw new InvalidOperationException($"five:{a + b + c + d + e}"), 1, 2, 3, 4, 5, (ex, a, b, c, d, e) => fiveCaught = new InvalidOperationException($"{ex.Message}:{a}:{b}:{c}:{d}:{e}")));

            Assert.Equal("one:7:7", oneCaught.Message);
            Assert.Equal("two:5:2:3", twoCaught.Message);
            Assert.Equal("three:6:1:2:3", threeCaught.Message);
            Assert.Equal("four:10:1:2:3:4", fourCaught.Message);
            Assert.Equal("five:15:1:2:3:4:5", fiveCaught.Message);
        }

        private sealed class PatternsExchangeProbe : IParameterObject
        {
            public string Name { get; set; }

            public bool Flag { get; set; }
        }

        private sealed class PatternsNoMatchProbe : IParameterObject
        {
            public DateTime Timestamp { get; set; }
        }
    }
}
