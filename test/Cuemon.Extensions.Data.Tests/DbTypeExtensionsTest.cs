using System;
using System.Data;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Data;
public class DbTypeExtensionsTest : Test
{
    public DbTypeExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToEquivalentType_ShouldMapDbTypeToBuiltInDotNetType()
    {
        Assert.Equal(typeof(byte), DbType.Byte.ToType());
        Assert.Equal(typeof(sbyte), DbType.SByte.ToType());
        Assert.Equal(typeof(byte[]), DbType.Binary.ToType());
        Assert.Equal(typeof(bool), DbType.Boolean.ToType());
        Assert.Equal(typeof(double), DbType.Currency.ToType());
        Assert.Equal(typeof(double), DbType.Double.ToType());
        Assert.Equal(typeof(DateTime), DbType.Date.ToType());
        Assert.Equal(typeof(DateTime), DbType.DateTime.ToType());
        Assert.Equal(typeof(DateTime), DbType.DateTime2.ToType());
        Assert.Equal(typeof(DateTime), DbType.Time.ToType());
        Assert.Equal(typeof(DateTimeOffset), DbType.DateTimeOffset.ToType());
        Assert.Equal(typeof(Guid), DbType.Guid.ToType());
        Assert.Equal(typeof(long), DbType.Int64.ToType());
        Assert.Equal(typeof(int), DbType.Int32.ToType());
        Assert.Equal(typeof(short), DbType.Int16.ToType());
        Assert.Equal(typeof(object), DbType.Object.ToType());
        Assert.Equal(typeof(float), DbType.Single.ToType());
        Assert.Equal(typeof(ulong), DbType.UInt64.ToType());
        Assert.Equal(typeof(uint), DbType.UInt32.ToType());
        Assert.Equal(typeof(ushort), DbType.UInt16.ToType());
        Assert.Equal(typeof(decimal), DbType.Decimal.ToType());
        Assert.Equal(typeof(decimal), DbType.VarNumeric.ToType());
        Assert.Equal(typeof(string), DbType.AnsiString.ToType());
        Assert.Equal(typeof(string), DbType.AnsiStringFixedLength.ToType());
        Assert.Equal(typeof(string), DbType.StringFixedLength.ToType());
        Assert.Equal(typeof(string), DbType.String.ToType());
        Assert.Equal(typeof(string), DbType.Xml.ToType());

        var sut = Assert.Throws<ArgumentOutOfRangeException>(() => ((DbType)42).ToType());
        Assert.Equal("dbType", sut.ParamName);
        Assert.StartsWith("DbType, '42', is not supported.", sut.Message);
    }
}
