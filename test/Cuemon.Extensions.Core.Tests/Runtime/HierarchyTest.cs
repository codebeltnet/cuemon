using System;
using System.Globalization;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Runtime
{
    public class HierarchyTest : Test
    {
        public HierarchyTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Add_ShouldAssignRelationshipsAndMetadata_WhenHierarchyGrows()
        {
            var root = new Hierarchy<string>();
            var rootNode = root.Add("root");
            var child = root.Add("child");
            var grandchild = child.Add("grandchild");
            var sibling = root.Add("sibling");

            Assert.Same(root, rootNode);
            Assert.Equal(0, root.Depth);
            Assert.Equal(0, root.Index);
            Assert.False(root.HasParent);
            Assert.True(root.HasChildren);
            Assert.Equal(1, child.Depth);
            Assert.Equal(1, child.Index);
            Assert.True(child.HasParent);
            Assert.True(child.HasChildren);
            Assert.Equal(2, grandchild.Depth);
            Assert.Equal(2, grandchild.Index);
            Assert.Equal(1, sibling.Depth);
            Assert.Equal(3, sibling.Index);
            Assert.Same(root, child.GetParent());
            Assert.Same(child, grandchild.GetParent());
            Assert.Equal(new[] { child, sibling }, root.GetChildren().ToArray());
            Assert.Same(grandchild, root[2]);
        }

        [Fact]
        public void Replace_ShouldUpdateWrappedInstance_WhenUsingOverloads()
        {
            var root = new Hierarchy<object>();
            root.Add("42", typeof(string));

            root.Replace(84);
            Assert.Equal(84, root.Instance);
            Assert.Equal(typeof(int), root.InstanceType);

            root.Replace("84", typeof(string));
            Assert.Equal("84", root.Instance);
            Assert.Equal(typeof(string), root.InstanceType);
            Assert.Throws<ArgumentNullException>(() => root.Replace("ignored", null));
        }

        [Fact]
        public void GetPath_ShouldReturnExpectedPaths_WhenUsingDefaultAndCustomResolvers()
        {
            var root = new Hierarchy<object>();
            root.Add(new RootNode(), typeof(RootNode));
            var child = root.Add(new ChildNode(), typeof(ChildNode));
            var leaf = child.Add(new LeafNode(), typeof(LeafNode));

            Assert.Equal("RootNode.ChildNode.LeafNode", leaf.GetPath());
            Assert.Equal("0.1.2", leaf.GetPath(h => h.Index.ToString(CultureInfo.InvariantCulture)));
        }

        [Fact]
        public void GetObjectHierarchy_ShouldBuildObjectTree_WhenObjectContainsNestedProperties()
        {
            var source = new ObjectGraph { Name = "alpha", Child = new ChildGraph { Count = 7 } };
            var hierarchy = Hierarchy.GetObjectHierarchy(source);
            var flattened = Decorator.Enclose(hierarchy).FlattenAll().ToList();

            Assert.Equal(typeof(ObjectGraph), hierarchy.InstanceType);
            Assert.Contains(flattened, h => Equals(h.Instance, "alpha"));
            var childNode = flattened.Single(h => Equals(h.MemberReference?.Name, nameof(ObjectGraph.Child)));
            Assert.Equal(typeof(ChildGraph), childNode.InstanceType);
            Assert.Equal(3, flattened.Count);
        }

        [Fact]
        public void GetObjectHierarchy_ShouldReturnRootOnly_WhenSourceIsSimpleType()
        {
            var hierarchy = Hierarchy.GetObjectHierarchy("alpha");

            Assert.Equal("alpha", hierarchy.Instance);
            Assert.Equal(typeof(string), hierarchy.InstanceType);
            Assert.False(hierarchy.HasChildren);
        }

        [Fact]
        public void StaticOperations_ShouldTraverseAndFindNodes_WhenHierarchyIsQueried()
        {
            var root = new Hierarchy<string>();
            root.Add("root");
            var child = root.Add("child");
            var grandchild = child.Add("grandchild");
            var found = Hierarchy.Find(root, h => h.Instance.Contains("child", StringComparison.Ordinal)).ToList();
            var ancestors = Hierarchy.TraverseWhileNotNull(grandchild, h => h.GetParent()).Select(h => h.Instance).ToList();
            var traversed = Hierarchy.TraverseWhileNotEmpty<IHierarchy<string>>(root, h => h.GetChildren()).Select(h => h.Instance).ToList();

            Assert.Equal(1, found.Count);
            Assert.Equal(new[] { "child", "root" }, ancestors);
            Assert.Equal(new[] { "root", "child", "grandchild" }, traversed);
            Assert.Throws<ArgumentNullException>(() => Hierarchy.Find(root, null));
            Assert.Throws<ArgumentNullException>(() => Hierarchy.TraverseWhileNotNull<string>(null, h => h));
        }

        private sealed class RootNode
        {
        }

        private sealed class ChildNode
        {
        }

        private sealed class LeafNode
        {
        }

        private sealed class ObjectGraph
        {
            public string Name { get; set; }

            public ChildGraph Child { get; set; }
        }

        private sealed class ChildGraph
        {
            public int Count { get; set; }
        }
    }
}
