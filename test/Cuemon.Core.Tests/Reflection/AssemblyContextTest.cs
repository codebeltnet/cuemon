using Codebelt.Extensions.Xunit;
using Cuemon.Collections.Generic;
using System;
using System.Linq;
using Xunit;

namespace Cuemon.Reflection;
public class AssemblyContextTest : Test
{
    public AssemblyContextTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldReturnNonEmptyList_WithDefaultOptions()
    {
        var result = AssemblyContext.GetCurrentDomainAssemblies();

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        TestOutput.WriteLine($"Total assemblies returned: {result.Count}");
        foreach (var assembly in result.Take(5))
        {
            TestOutput.WriteLine(assembly.GetName().Name);
        }
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldExcludeCuemonCoreAssembly()
    {
        var cuemonCore = typeof(AssemblyContext).Assembly;

        var result = AssemblyContext.GetCurrentDomainAssemblies();

        TestOutput.WriteLine($"Excluded assembly: {cuemonCore.GetName().Name}");

        Assert.DoesNotContain(cuemonCore, result);
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldContainTestAssembly_WithDefaultOptions()
    {
        var testAssembly = GetType().Assembly;

        var result = AssemblyContext.GetCurrentDomainAssemblies();

        TestOutput.WriteLine($"Test assembly: {testAssembly.GetName().Name}");

        Assert.Contains(testAssembly, result);
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldReturnDistinctAssemblies()
    {
        var result = AssemblyContext.GetCurrentDomainAssemblies();

        TestOutput.WriteLine($"Total: {result.Count}, distinct: {result.Distinct().Count()}");

        Assert.Equal(result.Count, result.Distinct().Count());
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldThrowArgumentException_WhenSetupIsInvalid()
    {
        var result = Assert.Throws<ArgumentException>(() =>
            AssemblyContext.GetCurrentDomainAssemblies(o => o.AssemblyFilter = null));

        Assert.StartsWith("Delegate must configure the public read-write properties to be in a valid state.", result.Message);
        Assert.Contains("setup", result.Message);
        Assert.IsType<InvalidOperationException>(result.InnerException);
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldOnlyReturnDomainAssemblies_WhenReferencedAssembliesNotIncluded()
    {
        var domainSnapshot = AppDomain.CurrentDomain.GetAssemblies();

        var result = AssemblyContext.GetCurrentDomainAssemblies(o =>
        {
            o.AssemblyFilter = _ => true;
            o.IncludeReferencedAssemblies = false;
        });

        TestOutput.WriteLine($"Domain assemblies: {domainSnapshot.Length}, returned: {result.Count}");

        Assert.All(result, assembly => Assert.Contains(assembly, domainSnapshot));
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldRespectCustomAssemblyFilter_WhenPermissive()
    {
        var defaultResult = AssemblyContext.GetCurrentDomainAssemblies(o => o.IncludeReferencedAssemblies = false);
        var permissiveResult = AssemblyContext.GetCurrentDomainAssemblies(o =>
        {
            o.AssemblyFilter = _ => true;
            o.IncludeReferencedAssemblies = false;
        });

        TestOutput.WriteLine($"Default filter count: {defaultResult.Count}, permissive filter count: {permissiveResult.Count}");

        Assert.True(permissiveResult.Count > defaultResult.Count);
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldReturnAtLeastAsManyAssemblies_WhenReferencedAssembliesIncluded()
    {
        var withoutRefs = AssemblyContext.GetCurrentDomainAssemblies(o => o.IncludeReferencedAssemblies = false);
        var withRefs = AssemblyContext.GetCurrentDomainAssemblies(o => o.IncludeReferencedAssemblies = true);
        var missing = withoutRefs.Except(withRefs).ToList();

        TestOutput.WriteLine($"With referenced: {withRefs.Count}, without referenced: {withoutRefs.Count}");

        Assert.Empty(missing);
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldExcludeSystemAndMicrosoftAssemblies_WithDefaultOptions()
    {
        var result = AssemblyContext.GetCurrentDomainAssemblies();

        Assert.All(result, assembly =>
        {
            Assert.False(assembly.FullName.StartsWith("System", StringComparison.Ordinal),
                $"Expected '{assembly.GetName().Name}' to be excluded by the default System filter.");
            Assert.False(assembly.FullName.StartsWith("Microsoft", StringComparison.Ordinal),
                $"Expected '{assembly.GetName().Name}' to be excluded by the default Microsoft filter.");
        });
    }
}
