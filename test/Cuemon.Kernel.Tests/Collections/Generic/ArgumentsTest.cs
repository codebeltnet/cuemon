using System;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Collections.Generic
{
    public class ArgumentsTest : Test
    {
        public ArgumentsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Concat_ShouldReturnEmptyArray_WhenFirstArrayIsNull()
        {
            var result = Arguments.Concat<int>(null, new[] { 1, 2, 3 });

            Assert.Empty(result);
        }

        [Fact]
        public void Concat_ShouldReturnFirstArray_WhenSecondArrayIsNull()
        {
            var args1 = new[] { 1, 2, 3 };

            var result = Arguments.Concat(args1, null);

            Assert.Same(args1, result);
        }

        [Fact]
        public void Concat_ShouldReturnSecondArray_WhenFirstArrayIsEmpty()
        {
            var args1 = Array.Empty<int>();
            var args2 = new[] { 1, 2, 3 };

            var result = Arguments.Concat(args1, args2);

            Assert.Same(args2, result);
        }

        [Fact]
        public void Concat_ShouldReturnFirstArray_WhenSecondArrayIsEmpty()
        {
            var args1 = new[] { 1, 2, 3 };
            var args2 = Array.Empty<int>();

            var result = Arguments.Concat(args1, args2);

            Assert.Same(args1, result);
        }

        [Fact]
        public void Concat_ShouldAppendSecondArrayAfterFirstArray_WhenBothArraysContainValues()
        {
            var args1 = new[] { 1, 2, 3 };
            var args2 = new[] { 4, 5 };

            var result = Arguments.Concat(args1, args2);

            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, result);
            Assert.NotSame(args1, result);
            Assert.NotSame(args2, result);
        }

        [Fact]
        public void ToArrayOf_ShouldReturnSameArrayReference()
        {
            var args = new[] { "alpha", "beta" };

            var result = Arguments.ToArrayOf(args);

            Assert.Same(args, result);
        }

        [Fact]
        public void ToArray_ShouldReturnSameArrayReference()
        {
            object[] args = ["alpha", 42, null];

            var result = Arguments.ToArray(args);

            Assert.Same(args, result);
        }

        [Fact]
        public void ToEnumerableOf_ShouldExposeArrayAsEnumerable()
        {
            var args = new[] { "alpha", "beta" };

            var result = Arguments.ToEnumerableOf(args);

            Assert.Same(args, result);
            Assert.Equal(args, result);
        }

        [Fact]
        public void ToEnumerable_ShouldExposeArrayAsEnumerable()
        {
            object[] args = ["alpha", 42, null];

            var result = Arguments.ToEnumerable(args);

            Assert.Same(args, result);
            Assert.Equal(args, result);
        }

        [Fact]
        public void Yield_ShouldReturnSequenceContainingOnlySpecifiedArgument()
        {
            var result = Arguments.Yield("alpha");

            Assert.Collection(result, item => Assert.Equal("alpha", item));
        }

        [Fact]
        public void Yield_ShouldSupportRepeatedEnumeration()
        {
            var result = Arguments.Yield(42);

            Assert.Equal(new[] { 42 }, result.ToArray());
            Assert.Equal(new[] { 42 }, result.ToArray());
        }
    }
}
