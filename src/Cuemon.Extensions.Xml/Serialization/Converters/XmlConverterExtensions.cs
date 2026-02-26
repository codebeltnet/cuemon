using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Cuemon.Diagnostics;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Converters;

namespace Cuemon.Extensions.Xml.Serialization.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="XmlConverter"/> class.
    /// </summary>
    public static class XmlConverterExtensions
    {
        /// <summary>
        /// Returns the first <see cref="XmlConverter"/> of the <paramref name="converters"/> that <see cref="XmlConverter.CanConvert"/> and <see cref="XmlConverter.CanRead"/> the specified <paramref name="objectType"/>; otherwise <c>null</c> if no <see cref="XmlConverter"/> is found.
        /// </summary>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <param name="objectType">Type of the object to deserialize.</param>
        /// <returns>An <see cref="XmlConverter"/> that can deserialize the specified <paramref name="objectType"/>; otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static XmlConverter FirstOrDefaultReaderConverter(this IList<XmlConverter> converters, Type objectType)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).FirstOrDefaultReaderConverter(objectType);
        }

        /// <summary>
        /// Returns the first <see cref="XmlConverter"/> of the <paramref name="converters"/> that <see cref="XmlConverter.CanConvert"/> and <see cref="XmlConverter.CanWrite"/> the specified <paramref name="objectType"/>; otherwise <c>null</c> if no <see cref="XmlConverter"/> is found.
        /// </summary>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <param name="objectType">Type of the object to serialize.</param>
        /// <returns>An <see cref="XmlConverter"/> that can serialize the specified <paramref name="objectType"/>; otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static XmlConverter FirstOrDefaultWriterConverter(this IList<XmlConverter> converters, Type objectType)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).FirstOrDefaultWriterConverter(objectType);
        }

        /// <summary>
        /// Adds an XML converter to the list.
        /// </summary>
        /// <typeparam name="T">The type of the object to converts to and from XML.</typeparam>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <param name="writer">The delegate that converts <typeparamref name="T"/> to its XML representation.</param>
        /// <param name="reader">The delegate that generates <typeparamref name="T"/> from its XML representation.</param>
        /// <param name="canConvertPredicate">The delegate that determines if an object can be converted.</param>
        /// <param name="qe">The optional <seealso cref="XmlQualifiedEntity"/> that will provide the name of the root element.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static IList<XmlConverter> AddXmlConverter<T>(this IList<XmlConverter> converters, Action<XmlWriter, T, XmlQualifiedEntity> writer = null, Func<XmlReader, Type, T> reader = null, Func<Type, bool> canConvertPredicate = null, XmlQualifiedEntity qe = null)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).AddXmlConverter(writer, reader, canConvertPredicate, qe).Inner;
        }

        /// <summary>
        /// Inserts an XML converter to the list at the specified <paramref name="index" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to converts to and from XML.</typeparam>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <param name="index">The zero-based index at which an XML converter should be inserted.</param>
        /// <param name="writer">The delegate that converts <typeparamref name="T" /> to its XML representation.</param>
        /// <param name="reader">The delegate that generates <typeparamref name="T" /> from its XML representation.</param>
        /// <param name="canConvertPredicate">The delegate that determines if an object can be converted.</param>
        /// <param name="qe">The optional <seealso cref="XmlQualifiedEntity"/> that will provide the name of the root element.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static IList<XmlConverter> InsertXmlConverter<T>(this IList<XmlConverter> converters, int index, Action<XmlWriter, T, XmlQualifiedEntity> writer = null, Func<XmlReader, Type, T> reader = null, Func<Type, bool> canConvertPredicate = null, XmlQualifiedEntity qe = null)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).InsertXmlConverter(index, writer, reader, canConvertPredicate, qe).Inner;
        }

        /// <summary>
        /// Adds an <see cref="IEnumerable"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <param name="flattenItems">When <c>true</c> and a qualified element name is available, each collection item is serialized as a repeated element using that name instead of being wrapped in a generic <c>Item</c> element. The default is <c>false</c>.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static IList<XmlConverter> AddEnumerableConverter(this IList<XmlConverter> converters, bool flattenItems = false)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).AddEnumerableConverter(flattenItems).Inner;
        }

        /// <summary>
        /// Adds an <see cref="ExceptionDescriptor"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static IList<XmlConverter> AddExceptionDescriptorConverter(this IList<XmlConverter> converters, Action<ExceptionDescriptorOptions> setup)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).AddExceptionDescriptorConverter(setup).Inner;
        }

        /// <summary>
        /// Adds a <see cref="Uri"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static IList<XmlConverter> AddUriConverter(this IList<XmlConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).AddUriConverter().Inner;
        }

        /// <summary>
        /// Adds an <see cref="DateTime"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static IList<XmlConverter> AddDateTimeConverter(this IList<XmlConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).AddDateTimeConverter().Inner;
        }

        /// <summary>
        /// Adds an <see cref="TimeSpan"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static IList<XmlConverter> AddTimeSpanConverter(this IList<XmlConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).AddTimeSpanConverter().Inner;
        }

        /// <summary>
        /// Adds an <see cref="string"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static IList<XmlConverter> AddStringConverter(this IList<XmlConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).AddStringConverter().Inner;
        }

        /// <summary>
        /// Adds an <see cref="Exception" /> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <param name="includeStackTrace">The value that determine whether the stack of an exception is included in the converted result.</param>
        /// <param name="includeData">The value that determine whether the data of an exception is included in the converted result.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static IList<XmlConverter> AddExceptionConverter(this IList<XmlConverter> converters, bool includeStackTrace, bool includeData)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).AddExceptionConverter(includeStackTrace, includeData).Inner;
        }

        /// <summary>
        /// Adds an <see cref="Failure" /> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static IList<XmlConverter> AddFailureConverter(this IList<XmlConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            return Decorator.Enclose(converters).AddFailureConverter().Inner;
        }
    }
}
