using System;
using System.Linq;
using Cuemon.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class ObjectExtensionsTest : Test
{
    public ObjectExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void UseWrapper_ShouldWrapTheAnswerToEverything()
    {
        var si = 42.UseWrapper(info => info.Add("funFact", "THE ANSWER TO LIFE, THE UNIVERSE AND EVERYTHING"));

        Assert.Equal(42, si.Instance);
        Assert.Equal(typeof(int), si.InstanceType);
        Assert.Equal(typeof(long), si.InstanceAs<long>().GetType());
        Assert.Equal(typeof(byte), si.InstanceAs<byte>().GetType());
        Assert.Equal("THE ANSWER TO LIFE, THE UNIVERSE AND EVERYTHING", si.Data.Single(pair => pair.Key == "funFact").Value);
    }

    [Fact]
    public void As_ConvertAnythingOrDefault()
    {
        object answer = 42;
        Assert.Equal("42", 42.As<string>());
        Assert.Equal(42, 42.As<long>());
        Assert.Equal(42, 42.As<byte>());
        Assert.Equal(42, 42.As<ushort>());
        Assert.Equal(42, 42.As<short>());
        Assert.Equal((ulong)42, 42.As<ulong>());
        Assert.Equal((uint)42, 42.As<uint>());
        Assert.Equal(42, 42.As<decimal>());
        Assert.Equal(42, 42.As<sbyte>());
        Assert.Equal(TimeSpan.FromTicks(42), 42.As(TimeSpan.FromTicks(42)));
    }

    [Fact]
    public void GetHashCode32_ShouldGenerateASuitable32bitHashCode()
    {
        var v = Arguments.ToEnumerableOf<IConvertible>(42, Guid.Empty.ToString("N"), DateTime.MaxValue, TimeSpan.TicksPerDay, true, decimal.MaxValue, double.Epsilon, float.Epsilon);
        Assert.Equal(1246074942, v.GetHashCode32());
    }

    [Fact]
    public void GetHashCode64_ShouldGenerateASuitable32bitHashCode()
    {
        var v = Arguments.ToEnumerableOf<IConvertible>(42, Guid.Empty.ToString("N"), DateTime.MaxValue, TimeSpan.TicksPerDay, true, decimal.MaxValue, double.Epsilon, float.Epsilon);
        Assert.Equal(6647510224603551806, v.GetHashCode64());
    }

    [Fact]
    public void ToDelimitedString_ShouldGenerateDelimitedString()
    {
        var s = Generate.RangeOf(10, i => i);
        var ds = s.ToDelimitedString();

        Assert.Equal("0,1,2,3,4,5,6,7,8,9", ds);
    }

    [Fact]
    public void Adjust_ShouldChangeTheStateOfExistingInstance()
    {
        var dt1 = DateTime.MinValue;
        var dt2 = dt1.Adjust(i => DateTime.MaxValue);

        Assert.Equal(dt1, DateTime.MinValue);
        Assert.Equal(dt2, DateTime.MaxValue);
    }

    [Fact]
    public void IsNullable_ShouldBeFalse()
    {
        var dt1 = DateTime.MinValue;

        Assert.False(dt1.IsNullable());
    }

    [Fact]
    public void IsNullable_ShouldBeTrue()
    {
        DateTime? dt1 = null;

        Assert.True(dt1.IsNullable());
    }
}
