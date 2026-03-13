using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
    public class SuccessfulValueTest : Test
    {
        public SuccessfulValueTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_SucceededShouldBeTrue()
        {
            var sut = new SuccessfulValue();

            Assert.True(sut.Succeeded);
            Assert.Null(sut.Failure);
        }

        [Fact]
        public void Ctor_SucceededShouldBeTrueWithExpectedResult()
        {
            var value = Guid.NewGuid();
            var sut = new SuccessfulValue<Guid>(value);

            Assert.True(sut.Succeeded);
            Assert.Equal(value, sut.Result);
            Assert.Null(sut.Failure);
        }
    }
}
