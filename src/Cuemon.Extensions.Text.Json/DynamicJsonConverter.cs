using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cuemon.Extensions.Text.Json
{
    /// <summary>
    /// Provides a factory based way to create and wrap an <see cref="JsonConverter"/> implementation.
    /// </summary>
    public static class DynamicJsonConverter
    {
        /// <summary>
        /// Creates a dynamic instance of an <see cref="JsonConverter"/> implementation wrapping <see cref="JsonConverter{T}.Write"/> through <paramref name="writer"/> and <see cref="JsonConverter{T}.Read"/> through <paramref name="reader"/>.
        /// </summary>
        /// <typeparam name="T">The type to implement an <see cref="JsonConverter"/>.</typeparam>
        /// <param name="writer">The delegate that converts <typeparamref name="T"/> to its JSON representation.</param>
        /// <param name="reader">The function delegate that generates <typeparamref name="T"/> from its JSON representation.</param>
        /// <returns>An <see cref="JsonConverter"/> implementation of <typeparamref name="T"/>.</returns>
        public static JsonConverter Create<T>(Utf8JsonWriterAction<T> writer = null, Utf8JsonReaderFunc<T> reader = null)
        {
            return Create(typeof(T).IsAssignableFrom, writer, reader);
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="JsonConverter" /> implementation wrapping <see cref="JsonConverter{T}.Write" /> through <paramref name="writer" /> and <see cref="JsonConverter{T}.Read" /> through <paramref name="reader" />.
        /// </summary>
        /// <typeparam name="T">The type to implement an <see cref="JsonConverter"/>.</typeparam>
        /// <param name="predicate">The function delegate that validates if given <see cref="Type"/> can be converted to and from JSON.</param>
        /// <param name="writer">The delegate that, when <paramref name="predicate"/> returns true, converts <typeparamref name="T"/> to its JSON representation.</param>
        /// <param name="reader">The function delegate that, when <paramref name="predicate"/> returns true, generates <typeparamref name="T"/> from its JSON representation.</param>
        /// <returns>An <see cref="JsonConverter" /> implementation of <typeparamref name="T"/>.</returns>
        public static JsonConverter Create<T>(Func<Type, bool> predicate, Utf8JsonWriterAction<T> writer = null, Utf8JsonReaderFunc<T> reader = null)
        {
            Validator.ThrowIfNull(predicate);
            return new DynamicJsonConverter<T>(predicate, writer, reader);
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="JsonConverter" /> implementation wrapping <see cref="JsonConverter.CanConvert" /> through <paramref name="typeToConvert" /> and <see cref="JsonConverterFactory.CreateConverter" /> through <paramref name="converterFactory" />.
        /// </summary>
        /// <param name="typeToConvert">The type of the object to convert.</param>
        /// <param name="converterFactory">The function delegate that converts <paramref name="typeToConvert"/> to its JSON representation using a factory pattern.</param>
        /// <returns>An <see cref="JsonConverter" /> implementation of <paramref name="typeToConvert"/>.</returns>
        public static JsonConverter Create(Type typeToConvert, Func<Type, JsonSerializerOptions, JsonConverter> converterFactory)
        {
            Validator.ThrowIfNull(typeToConvert);
            return new DynamicJsonConverterFactory(typeToConvert.IsAssignableFrom, converterFactory);
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="JsonConverter" /> implementation wrapping <see cref="JsonConverter.CanConvert"/> through <paramref name="predicate"/> and <see cref="JsonConverterFactory.CreateConverter" /> through <paramref name="converterFactory" />.
        /// </summary>
        /// <param name="predicate">The function delegate that validates if a given <see cref="Type"/> can be converted to and from JSON.</param>
        /// <param name="converterFactory">The function delegate that converts a given <see cref="Type"/> to its JSON representation using a factory pattern.</param>
        /// <returns>An <see cref="JsonConverter" /> implementation of <see cref="Type"/>.</returns>
        public static JsonConverter Create(Func<Type, bool> predicate, Func<Type, JsonSerializerOptions, JsonConverter> converterFactory)
        {
            Validator.ThrowIfNull(predicate);
            Validator.ThrowIfNull(converterFactory);
            return new DynamicJsonConverterFactory(predicate, converterFactory);
        }
    }

    internal sealed class DynamicJsonConverterFactory : JsonConverterFactory
    {
        public DynamicJsonConverterFactory(Func<Type, bool> predicate, Func<Type, JsonSerializerOptions, JsonConverter> converterFactory)
        {
            Predicate = predicate;
            ConverterFactory = converterFactory;
        }

        private Func<Type, bool> Predicate { get; }

        private Func<Type, JsonSerializerOptions, JsonConverter> ConverterFactory { get; }

        public override bool CanConvert(Type typeToConvert)
        {
            return Predicate(typeToConvert);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return ConverterFactory(typeToConvert, options);
        }
    }

    internal sealed class DynamicJsonConverter<T> : JsonConverter<T>
    {
        internal DynamicJsonConverter(Func<Type, bool> predicate, Utf8JsonWriterAction<T> writer, Utf8JsonReaderFunc<T> reader)
        {
            Predicate = predicate;
            Writer = writer;
            Reader = reader;
        }

        private Func<Type, bool> Predicate { get; }

        private Utf8JsonWriterAction<T> Writer { get; }

        private Utf8JsonReaderFunc<T> Reader { get; }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return Predicate(typeToConvert);
        }

        /// <summary>
        /// Reads and converts the JSON to type <typeparamref name="T" />.
        /// </summary>
        /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="NotImplementedException">Delegate reader is null.</exception>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (Reader == null) { throw new NotImplementedException("Delegate reader is null."); }
            return Reader.Invoke(ref reader, typeToConvert, options);
        }

        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <exception cref="NotImplementedException">Delegate writer is null.</exception>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (Writer == null) { throw new NotImplementedException("Delegate writer is null."); }
            Writer.Invoke(writer, value, options);
        }
    }
}
