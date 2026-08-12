using System;
using Cuemon.AspNetCore.Mvc;
using Cuemon.Data.Integrity;
using Codebelt.Extensions.Xunit;
using Cuemon.Security;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Mvc;
public class CacheableObjectResultExtensionsTest : Test
{
    public CacheableObjectResultExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void WithLastModifiedHeader_ShouldWrapObjectInCacheableObjectResultOfTypeEntityDataTimestamp()
    {
        var sut1 = Generate.RandomString(2048);
        var sut2 = sut1.WithLastModifiedHeader(o =>
        {
            o.TimestampProvider = _ => DateTime.MinValue;
            o.ChangedTimestampProvider = _ => DateTime.MaxValue;
        });
        var sut3 = sut2 as IEntityDataTimestamp;

        Assert.IsAssignableFrom<ICacheableObjectResult>(sut2);
        Assert.IsAssignableFrom<IEntityDataTimestamp>(sut2);
        Assert.NotNull(sut3);
        Assert.Equal(DateTime.MinValue, sut3.Created);
        Assert.Equal(DateTime.MaxValue, sut3.Modified);
        Assert.Equal(sut2.Value, sut1);
    }

    [Fact]
    public void WithEntityTagHeader_ShouldWrapObjectInCacheableObjectResultOfTypeEntityDataIntegrity_WithStrongValidation()
    {
        var sut1 = Generate.RandomString(2048);
        var sut2 = HashFactory.CreateFnv64().ComputeHash(sut1).GetBytes();
        var sut3 = sut1.WithEntityTagHeader(o =>
        {
            o.ChecksumProvider = _ => sut2;
            o.WeakChecksumProvider = _ => false;
        });
        var sut4 = sut3 as IEntityDataIntegrity;

        Assert.IsAssignableFrom<ICacheableObjectResult>(sut3);
        Assert.IsAssignableFrom<IEntityDataIntegrity>(sut3);
        Assert.NotNull(sut4);
        Assert.Equal(EntityDataIntegrityValidation.Strong, sut4.Validation);
        Assert.Equal(sut2, sut4.Checksum.GetBytes());
        Assert.Equal(sut3.Value, sut1);
    }

    [Fact]
    public void WithEntityTagHeader_ShouldWrapObjectInCacheableObjectResultOfTypeEntityDataIntegrity_WithWeakValidation()
    {
        var sut1 = Generate.RandomString(2048);
        var sut2 = HashFactory.CreateFnv64().ComputeHash(sut1).GetBytes();
        var sut3 = sut1.WithEntityTagHeader(o =>
        {
            o.ChecksumProvider = _ => sut2;
            o.WeakChecksumProvider = _ => true;
        });
        var sut4 = sut3 as IEntityDataIntegrity;

        Assert.IsAssignableFrom<ICacheableObjectResult>(sut3);
        Assert.IsAssignableFrom<IEntityDataIntegrity>(sut3);
        Assert.NotNull(sut4);
        Assert.Equal(EntityDataIntegrityValidation.Weak, sut4.Validation);
        Assert.Equal(sut2, sut4.Checksum.GetBytes());
        Assert.Equal(sut3.Value, sut1);
    }

    [Fact]
    public void WithCacheableHeaders_ShouldWrapObjectInCacheableObjectResultOfTypeEntityDataIntegrityAndOfTypeEntityDataTimestamp_WithStrongValidation()
    {
        var sut1 = Generate.RandomString(2048);
        var sut2 = HashFactory.CreateFnv64().ComputeHash(sut1).GetBytes();
        var sut3 = sut1.WithCacheableHeaders(o =>
        {
            o.TimestampProvider = _ => DateTime.MinValue;
            o.ChecksumProvider = _ => sut2;
            o.ChangedTimestampProvider = _ => DateTime.MaxValue;
            o.WeakChecksumProvider = _ => true;
        });
        var sut4 = sut3 as IEntityDataIntegrity;
        var sut5 = sut3 as IEntityDataTimestamp;

        Assert.IsAssignableFrom<ICacheableObjectResult>(sut3);
        Assert.IsAssignableFrom<IEntityDataIntegrity>(sut3);
        Assert.IsAssignableFrom<IEntityDataTimestamp>(sut3);
        Assert.NotNull(sut4);
        Assert.NotNull(sut5);
        Assert.Equal(EntityDataIntegrityValidation.Weak, sut4.Validation);
        Assert.Equal(sut2, sut4.Checksum.GetBytes());
        Assert.Equal(DateTime.MinValue, sut5.Created);
        Assert.Equal(DateTime.MaxValue, sut5.Modified);
        Assert.Equal(sut3.Value, sut1);
    }

    [Fact]
    public void WithCacheableHeaders_ShouldWrapObjectInCacheableObjectResultOfTypeEntityDataIntegrityAndOfTypeEntityDataTimestamp_WithWeakValidation()
    {
        var sut1 = Generate.RandomString(2048);
        var sut2 = HashFactory.CreateFnv64().ComputeHash(sut1).GetBytes();
        var sut3 = sut1.WithCacheableHeaders(o =>
        {
            o.TimestampProvider = _ => DateTime.MinValue;
            o.ChecksumProvider = _ => sut2;
            o.ChangedTimestampProvider = _ => DateTime.MaxValue;
            o.WeakChecksumProvider = _ => false;
        });
        var sut4 = sut3 as IEntityDataIntegrity;
        var sut5 = sut3 as IEntityDataTimestamp;

        Assert.IsAssignableFrom<ICacheableObjectResult>(sut3);
        Assert.IsAssignableFrom<IEntityDataIntegrity>(sut3);
        Assert.IsAssignableFrom<IEntityDataTimestamp>(sut3);
        Assert.NotNull(sut4);
        Assert.NotNull(sut5);
        Assert.Equal(EntityDataIntegrityValidation.Strong, sut4.Validation);
        Assert.Equal(sut2, sut4.Checksum.GetBytes());
        Assert.Equal(DateTime.MinValue, sut5.Created);
        Assert.Equal(DateTime.MaxValue, sut5.Modified);
        Assert.Equal(sut3.Value, sut1);
    }
}
