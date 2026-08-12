using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cuemon.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class StringExtensionsTest : Test
{
    public StringExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Difference_ShouldGetDifference()
    {
        var s1 = Alphanumeric.UppercaseLetters;
        var s2 = Alphanumeric.Letters;
        var s3 = s1.Difference(s2);

        Assert.Equal(Alphanumeric.LowercaseLetters, s3);
    }

    [Fact]
    public void ToCharArray_ShouldConvertStringToCharArray()
    {
        var s1 = Alphanumeric.LettersAndNumbers;
        var c1 = s1.ToCharArray();

        Assert.Equal(s1, string.Concat(c1));
    }

    [Fact]
    public void ToByteArray_ShouldConvertStringToByteArray()
    {
        var s1 = Alphanumeric.LettersAndNumbers;
        var b1 = s1.ToByteArray();

        Assert.Equal(s1, Convertible.ToString(b1));
    }

    [Fact]
    public void FromUrlEncodedBase64String_ShouldConvertUrlEncodedBase64StringToByteArray()
    {
        var s1 = "VGhpcyBpcyBhIHRlc3Qgd2l0aCBzcGVjaWFsIGNoYXJhY3RlcnMgISIjwqQlJi8oKUAhPQ";
        var b1 = s1.FromUrlEncodedBase64();
        var s2 = Convertible.ToString(b1);

        Assert.Equal("This is a test with special characters !\"#¤%&/()@!=", s2);
    }

    [Fact]
    public void FromBinaryDigits_ShouldConvertBinaryDigitsStringToByteArray()
    {
        var s1 = "01010100011010000110100101110011001000000110100101110011001000000110000100100000011101000110010101110011011101000010000001110111011010010111010001101000001000000111001101110000011001010110001101101001011000010110110000100000011000110110100001100001011100100110000101100011011101000110010101110010011100110010000000100001001000100010001111000010101001000010010100100110001011110010100000101001010000000010000100111101";
        var b1 = s1.FromBinaryDigits();
        var s2 = Convertible.ToString(b1);

        Assert.Equal("This is a test with special characters !\"#¤%&/()@!=", s2);
    }

    [Fact]
    public void FromBase64_ShouldConvertBase64StringToByteArray()
    {
        var s1 = "VGhpcyBpcyBhIHRlc3Qgd2l0aCBzcGVjaWFsIGNoYXJhY3RlcnMgISIjwqQlJi8oKUAhPQ==";
        var b1 = s1.FromBase64();
        var s2 = Convertible.ToString(b1);

        Assert.Equal("This is a test with special characters !\"#¤%&/()@!=", s2);
    }

    [Fact]
    public void ToCasing_ShouldConvertToDifferentCasingMethods()
    {
        var s1 = "Cuemon for .net";
        var s2 = s1.ToCasing();
        var s3 = s1.ToCasing(CasingMethod.LowerCase);
        var s4 = s1.ToCasing(CasingMethod.TitleCase);
        var s5 = s1.ToCasing(CasingMethod.UpperCase);

        Assert.Equal(s1, s2);
        Assert.Equal(s1.ToLowerInvariant(), s3);
        Assert.Equal(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s1), s4);
        Assert.Equal(s1.ToUpperInvariant(), s5);
    }

    [Fact]
    public void ToUri_ShouldConvertToUri()
    {
        var s1 = "https://www.cuemon.net/";
        var u1 = s1.ToUri();

        Assert.Equal(s1, u1.OriginalString);
    }

    [Fact]
    public void IsNullOrEmpty_ShouldGiveFalseOnAtLeastOneNullOrEmpty()
    {
        var l1 = Generate.RangeOf(5, i =>
        {
            if (i < 4) { return $"{i}"; }
            return null;
        });

        var l2 = Generate.RangeOf(5, i =>
        {
            if (i < 4) { return $"{i}"; }
            return "";
        });

        var l3 = Generate.RangeOf(5, i => $"{i}");

        Assert.True(l1.IsNullOrEmpty());
        Assert.True(l2.IsNullOrEmpty());
        Assert.False(l3.IsNullOrEmpty());
    }

    [Fact]
    public void Count_ShouldCountSpecifiedChar()
    {
        Assert.Equal(10, string.Concat(Alphanumeric.LettersAndNumbers, new string('a', 9)).Count('a'));
        Assert.Equal(10, string.Concat(Alphanumeric.LettersAndNumbers, new string('Z', 9)).Count('Z'));
    }

    [Fact]
    public void JsEscape_ShouldDoJavascriptEscape()
    {
        Assert.Equal("Need%20complemental%20framework%20for%20.NET%3F%20Use%20Cuemon%20for%20.NET%21", "Need complemental framework for .NET? Use Cuemon for .NET!".JsEscape());
    }

    [Fact]
    public void JsUnescape_ShouldDoJavascriptUnescape()
    {
        Assert.Equal("Need complemental framework for .NET? Use Cuemon for .NET!", "Need%20complemental%20framework%20for%20.NET%3F%20Use%20Cuemon%20for%20.NET%21".JsUnescape());
    }

    private static readonly string SentenceWithCuemonInIt = "Cuemon is following most of the Framework Design Guidelines.";
    private static readonly string SentenceWithForInIt = "Let's live for freedom!";
    private static readonly string SentenceWithDotNetInIt = "Microsoft .NET - one platform to rule them all.";

    [Fact]
    public void ContainsAny()
    {
        Assert.True("Cuemon for .NET".ContainsAny(SentenceWithCuemonInIt.Split(' ')));
        Assert.True("Cuemon for .NET".ContainsAny(SentenceWithForInIt.Split(' ')));
        Assert.True("Cuemon for .NET".ContainsAny(SentenceWithDotNetInIt.Split(' ')));
        Assert.False("Cuemon for .NET".ContainsAny("some", "random", "words"));
    }

    [Fact]
    public void ContainsAll()
    {
        Assert.True("Cuemon for .NET".ContainsAll("Cuemon", "for", ".NET"));
        Assert.False("Cuemon for .NET".ContainsAll("Cuemon", "for", "all"));
    }

    [Fact]
    public void EqualsAny()
    {
        Assert.True("Cuemon for .NET".EqualsAny("some sentence", "Cuemon for .NET"));
        Assert.False("Cuemon for .NET".EqualsAny(SentenceWithCuemonInIt.Split(' ')));
        Assert.False("Cuemon for .NET".EqualsAny(SentenceWithForInIt.Split(' ')));
        Assert.False("Cuemon for .NET".EqualsAny(SentenceWithDotNetInIt.Split(' ')));
        Assert.False("Cuemon for .NET".EqualsAny("some", "random", "words"));
    }

    [Fact]
    public void StartsWith()
    {
        Assert.True("Cuemon for .NET".StartsWith("some sentence", "Cuemon for .NET"));
        Assert.True("Cuemon for .NET".StartsWith(SentenceWithCuemonInIt.Split(' ')));
        Assert.False("Cuemon for .NET".StartsWith(SentenceWithForInIt.Split(' ')));
        Assert.False("Cuemon for .NET".StartsWith(SentenceWithDotNetInIt.Split(' ')));
        Assert.False("Cuemon for .NET".StartsWith("some", "random", "words"));
    }

    [Fact]
    public void ToGuid_ShouldConvertStringToGuid()
    {
        var g1 = Guid.NewGuid();
        var s1 = g1.ToString("N");
        var g2 = s1.ToGuid(o => o.Formats = GuidFormats.N);

        Assert.Equal(g1, g2);
    }

    [Fact]
    public void SplitDelimited_ShouldRevertDelimitedStringToSequence()
    {
        var i = Generate.RangeOf(10, i1 => i1.ToString());
        var s = "0,1,2,3,4,5,6,7,8,9".SplitDelimited().ToList();

        Assert.Equal(10, s.Count);
        Assert.True(s.SequenceEqual(i));
    }

    [Fact]
    public void IsNullOrEmpty_ShouldBeEqualToStringIsNullOrEmpty()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = (string)null;
        var sut3 = "";
        var sut4 = " ";

        Assert.False(sut1.IsNullOrEmpty());
        Assert.True(sut2.IsNullOrEmpty());
        Assert.True(sut3.IsNullOrEmpty());
        Assert.False(sut4.IsNullOrEmpty());

        Assert.Equal(sut1.IsNullOrEmpty(), string.IsNullOrEmpty(sut1));
        Assert.Equal(sut2.IsNullOrEmpty(), string.IsNullOrEmpty(sut2));
        Assert.Equal(sut3.IsNullOrEmpty(), string.IsNullOrEmpty(sut3));
        Assert.Equal(sut4.IsNullOrEmpty(), string.IsNullOrEmpty(sut4));
    }

    [Fact]
    public void IsNullOrEmpty_Sequence_ShouldBeEqualToStringIsNullOrEmpty()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = (string)null;
        var sut3 = "";
        var sut4 = " ";
        var sut5 = Arguments.ToEnumerableOf(sut1, sut2, sut3, sut4);
        var sut6 = Arguments.ToEnumerableOf(sut1, sut4);

        Assert.False(sut6.IsNullOrEmpty());
        Assert.True(sut5.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrWhiteSpace_ShouldBeEqualToStringIsNullOrWhiteSpace()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = (string)null;
        var sut3 = "";
        var sut4 = " ";

        Assert.False(sut1.IsNullOrWhiteSpace());
        Assert.True(sut2.IsNullOrWhiteSpace());
        Assert.True(sut3.IsNullOrWhiteSpace());
        Assert.True(sut4.IsNullOrWhiteSpace());

        Assert.Equal(sut1.IsNullOrWhiteSpace(), string.IsNullOrWhiteSpace(sut1));
        Assert.Equal(sut2.IsNullOrWhiteSpace(), string.IsNullOrWhiteSpace(sut2));
        Assert.Equal(sut3.IsNullOrWhiteSpace(), string.IsNullOrWhiteSpace(sut3));
        Assert.Equal(sut4.IsNullOrWhiteSpace(), string.IsNullOrWhiteSpace(sut4));
    }

    [Fact]
    public void IsEmailAddress_ShouldDetectEmailAddress()
    {
        var sut1 = "noreply@gmail.com";
        var sut2 = "noreply@gmail";
        var sut3 = "noreply@";
        var sut4 = "noreply";
        var sut5 = "noreply@gmail.";

        Assert.True(sut1.IsEmailAddress());
        Assert.False(sut2.IsEmailAddress());
        Assert.False(sut3.IsEmailAddress());
        Assert.False(sut4.IsEmailAddress());
        Assert.False(sut5.IsEmailAddress());
    }

    [Fact]
    public void IsGuid_ShouldDetectValidGuidFormat()
    {
        var sut1 = Guid.NewGuid().ToString("N");
        var sut2 = Guid.NewGuid().ToString("D");
        var sut3 = Guid.NewGuid().ToString("B");
        var sut4 = Guid.NewGuid().ToString("P");
        var sut5 = Guid.NewGuid().ToString("X");

        Assert.False(sut1.IsGuid());
        Assert.True(sut2.IsGuid());
        Assert.True(sut3.IsGuid());
        Assert.True(sut4.IsGuid());
        Assert.False(sut5.IsGuid());
        Assert.True(sut1.IsGuid(GuidFormats.Any));
        Assert.True(sut5.IsGuid(GuidFormats.Any));
    }

    [Fact]
    public void IsHex_ShouldDetectValidHexFormat()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToHexadecimal();

        Assert.False(sut1.IsHex());
        Assert.True(sut2.IsHex());
    }

    [Fact]
    public void IsNumeric_ShouldDetectValidNumber()
    {
        var sut1 = "1";
        var sut2 = "NaN";
        var sut3 = "Infinity";
        var sut4 = " 2 ";
        var sut5 = " 3";
        var sut6 = "4 ";
        var sut7 = "-5";
        var sut8 = "";
        var sut9 = "100.00";
        var sut10 = "1000,000.00";

        Assert.True(sut1.IsNumeric());
        Assert.False(sut2.IsNumeric());
        Assert.False(sut3.IsNumeric());
        Assert.True(sut4.IsNumeric());
        Assert.True(sut5.IsNumeric());
        Assert.True(sut6.IsNumeric());
        Assert.True(sut7.IsNumeric());
        Assert.False(sut8.IsNumeric());
        Assert.True(sut9.IsNumeric());
        Assert.True(sut10.IsNumeric());
    }

    [Fact]
    public void IsBase64_ShouldDetectValidBase64Format()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ToByteArray().ToBase64String();

        Assert.False(sut1.IsBase64());
        Assert.True(sut2.IsBase64());
    }

    [Fact]
    public void RemoveAll_ShouldRemoveSpecifiedOccurrences()
    {
        var sut1 = $"This IS A string THAT will be converted back and forth. Lets add some foreign characters: æøå and some letters and numbers as well: {Alphanumeric.LettersAndNumbers.ToUpperInvariant()}.";
        var sut2 = sut1.RemoveAll(" is", "a ", " that", Alphanumeric.LettersAndNumbers);
        var sut3 = sut1.RemoveAll(StringComparison.OrdinalIgnoreCase, " is", "a ", " that", Alphanumeric.LettersAndNumbers);

        Assert.Equal("This IS A string THAT will be converted back and forth. Lets add some foreign characters: æøå and some letters and numbers as well: ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.", sut2);
        Assert.Equal("This string will be converted back and forth. Lets add some foreign characters: æøå and some letters and numbers as well: .", sut3);
    }

    [Fact]
    public void RemoveAll_Char_ShouldRemoveSpecifiedOccurrences()
    {
        var sut1 = $"This IS A string THAT will be converted back and forth. Lets add some foreign characters: æøå and some letters and numbers as well: {Alphanumeric.LettersAndNumbers.ToUpperInvariant()}.";
        var sut2 = sut1.RemoveAll("is".ToCharArray().Concat("a".ToCharArray()).Concat("that".ToCharArray()).ToArray());

        Assert.Equal("T IS A rng THAT wll be convered bck nd for. Le dd ome foregn crcer: æøå nd ome leer nd number  well: ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.", sut2);
    }

    [Fact]
    public void RemoveAll_Array_ShouldRemoveSpecifiedOccurrences()
    {
        var sut1 = $"This IS A string THAT will be converted back and forth. Lets add some foreign characters: æøå and some letters and numbers as well: {Alphanumeric.LettersAndNumbers.ToUpperInvariant()}.".Split(' ');
        var sut2 = sut1.RemoveAll("is", "a", "that", Alphanumeric.LettersAndNumbers);
        var sut3 = sut1.RemoveAll(StringComparison.OrdinalIgnoreCase, "is", "a", "that", Alphanumeric.LettersAndNumbers);

        Assert.Equal("Th IS A string THAT will be converted bck nd forth. Lets dd some foreign chrcters: æøå nd some letters nd numbers s well: ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.".Split(' '), sut2);
        Assert.Equal("Th   string THT will be converted bck nd forth. Lets dd some foreign chrcters: æøå nd some letters nd numbers s well: BCDEFGHIJKLMNOPQRSTUVWXYZBCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.".Split(' '), sut3);
    }

    [Fact]
    public void ReplaceAll_ShouldReplaceAllSpecifiedOccurrences()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ReplaceAll("is a string that will", "string will");

        Assert.Equal("This string will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: !@#$%^&*()_-+=[{]};:<>|.,/?`~\\\"'.", sut2);
    }

    [Fact]
    public void JsEscape_ShouldEscapeLikeJavascript()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.JsEscape();
        var sut3 = sut2.JsUnescape();

        TestOutput.WriteLine(sut2);

        Assert.Equal("This%20is%20a%20string%20that%20will%20be%20converted%20back%20and%20forth.%20Lets%20add%20some%20foreign%20characters%3A%20%E6%F8%E5%20and%20some%20punctuations%20as%20well%3A%20%21@%23%24%25^%26*%28%29_-+%3D[{]}%3B%3A%3C%3E|.,/%3F`~%5C%22%27.", sut2);
        Assert.Equal(sut1, sut3);
    }

    [Fact]
    public void ContainsAny_ShouldFindAtLeastOnePartialMatch()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ContainsAny(StringComparison.OrdinalIgnoreCase, "ma", "th", "fort", "be", "wit", "yo");

        Assert.True(sut2);
    }

    [Fact]
    public void ContainsAny_ShouldFindAtLeastOneMatchFromString()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ContainsAny("back and forth");
        var sut3 = sut1.ContainsAny("forth and back");

        Assert.True(sut2);
        Assert.False(sut3);
    }

    [Fact]
    public void ContainsAny_Char_ShouldFindAtLeastOneMatch()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ContainsAny(StringComparison.OrdinalIgnoreCase, 'z', 'æ');

        Assert.True(sut2);
    }

    [Theory]
    [InlineData(null)]
    public void ContainsAny_ShouldThrowArgumentNullException(string value)
    {
        var ex = Assert.Throws<ArgumentNullException>(() => value.ContainsAny('a'));
        Assert.Equal(nameof(value), ex.ParamName);

        ex = Assert.Throws<ArgumentNullException>(() => value.ContainsAny(StringComparison.OrdinalIgnoreCase, 'a'));
        Assert.Equal(nameof(value), ex.ParamName);
    }

    [Fact]
    public void ContainsAny_Char_ShouldFindOneMatch()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ContainsAny('ø');
        var sut3 = sut1.ContainsAny('Ø');

        Assert.True(sut2);
        Assert.True(sut3);
    }

    [Fact]
    public void ContainsAny_Char_ShouldFindAtLeastOneMatch_Default()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ContainsAny('ø', 'z', 'x');
        var sut3 = sut1.ContainsAny('Ø', 'Z', 'X');

        Assert.True(sut2);
        Assert.False(sut3);
    }

    [Fact]
    public void ContainsAll_Default_ShouldMatchAllOrNone()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ContainsAll("may", "the", "forth", "be", "with", "you");
        var sut3 = sut1.ContainsAll("This", "is", "forth", "be", "converted", "punctuations");
        var sut4 = sut1.ContainsAll("THIS", "IS", "CONVERTED");

        Assert.False(sut2);
        Assert.True(sut3);
        Assert.True(sut4);
    }

    [Fact]
    public void ContainsAll_ShouldMatchAllOrNone()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.ContainsAll(StringComparison.Ordinal, "may", "the", "forth", "be", "with", "you");
        var sut3 = sut1.ContainsAll(StringComparison.Ordinal, "This", "is", "forth", "be", "converted", "punctuations");
        var sut4 = sut1.ContainsAll(StringComparison.Ordinal, "THIS", "IS", "CONVERTED");

        Assert.False(sut2);
        Assert.True(sut3);
        Assert.False(sut4);
    }

    [Fact]
    public void EqualsAny_ShouldFindAtLeastOneMatch()
    {
        var sut1 = "This is a string.";
        var sut2 = sut1.EqualsAny(StringComparison.OrdinalIgnoreCase, "This is a string.");
        var sut3 = sut1.EqualsAny(StringComparison.OrdinalIgnoreCase, "THIS IS A STRING.");
        var sut4 = sut1.EqualsAny(StringComparison.OrdinalIgnoreCase, "THIS");

        Assert.True(sut2);
        Assert.True(sut3);
        Assert.False(sut4);
    }

    [Fact]
    public void EqualsAny_Default_ShouldFindAtLeastOneMatch()
    {
        var sut1 = "This is a string.";
        var sut2 = sut1.EqualsAny("This is a string.");
        var sut3 = sut1.EqualsAny("THIS IS A STRING.");
        var sut4 = sut1.EqualsAny("THIS");

        Assert.True(sut2);
        Assert.False(sut3);
        Assert.False(sut4);
    }

    [Fact]
    public void StarsWith_Default_SequenceShouldStartsWith()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = new List<string>()
        {
            "and this is also a string",
            "and so is this",
            "only one match should be obtained",
            "THIS IS A STRING"
        };
        var sut3 = sut1.StartsWith(sut2);

        Assert.True(sut3);
    }

    [Fact]
    public void StarsWith_SequenceShouldStartsWith()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = new List<string>()
        {
            "and this is also a string",
            "and so is this",
            "only one match should be obtained",
            "THIS IS A STRING"
        };
        var sut3 = sut1.StartsWith(StringComparison.Ordinal, sut2);

        Assert.False(sut3);
    }

    [Fact]
    public void StarsWith_Default_Params_SequenceShouldStartsWith()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.StartsWith("and this is also a string", "and so is this", "only one match should be obtained", "THIS IS A STRING");

        Assert.True(sut2);
    }

    [Fact]
    public void StarsWith_Params_SequenceShouldStartsWith()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.StartsWith(StringComparison.Ordinal, "and this is also a string", "and so is this", "only one match should be obtained", "THIS IS A STRING");

        Assert.False(sut2);
    }

    [Fact]
    public void TrimAll_ShouldTrimAllWhitespaceOccurrences()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some whitespace as well: {Alphanumeric.WhiteSpace}.";
        var sut2 = sut1.TrimAll();
        var sut3 = sut1.TrimAll('t');

        Assert.Equal("Thisisastringthatwillbeconvertedbackandforth.Letsaddsomeforeigncharacters:æøåandsomewhitespaceaswell:.", sut2);
        Assert.Equal($"This is a sring ha will be convered back and forh. Les add some foreign characers: æøå and some whiespace as well: {Alphanumeric.WhiteSpace}.", sut3);
    }

    [Fact]
    public void IsSequenceOf_ShouldConvertSomeStringToSpecificType()
    {
        var sut1 = Generate.RangeOf(10, _ => Guid.NewGuid().ToString("D"));
        var sut2 = Generate.RangeOf(10, i => Generate.RandomNumber(i * 255, int.MaxValue).ToString());

        Assert.True(sut1.IsSequenceOf<Guid>());
        Assert.True(sut1.IsSequenceOf<string>());
        Assert.False(sut1.IsSequenceOf<long>());

        Assert.False(sut2.IsSequenceOf<Guid>());
        Assert.False(sut2.IsSequenceOf<byte>());
        Assert.True(sut2.IsSequenceOf<long>());
        Assert.True(sut2.IsSequenceOf<int>());
        Assert.True(sut2.IsSequenceOf<uint>());
        Assert.True(sut2.IsSequenceOf<ulong>());
        Assert.True(sut2.IsSequenceOf<string>());
    }

    [Fact]
    public void ToEnum_ShouldConvertStringToEnum()
    {
        var sut1 = AssignmentOperator.Addition.ToString();
        var sut2 = AssignmentOperator.Assign.ToString();
        var sut3 = AssignmentOperator.Multiplication.ToString();
        var sut4 = AssignmentOperator.Subtraction.ToString();
        var sut5 = "NotAnEnum";

        Assert.Equal(AssignmentOperator.Addition, sut1.ToEnum<AssignmentOperator>());
        Assert.Equal(AssignmentOperator.Assign, sut2.ToEnum<AssignmentOperator>());
        Assert.Equal(AssignmentOperator.Multiplication, sut3.ToEnum<AssignmentOperator>());
        Assert.Equal(AssignmentOperator.Subtraction, sut4.ToEnum<AssignmentOperator>());

        Assert.Throws<ArgumentNullException>(() => ((string)null).ToEnum<AssignmentOperator>());
        Assert.Throws<TypeArgumentException>(() => sut4.ToEnum<DateTime>());
        Assert.Throws<ArgumentException>(() => sut5.ToEnum<AssignmentOperator>());
    }

    [Fact]
    public void ToTimeSpan_ShouldConvertStringToTimeSpan()
    {
        var sut1 = TimeSpan.MaxValue;
        var sut2 = TimeSpan.MinValue;
        var sut3 = 1617738277d.ToTimeSpan(TimeUnit.Seconds);
        var sut4 = 864000000000d.ToTimeSpan(TimeUnit.Ticks);
        var sut5 = 1d.ToTimeSpan(TimeUnit.Days);
        var sut6 = sut1.Ticks.ToString().ToTimeSpan(TimeUnit.Ticks);
        var sut7 = sut2.Ticks.ToString().ToTimeSpan(TimeUnit.Ticks);
        var sut8 = sut3.TotalDays.ToString(CultureInfo.InvariantCulture).ToTimeSpan(TimeUnit.Days);
        var sut9 = sut4.TotalHours.ToString(CultureInfo.InvariantCulture).ToTimeSpan(TimeUnit.Hours);
        var sut10 = sut5.TotalMinutes.ToString(CultureInfo.InvariantCulture).ToTimeSpan(TimeUnit.Minutes);
        var sut11 = sut5.TotalMilliseconds.ToString(CultureInfo.InvariantCulture).ToTimeSpan(TimeUnit.Milliseconds);
        var sut12 = sut5.TotalSeconds.ToString(CultureInfo.InvariantCulture).ToTimeSpan(TimeUnit.Seconds);

        Assert.Equal(sut1, sut6);
        Assert.Equal(sut2, sut7);
        Assert.Equal(sut3, sut8);
        Assert.Equal(sut4, sut9);
        Assert.Equal(sut5, sut10);
        Assert.Equal(sut5, sut11);
        Assert.Equal(sut5, sut12);
    }

    [Fact]
    public void SubstringBefore_ShouldExtractValueBeforeMatch()
    {
        var sut1 = $"This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some punctuations as well: {Alphanumeric.PunctuationMarks}.";
        var sut2 = sut1.SubstringBefore(" converted");
        var sut3 = sut1.SubstringBefore(" punctuations");

        Assert.Equal("This is a string that will be", sut2);
        Assert.Equal("This is a string that will be converted back and forth. Lets add some foreign characters: æøå and some", sut3);
    }

    [Fact]
    public void Chunk_ShouldMakeLongStringIntoSequenceOfSmallStrings()
    {
        var sut1 = Generate.RandomString(2048);
        var sut2 = sut1.Chunk();
        var sut3 = sut1.Chunk(8);

        TestOutput.WriteLine(sut3.First());

        Assert.Equal(2, sut2.Count());
        Assert.Equal(256, sut3.Count());
        Assert.All(sut2, s => Assert.Equal(s.Length, 1024));
        Assert.All(sut3, s => Assert.Equal(s.Length, 8));
    }

    [Fact]
    public void SuffixWith_ShouldSuffixWithValue()
    {
        var sut1 = "Unit";
        var sut2 = sut1.SuffixWith("Test");
        var sut3 = sut2.SuffixWithForwardingSlash();
        var sut4 = sut3.SuffixWithForwardingSlash();

        Assert.Equal("Unit", sut1);
        Assert.Equal("UnitTest", sut2);
        Assert.Equal("UnitTest/", sut3);
        Assert.Equal(sut3, sut4);
    }

    [Fact]
    public void PrefixWith_ShouldPrefixWithValue()
    {
        var sut1 = "Test";
        var sut2 = sut1.PrefixWith("Unit");
        var sut3 = sut2.PrefixWith("\\");
        var sut4 = sut3.PrefixWith("\\");

        Assert.Equal("Test", sut1);
        Assert.Equal("UnitTest", sut2);
        Assert.Equal("\\UnitTest", sut3);
        Assert.Equal(sut3, sut4);
    }
}
