using System.Diagnostics;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Diagnostics;
public class FileVersionInfoExtensionsTest : Test
{
    public FileVersionInfoExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToProductVersion_ShouldConvertFileVersionInfoToVersionResult()
    {
        var sut1 = FileVersionInfo.GetVersionInfo(typeof(FileVersionInfoExtensions).Assembly.Location);
        var sut2 = sut1.ToProductVersion();

        TestOutput.WriteLine(sut1.ToString());
        TestOutput.WriteLine(sut2.ToString());

        Assert.True(sut2.HasAlphanumericVersion);
        Assert.True(sut2.IsSemanticVersion());
        Assert.Equal(sut2.AlphanumericVersion, sut1.ProductVersion);
        Assert.Equal(sut2.ToString(), sut1.ProductVersion);
    }

    [Fact]
    public void ToFileVersion_ShouldConvertFileVersionInfoToVersionResult()
    {
        var sut1 = FileVersionInfo.GetVersionInfo(typeof(FileVersionInfoExtensions).Assembly.Location);
        var sut2 = sut1.ToFileVersion();

        TestOutput.WriteLine(sut1.ToString());
        TestOutput.WriteLine(sut2.ToString());

        Assert.False(sut2.IsSemanticVersion());
        Assert.Equal(sut2.ToString(), sut1.FileVersion);
    }
}
