using System;
using System.IO;
using Cuemon.Data.Integrity;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Data.Integrity;

public class FileInfoExtensionsTest : Test
{
    public FileInfoExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetCacheValidator_ShouldThrowArgumentNullException_WhenFileInfoIsNull()
    {
        FileInfo file = null;

        Assert.Throws<ArgumentNullException>(() => file.GetCacheValidator());
    }

    [Fact]
    public void GetCacheValidator_ShouldReturnValidCacheValidator_WhenFileExists()
    {
        var path = Path.Combine(AppContext.BaseDirectory, $"file-info-{Guid.NewGuid():N}.tmp");

        try
        {
            File.WriteAllText(path, "cuemon");

            var result = new FileInfo(path).GetCacheValidator();

            Assert.NotNull(result);
            Assert.NotEqual(CacheValidator.Default.ToString(), result.ToString());
            TestOutput.WriteLine(result.ToString());
        }
        finally
        {
            if (File.Exists(path)) { File.Delete(path); }
        }
    }

    [Fact]
    public void GetCacheValidator_ShouldReturnDefault_WhenFileDoesNotExist()
    {
        var path = Path.Combine(AppContext.BaseDirectory, $"missing-file-{Guid.NewGuid():N}.tmp");
        var file = new FileInfo(path);

        var result = file.GetCacheValidator(setup: options => options.BytesToRead = 1);

        Assert.Equal(CacheValidator.Default.ToString(), result.ToString());
        TestOutput.WriteLine(result.ToString());
    }
}
