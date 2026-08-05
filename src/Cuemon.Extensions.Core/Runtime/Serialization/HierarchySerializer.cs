using System;
using System.Globalization;
using System.Text;

namespace Cuemon.Extensions.Runtime.Serialization
{
    /// <summary>
    /// Provides a way to serialize objects to nodes of <see cref="IHierarchy{T}"/>.
    /// </summary>
    public class HierarchySerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchySerializer"/> class.
        /// </summary>
        /// <param name="source">The object to convert to nodes of <see cref="IHierarchy{T}"/>.</param>
        /// <param name="setup">The <see cref="HierarchyOptions"/> which need to be configured.</param>
        public HierarchySerializer(object source, Action<HierarchyOptions> setup = null)
        {
            Nodes = Decorator.Enclose(Hierarchy.GetObjectHierarchy(source, setup)).Root();
        }

        /// <summary>
        /// Gets the result of the <see cref="IHierarchy{T}"/>.
        /// </summary>
        /// <value>The converted nodes of the by constructor defined source object.</value>
        public IHierarchy<object> Nodes { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            var node = Nodes;
            sb.AppendLine(node.GetPath());
            ToString(sb, node);
            return sb.ToString();
        }

        private static void ToString(StringBuilder sb, IHierarchy<object> node)
        {
            foreach (var child in node.GetChildren())
            {
#if NETSTANDARD
                sb.AppendLine($"{new string(' ', child.Depth)}{child.GetPath()}"); // MemberReference.Name
#else
                sb.AppendLine(CultureInfo.InvariantCulture, $"{new string(' ', child.Depth)}{child.GetPath()}"); // MemberReference.Name
#endif
                ToString(sb, child);
            }
        }
    }
}
