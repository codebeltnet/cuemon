using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Cuemon.Extensions.Assets;
using Codebelt.Extensions.Xunit;
using Cuemon.Xml.Serialization;
using Xunit;

namespace Cuemon.Extensions;
public class TypeExtensionsTest : Test
{
    public TypeExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToFriendlyName_ShouldConvertTypeToHumanFriendlyRepresentation_EventIfFullNameIsNull()
    {
        var type = typeof(GenericClass<>).GetGenericArguments().Single();
        var typeFriendlyString = type.ToFriendlyName();
        var typeFriendlyFqString = type.ToFriendlyName(o => o.FullName = true);

        Assert.Equal("T", typeFriendlyString);
        Assert.Equal("T", typeFriendlyFqString); // fullname is null; fallback to name
    }

    [Fact]
    public void ToFriendlyName_ShouldConvertTypeToHumanFriendlyRepresentation()
    {
        var type = typeof(IList<string>);
        var typeFriendlyString = type.ToFriendlyName();
        var typeFriendlyFqString = type.ToFriendlyName(o => o.FullName = true);

        Assert.Equal("IList<String>", typeFriendlyString);
        Assert.Equal("System.Collections.Generic.IList<System.String>", typeFriendlyFqString);
    }

    [Fact]
    public void ToTypeCode_ShouldConvertTypeToCorrectTypeCode()
    {
        Type o = null;
        Assert.Equal(TypeCode.Boolean, typeof(bool).ToTypeCode());
        Assert.Equal(TypeCode.Byte, typeof(byte).ToTypeCode());
        Assert.Equal(TypeCode.Char, typeof(char).ToTypeCode());
        Assert.Equal(TypeCode.DBNull, typeof(DBNull).ToTypeCode());
        Assert.Equal(TypeCode.DateTime, typeof(DateTime).ToTypeCode());
        Assert.Equal(TypeCode.Decimal, typeof(decimal).ToTypeCode());
        Assert.Equal(TypeCode.Double, typeof(double).ToTypeCode());
        Assert.Equal(TypeCode.Empty, o.ToTypeCode());
        Assert.Equal(TypeCode.Int16, typeof(short).ToTypeCode());
        Assert.Equal(TypeCode.Int32, typeof(int).ToTypeCode());
        Assert.Equal(TypeCode.Int64, typeof(long).ToTypeCode());
        Assert.Equal(TypeCode.Object, typeof(object).ToTypeCode());
        Assert.Equal(TypeCode.SByte, typeof(sbyte).ToTypeCode());
        Assert.Equal(TypeCode.Single, typeof(float).ToTypeCode());
        Assert.Equal(TypeCode.String, typeof(string).ToTypeCode());
        Assert.Equal(TypeCode.UInt16, typeof(ushort).ToTypeCode());
        Assert.Equal(TypeCode.UInt32, typeof(uint).ToTypeCode());
        Assert.Equal(TypeCode.UInt64, typeof(ulong).ToTypeCode());
    }

    [Fact]
    public void HasEqualityComparerImplementation()
    {
        var truetype = typeof(StringComparer);
        var falseType = typeof(ArgumentException);
        Assert.True(truetype.HasEqualityComparerImplementation());
        Assert.False(falseType.HasEqualityComparerImplementation());
    }

    [Fact]
    public void HasComparableImplementation()
    {
        var truetype = typeof(string);
        var falseType = typeof(ArgumentException);
        Assert.True(truetype.HasComparableImplementation());
        Assert.False(falseType.HasComparableImplementation());
    }

    [Fact]
    public void HasComparerImplementation()
    {
        var truetype = typeof(StringComparer);
        var falseType = typeof(ArgumentException);
        Assert.True(truetype.HasComparerImplementation());
        Assert.False(falseType.HasComparerImplementation());
    }

    [Fact]
    public void HasEnumerableImplementation()
    {
        var truetype = typeof(ConcurrentBag<>);
        var falseType = typeof(ArgumentException);
        Assert.True(truetype.HasEnumerableImplementation());
        Assert.False(falseType.HasEnumerableImplementation());
    }

    [Fact]
    public void HasDictionaryImplementation()
    {
        var truetype = typeof(ConcurrentDictionary<,>);
        var falseType = typeof(List<>);
        Assert.True(truetype.HasDictionaryImplementation());
        Assert.False(falseType.HasDictionaryImplementation());
    }

    [Fact]
    public void HasKeyValuePairImplementation()
    {
        var truetype = typeof(KeyValuePair<,>);
        var falseType = typeof(List<>);
        Assert.True(truetype.HasKeyValuePairImplementation());
        Assert.False(falseType.HasKeyValuePairImplementation());
    }

    [Fact]
    public void IsNullable()
    {
        var truetype = typeof(int?);
        var falseType = typeof(int);
        Assert.True(truetype.IsNullable());
        Assert.False(falseType.IsNullable());
    }

    [Fact]
    public void HasAnonymousCharacteristics()
    {
        var truetype = new Func<string, string>(s => s).Target.GetType();
        var falseType = typeof(int);
        Assert.True(truetype.HasAnonymousCharacteristics());
        Assert.False(falseType.HasAnonymousCharacteristics());
    }

    [Fact]
    public void IsComplex()
    {
        var truetype = typeof(Stream);
        var falseType = typeof(int);
        Assert.True(truetype.IsComplex());
        Assert.False(falseType.IsComplex());
    }

    [Fact]
    public void IsSimple()
    {
        var truetype = typeof(int);
        var falseType = typeof(Stream);
        Assert.True(truetype.IsSimple());
        Assert.False(falseType.IsSimple());
    }

    [Fact]
    public void GetDefaultValue_ShouldBeDefaultValueFromValueTypesAndReferenceTypesWithDefaultConstructor()
    {
        Assert.Equal(0, typeof(int).GetDefaultValue());
        Assert.Equal(decimal.Zero, typeof(decimal).GetDefaultValue());
        Assert.Equal(Guid.Empty, typeof(Guid).GetDefaultValue());
        Assert.Equal(DateTime.MinValue, typeof(DateTime).GetDefaultValue());
        Assert.Equal(TimeSpan.Zero, typeof(TimeSpan).GetDefaultValue());
        Assert.Null(typeof(int?).GetDefaultValue());
        Assert.Null(typeof(bool?).GetDefaultValue());
    }

    [Fact]
    public void HasTypes_ShouldBeTrueForThoseImplementingTheTypeAndFalseForRest()
    {
        Assert.True(typeof(FileStream).HasTypes(typeof(Stream)));
        Assert.True(typeof(MemoryStream).HasTypes(typeof(MarshalByRefObject)));
        Assert.False(typeof(StringBuilder).HasTypes(typeof(string)));
        Assert.False(typeof(UTF32Encoding).HasTypes(typeof(string)));
    }

    [Fact]
    public void HasInterfaces_ShouldBeTrueForThoseImplementingTheInterfaceAndFalseForRest()
    {
        Assert.True(typeof(FileStream).HasInterfaces(typeof(IDisposable)));
        Assert.True(typeof(List<>).HasInterfaces(typeof(IEnumerable<>)));
        Assert.False(typeof(StringBuilder).HasInterfaces(typeof(IDisposable)));
        Assert.False(typeof(UTF32Encoding).HasInterfaces(typeof(IEnumerable<>)));
    }

    [Fact]
    public void HasAttributes_ShouldBeTrueForThoseMembersImplementingTheAttributesAndFalseForRest()
    {
        Assert.True(typeof(EventAttribute).HasAttributes(typeof(AttributeUsageAttribute)));
        Assert.True(typeof(XmlWrapper).HasAttributes(typeof(XmlIgnoreAttribute)));
        Assert.True(typeof(StringBuilder).HasAttributes(typeof(SerializableAttribute)));
        Assert.True(typeof(UTF32Encoding).HasAttributes(typeof(CLSCompliantAttribute)));
        Assert.False(typeof(int).HasAttributes(typeof(AttributeUsageAttribute)));
        Assert.False(typeof(string).HasAttributes(typeof(AttributeUsageAttribute)));
    }
}
