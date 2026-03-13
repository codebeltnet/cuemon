using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
    public class TypeArgumentOutOfRangeExceptionTest : Test
    {
        public TypeArgumentOutOfRangeExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldUseDefaultMessageAndParamName()
        {
            var sut = new TypeArgumentOutOfRangeException("TValue");

            Assert.Equal("TValue", sut.ParamName);
            Assert.StartsWith("Specified type argument was out of the range of valid values.", sut.Message);
        }

        [Fact]
        public void Ctor_ShouldUseCustomMessageAndParamName()
        {
            var sut = new TypeArgumentOutOfRangeException("TValue", "Type argument was invalid.");

            Assert.Equal("TValue", sut.ParamName);
            Assert.StartsWith("Type argument was invalid.", sut.Message);
        }

        [Fact]
        public void Ctor_ShouldAssignActualValue()
        {
            var sut = new TypeArgumentOutOfRangeException("TValue", typeof(Guid), "Type argument was invalid.");

            Assert.Equal("TValue", sut.ParamName);
            Assert.Equal(typeof(Guid), sut.ActualValue);
            Assert.StartsWith("Type argument was invalid.", sut.Message);
        }

        [Fact]
        public void Ctor_ShouldAssignInnerException()
        {
            var inner = new InvalidOperationException("boom");
            var sut = new TypeArgumentOutOfRangeException("Type argument was invalid.", inner);

            Assert.Equal("Type argument was invalid.", sut.Message);
            Assert.Same(inner, sut.InnerException);
        }
    }
}
