using System;
using Cuemon.Data.Integrity;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Data.Integrity;
public class DateTimeExtensionsTest : Test
{
    public DateTimeExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetCacheValidator_UseDefaultMethod_ShouldHaveUnspecifiedIntegrityValidation()
    {
        var dt = 0d.FromUnixEpochTime().GetCacheValidator();
        var expected = "6c62272e07bb014262b821756295c58d";
        Assert.Equal(expected, dt.ToString());
        Assert.Equal(EntityDataIntegrityValidation.Unspecified, dt.Validation);
        TestOutput.WriteLine(dt.ToString());
    }

    [Fact]
    public void GetCacheValidator_UseTimestampMethod_ShouldHaveUnspecifiedIntegrityValidation()
    {
        var dt = 0d.FromUnixEpochTime().GetCacheValidator(0d.FromUnixEpochTime().AddDays(7), method: EntityDataIntegrityMethod.Timestamp);
        var expected = "1192c6957365995ad3a62bff3cf1b3ad";
        Assert.Equal(expected, dt.ToString());
        Assert.Equal(EntityDataIntegrityValidation.Unspecified, dt.Validation);
        TestOutput.WriteLine(dt.ToString());
    }

    [Fact]
    public void GetCacheValidator_UseDefaultMethod_ShouldHaveWeakIntegrityValidation()
    {
        var dt = 0d.FromUnixEpochTime().GetCacheValidator(0d.FromUnixEpochTime().AddDays(7), Convertible.GetBytes(1234567890));
        var expected = "65567817ef757277b806e833c0139a60";
        Assert.Equal(expected, dt.ToString());
        Assert.Equal(EntityDataIntegrityValidation.Weak, dt.Validation);
        TestOutput.WriteLine(dt.ToString());
    }

    [Fact]
    public void GetCacheValidator_UseDefaultMethod_ShouldHaveStrongIntegrityValidation()
    {
        var dt = 0d.FromUnixEpochTime().GetCacheValidator(0d.FromUnixEpochTime().AddDays(7), Convertible.GetBytes(1234567890), EntityDataIntegrityValidation.Strong);
        var expected = "65567817ef757277b806e833c0139a60";
        Assert.Equal(expected, dt.ToString());
        Assert.Equal(EntityDataIntegrityValidation.Strong, dt.Validation);
        TestOutput.WriteLine(dt.ToString());
    }
}
