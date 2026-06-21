using System.Reflection;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Extensions
{
    public class MethodDescriptorExtensionsTest : Test
    {
        public MethodDescriptorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void HasParameters_ShouldReturnFalse_WhenDescriptorHasNoParameters()
        {
            var method = typeof(MethodDescriptorExtensionsTest).GetMethod(nameof(ParameterlessMethod), BindingFlags.NonPublic | BindingFlags.Static);
            var sut = MethodDescriptor.Create(method);

            Assert.False(sut.HasParameters());
        }

        [Fact]
        public void HasParameters_ShouldReturnTrue_WhenDescriptorHasParameters()
        {
            var method = typeof(MethodDescriptorExtensionsTest).GetMethod(nameof(MethodWithParameters), BindingFlags.NonPublic | BindingFlags.Static);
            var sut = MethodDescriptor.Create(method);

            Assert.True(sut.HasParameters());
        }

        private static void ParameterlessMethod()
        {
        }

        private static void MethodWithParameters(int number, string text)
        {
        }
    }
}
