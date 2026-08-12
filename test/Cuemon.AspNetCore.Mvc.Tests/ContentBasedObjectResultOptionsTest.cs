using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Mvc;
public class ContentBasedObjectResultOptionsTest : Test
{
    public ContentBasedObjectResultOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ContentBasedObjectResultOptions_ShouldThrowInvalidOperationExceptionWhenChecksumProviderIsNull()
    {
        var sut1 = new ContentBasedObjectResultOptions<object>();
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'ChecksumProvider == null')", sut2.Message);
        Assert.Equal("ContentBasedObjectResultOptions<Object> are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void ContentBasedObjectResultOptions_ShouldHaveDefaultValues()
    {
        var sut = new ContentBasedObjectResultOptions<object>();

        Assert.Null(sut.ChecksumProvider);
        Assert.Null(sut.WeakChecksumProvider);
    }
}
