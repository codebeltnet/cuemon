using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Cuemon.Collections.Generic;

namespace Cuemon.Extensions.Runtime
{
    /// <summary>
    /// Extension methods for the <see cref="IHierarchy{T}"/> interface hidden behind the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class HierarchyDecoratorExtensions
    {
        private static readonly IList<KeyValuePair<Type, string>> ConvertibleTypes = typeof(bool).GetTypeInfo().Assembly.GetTypes().Where(t => t.GetTypeInfo().IsPrimitive).Select(t => new KeyValuePair<Type, string>(t, t.Name.Split('.').Last())).ToList();

        /// <summary>
        /// A formatter implementation that resolves a <see cref="IConvertible"/>.
        /// </summary>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{DataPair}"/> to extend.</param>
        /// <returns>A <see cref="IConvertible"/> from the enclosed <see cref="IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static IConvertible UseConvertibleFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            var i = decorator.FindSingleInstance(h => ConvertibleTypes.Select(pair => pair.Value).Contains(h.Instance.Name));
            return Decorator.Enclose(i.Value).ChangeType(ConvertibleTypes.Single(pair => pair.Value == i.Name).Key) as IConvertible;
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="Uri"/>.
        /// </summary>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{DataPair}"/> to extend.</param>
        /// <returns>A <see cref="Uri"/> from the enclosed <see cref="IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static Uri UseUriFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            var uri = decorator.FindSingleInstance(h => h.Instance.Name.Equals("OriginalString", StringComparison.OrdinalIgnoreCase));
            return uri == null ? decorator.Inner.UseGenericConverter<Uri>() : Decorator.Enclose(uri.Value.ToString()).ToUri();
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{DataPair}"/> to extend.</param>
        /// <returns>A <see cref="DateTime"/> from the enclosed <see cref="IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        public static DateTime UseDateTimeFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            return decorator.Inner.Instance.Type == typeof(DateTime) ? Decorator.Enclose(decorator.Inner.Instance.Value).ChangeTypeOrDefault<DateTime>() : decorator.Inner.UseGenericConverter<DateTime>();
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="Guid"/>.
        /// </summary>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{DataPair}"/> to extend.</param>
        /// <returns>A <see cref="Guid"/> from the enclosed <see cref="IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static Guid UseGuidFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            return decorator.Inner.UseGenericConverter<Guid>();
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="string"/>.
        /// </summary>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{DataPair}"/> to extend.</param>
        /// <returns>A <see cref="string"/> from the enclosed <see cref="IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static string UseStringFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            return decorator.Inner.UseGenericConverter<string>();
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="decimal"/>.
        /// </summary>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{DataPair}"/> to extend.</param>
        /// <returns>A <see cref="decimal"/> from the enclosed <see cref="IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static decimal UseDecimalFormatter(this IDecorator<IHierarchy<DataPair>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            return decorator.Inner.UseGenericConverter<decimal>();
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="ICollection"/>.
        /// </summary>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{DataPair}"/> to extend.</param>
        /// <param name="valueType">The type of the objects in the collection.</param>
        /// <returns>A <see cref="ICollection"/> of <paramref name="valueType"/> from the enclosed <see cref="IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static ICollection UseCollection(this IDecorator<IHierarchy<DataPair>> decorator, Type valueType)
        {
            Validator.ThrowIfNull(decorator);
            var items = decorator.Inner.GetChildren();
            var list = typeof(List<>).MakeGenericType(valueType);
            var listInstance = Activator.CreateInstance(list);
            var addMethod = list.GetMethod("Add");
            foreach (var item in items.ParseCollectionItem(valueType))
            {
                addMethod.Invoke(listInstance, new[] { item });
            }
            return listInstance as ICollection;
        }

        /// <summary>
        /// A formatter implementation that resolves a <see cref="IDictionary"/>.
        /// </summary>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{DataPair}"/> to extend.</param>
        /// <param name="valueTypes">The value types that forms a <see cref="KeyValuePair{TKey,TValue}"/>.</param>
        /// <returns>A <see cref="IDictionary"/> with <see cref="KeyValuePair{TKey,TValue}"/> of <paramref name="valueTypes"/> from the enclosed <see cref="IHierarchy{DataPair}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static IDictionary UseDictionary(this IDecorator<IHierarchy<DataPair>> decorator, Type[] valueTypes)
        {
            Validator.ThrowIfNull(decorator);
            var items = decorator.Inner.GetChildren();
            var dic = typeof(Dictionary<,>).MakeGenericType(valueTypes);
            var dicInstance = Activator.CreateInstance(dic);
            var addMethod = dic.GetMethod("Add");
            foreach (var item in items.ParseDictionaryItem(valueTypes))
            {
                addMethod.Invoke(dicInstance, new[] { item.Key, item.Value });
            }
            return dicInstance as IDictionary;
        }

        /// <summary>
        /// Returns the first node instance that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IWrapper{T}.Instance"/>  that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found.</returns>
        public static T FindFirstInstance<T>(this IDecorator<IHierarchy<T>> decorator, Func<IHierarchy<T>, bool> match)
        {
            return decorator.FindInstance(match).FirstOrDefault();
        }

        /// <summary>
        /// Returns the only node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node instance is found; this method throws an exception if more than one node is found.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IWrapper{T}.Instance"/> node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node instance is found.</returns>
        public static T FindSingleInstance<T>(this IDecorator<IHierarchy<T>> decorator, Func<IHierarchy<T>, bool> match)
        {
            return decorator.FindInstance(match).SingleOrDefault();
        }

        /// <summary>
        /// Retrieves all node instances that match the conditions defined by the function delegate <paramref name="match"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all node instances that match the conditions defined by the specified predicate, if found.</returns>
        public static IEnumerable<T> FindInstance<T>(this IDecorator<IHierarchy<T>> decorator, Func<IHierarchy<T>, bool> match)
        {
            return decorator.Find(match).Select(h => h.Instance);
        }

        /// <summary>
        /// Returns the first node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found.</returns>
        public static IHierarchy<T> FindFirst<T>(this IDecorator<IHierarchy<T>> decorator, Func<IHierarchy<T>, bool> match)
        {
            return decorator.Find(match).FirstOrDefault();
        }

        /// <summary>
        /// Returns the only node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found; this method throws an exception if more than one node is found.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> node that match the conditions defined by the function delegate <paramref name="match"/>, or a default value if no node is found.</returns>
        public static IHierarchy<T> FindSingle<T>(this IDecorator<IHierarchy<T>> decorator, Func<IHierarchy<T>, bool> match)
        {
            return decorator.Find(match).SingleOrDefault();
        }

        /// <summary>
        /// Retrieves all nodes that match the conditions defined by the function delegate <paramref name="match"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <param name="match">The function delegate that defines the conditions of the nodes to search for.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all nodes that match the conditions defined by the specified predicate, if found.</returns>
        public static IEnumerable<IHierarchy<T>> Find<T>(this IDecorator<IHierarchy<T>> decorator, Func<IHierarchy<T>, bool> match)
        {
            Validator.ThrowIfNull(decorator);
            Validator.ThrowIfNull(match);
            return decorator.DescendantsAndSelf().Where(match);
        }

        /// <summary>
        /// Replace the instance of the <paramref name="decorator"/> with a <paramref name="replacer"/> delegate.
        /// </summary>
        /// <typeparam name="T">The type of the instance that this node represents.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <param name="replacer">The delegate that will replace the wrapped instance of the <paramref name="decorator"/>.</param>
        public static void Replace<T>(this IDecorator<IHierarchy<T>> decorator, Action<IHierarchy<T>, T> replacer)
        {
            Validator.ThrowIfNull(decorator);
            Validator.ThrowIfNull(replacer);
            replacer(decorator.Inner, decorator.Inner.Instance);
        }

        /// <summary>
        /// Replace all instances of the <paramref name="decorator"/> with a <paramref name="replacer"/> delegate.
        /// </summary>
        /// <typeparam name="T">The type of the instance that these nodes represents.</typeparam>
        /// <param name="decorator">The decorator that wraps the sequence of <see cref="IHierarchy{T}"/> values to extend.</param>
        /// <param name="replacer">The delegate that will replace all wrapped instances of the <paramref name="decorator"/>.</param>
        public static void ReplaceAll<T>(this IDecorator<IEnumerable<IHierarchy<T>>> decorator, Action<IHierarchy<T>, T> replacer)
        {
            Validator.ThrowIfNull(decorator);
            Validator.ThrowIfNull(replacer);
            foreach (var node in decorator.Inner)
            {
                Decorator.Enclose(node).Replace(replacer);
            }
        }

        /// <summary>
        /// Returns the root node of the specified <paramref name="decorator"/> in the hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="decorator"/> in the hierarchical structure.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> node that represents the root of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null.
        /// </exception>
        public static IHierarchy<T> Root<T>(this IDecorator<IHierarchy<T>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            return decorator.Inner.HasParent ? decorator.AncestorsAndSelf().FirstOrDefault() : decorator.Inner;
        }

        /// <summary>
        /// Gets all ancestors (parent, grandparent, etc.) and self of the specified <paramref name="decorator"/> in the hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="decorator"/> in the hierarchical structure.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equal to ancestors and self of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null.
        /// </exception>
        public static IEnumerable<IHierarchy<T>> AncestorsAndSelf<T>(this IDecorator<IHierarchy<T>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            IList<IHierarchy<T>> result = new List<IHierarchy<T>>(Hierarchy.TraverseWhileNotNull(decorator.Inner, Hierarchy.AncestorsAndSelf));
            return result.Count > 0 ? result.Reverse() : Arguments.Yield(decorator.Inner);
        }

        /// <summary>
        /// Gets all descendants (children, grandchildren, etc.) anf self of the current <paramref name="decorator"/> in the hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="decorator"/> in the hierarchical structure.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equal to the descendants and self of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null.
        /// </exception>
        public static IEnumerable<IHierarchy<T>> DescendantsAndSelf<T>(this IDecorator<IHierarchy<T>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            return Hierarchy.TraverseWhileNotEmpty(decorator.Inner, Hierarchy.DescendantsAndSelf).Reverse();
        }

        /// <summary>
        /// Gets all siblings and self after the current <paramref name="decorator"/> in the hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="decorator"/> in the hierarchical structure.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equal to the siblings and self of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null.
        /// </exception>
        public static IEnumerable<IHierarchy<T>> SiblingsAndSelf<T>(this IDecorator<IHierarchy<T>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            return decorator.SiblingsAndSelfAt(decorator.Inner.Depth);
        }

        /// <summary>
        /// Gets all siblings and self after the current <paramref name="decorator"/> in the hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="decorator"/> in the hierarchical structure.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <param name="depth">The depth in the hierarchical structure from where to locate the siblings and self nodes.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equal to the siblings and self of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="depth"/> is less than zero.
        /// </exception>
        public static IEnumerable<IHierarchy<T>> SiblingsAndSelfAt<T>(this IDecorator<IHierarchy<T>> decorator, int depth)
        {
            Validator.ThrowIfNull(decorator);
            Validator.ThrowIfLowerThan(depth, 0, nameof(depth));
            var root = decorator.AncestorsAndSelf().FirstOrDefault();
            var descendantsFromRoot = Decorator.Enclose(root).DescendantsAndSelf();
            foreach (var descendantItem in descendantsFromRoot)
            {
                if (descendantItem.Depth == depth) { yield return descendantItem; }
            }
        }

        /// <summary>
        /// Returns the node at the specified index of a hierarchical structure.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="decorator"/> in the hierarchical structure.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <param name="index">The zero-based index at which a node should be retrieved in the hierarchical structure.</param>
        /// <returns>The node at the specified <paramref name="index"/> in the hierarchical structure.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than zero - or - <paramref name="index"/> exceeded the count of nodes in the hierarchical structure.
        /// </exception>
        public static IHierarchy<T> NodeAt<T>(this IDecorator<IHierarchy<T>> decorator, int index)
        {
            Validator.ThrowIfNull(decorator);
            Validator.ThrowIfLowerThan(index, 0, nameof(index));
            if (decorator.Inner.Index == index) { return decorator.Inner; }
            var allNodes = decorator.FlattenAll();
            foreach (var element in allNodes)
            {
                if (element.Index == index) { return element; }
            }
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        /// <summary>
        /// Flattens the entirety of a hierarchical structure representation into an <see cref="IEnumerable{T}"/> sequence of nodes.
        /// </summary>
        /// <typeparam name="T">The type of the instance represented by the specified <paramref name="decorator"/> in the hierarchical structure.</typeparam>
        /// <param name="decorator">The decorator that wraps the <see cref="IHierarchy{T}"/> to extend.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence of <see cref="IHierarchy{T}"/> all nodes represented by the hierarchical structure.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null.
        /// </exception>
        public static IEnumerable<IHierarchy<T>> FlattenAll<T>(this IDecorator<IHierarchy<T>> decorator)
        {
            Validator.ThrowIfNull(decorator);
            var root = decorator.AncestorsAndSelf().FirstOrDefault();
            return Decorator.Enclose(root).DescendantsAndSelf();
        }

        private static T UseGenericConverter<T>(this IHierarchy<DataPair> hierarchy)
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(hierarchy.Instance.Value.ToString());
        }

        private static IEnumerable<object> ParseCollectionItem(this IEnumerable<IHierarchy<DataPair>> items, Type valueType)
        {
            var valueTypeInfo = valueType.GetTypeInfo();
            if (valueTypeInfo.IsPrimitive)
            {
                return items.Select(i => Decorator.Enclose(i).UseConvertibleFormatter());
            }

            if (valueType == typeof(Uri))
            {
                return items.Select(i => Decorator.Enclose(i).UseUriFormatter());
            }

            if (valueType == typeof(decimal))
            {
                return items.Select(i => Decorator.Enclose(i).UseDecimalFormatter()).Cast<object>();
            }

            if (valueType == typeof(string))
            {
                return items.Select(i => Decorator.Enclose(i).UseStringFormatter());
            }

            if (valueType == typeof(Guid))
            {
                return items.Select(i => Decorator.Enclose(i).UseGuidFormatter()).Cast<object>();
            }

            if (valueType == typeof(DateTime))
            {
                return items.Select(i => Decorator.Enclose(i).UseDateTimeFormatter()).Cast<object>();
            }

            return new List<object>();
        }

        private static IEnumerable<KeyValuePair<object, object>> ParseDictionaryItem(this IEnumerable<IHierarchy<DataPair>> items, Type[] valueTypes)
        {
            var valueType = valueTypes[1];
            var valueTypeInfo = valueType.GetTypeInfo();
            var dicItems = items.ToDictionary(h => h, h => h.GetChildren().SingleOrDefault());

            if (valueTypeInfo.IsPrimitive)
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseConvertibleFormatter()));
            }

            if (valueType == typeof(Uri))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseUriFormatter()));
            }

            if (valueType == typeof(decimal))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseDecimalFormatter()));
            }

            if (valueType == typeof(string))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseStringFormatter()));
            }

            if (valueType == typeof(Guid))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseGuidFormatter()));
            }

            if (valueType == typeof(DateTime))
            {
                return dicItems.Select(i => new KeyValuePair<object, object>(Decorator.Enclose(i.Key.Instance.Value).ChangeType(valueTypes[0]), Decorator.Enclose(i.Value).UseDateTimeFormatter()));
            }

            return new Dictionary<object, object>();
        }
    }
}