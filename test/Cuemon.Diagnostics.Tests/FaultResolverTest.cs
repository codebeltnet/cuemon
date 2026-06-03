using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Diagnostics
{
    public class FaultResolverTest : Test
    {
        public FaultResolverTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TryResolveFault_ShouldReturnTrue_WhenValidatorMatches()
        {
            var resolver = new FaultResolver(
                ex => ex is InvalidOperationException,
                ex => new ExceptionDescriptor(ex, "ERR001", "Oops"));

            var exception = new InvalidOperationException("test error");
            var result = resolver.TryResolveFault(exception, out var descriptor);

            Assert.True(result);
            Assert.NotNull(descriptor);
            Assert.Same(exception, descriptor.Failure);
            Assert.Equal("ERR001", descriptor.Code);
            Assert.Equal("Oops", descriptor.Message);
        }

        [Fact]
        public void TryResolveFault_ShouldReturnFalse_WhenValidatorDoesNotMatch()
        {
            var resolver = new FaultResolver(
                ex => ex is ArgumentNullException,
                ex => new ExceptionDescriptor(ex, "ERR002", "Null"));

            var exception = new InvalidOperationException("test error");
            var result = resolver.TryResolveFault(exception, out var descriptor);

            Assert.False(result);
            Assert.Null(descriptor);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenValidatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FaultResolver(null, ex => new ExceptionDescriptor(ex, "ERR", "Err")));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenDescriptorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FaultResolver(ex => true, null));
        }
    }
}
