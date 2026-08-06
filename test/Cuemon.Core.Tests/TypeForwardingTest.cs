using System;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class TypeForwardingTest : Test
{
    public TypeForwardingTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CoreAssembly_ShouldTypeForwardAllPublicTypesFromKernelAssembly()
    {
        var coreAssembly = typeof(DateSpan).Assembly;
        var kernelAssembly = typeof(ArgumentReservedKeywordException).Assembly;

        var forwardedTypeNames = kernelAssembly.GetExportedTypes()
            .Select(type => coreAssembly.GetType(type.FullName ?? string.Empty, false))
            .Where(type => type != null && type.Assembly == kernelAssembly)
            .Select(type => type.FullName)
            .Where(name => name != null)
            .ToHashSet(StringComparer.Ordinal);

        var missingForwardedTypeNames = kernelAssembly.GetExportedTypes()
            .Select(type => type.FullName)
            .Where(name => name != null && !forwardedTypeNames.Contains(name))
            .OrderBy(name => name)
            .ToList();

        if (missingForwardedTypeNames.Count > 0)
        {
            TestOutput.WriteLine(string.Join(Environment.NewLine, missingForwardedTypeNames));
        }

        Assert.Empty(missingForwardedTypeNames);
    }
}
