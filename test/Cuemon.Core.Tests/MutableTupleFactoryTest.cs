using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
    public class MutableTupleFactoryTest : Test
    {
        public MutableTupleFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ActionFactory_Ctor_WithNullTuple_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionFactory<MutableTuple<int>>(IncrementAction, null));
        }

        [Fact]
        public void ActionFactory_ExecuteMethod_InvokesDelegateAndExposesDelegateInfo()
        {
            var tuple = new MutableTuple<int>(41);
            var sut = new ActionFactory<MutableTuple<int>>(IncrementAction, tuple);

            sut.ExecuteMethod();

            Assert.True(sut.HasDelegate);
            Assert.NotNull(sut.DelegateInfo);
            Assert.Equal(42, tuple.Arg1);
            Assert.Contains(nameof(IncrementAction), sut.ToString());
        }

        [Fact]
        public void ActionFactory_Clone_CreatesIndependentTupleCopy()
        {
            var sut = new ActionFactory<MutableTuple<int>>(IncrementAction, new MutableTuple<int>(1));

            var clone = Assert.IsType<ActionFactory<MutableTuple<int>>>(sut.Clone());
            clone.ExecuteMethod();

            Assert.NotSame(sut, clone);
            Assert.NotSame(sut.GenericArguments, clone.GenericArguments);
            Assert.Equal(1, sut.GenericArguments.Arg1);
            Assert.Equal(2, clone.GenericArguments.Arg1);
        }

        [Fact]
        public void ActionFactory_ExecuteMethod_WithoutDelegate_ThrowsInvalidOperationException()
        {
            var sut = new ActionFactory<MutableTuple<int>>(null, new MutableTuple<int>(1), null);

            var ex = Assert.Throws<InvalidOperationException>(() => sut.ExecuteMethod());

            Assert.False(sut.HasDelegate);
            Assert.Null(sut.DelegateInfo);
            Assert.Equal("There is no delegate specified on the factory.", ex.Message);
        }

        [Fact]
        public void ActionFactory_ExecuteMethod_WithNullOriginalDelegate_ThrowsInvalidOperationException()
        {
            var sut = new ActionFactory<MutableTuple<int>>(IncrementAction, new MutableTuple<int>(1), null);

            var ex = Assert.Throws<InvalidOperationException>(() => sut.ExecuteMethod());

            Assert.False(sut.HasDelegate);
            Assert.Contains("null referenced delegate wrapper", ex.Message);
        }

        [Fact]
        public void FuncFactory_ExecuteMethod_ReturnsResultAndExposesDelegateInfo()
        {
            var sut = new FuncFactory<MutableTuple<int, int>, int>(SumValues, new MutableTuple<int, int>(20, 22));

            var result = sut.ExecuteMethod();

            Assert.True(sut.HasDelegate);
            Assert.NotNull(sut.DelegateInfo);
            Assert.Equal(42, result);
            Assert.Contains(nameof(SumValues), sut.ToString());
        }

        [Fact]
        public void FuncFactory_Clone_CreatesIndependentTupleCopy()
        {
            var sut = new FuncFactory<MutableTuple<int, int>, int>(SumValues, new MutableTuple<int, int>(7, 8));

            var clone = Assert.IsType<FuncFactory<MutableTuple<int, int>, int>>(sut.Clone());
            clone.GenericArguments.Arg1 = 30;

            Assert.NotSame(sut, clone);
            Assert.NotSame(sut.GenericArguments, clone.GenericArguments);
            Assert.Equal(15, sut.ExecuteMethod());
            Assert.Equal(38, clone.ExecuteMethod());
        }

        [Fact]
        public void FuncFactory_ExecuteMethod_WithoutDelegate_ThrowsInvalidOperationException()
        {
            var sut = new FuncFactory<MutableTuple<int>, int>(null, new MutableTuple<int>(1), null);

            var ex = Assert.Throws<InvalidOperationException>(() => sut.ExecuteMethod());

            Assert.False(sut.HasDelegate);
            Assert.Equal("There is no delegate specified on the factory.", ex.Message);
        }

        [Fact]
        public void TesterFuncFactory_ExecuteMethod_ReturnsOutValueAndSuccessFlag()
        {
            var sut = new TesterFuncFactory<MutableTuple<int>, string, bool>(TryDescribe, new MutableTuple<int>(42));

            var success = sut.ExecuteMethod(out var result);

            Assert.True(success);
            Assert.Equal("42", result);
            Assert.True(sut.HasDelegate);
            Assert.NotNull(sut.DelegateInfo);
            Assert.Contains(nameof(TryDescribe), sut.ToString());
        }

        [Fact]
        public void TesterFuncFactory_Clone_CreatesIndependentTupleCopy()
        {
            var sut = new TesterFuncFactory<MutableTuple<int>, string, bool>(TryDescribe, new MutableTuple<int>(5));

            var clone = Assert.IsType<TesterFuncFactory<MutableTuple<int>, string, bool>>(sut.Clone());
            clone.GenericArguments.Arg1 = 0;

            var originalSuccess = sut.ExecuteMethod(out var originalResult);
            var cloneSuccess = clone.ExecuteMethod(out var cloneResult);

            Assert.NotSame(sut, clone);
            Assert.NotSame(sut.GenericArguments, clone.GenericArguments);
            Assert.True(originalSuccess);
            Assert.False(cloneSuccess);
            Assert.Equal("5", originalResult);
            Assert.Equal("0", cloneResult);
        }

        [Fact]
        public void TesterFuncFactory_WithNullOriginalDelegate_StillExecutesMethod()
        {
            var sut = new TesterFuncFactory<MutableTuple<int>, string, bool>(TryDescribe, new MutableTuple<int>(3), null);

            var success = sut.ExecuteMethod(out var result);

            Assert.False(sut.HasDelegate);
            Assert.NotNull(sut.DelegateInfo);
            Assert.True(success);
            Assert.Equal("3", result);
        }

        private static void IncrementAction(MutableTuple<int> tuple)
        {
            tuple.Arg1++;
        }

        private static int SumValues(MutableTuple<int, int> tuple)
        {
            return tuple.Arg1 + tuple.Arg2;
        }

        private static bool TryDescribe(MutableTuple<int> tuple, out string result)
        {
            result = tuple.Arg1.ToString();
            return tuple.Arg1 > 0;
        }
    }
}
