using System;
using Cuemon.Data.Integrity;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Mvc;
public class CacheableObjectTest : Test
{
    public CacheableObjectTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CreateHttpLastModified_ShouldHaveICacheableTimestampImplementation_WhenCreatedWithTimestampRelatedArguments()
    {
        var or = Generate.RandomString(2048);
        var cor = CacheableFactory.CreateHttpLastModified(or, o =>
        {
            o.TimestampProvider = _ => DateTime.MinValue;
            o.ChangedTimestampProvider = _ => DateTime.MaxValue;
        });
        Assert.IsAssignableFrom<IEntityDataTimestamp>(cor);
        if (cor.Value is IEntityDataTimestamp timestamp)
        {
            Assert.True(timestamp.Created == DateTime.MinValue);
            Assert.True(timestamp.Modified == DateTime.MinValue);
        }
        Assert.Equal(cor.Value, or);
    }

    [Fact]
    public void CreateHttpEntityTag_ShouldHaveICacheableIntegrityImplementation_WhenCreatedWithIntegrityRelatedArguments()
    {
        var or = Generate.RandomString(2048);
        var orBytes = Convertible.GetBytes(or);
        var cor = CacheableFactory.CreateHttpEntityTag(or, o =>
        {
            o.ChecksumProvider = _ => orBytes;
            o.WeakChecksumProvider = _ => false;
        });
        Assert.IsAssignableFrom<IEntityDataIntegrity>(cor);
        if (cor.Value is IEntityDataIntegrity integrity)
        {
            Assert.True(integrity.Validation == EntityDataIntegrityValidation.Strong);
            Assert.True(integrity.Checksum.GetBytes() == orBytes);
        }
        Assert.Equal(cor.Value, or);
    }

    [Fact]
    public void Create_ShouldHaveICacheableEntityImplementation_WhenCreatedWithTimestampRelatedArguments_And_IntegrityRelatedArguments()
    {
        var or = Generate.RandomString(2048);
        var orBytes = Convertible.GetBytes(or);
        var cor = CacheableFactory.Create(or, o =>
        {
            o.TimestampProvider = _ => DateTime.MinValue;
            o.ChecksumProvider = _ => orBytes;
            o.ChangedTimestampProvider = _ => DateTime.MaxValue;
            o.WeakChecksumProvider = _ => false;
        });
        Assert.IsAssignableFrom<IEntityInfo>(cor);
        if (cor.Value is IEntityInfo entity)
        {
            Assert.True(entity.Created == DateTime.MinValue);
            Assert.True(entity.Modified == DateTime.MinValue);
            Assert.True(entity.Validation == EntityDataIntegrityValidation.Strong);
            Assert.True(entity.Checksum.GetBytes() == orBytes);
        }
        Assert.Equal(cor.Value, or);
    }
}
