using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Mvc;
public class CacheableObjectResultOptionsTest : Test
{
    public CacheableObjectResultOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CacheableObjectResultOptions_ShouldThrowInvalidOperationExceptionWhenChecksumProviderIsNull()
    {
        var sut1 = new CacheableObjectResultOptions<Guid>();
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'ChecksumProvider == null')", sut2.Message);
        Assert.Equal("CacheableObjectResultOptions<Guid> are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void CacheableObjectResultOptions_ShouldThrowInvalidOperationExceptionWhenTimestampProviderIsNull()
    {
        var sut1 = new CacheableObjectResultOptions<Guid>()
        {
            ChecksumProvider = _ => Array.Empty<byte>()
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'TimestampProvider == null')", sut2.Message);
        Assert.Equal("CacheableObjectResultOptions<Guid> are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void CacheableObjectResultOptions_ShouldHaveDefaultValues()
    {
        var sut = new CacheableObjectResultOptions<Guid>();

        Assert.Null(sut.TimestampProvider);
        Assert.Null(sut.ChangedTimestampProvider);
        Assert.Null(sut.ChecksumProvider);
        Assert.Null(sut.WeakChecksumProvider);
    }
}
