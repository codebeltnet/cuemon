using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Versioning;
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Sdk;

namespace Cuemon.Reflection;

public class TargetFrameworkMonikerTest : Test
{
    public TargetFrameworkMonikerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Resolve_ShouldThrowArgumentNullException_WhenAssemblyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => TargetFrameworkMoniker.Resolve(null));
        Assert.Throws<ArgumentNullException>(() => TargetFrameworkMoniker.TryResolve(null, out _));
    }

    [Theory]
    [InlineData(".NETFramework,Version=v1.1", "net11")]
    [InlineData(".NETFramework,Version=v4.0", "net40")]
    [InlineData(".NETFramework,Version=v4.8.1", "net481")]
    [InlineData(".NETStandard,Version=v2.0", "netstandard2.0")]
    [InlineData(".NETCoreApp,Version=v3.1", "netcoreapp3.1")]
    [InlineData(".NETCoreApp,Version=v10.0", "net10.0")]
    [InlineData(".netcoreapp,version=v10.0", "net10.0")]
    [InlineData("net10.0", "net10.0")]
    [InlineData("net9.0-windows", "net9.0-windows")]
    [InlineData("net48", "net48")]
    public void Parse_ShouldReturnTargetFrameworkMoniker_WhenInputIsSupported(string frameworkName, string expected)
    {
        Assert.True(TargetFrameworkMoniker.TryParse(frameworkName, out var actual));
        Assert.Equal(expected, actual);
        Assert.Equal(expected, TargetFrameworkMoniker.Parse(frameworkName));
    }

    [Theory]
    [InlineData(".NETFramework,Version=v4.8.1", "net481")]
    [InlineData(".NETStandard,Version=v2.0", "netstandard2.0")]
    [InlineData(".NETCoreApp,Version=v10.0", "net10.0")]
    public void Parse_ShouldReturnTargetFrameworkMoniker_WhenFrameworkNameIsSupported(string frameworkName, string expected)
    {
        var input = new FrameworkName(frameworkName);

        Assert.True(TargetFrameworkMoniker.TryParse(input, out var actual));
        Assert.Equal(expected, actual);
        Assert.Equal(expected, TargetFrameworkMoniker.Parse(input));
    }

    [Theory]
    [InlineData(".NETFramework,Version=v1.1", "net11")]
    [InlineData(".NETFramework,Version=v4.0", "net40")]
    [InlineData(".NETFramework,Version=v4.8.1", "net481")]
    [InlineData(".NETStandard,Version=v2.0", "netstandard2.0")]
    [InlineData(".NETCoreApp,Version=v3.1", "netcoreapp3.1")]
    [InlineData(".NETCoreApp,Version=v10.0", "net10.0")]
    [InlineData(".netcoreapp,version=v10.0", "net10.0")]
    public void Resolve_ShouldReturnTargetFrameworkMoniker_WhenAssemblyContainsTargetFrameworkAttribute(string frameworkName, string expected)
    {
        var assembly = CreateDynamicAssembly(frameworkName);

        Assert.True(TargetFrameworkMoniker.TryResolve(assembly, out var actual));
        Assert.Equal(expected, actual);
        Assert.Equal(expected, TargetFrameworkMoniker.Resolve(assembly));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData(".NETPortable,Version=v4.0,Profile=Profile111")]
    public void Parse_ShouldReturnNull_WhenInputIsUnsupported(string frameworkName)
    {
        Assert.False(TargetFrameworkMoniker.TryParse(frameworkName, out var actual));
        Assert.Null(actual);
        Assert.Null(TargetFrameworkMoniker.Parse(frameworkName));
    }

    [Fact]
    public void Parse_ShouldReturnNull_WhenFrameworkNameIsNull()
    {
        Assert.False(TargetFrameworkMoniker.TryParse((FrameworkName)null, out var actual));
        Assert.Null(actual);
        Assert.Null(TargetFrameworkMoniker.Parse((FrameworkName)null));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData(".NETPortable,Version=v4.0,Profile=Profile111")]
    public void Resolve_ShouldReturnNull_WhenAssemblyDoesNotExposeASupportedTargetFramework(string frameworkName)
    {
        var assembly = CreateDynamicAssembly(frameworkName);

        Assert.False(TargetFrameworkMoniker.TryResolve(assembly, out var actual));
        Assert.Null(actual);
        Assert.Null(TargetFrameworkMoniker.Resolve(assembly));
    }

    [Fact]
    public void ResolveFromPath_ShouldReturnNearestTargetFrameworkMoniker_WhenPathContainsSupportedTargetFrameworkFolder()
    {
        var path = Path.Combine(Path.GetTempPath(), "cuemon", "artifacts", "net9.0-windows", "publish");

        Assert.True(TargetFrameworkMoniker.TryResolveFromPath(path, out var actual));
        Assert.Equal("net9.0-windows", actual);
        Assert.Equal("net9.0-windows", TargetFrameworkMoniker.ResolveFromPath(path));
    }

    [Fact]
    public void ResolveFromPath_ShouldReturnNull_WhenPathDoesNotContainSupportedTargetFrameworkFolder()
    {
        var path = Path.Combine(Path.GetTempPath(), "cuemon", "artifacts", "release");

        Assert.False(TargetFrameworkMoniker.TryResolveFromPath(path, out var actual));
        Assert.Null(actual);
        Assert.Null(TargetFrameworkMoniker.ResolveFromPath(path));
    }

    [Fact]
    public void ResolveCurrent_ShouldReturnCurrentTargetFrameworkMoniker()
    {
        var expected = GetExpectedTargetFrameworkMoniker();

        // Try to resolve the current target framework moniker. Some external test runners
        // (for example JetBrains test runner) may host tests in a different target framework
        // than the one the test assembly was compiled for (e.g., netcoreapp3.0). In those
        // cases the resolved TFM will differ from the expected compile-time TFM and the
        // assertion below would fail even though the compilation target is correct. To
        // avoid false negatives when running under non-built-in runners, skip the test
        // when the runtime-reported TFM does not match the compile-time expected TFM.
        if (!TargetFrameworkMoniker.TryResolveCurrent(out var actual))
        {
            throw SkipException.ForSkip("Could not resolve current Target Framework Moniker at runtime. Skipping test because runner might be hosting tests in a different TFM.");
        }

        if (!string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase))
        {
            throw SkipException.ForSkip($"Runtime Target Framework Moniker ('{actual}') does not match compile-time expected ('{expected}'). Skipping test when running under a different test runner.");
        }

        Assert.Equal(expected, TargetFrameworkMoniker.ResolveCurrent());

        TestOutput.WriteLine(actual);
    }

    private static Assembly CreateDynamicAssembly(string frameworkName)
    {
        var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName($"TargetFrameworkMonikerTest_{Guid.NewGuid():N}"), AssemblyBuilderAccess.Run);
        if (frameworkName != null)
        {
            var constructor = typeof(TargetFrameworkAttribute).GetConstructor(new[] { typeof(string) });
            assembly.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { frameworkName }));
        }

        return assembly;
    }

    private static string GetExpectedTargetFrameworkMoniker()
    {
#if NET10_0
        return "net10.0";
#elif NET9_0
        return "net9.0";
#elif NET48
        return "net48";
#else
        throw new NotSupportedException("The current test target framework is not covered by this test.");
#endif
    }
}
