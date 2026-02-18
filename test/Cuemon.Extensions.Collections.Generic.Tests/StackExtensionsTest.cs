#if !NETCOREAPP2_0_OR_GREATER
using System.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Collections.Generic
{
    public class StackExtensionsTest : Test
    {
        public StackExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TryPop_ShouldReturnTrueAndTopElement_WhenStackIsNonEmpty()
        {
            var sut = new Stack<int>();
            sut.Push(1);
            sut.Push(2);
            sut.Push(3); // top = 3 (LIFO)

            var result = sut.TryPop(out var value);

            TestOutput.WriteLine($"TryPop result: {result}, value: {value}");

            Assert.True(result);
            Assert.Equal(3, value);
        }

        [Fact]
        public void TryPop_ShouldReturnFalseAndDefaultValue_WhenStackIsEmpty()
        {
            var sut = new Stack<int>();

            var result = sut.TryPop(out var value);

            TestOutput.WriteLine($"TryPop result: {result}, value: {value}");

            Assert.False(result);
            Assert.Equal(default, value);
        }

        [Fact]
        public void TryPop_ShouldReturnFalseAndNull_WhenStackOfReferenceTypeIsEmpty()
        {
            var sut = new Stack<string>();

            var result = sut.TryPop(out var value);

            TestOutput.WriteLine($"TryPop result: {result}, value: {value ?? "null"}");

            Assert.False(result);
            Assert.Null(value);
        }

        [Fact]
        public void TryPop_ShouldDecrementCount_AfterSuccessfulPop()
        {
            var sut = new Stack<int>(new[] { 10, 20, 30 });

            Assert.Equal(3, sut.Count);

            sut.TryPop(out _);

            TestOutput.WriteLine($"Count after pop: {sut.Count}");

            Assert.Equal(2, sut.Count);
        }

        [Fact]
        public void TryPop_ShouldPreserveLifoOrdering_WhenCalledSequentially()
        {
            var sut = new Stack<int>();
            sut.Push(1);
            sut.Push(2);
            sut.Push(3);

            sut.TryPop(out var first);
            sut.TryPop(out var second);
            sut.TryPop(out var third);

            TestOutput.WriteLine($"LIFO order: {first}, {second}, {third}");

            Assert.Equal(3, first);
            Assert.Equal(2, second);
            Assert.Equal(1, third);
            Assert.Equal(0, sut.Count);
        }

        [Fact]
        public void TryPop_ShouldReturnFalse_AfterAllElementsAreExhausted()
        {
            var sut = new Stack<string>(new[] { "a", "b" });

            var first = sut.TryPop(out var v1);
            var second = sut.TryPop(out var v2);
            var third = sut.TryPop(out var v3); // empty at this point

            TestOutput.WriteLine($"Popped: {v1}, {v2}; exhausted: {!third}");

            Assert.True(first);
            Assert.True(second);
            Assert.False(third);
            Assert.Null(v3);
            Assert.Equal(0, sut.Count);
        }
    }
}
#endif
