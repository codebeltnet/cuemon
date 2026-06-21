using System;
using System.ComponentModel;
using System.Reflection;
using Cuemon.Extensions;
using Cuemon.Extensions.Data.Integrity;
using Cuemon.Security;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Data.Integrity;

public class CacheValidatorTest : Test
{
    public CacheValidatorTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetMostSignificant_ShouldReturnDefault_WhenSequenceIsEmpty()
    {
        var result = CacheValidator.GetMostSignificant();

        Assert.NotNull(result);
        Assert.Equal(CacheValidator.Default.ToString(), result.ToString());
    }

    [Fact]
    public void Constructor_ShouldInitializeCreatedOnly_WhenEntityInfoHasNoModifiedDate()
    {
        var created = 0d.FromUnixEpochTime();
        var entity = new EntityInfo(created);

        Assert.Equal(created, entity.Created);
        Assert.Null(entity.Modified);
    }

    [Fact]
    public void GetMostSignificant_ShouldReturnMostSignificant_WhenMultipleCacheValidatorsProvided()
    {
        var first = 0d.FromUnixEpochTime().GetCacheValidator();
        var second = 1d.FromUnixEpochTime().GetCacheValidator();

        var result = CacheValidator.GetMostSignificant(first, second);

        Assert.Same(second, result);
    }

    [Fact]
    public void AssemblyReference_ShouldSetAndGetAssembly()
    {
        var original = CacheValidator.AssemblyReference;
        var expected = typeof(CacheValidatorTest).Assembly;

        try
        {
            CacheValidator.AssemblyReference = expected;

            Assert.Same(expected, CacheValidator.AssemblyReference);
        }
        finally
        {
            CacheValidator.AssemblyReference = original;
        }
    }

    [Fact]
    public void AssemblyReference_ShouldThrowArgumentNullException_WhenSetToNull()
    {
        Assert.Throws<ArgumentNullException>(() => CacheValidator.AssemblyReference = null);
    }

    [Fact]
    public void ReferencePoint_ShouldReturnNonDefaultCacheValidator()
    {
        var original = CacheValidator.AssemblyReference;
        var assembly = typeof(CacheValidatorTest).Assembly;

        try
        {
            CacheValidator.AssemblyReference = assembly;
            var result = CacheValidator.ReferencePoint;

            Assert.NotNull(result);
            Assert.NotEqual(CacheValidator.Default.ToString(), result.ToString());
            TestOutput.WriteLine(result.ToString());
        }
        finally
        {
            CacheValidator.AssemblyReference = original;
        }
    }

    [Fact]
    public void Default_ShouldReturnNewInstanceEachTime()
    {
        var first = CacheValidator.Default;
        var second = CacheValidator.Default;

        Assert.NotSame(first, second);
        Assert.Equal(first.ToString(), second.ToString());
    }

    [Fact]
    public void Clone_ShouldReturnEqualButDistinctInstance()
    {
        var sut = CacheValidator.Default;

        var clone = sut.Clone();

        Assert.NotSame(sut, clone);
        Assert.Equal(sut.ToString(), clone.ToString());
    }

    [Fact]
    public void GetMostSignificant_Instance_ShouldReturnMaxTickDateTime()
    {
        var created = 0d.FromUnixEpochTime();
        var modified = created.AddDays(7);
        var sut = created.GetCacheValidator(modified);

        var result = sut.GetMostSignificant();

        Assert.Equal(modified, result);
    }

    [Fact]
    public void CombineWith_ShouldModifyChecksum()
    {
        var sut = 0d.FromUnixEpochTime().GetCacheValidator();
        var original = sut.ToString();

        var result = sut.CombineWith(Convertible.GetBytes(1234567890));

        Assert.Same(sut, result);
        Assert.NotEqual(original, sut.ToString());
        TestOutput.WriteLine(sut.ToString());
    }

    [Fact]
    public void Constructor_ShouldCombineChecksum_WhenMethodIsCombined()
    {
        var created = 0d.FromUnixEpochTime();
        var modified = created.AddDays(7);
        var entity = new EntityInfo(created, modified, Convertible.GetBytes(1234567890));
        var unaltered = new CacheValidator(entity, () => HashFactory.CreateFnv128());

        var sut = new CacheValidator(entity, () => HashFactory.CreateFnv128(), EntityDataIntegrityMethod.Combined);

        Assert.Equal(EntityDataIntegrityMethod.Combined, sut.Method);
        Assert.NotEqual(unaltered.ToString(), sut.ToString());
    }

    [Fact]
    public void Constructor_ShouldThrowInvalidEnumArgumentException_WhenMethodIsInvalid()
    {
        var entity = new EntityInfo(0d.FromUnixEpochTime(), 0d.FromUnixEpochTime().AddDays(7), Convertible.GetBytes(1234567890));

        Assert.Throws<InvalidEnumArgumentException>(() => new CacheValidator(entity, () => HashFactory.CreateFnv128(), (EntityDataIntegrityMethod)int.MaxValue));
    }
}
