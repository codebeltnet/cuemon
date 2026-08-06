using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cuemon.Extensions.Runtime;
/// <summary>
/// Provides a generic way to expose a node of a hierarchical structure, including the node object of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the node represented in the hierarchical structure.</typeparam>
public interface IHierarchy<T> : IWrapper<T>
{
    /// <summary>
    /// Indicates whether the current node has a parent node.
    /// </summary>
    /// <value><c>true</c> if the current node has a parent node; otherwise, <c>false</c>.</value>
    bool HasParent { get; }

    /// <summary>
    /// Indicates whether the current node has any child nodes.
    /// </summary>
    /// <value><c>true</c> if the current node has any child nodes; otherwise, <c>false</c>.</value>
    bool HasChildren { get; }

    /// <summary>
    /// Gets the current depth of the node in the hierarchical structure.
    /// </summary>
    /// <value>The current depth of the node in the hierarchical structure.</value>
    int Depth { get; }

    /// <summary>
    /// Gets the zero-based index of the current node that this hierarchical structure represents.
    /// </summary>
    /// <value>The zero-based index of the current node that this hierarchical structure represents.</value>
    int Index { get; }

    /// <summary>
    /// Gets the node at the specified index.
    /// </summary>
    /// <value>The node at the specified index.</value>
    IHierarchy<T> this[int index] { get; }

    /// <summary>
    /// Adds the specified instance to a node in the hierarchical structure representation.
    /// </summary>
    /// <param name="instance">The instance to a node in the hierarchical structure represents.</param>
    /// <returns>A reference to the newly added hierarchical node.</returns>
    IHierarchy<T> Add(T instance);

    /// <summary>
    /// Adds the specified instance to a node in the hierarchical structure representation.
    /// </summary>
    /// <param name="instance">The instance to a node in the hierarchical structure represents.</param>
    /// <param name="member">The member from where <paramref name="instance"/> was referenced.</param>
    /// <returns>A reference to the newly added hierarchical node.</returns>
    IHierarchy<T> Add(T instance, MemberInfo member);

    /// <summary>
    /// Allows for the instance on the current node to be replaced with a new <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">The new instance to replace the original with.</param>
    void Replace(T instance);

    /// <summary>
    /// Gets the parent node of the current node in the hierarchical structure.
    /// </summary>
    /// <returns>The parent node of the current node in the hierarchical structure.</returns>
    IHierarchy<T> GetParent();

    /// <summary>
    /// Gets the hierarchical path of the node in the hierarchical structure.
    /// </summary>
    /// <returns>A <see cref="string"/> that identifies the hierarchical path relative to the current node.</returns>
    string GetPath();

    /// <summary>
    /// Gets the hierarchical path of the node in the hierarchical structure.
    /// </summary>
    /// <param name="pathResolver">The function delegate that resolves the hierarchical path of the node in the hierarchical structure.</param>
    /// <returns>A <see cref="string" /> that identifies the hierarchical path relative to the current node.</returns>
    string GetPath(Func<IHierarchy<T>, string> pathResolver);

    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> sequence that represents all the child nodes of the current hierarchical node.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> sequence that represents all the child nodes of the current hierarchical node.</returns>
    IEnumerable<IHierarchy<T>> GetChildren();
}
