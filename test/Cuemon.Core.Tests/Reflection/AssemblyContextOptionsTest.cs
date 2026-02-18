using System;
using System.Linq;
using System.Reflection;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Reflection
{
    public class AssemblyContextOptionsTest : Test
    {
        public AssemblyContextOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AssemblyContextOptions_ShouldHaveDefaultValues()
        {
            var sut = new AssemblyContextOptions();

            Assert.True(sut.IncludeReferencedAssemblies);
            Assert.NotNull(sut.AssemblyFilter);
            Assert.NotNull(sut.ReferencedAssemblyFilter);
        }

        [Fact]
        public void ValidateOptions_ShouldThrowInvalidOperationException_WhenAssemblyFilterIsNull()
        {
            var sut1 = new AssemblyContextOptions()
            {
                AssemblyFilter = null
            };

            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'AssemblyFilter is null')", sut2.Message);
            Assert.StartsWith("AssemblyContextOptions are not in a valid state.", sut3.Message);
            Assert.Contains("sut1", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void ValidateOptions_ShouldThrowInvalidOperationException_WhenReferencedAssemblyFilterIsNull()
        {
            var sut1 = new AssemblyContextOptions()
            {
                ReferencedAssemblyFilter = null
            };

            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'ReferencedAssemblyFilter is null')", sut2.Message);
            Assert.StartsWith("AssemblyContextOptions are not in a valid state.", sut3.Message);
            Assert.Contains("sut1", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void DefaultAssemblyFilter_ShouldExcludeSystemAssemblies()
        {
            var sut = new AssemblyContextOptions();
            var systemAssembly = typeof(UriKind).Assembly; // System.dll
            var corlibAssembly = typeof(string).Assembly; // mscorlib

            TestOutput.WriteLine(systemAssembly.FullName);
            TestOutput.WriteLine(corlibAssembly.FullName);

            Assert.False(sut.AssemblyFilter(systemAssembly));
            Assert.False(sut.AssemblyFilter(corlibAssembly));
        }

        [Fact]
        public void DefaultAssemblyFilter_ShouldExcludeMicrosoftAssemblies()
        {
            var sut = new AssemblyContextOptions();
            var microsoftAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .First(a => a.FullName.StartsWith("Microsoft.", StringComparison.Ordinal));

            Assert.False(sut.AssemblyFilter(microsoftAssembly));

            TestOutput.WriteLine(microsoftAssembly.FullName);
        }

#if NET9_0_OR_GREATER
        [Fact]
        public void DefaultAssemblyFilter_ShouldIncludeNonSystemNonMicrosoftAssemblies()
        {
            var sut = new AssemblyContextOptions();
            var cuemonAssembly = typeof(AssemblyContextOptions).Assembly; // Cuemon.Core

            TestOutput.WriteLine(cuemonAssembly.FullName);

            Assert.True(sut.AssemblyFilter(cuemonAssembly));
        }
#endif

        [Theory]
        [InlineData("System.Runtime")]
        [InlineData("System.Collections")]
        public void DefaultReferencedAssemblyFilter_ShouldExcludeSystemAssemblyNames(string name)
        {
            var sut = new AssemblyContextOptions();
            var assemblyName = new AssemblyName(name);

            Assert.False(sut.ReferencedAssemblyFilter(assemblyName));
        }

        [Theory]
        [InlineData("Microsoft.Extensions.Logging")]
        [InlineData("Microsoft.AspNetCore.Http")]
        public void DefaultReferencedAssemblyFilter_ShouldExcludeMicrosoftAssemblyNames(string name)
        {
            var sut = new AssemblyContextOptions();
            var assemblyName = new AssemblyName(name);

            Assert.False(sut.ReferencedAssemblyFilter(assemblyName));
        }

        [Theory]
        [InlineData("Cuemon.Core")]
        [InlineData("Cuemon.Extensions.Core")]
        public void DefaultReferencedAssemblyFilter_ShouldIncludeNonSystemNonMicrosoftAssemblyNames(string name)
        {
            var sut = new AssemblyContextOptions();
            var assemblyName = new AssemblyName(name);

            Assert.True(sut.ReferencedAssemblyFilter(assemblyName));
        }
    }
}
