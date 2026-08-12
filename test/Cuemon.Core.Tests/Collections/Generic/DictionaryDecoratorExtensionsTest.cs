using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cuemon.Collections.Generic;
public class DictionaryDecoratorExtensionsTest
{
    [Fact]
    public void Extend_Dictionary_With_GetValueOrDefault_Expect_Actual_Value_At_Key_42()
    {
        var dic = Generate.RangeOf(500, x => new KeyValuePair<int, string>(x, Generate.RandomString(24))).ToDictionary(x => x.Key, x => x.Value);
        var result = Decorator.Enclose(dic).GetValueOrDefault(42);
        Assert.Equal(dic[42], result);
    }

    [Fact]
    public void Extend_Dictionary_With_GetValueOrDefault_Expect_Default_Provided_Value()
    {
        var dic = Generate.RangeOf(500, x => new KeyValuePair<int, string>(x, Generate.RandomString(24))).ToDictionary(x => x.Key, x => x.Value);
        var expected = Generate.RandomString(24);
        var result = Decorator.Enclose(dic).GetValueOrDefault(700, () => expected);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Extend_Dictionary_With_TryGetValueOrFallback_Expect_Actual_Value_At_Key_42()
    {
        var dic = Generate.RangeOf(500, x => new KeyValuePair<int, string>(x, Generate.RandomString(24))).ToDictionary(x => x.Key, x => x.Value);
        var found = Decorator.Enclose(dic).TryGetValueOrFallback(42, keys => keys.Max(), out var result);
        Assert.True(found);
        Assert.Equal(dic[42], result);
    }

    [Fact]
    public void Extend_Dictionary_With_TryGetValueOrFallback_Expect_Fallback_Value_At_Key_42()
    {
        var dic = Generate.RangeOf(500, x => new KeyValuePair<int, string>(x, Generate.RandomString(24))).ToDictionary(x => x.Key, x => x.Value);
        var found = Decorator.Enclose(dic).TryGetValueOrFallback(-1, keys => 42, out var result);
        Assert.True(found);
        Assert.Equal(dic[42], result);
    }

    [Fact]
    public void Extend_Dictionary_With_TryGetValueOrFallback_Expect_Not_Found()
    {
        var dic = Generate.RangeOf(500, x => new KeyValuePair<int, string>(x, Generate.RandomString(24))).ToDictionary(x => x.Key, x => x.Value);
        var found = Decorator.Enclose(dic).TryGetValueOrFallback(-1, keys => -42, out var result);
        Assert.False(found);
        Assert.Equal(default, result);
    }

    [Fact]
    public void Extend_Dictionary_With_ToEnumerable_Expect_Sequence()
    {
        var dic = Generate.RangeOf(500, x => new KeyValuePair<int, string>(x, Generate.RandomString(24))).ToDictionary(x => x.Key, x => x.Value);
        var result = Decorator.Enclose(dic).ToEnumerable();
        Assert.Equal(dic, result);
        Assert.IsAssignableFrom<IEnumerable<KeyValuePair<int, string>>>(result);
    }

    [Fact]
    public void Extend_Dictionary_With_TryAdd_Expect_True()
    {
        var dic = Generate.RangeOf(500, x => new KeyValuePair<int, string>(x, Generate.RandomString(24))).ToDictionary(x => x.Key, x => x.Value);
        Assert.Equal(500, dic.Count);
        var added = Decorator.Enclose(dic).TryAdd(501, "First Legion");
        Assert.True(added);
        Assert.Equal(501, dic.Count);
    }

    [Fact]
    public void Extend_Dictionary_With_TryAdd_Expect_False()
    {
        var dic = Generate.RangeOf(500, x => new KeyValuePair<int, string>(x, Generate.RandomString(24))).ToDictionary(x => x.Key, x => x.Value);
        var key = dic.Keys.Last();
        Assert.Equal(500, dic.Count);
        var added = Decorator.Enclose(dic).TryAdd(key, "First Legion");
        Assert.False(added);
        Assert.Equal(500, dic.Count);
    }

    [Fact]
    public void Extend_Dictionary_With_TryAddOrUpdate_Add_And_Update_Expect_True()
    {
        var dic = Generate.RangeOf(500, x => new KeyValuePair<int, string>(x, Generate.RandomString(24))).ToDictionary(x => x.Key, x => x.Value);
        var key = 501;
        var txt = "First Legion";
        var subtxt = " Rules the Galaxy";
        Assert.Equal(500, dic.Count);
        Decorator.Enclose(dic).AddOrUpdate(key, txt);
        Assert.Equal(txt, dic[key]);
        Decorator.Enclose(dic).AddOrUpdate(key, txt + subtxt);
        Assert.Equal(string.Concat(txt, subtxt), dic[key]);
    }

    [Fact]
    public void Extend_Dictionary_With_TryAddOrUpdate_Update_Expect_True()
    {
        var dic = Generate.RangeOf(500, x => new KeyValuePair<int, string>(x, Generate.RandomString(24))).ToDictionary(x => x.Key, x => x.Value);
        var elm = dic.Last();
        var txt = "First Legion";
        Assert.Equal(500, dic.Count);
        Assert.Equal(elm.Value, dic.Last().Value);
        Decorator.Enclose(dic).AddOrUpdate(elm.Key, txt);
        Assert.Equal(txt, dic.Last().Value);
        Assert.Equal(500, dic.Count);
    }
}
