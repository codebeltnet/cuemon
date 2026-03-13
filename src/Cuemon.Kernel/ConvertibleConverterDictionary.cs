using System;
using System.Collections;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Represents a collection of converters that map <see cref="IConvertible"/> implementations to byte arrays.
    /// </summary>
    public class ConvertibleConverterDictionary : IReadOnlyDictionary<Type, Func<IConvertible, byte[]>>
    {
        private readonly Dictionary<Type, Func<IConvertible, byte[]>> _converters = new();

        /// <summary>
        /// Adds a converter for the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IConvertible"/>.</typeparam>
        /// <param name="converter">The delegate that converts an instance of <typeparamref name="T"/> to a byte array.</param>
        /// <returns>This instance so that additional converters can be configured.</returns>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> does not implement <see cref="IConvertible"/>.
        /// </exception>
        public ConvertibleConverterDictionary Add<T>(Func<T, byte[]> converter) where T : IConvertible
        {
            Validator.ThrowIfNotContainsInterface<T>(nameof(T), typeof(IConvertible));
            Add(typeof(T), c => converter((T)c));
            return this;
        }

        /// <summary>
        /// Adds a converter for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type that implements <see cref="IConvertible"/>.</param>
        /// <param name="converter">The delegate that converts an <see cref="IConvertible"/> instance to a byte array.</param>
        /// <returns>This instance so that additional converters can be configured.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="type"/> does not implement <see cref="IConvertible"/>.
        /// </exception>
        public ConvertibleConverterDictionary Add(Type type, Func<IConvertible, byte[]> converter)
        {
            Validator.ThrowIfNotContainsInterface(type, Arguments.ToArrayOf(typeof(IConvertible)));
            _converters.Add(type, converter);
            return this;
        }

        /// <summary>
        /// Determines whether this dictionary contains a converter for the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The type to locate.</param>
        /// <returns><see langword="true"/> if this dictionary contains a converter for <paramref name="key"/>; otherwise, <see langword="false"/>.</returns>
        public bool ContainsKey(Type key)
        {
            return _converters.ContainsKey(key);
        }

        /// <summary>
        /// Gets the converter associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The type whose associated converter to retrieve.</param>
        /// <param name="value">
        /// When this method returns, contains the converter associated with the specified <paramref name="key"/>,
        /// if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter.
        /// </param>
        /// <returns><see langword="true"/> if this dictionary contains a converter for <paramref name="key"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key"/> is <see langword="null"/>.
        /// </exception>
        public bool TryGetValue(Type key, out Func<IConvertible, byte[]> value)
        {
            return _converters.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the converter associated with the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose associated converter to retrieve.</param>
        /// <returns>
        /// The converter associated with <paramref name="type"/>, or <see langword="null"/> if no converter is registered
        /// for the specified type.
        /// </returns>
        public Func<IConvertible, byte[]> this[Type type]
        {
            get
            {
                if (type == null) { return null; }
                return _converters.TryGetValue(type, out var converter) ? converter : null;
            }
        }

        /// <summary>
        /// Gets the collection of types for which converters are registered.
        /// </summary>
        /// <value>A collection that contains the registered converter types.</value>
        public IEnumerable<Type> Keys => _converters.Keys;

        /// <summary>
        /// Gets the collection of registered converters.
        /// </summary>
        /// <value>A collection that contains the registered converters.</value>
        public IEnumerable<Func<IConvertible, byte[]>> Values => _converters.Values;

        /// <summary>
        /// Gets the number of converters contained in this instance.
        /// </summary>
        /// <value>The number of registered converters.</value>
        public int Count => _converters.Count;

        /// <summary>
        /// Returns an enumerator that iterates through the registered converters.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the registered converters.</returns>
        public IEnumerator<KeyValuePair<Type, Func<IConvertible, byte[]>>> GetEnumerator()
        {
            return _converters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
