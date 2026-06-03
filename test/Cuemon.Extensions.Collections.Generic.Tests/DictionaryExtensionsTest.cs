using System;
using System.Collections.Generic;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Collections.Generic
{
    public class DictionaryExtensionsTest : Test
    {
        public DictionaryExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void GetValueOrDefault_ShouldReturnValue()
        {
            var sut1 = new Dictionary<int, string>()
            {
                { 1, "Cuemon" }
            };
            var sut2 = sut1.GetValueOrDefault(1);

            Assert.Equal("Cuemon", sut2);
        }

        [Fact]
        public void GetValueOrDefault_ShouldReturnDefault()
        {
            var sut1 = new Dictionary<int, string>()
            {
                { 1, "Cuemon" }
            };
            var sut2 = sut1.GetValueOrDefault(2);

            Assert.Equal(default, sut2);
        }

        [Fact]
        public void GetValueOrDefault_ShouldReturnDefault_Customized()
        {
            var sut1 = new Dictionary<int, string>()
            {
                { 1, "Cuemon" }
            };
            var sut2 = sut1.GetValueOrDefault(2, () => "InvalidKeySpecified");

            Assert.Equal("InvalidKeySpecified", sut2);
        }

        [Fact]
        public void TryGetValueOrFallback_ShouldReturnFallback()
        {
            var sut1 = new Dictionary<int, string>()
            {
                { 1, "Cuemon" },
                { 3, "FallbackValue" }
            };
            var sut3 = sut1.TryGetValueOrFallback(2, ints => ints.Max(), out var sut2);

            Assert.Equal("FallbackValue", sut2);
            Assert.True(sut3);
        }

        [Fact]
        public void TryGetValueOrFallback_ShouldReturnValue()
        {
            var sut1 = new Dictionary<int, string>()
            {
                { 1, "Cuemon" },
                { 3, "FallbackValue" }
            };
            var sut3 = sut1.TryGetValueOrFallback(1, ints => ints.Max(), out var sut2);

            Assert.Equal("Cuemon", sut2);
            Assert.True(sut3);
        }

        [Fact]
        public void TryGetValueOrFallback_ShouldReturnDefault()
        {
            var sut1 = new Dictionary<int, string>()
            {
                { 1, "Cuemon" },
                { 3, "FallbackValue" }
            };
            var sut3 = sut1.TryGetValueOrFallback(2, ints => 4, out var sut2);

            Assert.Equal(default, sut2);
            Assert.False(sut3);
        }

        [Fact]
        public void ToEnumerable_ShouldTypeDictionaryToKeyValuePairSequence()
        {
            var sut1 = Enumerable.Range(1, 10).ToDictionary(i => i, Generate.RandomString);
            var sut2 = sut1.ToEnumerable();

            Assert.Equal(sut1, sut2);
            Assert.True(sut1.Count == 10);
            Assert.True(sut2.Count() == 10);
            Assert.IsAssignableFrom<IEnumerable<KeyValuePair<int, string>>>(sut2);
        }

#if NET461
        [Fact]
        public void TryAdd_ShouldNotSucceed()
        {
            
            var sut1 = Enumerable.Range(1, 10).ToDictionary(i => i, Generate.RandomString);
            var sut2 = sut1.TryAdd(1, "Cuemon");

            Assert.False(sut2);
            Assert.True(sut1.Count == 10);
        }

        [Fact]
        public void TryAdd_ShouldSucceed()
        {
            
            var sut1 = Enumerable.Range(1, 10).ToDictionary(i => i, Generate.RandomString);
            var sut2 = sut1.TryAdd(11, "Cuemon");

            Assert.True(sut2);
            Assert.True(sut1.Count == 11);
        }
#endif

        [Fact]
        public void TryAddWithCondition_ShouldNotSucceed()
        {

            var sut1 = Enumerable.Range(1, 10).ToDictionary(i => i, Generate.RandomString);
            var sut2 = sut1.TryAdd(1, "Cuemon", d => !d.ContainsKey(1));

            Assert.False(sut2);
            Assert.True(sut1.Count == 10);
        }

        [Fact]
        public void TryAddWithCondition_ShouldSucceed()
        {

            var sut1 = Enumerable.Range(1, 10).ToDictionary(i => i, Generate.RandomString);
            var sut2 = sut1.TryAdd(11, "Cuemon", d => !d.ContainsKey(11));

            Assert.True(sut2);
            Assert.True(sut1.Count == 11);
        }

        [Fact]
        public void AddOrUpdate_ShouldAddNewItem()
        {

            var sut1 = Enumerable.Range(1, 10).ToDictionary(i => i, Generate.RandomString);
            sut1.AddOrUpdate(11, "Cuemon");

            Assert.True(sut1.ContainsKey(11));
            Assert.Equal("Cuemon", sut1[11]);
            Assert.True(sut1.Count == 11);
        }

        [Fact]
        public void AddOrUpdate_ShouldUpdateExistingItem()
        {

            var sut1 = Enumerable.Range(1, 10).ToDictionary(i => i, Generate.RandomString);
            sut1.AddOrUpdate(1, "Cuemon");

            Assert.True(sut1.ContainsKey(1));
            Assert.Equal("Cuemon", sut1[1]);
            Assert.True(sut1.Count == 10);
        }
        [Fact]
        public void CopyTo_ShouldCopyEntriesToDestination()
        {
            var sut1 = new Dictionary<int, string>
            {
                { 1, "Cuemon" },
                { 2, "Geekle" }
            };
            var sut2 = sut1.CopyTo(new Dictionary<int, string>());

            Assert.Equal(sut1, sut2);
        }

        [Fact]
        public void CopyTo_ShouldThrowArgumentNullException_WhenSourceIsNull()
        {
            IDictionary<int, string> sut = null;

            Assert.Throws<ArgumentNullException>(() => sut.CopyTo(new Dictionary<int, string>()));
        }

        [Fact]
        public void CopyTo_ShouldThrowArgumentNullException_WhenDestinationIsNull()
        {
            var sut = new Dictionary<int, string>();

            Assert.Throws<ArgumentNullException>(() => sut.CopyTo(null));
        }

        [Fact]
        public void CopyToWithCopier_ShouldCopyEntriesUsingCustomCopier()
        {
            var sut1 = new Dictionary<int, string>
            {
                { 1, "Cuemon" },
                { 2, "Geekle" }
            };
            var sut2 = sut1.CopyTo(new Dictionary<int, string>(), (source, destination) =>
            {
                foreach (var item in source)
                {
                    destination[item.Key] = item.Value.ToUpperInvariant();
                }
            });

            Assert.Equal("CUEMON", sut2[1]);
            Assert.Equal("GEEKLE", sut2[2]);
        }

        [Fact]
        public void CopyToWithCopier_ShouldThrowArgumentNullException_WhenCopierIsNull()
        {
            var sut = new Dictionary<int, string>();

            Assert.Throws<ArgumentNullException>(() => sut.CopyTo(new Dictionary<int, string>(), null));
        }

#if NETSTANDARD2_0_OR_GREATER
        [Fact]
        public void GetValueOrDefault_ShouldThrowArgumentNullException_WhenDictionaryIsNull()
        {
            IDictionary<string, string> sut = null;

            Assert.Throws<ArgumentNullException>(() => sut.GetValueOrDefault("key"));
        }

        [Fact]
        public void GetValueOrDefault_ShouldThrowArgumentNullException_WhenKeyIsNull()
        {
            var sut = new Dictionary<string, string>();

            Assert.Throws<ArgumentNullException>(() => sut.GetValueOrDefault(null));
        }
#endif

        [Fact]
        public void GetValueOrDefault_ShouldThrowArgumentNullException_WhenDefaultProviderIsNull()
        {
            var sut = new Dictionary<string, string>();

            Assert.Throws<ArgumentNullException>(() => sut.GetValueOrDefault("key", null));
        }

        [Fact]
        public void TryGetValueOrFallback_ShouldThrowArgumentNullException_WhenDictionaryIsNull()
        {
            IDictionary<string, string> sut = null;

            Assert.Throws<ArgumentNullException>(() => sut.TryGetValueOrFallback("key", keys => keys.First(), out _));
        }

        [Fact]
        public void ToEnumerable_ShouldThrowArgumentNullException_WhenDictionaryIsNull()
        {
            IDictionary<int, string> sut = null;

            Assert.Throws<ArgumentNullException>(() => sut.ToEnumerable());
        }

        [Fact]
        public void TryAddWithCondition_ShouldThrowArgumentNullException_WhenDictionaryIsNull()
        {
            IDictionary<string, string> sut = null;

            Assert.Throws<ArgumentNullException>(() => sut.TryAdd("key", "value", _ => true));
        }

        [Fact]
        public void TryAddWithCondition_ShouldThrowArgumentNullException_WhenKeyIsNull()
        {
            var sut = new Dictionary<string, string>();

            Assert.Throws<ArgumentNullException>(() => sut.TryAdd(null, "value", _ => true));
        }

        [Fact]
        public void TryAddWithCondition_ShouldThrowArgumentNullException_WhenConditionIsNull()
        {
            var sut = new Dictionary<string, string>();

            Assert.Throws<ArgumentNullException>(() => sut.TryAdd("key", "value", null));
        }

        [Fact]
        public void AddOrUpdate_ShouldThrowArgumentNullException_WhenDictionaryIsNull()
        {
            IDictionary<string, string> sut = null;

            Assert.Throws<ArgumentNullException>(() => sut.AddOrUpdate("key", "value"));
        }

        [Fact]
        public void AddOrUpdate_ShouldThrowArgumentNullException_WhenKeyIsNull()
        {
            var sut = new Dictionary<string, string>();

            Assert.Throws<ArgumentNullException>(() => sut.AddOrUpdate(null, "value"));
        }
    }
}