using System;
using System.Collections.Generic;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
    public class GenerateTest : Test
    {
        public GenerateTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RangeOf_WithPositiveCount_ReturnsSequenceFromGenerator()
        {
            var result = Generate.RangeOf(5, i => i * 2).ToList();

            Assert.Equal(new[] { 0, 2, 4, 6, 8 }, result);
        }

        [Fact]
        public void RangeOf_WithZeroCount_ReturnsEmptySequence_AndGeneratorNotCalled()
        {
            var calls = 0;
            var result = Generate.RangeOf(0, i =>
            {
                calls++;
                return i;
            }).ToList();

            Assert.Empty(result);
            Assert.Equal(0, calls);
        }

        [Fact]
        public void RangeOf_GeneratorReceivesCorrectIndexes_AndMaintainsOrder()
        {
            var seen = new List<int>();
            var result = Generate.RangeOf(4, i =>
            {
                seen.Add(i);
                return $"Item{i}";
            }).ToList();

            Assert.Equal(new[] { "Item0", "Item1", "Item2", "Item3" }, result);
            Assert.Equal(new[] { 0, 1, 2, 3 }, seen);
        }

        [Fact]
        public void RangeOf_WithNegativeCount_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Generate.RangeOf(-1, i => i).ToList());
        }

        [Fact]
        public void RandomString_WithLength_ReturnsStringOfRequestedLength_AndOnlyLettersAndDigits()
        {
            const int length = 64;

            var result = Generate.RandomString(length);

            Assert.NotNull(result);
            Assert.Equal(length, result.Length);
            Assert.All(result, c => Assert.True(char.IsLetterOrDigit(c), $"Character '{c}' is not a letter or digit."));
        }

        [Fact]
        public void RandomString_WithZeroLength_ReturnsEmptyString()
        {
            var result = Generate.RandomString(0);

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void RandomString_WithCustomValues_ProducesCharsOnlyFromProvidedValues()
        {
            var values = new[] { "AB", "12", "x" }; // allowed characters: A,B,1,2,x
            var allowed = string.Concat(values).ToCharArray().Distinct().ToArray();
            const int length = 200;

            var result = Generate.RandomString(length, values);

            Assert.NotNull(result);
            Assert.Equal(length, result.Length);
            Assert.All(result, c => Assert.Contains(c, allowed));
        }

        [Fact]
        public void RandomString_WithNullValues_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Generate.RandomString(1, (string[])null));
        }

        [Fact]
        public void RandomString_WithEmptyValues_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Generate.RandomString(1, Array.Empty<string>()));
        }

        [Fact]
        public void RandomString_ShouldGenerateUniqueStringsOfSpecifiedLength()
        {
            var length = 256;
            var strings = new List<string>();
            for (var i = 0; i < 1024; i++)
            {
                strings.Add(Generate.RandomString(length));
            }
            Assert.All(strings, s => Assert.True(s.Length == length));
            Assert.All(strings, s => Assert.Single(strings, s));
        }


        [Fact]
        public void HashCode32_ShouldGenerateSameHashCode()
        {
            var hc1 = Generate.HashCode32(1, 2, 3, 4, 5);
            var hc2 = Generate.HashCode32(10, TimeSpan.FromSeconds(5).Ticks, TimeSpan.FromSeconds(15).Ticks, TimeSpan.FromSeconds(2).Ticks, "Class1.SomeMethod()");
            Assert.Equal(-3271143, hc1);
            Assert.Equal(1191125869, hc2);
        }

        [Fact]
        public void HashCode32_ParamsAndEnumerable_ProduceSameResult()
        {
            var inputs = new IConvertible[] { 1, 2, 3, 4, 5 };
            var fromParams = Generate.HashCode32((IConvertible[])inputs);
            var fromEnumerable = Generate.HashCode32((IEnumerable<IConvertible>)inputs);
            Assert.Equal(fromParams, fromEnumerable);
        }

        [Fact]
        public void HashCode32_Deterministic_ForSameInput()
        {
            var a = Generate.HashCode32(7, 11, 13, "x");
            var b = Generate.HashCode32(7, 11, 13, "x");
            Assert.Equal(a, b);
        }

        [Fact]
        public void HashCode32_OrderMatters()
        {
            var a = Generate.HashCode32(1, 2, 3);
            var b = Generate.HashCode32(3, 2, 1);
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void HashCode32_EmptySequence_ConsistentAcrossOverloads()
        {
            var emptyArray = Array.Empty<IConvertible>();
            var a = Generate.HashCode32((IConvertible[])emptyArray);
            var b = Generate.HashCode32((IEnumerable<IConvertible>)emptyArray);
            Assert.Equal(a, b);
            Assert.Equal(a, Generate.HashCode32()); // params with zero args
        }

        [Fact]
        public void HashCode64_ParamsAndEnumerable_ProduceSameResult()
        {
            var inputs = new IConvertible[] { 42L, 24L, 100, "abc" };
            var fromParams = Generate.HashCode64((IConvertible[])inputs);
            var fromEnumerable = Generate.HashCode64((IEnumerable<IConvertible>)inputs);
            Assert.Equal(fromParams, fromEnumerable);
        }

        [Fact]
        public void HashCode64_Deterministic_ForSameInput()
        {
            var a = Generate.HashCode64(123, "steady", 456.789);
            var b = Generate.HashCode64(123, "steady", 456.789);
            Assert.Equal(a, b);
        }

        [Fact]
        public void HashCode64_DifferentInputs_ProduceDifferentHashes()
        {
            var a = Generate.HashCode64(1, 2, 3);
            var b = Generate.HashCode64(3, 2, 1);
            Assert.NotEqual(a, b);
            var c = Generate.HashCode64(1, 2, 4);
            Assert.NotEqual(a, c);
        }

        [Fact]
        public void HashCode64_EmptySequence_ConsistentAcrossOverloads()
        {
            var emptyArray = Array.Empty<IConvertible>();
            var a = Generate.HashCode64((IConvertible[])emptyArray);
            var b = Generate.HashCode64((IEnumerable<IConvertible>)emptyArray);
            Assert.Equal(a, b);
            Assert.Equal(a, Generate.HashCode64()); // params with zero args
        }

        [Fact]
        public void ObjectPortrayal_Null_ReturnsConfiguredNullValue()
        {
            var result = Generate.ObjectPortrayal(null);
            Assert.Equal("<null>", result);
        }

        [Fact]
        public void ObjectPortrayal_Boolean_ReturnsLowercaseBooleanString()
        {
            var trueResult = Generate.ObjectPortrayal(true);
            var falseResult = Generate.ObjectPortrayal(false);

            Assert.Equal("true", trueResult);
            Assert.Equal("false", falseResult);
        }

        [Fact]
        public void ObjectPortrayal_WhenToStringIsOverridden_ReturnsToStringResult()
        {
            var sut = new WithToStringOverride { Value = 42 };
            var result = Generate.ObjectPortrayal(sut);

            Assert.Equal("OVERRIDDEN:42", result);
        }

        [Fact]
        public void ObjectPortrayal_WithoutOverride_IncludesPublicProperties_AndShowsNoGetterForWriteOnly()
        {
            var sut = new NoOverride { Id = 7, Name = "Alice" };
            Generate.ObjectPortrayal(sut); // produce output
            var result = Generate.ObjectPortrayal(sut);

            // Should start with full type name
            Assert.StartsWith(typeof(NoOverride).FullName, result);
            Assert.Contains(" { ", result);
            Assert.Contains(" }", result);

            // Should include property representations
            Assert.Contains("Id=7", result);
            Assert.Contains("Name=Alice", result);

            // WriteOnly has no getter; default NoGetterValue is "<no getter>"
            Assert.Contains("WriteOnly=<no getter>", result);
        }

        [Fact]
        public void ObjectPortrayal_BypassOverrideCheck_AllowsCallingFromOverriddenToString_AndReturnsPropertyListing()
        {
            var sut = new CallsObjectPortrayalFromToString { Value = 99 };
            // Generate.ObjectPortrayal will detect overridden ToString and call instance.ToString()
            // The overridden ToString uses BypassOverrideCheck = true which should cause a property listing to be returned.
            var result = Generate.ObjectPortrayal(sut);

            // Ensure we did not get the raw overridden marker but the property listing containing the Value
            Assert.DoesNotContain("TOSTRING-ENTRY", result);
            Assert.Contains("Value=99", result);
            Assert.StartsWith(typeof(CallsObjectPortrayalFromToString).FullName, result);
        }

        // Helper test types

        private class WithToStringOverride
        {
            public int Value { get; set; }

            public override string ToString()
            {
                return $"OVERRIDDEN:{Value}";
            }
        }

        private class NoOverride
        {
            public int Id { get; set; }
            public string Name { get; set; }

            // write-only property (no getter)
            private int _write;
            public int WriteOnly { set { _write = value; } }
        }

        private class CallsObjectPortrayalFromToString
        {
            public int Value { get; set; }

            public override string ToString()
            {
                // Simulates calling Generate.ObjectPortrayal from within an overridden ToString.
                return Generate.ObjectPortrayal(this, o => o.BypassOverrideCheck = true);
            }
        }
    }
}
