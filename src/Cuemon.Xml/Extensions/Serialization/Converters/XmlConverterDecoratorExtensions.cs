using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Runtime;
using Cuemon.Xml.Linq;

namespace Cuemon.Xml.Serialization.Converters;
/// <summary>
/// Extension methods for the <see cref="XmlConverter"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class XmlConverterDecoratorExtensions
{
    /// <summary>
    /// Returns the first <see cref="XmlConverter"/> of the enclosed <see cref="IList{XmlConverter}"/> of the specified <paramref name="decorator"/> that <see cref="XmlConverter.CanConvert"/> and <see cref="XmlConverter.CanRead"/> the specified <paramref name="objectType"/>; otherwise <c>null</c> if no <see cref="XmlConverter"/> is found.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <param name="objectType">Type of the object to deserialize.</param>
    /// <returns>An <see cref="XmlConverter"/> that can deserialize the specified <paramref name="objectType"/>; otherwise <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static XmlConverter FirstOrDefaultReaderConverter(this IDecorator<IList<XmlConverter>> decorator, Type objectType)
    {
        Validator.ThrowIfNull(decorator);
        return decorator.Inner.FirstOrDefault(c => c.CanConvert(objectType) && c.CanRead);
    }

    /// <summary>
    /// Returns the first <see cref="XmlConverter"/> of the enclosed <see cref="IList{XmlConverter}"/> of the specified <paramref name="decorator"/> that <see cref="XmlConverter.CanConvert"/> and <see cref="XmlConverter.CanWrite"/> the specified <paramref name="objectType"/>; otherwise <c>null</c> if no <see cref="XmlConverter"/> is found.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <param name="objectType">Type of the object to serialize.</param>
    /// <returns>An <see cref="XmlConverter"/> that can serialize the specified <paramref name="objectType"/>; otherwise <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static XmlConverter FirstOrDefaultWriterConverter(this IDecorator<IList<XmlConverter>> decorator, Type objectType)
    {
        Validator.ThrowIfNull(decorator);
        return decorator.Inner.FirstOrDefault(c => c.CanConvert(objectType) && c.CanWrite);
    }

    /// <summary>
    /// Adds an XML converter to the enclosed <see cref="IList{XmlConverter}" /> of the specified <paramref name="decorator" />.
    /// </summary>
    /// <typeparam name="T">The type of the object to converts to and from XML.</typeparam>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <param name="writer">The delegate that converts <typeparamref name="T" /> to its XML representation.</param>
    /// <param name="reader">The delegate that generates <typeparamref name="T" /> from its XML representation.</param>
    /// <param name="canConvertPredicate">The delegate that determines if an object can be converted.</param>
    /// <param name="qe">The optional <seealso cref="XmlQualifiedEntity" /> that will provide the name of the root element.</param>
    /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IDecorator<IList<XmlConverter>> AddXmlConverter<T>(this IDecorator<IList<XmlConverter>> decorator, Action<XmlWriter, T, XmlQualifiedEntity> writer = null, Func<XmlReader, Type, T> reader = null, Func<Type, bool> canConvertPredicate = null, XmlQualifiedEntity qe = null)
    {
        Validator.ThrowIfNull(decorator);
        decorator.Inner.Add(DynamicXmlConverter.Create(writer, reader, canConvertPredicate, qe));
        return decorator;
    }

    /// <summary>
    /// Inserts an XML converter to the enclosed <see cref="IList{XmlConverter}"/> of the specified <paramref name="decorator"/> at the specified <paramref name="index" />.
    /// </summary>
    /// <typeparam name="T">The type of the object to converts to and from XML.</typeparam>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <param name="index">The zero-based index at which an XML converter should be inserted.</param>
    /// <param name="writer">The delegate that converts <typeparamref name="T" /> to its XML representation.</param>
    /// <param name="reader">The delegate that generates <typeparamref name="T" /> from its XML representation.</param>
    /// <param name="canConvertPredicate">The delegate that determines if an object can be converted.</param>
    /// <param name="qe">The optional <seealso cref="XmlQualifiedEntity"/> that will provide the name of the root element.</param>
    /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IDecorator<IList<XmlConverter>> InsertXmlConverter<T>(this IDecorator<IList<XmlConverter>> decorator, int index, Action<XmlWriter, T, XmlQualifiedEntity> writer = null, Func<XmlReader, Type, T> reader = null, Func<Type, bool> canConvertPredicate = null, XmlQualifiedEntity qe = null)
    {
        Validator.ThrowIfNull(decorator);
        decorator.Inner.Insert(index, DynamicXmlConverter.Create(writer, reader, canConvertPredicate, qe));
        return decorator;
    }

    /// <summary>
    /// Adds an <see cref="IEnumerable"/> XML converter to the enclosed <see cref="IList{XmlConverter}"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <param name="flattenItems">When <c>true</c> and a qualified element name is available, each collection item is serialized as a repeated element using that name instead of being wrapped in a generic <c>Item</c> element. The default is <c>false</c>.</param>
    /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IDecorator<IList<XmlConverter>> AddEnumerableConverter(this IDecorator<IList<XmlConverter>> decorator, bool flattenItems = false)
    {
        Validator.ThrowIfNull(decorator);
        decorator.AddXmlConverter<IEnumerable>((w, o, q) =>
        {
            if (w.WriteState == WriteState.Start && q == null && !(o is IDictionary || o is IList)) { q = new XmlQualifiedEntity("Enumerable"); }

            var seqType = o.GetType();
            var hasKeyValuePairType = seqType.GetGenericArguments().Any(gt => Decorator.Enclose(gt).HasKeyValuePairImplementation());
            var isDictionaryLike = Decorator.Enclose(seqType).HasDictionaryImplementation() || hasKeyValuePairType;

            if (flattenItems && q != null)
            {
                if (isDictionaryLike)
                {
                    w.WriteStartElement(q.Prefix, q.LocalName, q.Namespace);
                    foreach (var element in o)
                    {
                        var elementType = element.GetType();
                        var keyProperty = elementType.GetProperty("Key");
                        var valueProperty = elementType.GetProperty("Value");
                        var keyValue = keyProperty.GetValue(element, null);
                        var valueValue = valueProperty.GetValue(element, null);
                        var valuePropertyType = valueProperty.PropertyType;
                        if (valuePropertyType == typeof(object) && valueValue != null) { valuePropertyType = valueValue.GetType(); }
                        var keyName = Decorator.Enclose(keyValue.ToString()).SanitizeXmlElementName();
                        if (Decorator.Enclose(valuePropertyType).IsComplex())
                        {
                            Decorator.Enclose(w).WriteObject(valueValue, valuePropertyType, opts => opts.Settings.RootName = new XmlQualifiedEntity(keyName, q.Namespace));
                        }
                        else
                        {
                            w.WriteStartElement(keyName, q.Namespace);
                            w.WriteValue(valueValue);
                            w.WriteEndElement();
                        }
                    }
                    w.WriteEndElement();
                }
                else
                {
                    w.WriteStartElement(q.Prefix, q.LocalName, q.Namespace);
                    foreach (var item in o)
                    {
                        if (item == null) { continue; }
                        var itemType = item.GetType();
                        if (Decorator.Enclose(itemType).IsComplex())
                        {
                            Decorator.Enclose(w).WriteObject(item, itemType);
                        }
                        else
                        {
                            w.WriteStartElement(q.Prefix, q.LocalName, q.Namespace);
                            w.WriteValue(item);
                            w.WriteEndElement();
                        }
                    }
                    w.WriteEndElement();
                }
            }
            else
            {
                Decorator.Enclose(w).WriteXmlRootElement(o, (writer, sequence, _) =>
                {
                    if (isDictionaryLike)
                    {
                        foreach (var element in sequence)
                        {
                            var elementType = element.GetType();
                            var keyProperty = elementType.GetProperty("Key");
                            var valueProperty = elementType.GetProperty("Value");
                            var keyValue = keyProperty.GetValue(element, null);
                            var valueValue = valueProperty.GetValue(element, null);
                            var valuePropertyType = valueProperty.PropertyType;
                            if (valuePropertyType == typeof(object) && valueValue != null) { valuePropertyType = valueValue.GetType(); }
                            writer.WriteStartElement("Item");
                            writer.WriteAttributeString("name", keyValue.ToString());
                            if (Decorator.Enclose(valuePropertyType).IsComplex())
                            {
                                Decorator.Enclose(writer).WriteObject(valueValue, valuePropertyType);
                            }
                            else
                            {
                                writer.WriteValue(valueValue);
                            }
                            writer.WriteEndElement();
                        }
                    }
                    else
                    {
                        foreach (var item in sequence)
                        {
                            if (item == null) { continue; }
                            var itemType = item.GetType();
                            writer.WriteStartElement("Item");
                            if (Decorator.Enclose(itemType).IsComplex())
                            {
                                Decorator.Enclose(writer).WriteObject(item, itemType);
                            }
                            else
                            {
                                writer.WriteValue(item);
                            }
                            writer.WriteEndElement();
                        }
                    }
                }, q);
            }
        }, (reader, type) => Decorator.Enclose(type).HasDictionaryImplementation() ? Decorator.Enclose(Decorator.Enclose(reader).ToHierarchy()).UseDictionary(type.GetGenericArguments()) : Decorator.Enclose(Decorator.Enclose(reader).ToHierarchy()).UseCollection(type.GetGenericArguments().First()), type => type != typeof(string));
        return decorator;
    }

    /// <summary>
    /// Adds an <see cref="ExceptionDescriptor"/> XML converter to the enclosed <see cref="IList{XmlConverter}"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which need to be configured.</param>
    /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IDecorator<IList<XmlConverter>> AddExceptionDescriptorConverter(this IDecorator<IList<XmlConverter>> decorator, Action<ExceptionDescriptorOptions> setup)
    {
        Validator.ThrowIfNull(decorator);
        decorator.AddXmlConverter<ExceptionDescriptor>((writer, descriptor, _) =>
        {
            var options = Patterns.Configure(setup);
            writer.WriteStartElement("ExceptionDescriptor");
            writer.WriteStartElement("Error");
            writer.WriteElementString("Code", descriptor.Code);
            writer.WriteElementString("Message", descriptor.Message);
            if (descriptor.HelpLink != null) { writer.WriteElementString("HelpLink", descriptor.HelpLink.OriginalString); }
            if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Failure))
            {
                writer.WriteStartElement("Failure");
                new ExceptionConverter(options.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data)).WriteXml(writer, descriptor.Failure);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence) && descriptor.Evidence.Any())
            {
                writer.WriteStartElement("Evidence");
                foreach (var evidence in descriptor.Evidence)
                {
                    if (evidence.Value == null) { continue; }
                    Decorator.Enclose(writer).WriteObject(evidence.Value, evidence.Value.GetType(), o => o.Settings.RootName = new XmlQualifiedEntity(evidence.Key));
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }, canConvertPredicate: type => type == typeof(ExceptionDescriptor));
        return decorator;
    }

    /// <summary>
    /// Adds a <see cref="Uri"/> XML converter to the enclosed <see cref="IList{XmlConverter}"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IDecorator<IList<XmlConverter>> AddUriConverter(this IDecorator<IList<XmlConverter>> decorator)
    {
        Validator.ThrowIfNull(decorator);
        decorator.AddXmlConverter((w, d, q) =>
        {
            if (w.WriteState == WriteState.Start && q == null) { q = new XmlQualifiedEntity(Decorator.Enclose(typeof(Uri)).ToFriendlyName()); }
            Decorator.Enclose(w).WriteEncapsulatingElementIfNotNull(d, q, (writer, value) =>
            {
                writer.WriteValue(value.OriginalString);
            });
        }, (reader, _) => Decorator.Enclose(Decorator.Enclose(reader).ToHierarchy()).UseUriFormatter());
        return decorator;
    }

    /// <summary>
    /// Adds an <see cref="DateTime"/> XML converter to the enclosed <see cref="IList{XmlConverter}"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IDecorator<IList<XmlConverter>> AddDateTimeConverter(this IDecorator<IList<XmlConverter>> decorator)
    {
        Validator.ThrowIfNull(decorator);
        decorator.AddXmlConverter((w, d, q) =>
        {
            if (w.WriteState == WriteState.Start && q == null) { q = new XmlQualifiedEntity(Decorator.Enclose(typeof(DateTime)).ToFriendlyName()); }
            Decorator.Enclose(w).WriteEncapsulatingElementIfNotNull(d, q, (writer, value) =>
            {
                writer.WriteValue(value.ToString("O", CultureInfo.InvariantCulture));
            });
        }, (reader, _) => Decorator.Enclose(Decorator.Enclose(reader).ToHierarchy()).UseDateTimeFormatter());
        return decorator;
    }

    /// <summary>
    /// Adds an <see cref="TimeSpan"/> XML converter to the enclosed <see cref="IList{XmlConverter}"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IDecorator<IList<XmlConverter>> AddTimeSpanConverter(this IDecorator<IList<XmlConverter>> decorator)
    {
        Validator.ThrowIfNull(decorator);
        decorator.AddXmlConverter((w, d, q) =>
        {
            if (w.WriteState == WriteState.Start && q == null) { q = new XmlQualifiedEntity(Decorator.Enclose(typeof(TimeSpan)).ToFriendlyName()); }
            Decorator.Enclose(w).WriteEncapsulatingElementIfNotNull(d, q, (writer, value) =>
            {
                writer.WriteValue(value.ToString());
            });
        }, (reader, _) =>
        {
            var decoratorHierarchy = Decorator.Enclose(Decorator.Enclose(reader).ToHierarchy());
            return decoratorHierarchy.Inner.Instance.Type == typeof(DateTime) ? Decorator.Enclose(decoratorHierarchy.Inner.Instance.Value).ChangeTypeOrDefault<DateTime>().TimeOfDay : TimeSpan.Parse(decoratorHierarchy.Inner.Instance.Value.ToString(), CultureInfo.InvariantCulture);
        });
        return decorator;
    }

    /// <summary>
    /// Adds an <see cref="string"/> XML converter to the enclosed <see cref="IList{XmlConverter}"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IDecorator<IList<XmlConverter>> AddStringConverter(this IDecorator<IList<XmlConverter>> decorator)
    {
        Validator.ThrowIfNull(decorator);
        decorator.AddXmlConverter<string>((w, s, q) =>
        {
            if (string.IsNullOrWhiteSpace(s)) { return; }
            if (w.WriteState == WriteState.Start && q == null) { q = new XmlQualifiedEntity(Decorator.Enclose(typeof(string)).ToFriendlyName()); }
            Decorator.Enclose(w).WriteEncapsulatingElementIfNotNull(s, q, (writer, value) =>
            {
                if (Decorator.Enclose(value).IsXmlString())
                {
                    writer.WriteCData(value);
                }
                else
                {
                    writer.WriteValue(value);
                }
            });
        });
        return decorator;
    }

    /// <summary>
    /// Adds an <see cref="Exception" /> XML converter to the enclosed <see cref="IList{XmlConverter}"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <param name="includeStackTrace">The value that determine whether the stack of an exception is included in the converted result.</param>
    /// <param name="includeData">The value that determine whether the data of an exception is included in the converted result.</param>
    /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IDecorator<IList<XmlConverter>> AddExceptionConverter(this IDecorator<IList<XmlConverter>> decorator, bool includeStackTrace, bool includeData)
    {
        Validator.ThrowIfNull(decorator);
        decorator.Inner.Add(new ExceptionConverter(includeStackTrace, includeData));
        return decorator;
    }

    /// <summary>
    /// Adds an <see cref="Failure" /> XML converter to the enclosed <see cref="IList{XmlConverter}"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The decorator that wraps the <see cref="IList{XmlConverter}" /> to extend.</param>
    /// <returns>A reference to <paramref name="decorator"/> after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IDecorator<IList<XmlConverter>> AddFailureConverter(this IDecorator<IList<XmlConverter>> decorator)
    {
        Validator.ThrowIfNull(decorator);
        decorator.Inner.Add(new FailureConverter());
        return decorator;
    }
}
