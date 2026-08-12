using System;
using System.Data;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Data;
public class DbTypeDecoratorExtensionsTest : Test
{
    public DbTypeDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToEquivalentType_ShouldMapDbTypeToBuiltInDotNetType()
    {
        Assert.Equal(typeof(byte), Decorator.Enclose(DbType.Byte).ToType());
        Assert.Equal(typeof(sbyte), Decorator.Enclose(DbType.SByte).ToType());
        Assert.Equal(typeof(byte[]), Decorator.Enclose(DbType.Binary).ToType());
        Assert.Equal(typeof(bool), Decorator.Enclose(DbType.Boolean).ToType());
        Assert.Equal(typeof(double), Decorator.Enclose(DbType.Currency).ToType());
        Assert.Equal(typeof(double), Decorator.Enclose(DbType.Double).ToType());
        Assert.Equal(typeof(DateTime), Decorator.Enclose(DbType.Date).ToType());
        Assert.Equal(typeof(DateTime), Decorator.Enclose(DbType.DateTime).ToType());
        Assert.Equal(typeof(DateTime), Decorator.Enclose(DbType.DateTime2).ToType());
        Assert.Equal(typeof(DateTime), Decorator.Enclose(DbType.Time).ToType());
        Assert.Equal(typeof(DateTimeOffset), Decorator.Enclose(DbType.DateTimeOffset).ToType());
        Assert.Equal(typeof(Guid), Decorator.Enclose(DbType.Guid).ToType());
        Assert.Equal(typeof(long), Decorator.Enclose(DbType.Int64).ToType());
        Assert.Equal(typeof(int), Decorator.Enclose(DbType.Int32).ToType());
        Assert.Equal(typeof(short), Decorator.Enclose(DbType.Int16).ToType());
        Assert.Equal(typeof(object), Decorator.Enclose(DbType.Object).ToType());
        Assert.Equal(typeof(float), Decorator.Enclose(DbType.Single).ToType());
        Assert.Equal(typeof(ulong), Decorator.Enclose(DbType.UInt64).ToType());
        Assert.Equal(typeof(uint), Decorator.Enclose(DbType.UInt32).ToType());
        Assert.Equal(typeof(ushort), Decorator.Enclose(DbType.UInt16).ToType());
        Assert.Equal(typeof(decimal), Decorator.Enclose(DbType.Decimal).ToType());
        Assert.Equal(typeof(decimal), Decorator.Enclose(DbType.VarNumeric).ToType());
        Assert.Equal(typeof(string), Decorator.Enclose(DbType.AnsiString).ToType());
        Assert.Equal(typeof(string), Decorator.Enclose(DbType.AnsiStringFixedLength).ToType());
        Assert.Equal(typeof(string), Decorator.Enclose(DbType.StringFixedLength).ToType());
        Assert.Equal(typeof(string), Decorator.Enclose(DbType.String).ToType());
        Assert.Equal(typeof(string), Decorator.Enclose(DbType.Xml).ToType());

        var sut = Assert.Throws<ArgumentOutOfRangeException>(() => Decorator.Enclose((DbType)42).ToType());
        Assert.Equal("decorator", sut.ParamName);
        Assert.StartsWith("DbType, '42', is not supported.", sut.Message);
    }
}
