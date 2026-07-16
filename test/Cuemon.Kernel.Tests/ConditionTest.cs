using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
    public class ConditionTest : Test
    {
        public ConditionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AreEqualAndReferenceComparisons_ShouldEvaluateExpectedResults()
        {
            var sameReference = new object();
            var differentReference = new object();

            Assert.True(Condition.AreEqual(42, 42));
            Assert.False(Condition.AreEqual("alpha", "ALPHA"));
            Assert.True(Condition.AreEqual("alpha", "ALPHA", StringComparer.OrdinalIgnoreCase));
            Assert.True(Condition.AreNotEqual("alpha", "ALPHA"));
            Assert.False(Condition.AreNotEqual("alpha", "ALPHA", StringComparer.OrdinalIgnoreCase));
            Assert.True(Condition.AreSame(sameReference, sameReference));
            Assert.False(Condition.AreSame(sameReference, differentReference));
            Assert.True(Condition.AreNotSame(sameReference, differentReference));
            Assert.False(Condition.AreNotSame(sameReference, sameReference));

            Assert.Throws<ArgumentNullException>(() => Condition.AreEqual("alpha", "ALPHA", null));
            Assert.Throws<ArgumentNullException>(() => Condition.AreNotEqual("alpha", "ALPHA", null));
        }

        [Fact]
        public void FlipFlop_ShouldInvokeExpectedBranch_ForAllActionOverloads()
        {
            var calls = new List<string>();

            Condition.FlipFlop(true, () => calls.Add("0T"), () => calls.Add("0F"));
            Condition.FlipFlop(false, () => calls.Add("0T"), () => calls.Add("0F"));

            Condition.FlipFlop(true, x => calls.Add($"1T:{x}"), x => calls.Add($"1F:{x}"), 1);
            Condition.FlipFlop(false, x => calls.Add($"1T:{x}"), x => calls.Add($"1F:{x}"), 2);

            Condition.FlipFlop(true, (x, y) => calls.Add($"2T:{x + y}"), (x, y) => calls.Add($"2F:{x + y}"), 1, 2);
            Condition.FlipFlop(false, (x, y) => calls.Add($"2T:{x + y}"), (x, y) => calls.Add($"2F:{x + y}"), 2, 3);

            Condition.FlipFlop(true, (x, y, z) => calls.Add($"3T:{x + y + z}"), (x, y, z) => calls.Add($"3F:{x + y + z}"), 1, 2, 3);
            Condition.FlipFlop(false, (x, y, z) => calls.Add($"3T:{x + y + z}"), (x, y, z) => calls.Add($"3F:{x + y + z}"), 2, 3, 4);

            Condition.FlipFlop(true, (a, b, c, d) => calls.Add($"4T:{a + b + c + d}"), (a, b, c, d) => calls.Add($"4F:{a + b + c + d}"), 1, 2, 3, 4);
            Condition.FlipFlop(false, (a, b, c, d) => calls.Add($"4T:{a + b + c + d}"), (a, b, c, d) => calls.Add($"4F:{a + b + c + d}"), 2, 3, 4, 5);

            Condition.FlipFlop(true, (a, b, c, d, e) => calls.Add($"5T:{a + b + c + d + e}"), (a, b, c, d, e) => calls.Add($"5F:{a + b + c + d + e}"), 1, 2, 3, 4, 5);
            Condition.FlipFlop(false, (a, b, c, d, e) => calls.Add($"5T:{a + b + c + d + e}"), (a, b, c, d, e) => calls.Add($"5F:{a + b + c + d + e}"), 2, 3, 4, 5, 6);

            Assert.Equal(new[]
            {
                "0T",
                "0F",
                "1T:1",
                "1F:2",
                "2T:3",
                "2F:5",
                "3T:6",
                "3F:9",
                "4T:10",
                "4F:14",
                "5T:15",
                "5F:20"
            }, calls);

            Assert.Throws<ArgumentNullException>(() => Condition.FlipFlop(true, (Action)null, () => { }));
            Assert.Throws<ArgumentNullException>(() => Condition.FlipFlop(true, () => { }, (Action)null));
        }

        [Fact]
        public async Task FlipFlopAsync_ShouldInvokeExpectedBranch()
        {
            var calls = new List<string>();

            await Condition.FlipFlopAsync(true,
                () =>
                {
                    calls.Add("async:true");
                    return Task.CompletedTask;
                },
                () =>
                {
                    calls.Add("async:false");
                    return Task.CompletedTask;
                });

            await Condition.FlipFlopAsync(false,
                () =>
                {
                    calls.Add("async:true");
                    return Task.CompletedTask;
                },
                () =>
                {
                    calls.Add("async:false");
                    return Task.CompletedTask;
                });

            Assert.Equal(new[] { "async:true", "async:false" }, calls);
            await Assert.ThrowsAsync<ArgumentNullException>(() => Condition.FlipFlopAsync(true, null, () => Task.CompletedTask));
            await Assert.ThrowsAsync<ArgumentNullException>(() => Condition.FlipFlopAsync(false, () => Task.CompletedTask, null));
        }

        [Fact]
        public void HasConsecutiveCharacters_ShouldDetectSequences_ForBothOverloads()
        {
            Assert.True(Condition.HasConsecutiveCharacters("bookkeeper", new[] { 'o', 'k' }));
            Assert.True(Condition.HasConsecutiveCharacters("bookkeeper", new[] { 'x', 'k' }));
            Assert.False(Condition.HasConsecutiveCharacters("bookkeeper", new[] { 'x', 'y' }));
            Assert.False(Condition.HasConsecutiveCharacters("a", new[] { 'a' }));
            Assert.False(Condition.HasConsecutiveCharacters(" ", new[] { ' ' }));
            Assert.False(Condition.HasConsecutiveCharacters(null, new[] { 'a' }));
            Assert.False(Condition.HasConsecutiveCharacters("bookkeeper", (IEnumerable<char>)null));

            Assert.True(Condition.HasConsecutiveCharacters("baaad", 'a', 3));
            Assert.True(Condition.HasConsecutiveCharacters("committee", 'm'));
            Assert.True(Condition.HasConsecutiveCharacters("bookkeeper", 'o', 1));
            Assert.False(Condition.HasConsecutiveCharacters("abcdef", 'a'));
            Assert.False(Condition.HasConsecutiveCharacters("a", 'a'));
            Assert.False(Condition.HasConsecutiveCharacters(null, 'a'));
        }

        [Fact]
        public void EncodingAndDigitChecks_ShouldReturnExpectedResults()
        {
            Assert.True(Condition.IsBase64("QQ=="));
            Assert.False(Condition.IsBase64(null));
            Assert.False(Condition.IsBase64(string.Empty));

            Assert.True(Condition.IsBinaryDigits("101010"));
            Assert.False(Condition.IsBinaryDigits("102010"));
            Assert.False(Condition.IsBinaryDigits(" "));

            Assert.True(Condition.IsHex("0AFF"));
            Assert.True(Condition.IsHex("0aff"));
            Assert.False(Condition.IsHex("0AF"));
            Assert.False(Condition.IsHex("0AGG"));
            Assert.False(Condition.IsHex(string.Empty));
            Assert.True(Condition.IsHex('5'));
            Assert.True(Condition.IsHex('F'));
            Assert.True(Condition.IsHex('a'));
            Assert.False(Condition.IsHex('G'));
            Assert.False(Condition.IsHex('/'));
        }

        [Fact]
        public void IsCountableSequence_ShouldEvaluateIntegralAndCharacterSequences()
        {
            Assert.True(Condition.IsCountableSequence(new[] { 1, 3, 5, 7 }));
            Assert.False(Condition.IsCountableSequence(new[] { 1, 3, 6, 7 }));

            Assert.True(Condition.IsCountableSequence(new long[] { 10, 7, 4, 1 }));
            Assert.True(Condition.IsCountableSequence(new long[] { 10, 7 }));
            Assert.False(Condition.IsCountableSequence(new long[] { 10, 7, 3, 1 }));
            Assert.False(Condition.IsCountableSequence((IEnumerable<long>)null));

            Assert.True(Condition.IsCountableSequence("abcd"));
            Assert.True(Condition.IsCountableSequence("dcba"));
            Assert.False(Condition.IsCountableSequence("abda"));
            Assert.False(Condition.IsCountableSequence("a"));
            Assert.False(Condition.IsCountableSequence(string.Empty));
            Assert.False(Condition.IsCountableSequence((string)null));
        }

        [Fact]
        public void DefaultNullAndStringChecks_ShouldReturnExpectedResults()
        {
            Assert.True(Condition.IsDefault(0));
            Assert.False(Condition.IsDefault(1));
            Assert.True(Condition.IsNotDefault(1));
            Assert.False(Condition.IsNotDefault(0));

            Assert.True(Condition.IsEmpty(string.Empty));
            Assert.False(Condition.IsEmpty(null));
            Assert.False(Condition.IsEmpty(" "));

            Assert.True(Condition.IsNull<string>(null));
            Assert.False(Condition.IsNull("value"));
            Assert.True(Condition.IsNotNull("value"));
            Assert.False(Condition.IsNotNull<string>(null));

            Assert.True(Condition.IsWhiteSpace(" \t"));
            Assert.False(Condition.IsWhiteSpace(" value "));
            Assert.False(Condition.IsWhiteSpace(null));
        }

        [Fact]
        public void AddressEnumGuidAndUriChecks_ShouldReturnExpectedResults()
        {
            var guid = Guid.NewGuid();

            Assert.True(Condition.IsEmailAddress("user@example.com"));
            Assert.False(Condition.IsEmailAddress("invalid-email"));
            Assert.False(Condition.IsEmailAddress(" "));

            Assert.True(Condition.IsEnum<AttributeTargets>("assembly"));
            Assert.False(Condition.IsEnum<AttributeTargets>("assembly", o => o.IgnoreCase = false));
            Assert.False(Condition.IsEnum<AttributeTargets>("invalid"));
            Assert.False(Condition.IsEnum<AttributeTargets>(" "));
            Assert.False(Condition.IsEnum<int>("1"));

            Assert.True(Condition.IsGuid(guid.ToString("D")));
            Assert.False(Condition.IsGuid(guid.ToString("N")));
            Assert.True(Condition.IsGuid(guid.ToString("N"), GuidFormats.N));
            Assert.True(Condition.IsGuid(guid.ToString("B"), GuidFormats.B));
            Assert.True(Condition.IsGuid(guid.ToString("P"), GuidFormats.P));
            Assert.True(Condition.IsGuid(guid.ToString("X"), GuidFormats.X));
            Assert.False(Condition.IsGuid("invalid", GuidFormats.Any));
            Assert.False(Condition.IsGuid(" ", GuidFormats.Any));

            Assert.True(Condition.IsProtocolRelativeUrl("//www.cuemon.net/about"));
            Assert.True(Condition.IsProtocolRelativeUrl("~www.cuemon.net/about", o =>
            {
                o.RelativeReference = "~";
                o.Protocol = UriScheme.Http;
            }));
            Assert.False(Condition.IsProtocolRelativeUrl("https://www.cuemon.net/about"));

            Assert.True(Condition.IsUri("https://www.cuemon.net/"));
            Assert.True(Condition.IsUri("/about", o =>
            {
                o.Kind = UriKind.Relative;
                o.Schemes.Clear();
            }));
            Assert.False(Condition.IsUri("not a valid uri"));
        }

        [Fact]
        public void IsEnum_ShouldPreserveParsingSemantics()
        {
            // named values
            Assert.True(Condition.IsEnum<DayOfWeek>("Monday"));
            Assert.False(Condition.IsEnum<DayOfWeek>("NotADay"));

            // case-insensitive names (default) vs case-sensitive
            Assert.True(Condition.IsEnum<DayOfWeek>("monday"));
            Assert.False(Condition.IsEnum<DayOfWeek>("monday", o => o.IgnoreCase = false));
            Assert.True(Condition.IsEnum<DayOfWeek>("Monday", o => o.IgnoreCase = false));

            // numeric values: defined vs undefined
            Assert.True(Condition.IsEnum<DayOfWeek>("1"));
            Assert.False(Condition.IsEnum<DayOfWeek>("42"));

            // flags and combined flags
            Assert.True(Condition.IsEnum<AttributeTargets>("Assembly"));
            Assert.True(Condition.IsEnum<AttributeTargets>("Assembly, Module"));
            Assert.True(Condition.IsEnum<AttributeTargets>("All"));

            // non-enum generic argument
            Assert.False(Condition.IsEnum<int>("1"));

            // null, empty, and whitespace
            Assert.False(Condition.IsEnum<DayOfWeek>(null));
            Assert.False(Condition.IsEnum<DayOfWeek>(string.Empty));
            Assert.False(Condition.IsEnum<DayOfWeek>(" "));
        }

        [Fact]
        public void NumericAndRangeChecks_ShouldReturnExpectedResults()
        {
            Assert.True(Condition.IsEven(4));
            Assert.False(Condition.IsEven(3));
            Assert.True(Condition.IsOdd(3));
            Assert.False(Condition.IsOdd(4));

            Assert.True(Condition.IsGreaterThan(5, 4));
            Assert.False(Condition.IsGreaterThan(4, 5));
            Assert.True(Condition.IsGreaterThanOrEqual(5, 5));
            Assert.True(Condition.IsGreaterThanOrEqual(6, 5));

            Assert.True(Condition.IsLowerThan(4, 5));
            Assert.False(Condition.IsLowerThan(5, 4));
            Assert.True(Condition.IsLowerThanOrEqual(5, 5));
            Assert.True(Condition.IsLowerThanOrEqual(4, 5));

            Assert.True(Condition.IsWithinRange(5, 1, 10));
            Assert.False(Condition.IsWithinRange(11, 1, 10));
            Assert.True(Condition.IsNotWithinRange(11, 1, 10));
            Assert.False(Condition.IsNotWithinRange(5, 1, 10));

            Assert.True(Condition.IsNumeric("1,23", NumberStyles.Number, new CultureInfo("da-DK")));
            Assert.True(Condition.IsNumeric("123.45"));
            Assert.False(Condition.IsNumeric("NaN"));
            Assert.False(Condition.IsNumeric("nan"));
            Assert.False(Condition.IsNumeric("Infinity"));
            Assert.False(Condition.IsNumeric(" "));
            Assert.False(Condition.IsNumeric("abc"));
        }

        [Fact]
        public void BooleanAndPrimeChecks_ShouldReturnExpectedResults()
        {
            var trueCalls = 0;
            var falseCalls = 0;

            Assert.True(Condition.IsTrue(true));
            Assert.False(Condition.IsTrue(false));
            Condition.IsTrue(true, () => trueCalls++);
            Condition.IsTrue(false, () => trueCalls++);
            Assert.Equal(1, trueCalls);

            Assert.True(Condition.IsFalse(false));
            Assert.False(Condition.IsFalse(true));
            Condition.IsFalse(false, () => falseCalls++);
            Condition.IsFalse(true, () => falseCalls++);
            Assert.Equal(1, falseCalls);

            Assert.Throws<ArgumentNullException>(() => Condition.IsTrue(true, null));
            Assert.Throws<ArgumentNullException>(() => Condition.IsFalse(false, null));

            Assert.True(Condition.IsPrime(2));
            Assert.True(Condition.IsPrime(13));
            Assert.False(Condition.IsPrime(1));
            Assert.False(Condition.IsPrime(9));
            Assert.Throws<ArgumentException>(() => Condition.IsPrime(-1));
        }

        [Fact]
        public void TernaryIf_ShouldReturnExpectedResult_ForAllOverloads()
        {
            Assert.Equal("first", Condition.TernaryIf(true, () => "first", () => "second"));
            Assert.Equal("second", Condition.TernaryIf(false, () => "first", () => "second"));
            Assert.Equal("value:10", Condition.TernaryIf(true, x => $"value:{x}", x => $"fallback:{x}", 10));
            Assert.Equal("sum:3", Condition.TernaryIf(true, (x, y) => $"sum:{x + y}", (x, y) => $"diff:{x - y}", 1, 2));
            Assert.Equal("mul:24", Condition.TernaryIf(true, (a, b, c) => $"mul:{a * b * c}", (a, b, c) => $"sum:{a + b + c}", 2, 3, 4));
            Assert.Equal("sum:10", Condition.TernaryIf(true, (a, b, c, d) => $"sum:{a + b + c + d}", (a, b, c, d) => $"sum:{a - b - c - d}", 1, 2, 3, 4));
            Assert.Equal("sum:15", Condition.TernaryIf(true, (a, b, c, d, e) => $"sum:{a + b + c + d + e}", (a, b, c, d, e) => $"sum:{a - b - c - d - e}", 1, 2, 3, 4, 5));

            Assert.Throws<ArgumentNullException>(() => Condition.TernaryIf(true, (Func<string>)null, () => "second"));
        }

        [Fact]
        public void HasDifference_ShouldProvideDifferenceBetweenFirstAndSecond()
        {
            var sut1 = "Cuemon for .NET";
            var sut2 = "There once was a library named Cuemon for .NET; it is getting better by the day!";
            var sut3 = "XYZ Cuemon for .NET ÆØÅ";
            var sut5 = Condition.HasDifference(sut1, sut2, out var sut4);
            var sut6 = Condition.HasDifference(sut1, sut1, out _);
            var sut8 = Condition.HasDifference(sut1, sut3, out var sut7);

            TestOutput.WriteLine(sut4);
            TestOutput.WriteLine(sut7);

            Assert.Equal("hcwaslibyd;tg!", sut4);
            Assert.True(sut5);
            Assert.False(sut6);
            Assert.Equal("XYZÆØÅ", sut7);
            Assert.True(sut8);
        }

        [Fact]
        public void HasDifference_ShouldTreatNullAsEmptyString()
        {
            Assert.False(Condition.HasDifference(null, null, out var bothNull));
            Assert.Equal(string.Empty, bothNull);

            Assert.False(Condition.HasDifference("abc", null, out var secondNull));
            Assert.Equal(string.Empty, secondNull);

            Assert.True(Condition.HasDifference(null, "abc", out var firstNull));
            Assert.Equal("abc", firstNull);
        }

        [Fact]
        public void HasDifference_ShouldEmitEachDifferenceCharacterOnceInOrderOfSecond()
        {
            Assert.True(Condition.HasDifference("a", "zzbzbcz", out var difference));
            Assert.Equal("zbc", difference); // duplicates removed, first-occurrence order from second
        }

        [Fact]
        public void HasDifference_ShouldReportNoDifference_WhenSecondIsSubsetOrReorderedFirst()
        {
            Assert.False(Condition.HasDifference("abc", "abc", out var equivalent));
            Assert.Equal(string.Empty, equivalent);

            Assert.False(Condition.HasDifference("abc", "cba", out var reordered));
            Assert.Equal(string.Empty, reordered);

            Assert.False(Condition.HasDifference("abc", "aaabbbccc", out var duplicateHeavy));
            Assert.Equal(string.Empty, duplicateHeavy);

            Assert.False(Condition.HasDifference("abc", string.Empty, out var emptySecond));
            Assert.Equal(string.Empty, emptySecond);
        }

        [Fact]
        public void HasDifference_ShouldDetectDifferenceAtStartMiddleAndEnd()
        {
            Assert.True(Condition.HasDifference("a", "Zaaaa", out var atStart));
            Assert.Equal("Z", atStart);

            Assert.True(Condition.HasDifference("a", "aaZaa", out var atMiddle));
            Assert.Equal("Z", atMiddle);

            Assert.True(Condition.HasDifference("a", "aaaaZ", out var atEnd));
            Assert.Equal("Z", atEnd);
        }

        [Fact]
        public void HasDifference_ShouldHandleNonAsciiCharacters()
        {
            Assert.False(Condition.HasDifference("ÆØÅ", "ÅØÆ", out var noDifference));
            Assert.Equal(string.Empty, noDifference);

            Assert.True(Condition.HasDifference("ÆØ", "ÆØÅÅ", out var difference));
            Assert.Equal("Å", difference);

            // characters outside the Latin-1 range (>= U+0100)
            Assert.False(Condition.HasDifference("Ā日本", "本日Ā", out var noDifferenceBmp));
            Assert.Equal(string.Empty, noDifferenceBmp);

            Assert.True(Condition.HasDifference("Ā日", "日Ā本本", out var differenceBmp));
            Assert.Equal("本", differenceBmp);
        }

        [Theory]
        [InlineData("0AFF", true)]
        [InlineData("0aff", true)]
        [InlineData("aAbB", true)]
        [InlineData("00", true)]
        [InlineData("1234567890", true)]
        [InlineData("abcdefABCDEF", true)]
        [InlineData("0AF", false)]  // odd length
        [InlineData("0", false)]    // odd length
        [InlineData("0AGG", false)] // G is not hexadecimal
        [InlineData(" 0", false)]   // space is not hexadecimal
        [InlineData("", false)]     // empty
        public void IsHex_ShouldPreserveEvenLengthAndDigitSemantics(string value, bool expected)
        {
            Assert.Equal(expected, Condition.IsHex(value));
        }

        [Theory]
        [InlineData("", true)]        // empty is treated as consisting only of white-space
        [InlineData(" ", true)]
        [InlineData(" \t", true)]
        [InlineData("\r\n", true)]
        [InlineData("\u00A0", true)]  // non-breaking space is white-space
        [InlineData(" value ", false)]
        [InlineData("value", false)]
        public void IsWhiteSpace_ShouldPreserveEmptyAndDetectNonWhitespace(string value, bool expected)
        {
            Assert.Equal(expected, Condition.IsWhiteSpace(value));
        }

        [Fact]
        public void IsWhiteSpace_ShouldReturnFalse_WhenNull()
        {
            Assert.False(Condition.IsWhiteSpace(null));
        }

        [Theory]
        [InlineData("0", true)]
        [InlineData("1", true)]
        [InlineData("101010", true)]
        [InlineData("102010", false)]
        [InlineData("2", false)]
        [InlineData("01 01", false)] // interior space
        [InlineData(" ", false)]
        [InlineData("", false)]
        public void IsBinaryDigits_ShouldValidateOnlyZeroAndOne(string value, bool expected)
        {
            Assert.Equal(expected, Condition.IsBinaryDigits(value));
        }

        [Fact]
        public void IsBinaryDigits_ShouldReturnFalse_WhenNull()
        {
            Assert.False(Condition.IsBinaryDigits(null));
        }

        [Theory]
        [InlineData("user@example.com", true)]
        [InlineData("benchmark@cuemon.net", true)]
        [InlineData("a@b.co", true)]
        [InlineData("MiXeD@CaSe.CoM", true)]           // case-insensitive
        [InlineData("x@[192.168.0.1]", true)]          // IP literal
        [InlineData("Ünïcode@example.com", true)]      // unanchored: matches embedded ASCII substring
        [InlineData("invalid-email", false)]
        [InlineData("plain", false)]
        [InlineData("café@example.com", false)]        // no valid ASCII local part before '@'
        [InlineData("Ünïcöde", false)]
        [InlineData(" ", false)]
        [InlineData("", false)]
        public void IsEmailAddress_ShouldValidateAcrossRepresentativeInputs(string value, bool expected)
        {
            Assert.Equal(expected, Condition.IsEmailAddress(value));
        }

        [Fact]
        public void IsEmailAddress_ShouldValidateBoundaryLengthAndNull()
        {
            Assert.False(Condition.IsEmailAddress(null));
            Assert.True(Condition.IsEmailAddress(new string('a', 240) + "@example.com"));
        }

        [Theory]
        [InlineData("QQ==", true)]
        [InlineData("Q3VlbW9u", true)]
        [InlineData("QUJD", true)]
        [InlineData("YWJjZA==", true)]
        [InlineData("abcd", true)]         // unpadded multiple of four
        [InlineData(" QQ== ", true)]       // surrounding white-space is ignored
        [InlineData("Q3Vl\nbW9u", true)]   // interior white-space is ignored
        [InlineData("QQ=", false)]         // malformed length
        [InlineData("QQ", false)]          // malformed length
        [InlineData("QQ=Q", false)]        // padding in the middle
        [InlineData("****", false)]        // invalid characters
        [InlineData("DJ BOBO", false)]
        [InlineData("", false)]            // empty
        public void IsBase64_ShouldValidateWhitespacePaddingAndInvalidInputs(string value, bool expected)
        {
            Assert.Equal(expected, Condition.IsBase64(value));
        }

        [Fact]
        public void IsBase64_ShouldReturnFalse_WhenNull()
        {
            Assert.False(Condition.IsBase64(null));
        }
    }
}
