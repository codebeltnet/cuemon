using System;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class ConvertibleConverterDictionaryTest : Test
{
    public ConvertibleConverterDictionaryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Add_Generic_ShouldRegisterConverter()
    {
        var sut = new ConvertibleConverterDictionary();

        var result = sut.Add<int>(i => BitConverter.GetBytes(i));
        var converter = sut[typeof(int)];

        Assert.Same(sut, result);
        Assert.True(sut.ContainsKey(typeof(int)));
        Assert.NotNull(converter);
        Assert.Equal(BitConverter.GetBytes(42), converter(42));
    }

    [Fact]
    public void Add_Type_ShouldRegisterConverter()
    {
        var sut = new ConvertibleConverterDictionary();
        Func<IConvertible, byte[]> converter = c => BitConverter.GetBytes((double)c);

        var result = sut.Add(typeof(double), converter);

        Assert.Same(sut, result);
        Assert.True(sut.ContainsKey(typeof(double)));
        Assert.Same(converter, sut[typeof(double)]);
    }

    [Fact]
    public void Add_Type_ShouldThrowArgumentNullException_WhenTypeIsNull()
    {
        var sut = new ConvertibleConverterDictionary();

        Assert.Throws<ArgumentNullException>(() => sut.Add(null, _ => Array.Empty<byte>()));
    }

    [Fact]
    public void Add_Type_ShouldThrowArgumentOutOfRangeException_WhenTypeDoesNotImplementIConvertible()
    {
        var sut = new ConvertibleConverterDictionary();

        Assert.Throws<ArgumentOutOfRangeException>(() => sut.Add(typeof(object), _ => Array.Empty<byte>()));
    }

    [Fact]
    public void Add_ShouldThrowArgumentException_WhenTypeAlreadyExists()
    {
        var sut = new ConvertibleConverterDictionary();
        sut.Add<int>(i => BitConverter.GetBytes(i));

        Assert.Throws<ArgumentException>(() => sut.Add(typeof(int), _ => Array.Empty<byte>()));
    }

    [Fact]
    public void ContainsKey_ShouldReturnFalse_WhenTypeIsNotRegistered()
    {
        var sut = new ConvertibleConverterDictionary();

        Assert.False(sut.ContainsKey(typeof(int)));
    }

    [Fact]
    public void ContainsKey_ShouldThrowArgumentNullException_WhenKeyIsNull()
    {
        var sut = new ConvertibleConverterDictionary();

        Assert.Throws<ArgumentNullException>(() => sut.ContainsKey(null));
    }

    [Fact]
    public void TryGetValue_ShouldReturnTrueAndConverter_WhenTypeIsRegistered()
    {
        var sut = new ConvertibleConverterDictionary();
        Func<IConvertible, byte[]> converter = c => BitConverter.GetBytes((long)c);
        sut.Add(typeof(long), converter);

        var found = sut.TryGetValue(typeof(long), out var value);

        Assert.True(found);
        Assert.Same(converter, value);
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalseAndNull_WhenTypeIsNotRegistered()
    {
        var sut = new ConvertibleConverterDictionary();

        var found = sut.TryGetValue(typeof(long), out var value);

        Assert.False(found);
        Assert.Null(value);
    }

    [Fact]
    public void TryGetValue_ShouldThrowArgumentNullException_WhenKeyIsNull()
    {
        var sut = new ConvertibleConverterDictionary();

        Assert.Throws<ArgumentNullException>(() => sut.TryGetValue(null, out _));
    }

    [Fact]
    public void Indexer_ShouldReturnNull_WhenTypeIsNullOrMissing()
    {
        var sut = new ConvertibleConverterDictionary();

        Assert.Null(sut[null]);
        Assert.Null(sut[typeof(decimal)]);
    }

    [Fact]
    public void Keys_Values_And_Count_ShouldReflectRegisteredConverters()
    {
        var sut = new ConvertibleConverterDictionary()
            .Add<int>(i => BitConverter.GetBytes(i))
            .Add(typeof(string), c => c.ToString() == null ? Array.Empty<byte>() : System.Text.Encoding.UTF8.GetBytes(c.ToString()));

        Assert.Equal(2, sut.Count);
        Assert.Equal(2, sut.Keys.Count());
        Assert.Equal(2, sut.Values.Count());
        Assert.Contains(typeof(int), sut.Keys);
        Assert.Contains(typeof(string), sut.Keys);
        Assert.All(sut.Values, value => Assert.NotNull(value));
    }

    [Fact]
    public void GetEnumerator_ShouldIterateRegisteredConverters()
    {
        var sut = new ConvertibleConverterDictionary()
            .Add<int>(i => BitConverter.GetBytes(i))
            .Add(typeof(double), c => BitConverter.GetBytes((double)c));

        var items = sut.ToList();

        Assert.Equal(2, items.Count);
        Assert.Contains(items, item => item.Key == typeof(int));
        Assert.Contains(items, item => item.Key == typeof(double));
        Assert.All(items, item => Assert.NotNull(item.Value));
    }

    [Fact]
    public void IEnumerableGetEnumerator_ShouldIterateRegisteredConverters()
    {
        System.Collections.IEnumerable sut = new ConvertibleConverterDictionary()
            .Add<int>(i => BitConverter.GetBytes(i));

        var enumerator = sut.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        var item = Assert.IsType<System.Collections.Generic.KeyValuePair<Type, Func<IConvertible, byte[]>>>(enumerator.Current);
        Assert.Equal(typeof(int), item.Key);
        Assert.NotNull(item.Value);
        Assert.False(enumerator.MoveNext());
    }
}
