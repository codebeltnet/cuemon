using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Runtime
{
    public class HierarchyDecoratorExtensionsTest : Test
    {
        public HierarchyDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void NavigationExtensions_ShouldReturnExpectedNodes_WhenHierarchyHasMultipleLevels()
        {
            var root = BuildStringHierarchy(out var childOne, out var grandchild, out var childTwo);
            var ancestors = Decorator.Enclose(grandchild).AncestorsAndSelf().Select(h => h.Instance).ToList();
            var descendants = Decorator.Enclose(root).DescendantsAndSelf().ToList();
            var siblings = Decorator.Enclose(childOne).SiblingsAndSelf().Select(h => h.Instance).ToList();
            var nodesAtDepth = Decorator.Enclose(grandchild).SiblingsAndSelfAt(1).Select(h => h.Instance).ToList();
            var allNodes = Decorator.Enclose(childOne).FlattenAll().ToList();

            Assert.Same(root, Decorator.Enclose(grandchild).Root());
            Assert.Equal(new[] { "root", "child-one" }, ancestors);
            Assert.Equal(4, descendants.Count);
            Assert.Contains(root, descendants);
            Assert.Contains(childOne, descendants);
            Assert.Contains(grandchild, descendants);
            Assert.Contains(childTwo, descendants);
            Assert.Equal(new[] { "child-one", "child-two" }, siblings);
            Assert.Equal(new[] { "child-one", "child-two" }, nodesAtDepth);
            Assert.Same(grandchild, Decorator.Enclose(root).NodeAt(2));
            Assert.Equal(4, allNodes.Count);
            Assert.Throws<ArgumentOutOfRangeException>(() => Decorator.Enclose(root).NodeAt(99));
        }

        [Fact]
        public void FindAndReplaceExtensions_ShouldTransformMatchingNodes_WhenPredicatesMatch()
        {
            var root = BuildStringHierarchy(out var childOne, out var grandchild, out var childTwo);

            Assert.Equal("child-one", Decorator.Enclose(root).FindFirstInstance(h => h.Instance.StartsWith("child", StringComparison.Ordinal)));
            Assert.Equal("grandchild", Decorator.Enclose(root).FindSingleInstance(h => h.Instance == "grandchild"));
            Assert.Same(childTwo, Decorator.Enclose(root).FindFirst(h => h.Instance == "child-two"));
            Assert.Same(grandchild, Decorator.Enclose(root).FindSingle(h => h.Instance == "grandchild"));
            Assert.Equal(new[] { "child-one", "child-two" }, Decorator.Enclose(root).FindInstance(h => h.Depth == 1).OrderBy(value => value).ToArray());
            Assert.Equal(2, Decorator.Enclose(root).Find(h => h.Depth == 1).Count());

            Decorator.Enclose(grandchild).Replace((node, value) => node.Replace(value.ToUpperInvariant()));
            Decorator.Enclose(Decorator.Enclose(root).Find(h => h.Depth == 1)).ReplaceAll((node, value) => node.Replace(value.ToUpperInvariant()));

            Assert.Equal("GRANDCHILD", grandchild.Instance);
            Assert.Equal(new[] { "CHILD-ONE", "CHILD-TWO" }, root.GetChildren().Select(h => h.Instance).ToArray());
        }

        [Fact]
        public void FormatterExtensions_ShouldConvertPrimitiveAndSpecialNodes_WhenDataPairsAreWrapped()
        {
            var integerNode = BuildDataPairHierarchy(new DataPair(typeof(int).Name, "42", typeof(string)));
            var uri = new Uri("https://example.com/path?value=42", UriKind.Absolute);
            var uriNode = BuildDataPairHierarchy(new DataPair("OriginalString", uri.OriginalString, typeof(string)));
            var fallbackUriNode = BuildDataPairHierarchy(new DataPair("Value", uri.OriginalString, typeof(string)));
            var timestamp = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);
            var directDateTimeNode = BuildDataPairHierarchy(new DataPair("When", timestamp, typeof(DateTime)));
            var fallbackDateTimeNode = BuildDataPairHierarchy(new DataPair("When", timestamp.ToString("O", CultureInfo.InvariantCulture), typeof(string)));
            var guid = Guid.Parse("11111111-2222-3333-4444-555555555555");
            var guidNode = BuildDataPairHierarchy(new DataPair("Value", guid.ToString("D"), typeof(string)));
            var stringNode = BuildDataPairHierarchy(new DataPair("Text", "hello", typeof(string)));
            var decimalNode = BuildDataPairHierarchy(new DataPair("Amount", "42.5", typeof(string)));

            Assert.Equal(42, Decorator.Enclose(integerNode).UseConvertibleFormatter());
            Assert.Equal(uri, Decorator.Enclose(uriNode).UseUriFormatter());
            Assert.Equal(uri, Decorator.Enclose(fallbackUriNode).UseUriFormatter());
            Assert.Equal(timestamp, Decorator.Enclose(directDateTimeNode).UseDateTimeFormatter());
            Assert.Equal(timestamp, Decorator.Enclose(fallbackDateTimeNode).UseDateTimeFormatter().ToUniversalTime());
            Assert.Equal(guid, Decorator.Enclose(guidNode).UseGuidFormatter());
            Assert.Equal("hello", Decorator.Enclose(stringNode).UseStringFormatter());
            Assert.Equal(42.5m, Decorator.Enclose(decimalNode).UseDecimalFormatter());
        }

        [Fact]
        public void CollectionFormatters_ShouldMaterializeTypedCollections_WhenChildrenRepresentStructuredValues()
        {
            var collectionNode = BuildCollectionHierarchy(typeof(int), 1, 2, 3);
            var dictionaryNode = BuildDictionaryHierarchy(typeof(int), new KeyValuePair<string, object>("alpha", 1), new KeyValuePair<string, object>("beta", 2));
            var collection = Decorator.Enclose(collectionNode).UseCollection(typeof(int));
            var dictionary = Decorator.Enclose(dictionaryNode).UseDictionary(new[] { typeof(string), typeof(int) });

            Assert.Equal(new[] { 1, 2, 3 }, collection.Cast<int>().ToArray());
            Assert.Equal(2, dictionary.Count);
            Assert.Equal(1, dictionary["alpha"]);
            Assert.Equal(2, dictionary["beta"]);
        }

        [Fact]
        public void SpecializedCollectionAndDictionaryFormatters_ShouldHandleSupportedAndUnsupportedTypes_WhenMaterializingValues()
        {
            var uri = new Uri("https://example.com/value", UriKind.Absolute);
            var timestamp = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);
            var guid = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

            var uriCollection = Decorator.Enclose(BuildCollectionHierarchy(typeof(Uri), uri.OriginalString)).UseCollection(typeof(Uri));
            var decimalCollection = Decorator.Enclose(BuildCollectionHierarchy(typeof(decimal), "42.5")).UseCollection(typeof(decimal));
            var stringCollection = Decorator.Enclose(BuildCollectionHierarchy(typeof(string), "alpha")).UseCollection(typeof(string));
            var guidCollection = Decorator.Enclose(BuildCollectionHierarchy(typeof(Guid), guid.ToString("D"))).UseCollection(typeof(Guid));
            var dateTimeCollection = Decorator.Enclose(BuildCollectionHierarchy(typeof(DateTime), timestamp)).UseCollection(typeof(DateTime));
            var unsupportedCollection = Decorator.Enclose(BuildCollectionHierarchy(typeof(object), new object())).UseCollection(typeof(object));

            Assert.Equal(uri, uriCollection.Cast<Uri>().Single());
            Assert.Equal(42.5m, decimalCollection.Cast<decimal>().Single());
            Assert.Equal("alpha", stringCollection.Cast<string>().Single());
            Assert.Equal(guid, guidCollection.Cast<Guid>().Single());
            Assert.Equal(timestamp, dateTimeCollection.Cast<DateTime>().Single());
            Assert.Empty(unsupportedCollection.Cast<object>());

            var uriDictionary = Decorator.Enclose(BuildDictionaryHierarchy(typeof(Uri), new KeyValuePair<string, object>("uri", uri.OriginalString))).UseDictionary(new[] { typeof(string), typeof(Uri) });
            var decimalDictionary = Decorator.Enclose(BuildDictionaryHierarchy(typeof(decimal), new KeyValuePair<string, object>("amount", "42.5"))).UseDictionary(new[] { typeof(string), typeof(decimal) });
            var stringDictionary = Decorator.Enclose(BuildDictionaryHierarchy(typeof(string), new KeyValuePair<string, object>("text", "alpha"))).UseDictionary(new[] { typeof(string), typeof(string) });
            var guidDictionary = Decorator.Enclose(BuildDictionaryHierarchy(typeof(Guid), new KeyValuePair<string, object>("id", guid.ToString("D")))).UseDictionary(new[] { typeof(string), typeof(Guid) });
            var dateTimeDictionary = Decorator.Enclose(BuildDictionaryHierarchy(typeof(DateTime), new KeyValuePair<string, object>("timestamp", timestamp))).UseDictionary(new[] { typeof(string), typeof(DateTime) });
            var unsupportedDictionary = Decorator.Enclose(BuildDictionaryHierarchy(typeof(object), new KeyValuePair<string, object>("unsupported", new object()))).UseDictionary(new[] { typeof(string), typeof(object) });

            Assert.Equal(uri, uriDictionary["uri"]);
            Assert.Equal(42.5m, decimalDictionary["amount"]);
            Assert.Equal("alpha", stringDictionary["text"]);
            Assert.Equal(guid, guidDictionary["id"]);
            Assert.Equal(timestamp, dateTimeDictionary["timestamp"]);
            Assert.Equal(0, unsupportedDictionary.Count);
        }

        private static Hierarchy<string> BuildStringHierarchy(out IHierarchy<string> childOne, out IHierarchy<string> grandchild, out IHierarchy<string> childTwo)
        {
            var root = new Hierarchy<string>();
            root.Add("root");
            childOne = root.Add("child-one");
            grandchild = childOne.Add("grandchild");
            childTwo = root.Add("child-two");
            return root;
        }

        private static IHierarchy<DataPair> BuildDataPairHierarchy(DataPair pair)
        {
            var hierarchy = new Hierarchy<DataPair>();
            hierarchy.Add(pair);
            return hierarchy;
        }

        private static IHierarchy<DataPair> BuildCollectionHierarchy(Type valueType, params object[] values)
        {
            var hierarchy = new Hierarchy<DataPair>();
            hierarchy.Add(new DataPair("Items", null, typeof(List<object>)));
            foreach (var value in values)
            {
                hierarchy.Add(CreateValuePair(valueType, value));
            }
            return hierarchy;
        }

        private static IHierarchy<DataPair> BuildDictionaryHierarchy(Type valueType, params KeyValuePair<string, object>[] values)
        {
            var hierarchy = new Hierarchy<DataPair>();
            hierarchy.Add(new DataPair("Entries", null, typeof(Dictionary<string, object>)));
            foreach (var value in values)
            {
                var keyNode = hierarchy.Add(new DataPair("Key", value.Key, typeof(string)));
                keyNode.Add(CreateValuePair(valueType, value.Value));
            }
            return hierarchy;
        }

        private static DataPair CreateValuePair(Type valueType, object value)
        {
            if (valueType.IsPrimitive)
            {
                return new DataPair(valueType.Name, value, value.GetType());
            }

            if (valueType == typeof(Uri))
            {
                return new DataPair("OriginalString", value, typeof(string));
            }

            if (valueType == typeof(DateTime))
            {
                return new DataPair("When", value, typeof(DateTime));
            }

            return new DataPair("Value", value, value?.GetType() ?? typeof(object));
        }
    }
}
