using Cuemon.Data.Integrity;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Data.Integrity;
public class AssemblyExtensionsTest : Test
{
    public AssemblyExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetCacheValidator_ShouldHaveStrongIntegrityChecksum()
    {

        var a = typeof(AssemblyExtensionsTest).Assembly;
        var cv1 = a.GetCacheValidator();
        Assert.Equal(EntityDataIntegrityValidation.Weak, cv1.Validation);
        var cv2 = a.GetCacheValidator(setup: o => o.BytesToRead = 400);
        Assert.Equal(EntityDataIntegrityValidation.Strong, cv2.Validation);
        Assert.NotEqual(cv1.ToString(), CacheValidator.Default.ToString());
        Assert.NotEqual(cv2.ToString(), CacheValidator.Default.ToString());
        Assert.NotEqual(cv1.ToString(), cv2.ToString());
        TestOutput.WriteLine(cv1.ToString());
        TestOutput.WriteLine(cv2.ToString());
    }
}
