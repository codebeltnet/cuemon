using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Cuemon.Reflection;
using Cuemon.Runtime.Serialization.Formatters;

namespace Cuemon.Xml.Serialization.Converters;
/// <summary>
/// Converts an <see cref="Exception"/> to XML.
/// </summary>
/// <seealso cref="XmlConverter" />
public class ExceptionConverter : XmlConverter<Exception>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionConverter"/> class.
    /// </summary>
    /// <param name="includeStackTrace">A value that indicates if the stack of an exception is included in the converted result.</param>
    /// <param name="includeData">A value that indicates if the data of an exception is included in the converted result.</param>
    public ExceptionConverter(bool includeStackTrace = false, bool includeData = false)
    {
        IncludeStackTrace = includeStackTrace;
        IncludeData = includeData;
    }

    /// <summary>
    /// Gets a value indicating whether the data of an exception is included in the converted result.
    /// </summary>
    /// <value><c>true</c> if the data of an exception is included in the converted result; otherwise, <c>false</c>.</value>
    public bool IncludeData { get; }

    /// <summary>
    /// Gets a value indicating whether the stack of an exception is included in the converted result.
    /// </summary>
    /// <value><c>true</c> if the stack of an exception is included in the converted result; otherwise, <c>false</c>.</value>
    public bool IncludeStackTrace { get; }

    /// <summary>
    /// Writes the XML representation of the <paramref name="value" />.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter" /> to write to.</param>
    /// <param name="value">The object to serialize.</param>
    /// <param name="elementName">The element name to encapsulate around <paramref name="value" />.</param>
    public override void WriteXml(XmlWriter writer, Exception value, XmlQualifiedEntity elementName = null)
    {
        var exceptionType = value.GetType();
        writer.WriteStartElement(Decorator.Enclose(exceptionType.Name).SanitizeXmlElementName());
        if (exceptionType.Namespace != null) { writer.WriteAttributeString("namespace", exceptionType.Namespace); }
        WriteExceptionCore(writer, value, IncludeStackTrace, IncludeData);
        writer.WriteEndElement();
    }

    /// <summary>
    /// Reads the XML representation of the <paramref name="objectType" />.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader" /> to read from.</param>
    /// <param name="objectType">The <seealso cref="Type" /> of the object.</param>
    /// <returns>An object of <paramref name="objectType" />.</returns>
    public override Exception ReadXml(Type objectType, XmlReader reader)
    {
        var stack = ParseXmlReader(reader, objectType);
        return Decorator.Enclose(stack).CreateException(true);
    }

    private static Stack<IList<MemberArgument>> ParseXmlReader(XmlReader reader, Type objectType)
    {
        var stack = new Stack<IList<MemberArgument>>();
        var properties = new List<PropertyInfo>();
        string exception = null;
        string lastException = null;
        var blueprints = new List<MemberArgument>();
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    exception = reader.Name.EndsWith("Exception", StringComparison.Ordinal) ? reader.Name : lastException;

                    if (blueprints.Count > 0
                        && blueprints.Single(ma => ma.Name == "Type") is { } typeOfException
                        && !((Type)typeOfException.Value).Name.Equals(exception, StringComparison.OrdinalIgnoreCase))
                    {
                        stack.Push(blueprints);
                        blueprints = new List<MemberArgument>();
                    }

                    var memberName = MapOrDefault(reader.Name);
                    var property = properties.SingleOrDefault(pi => pi.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
                    if (property != null)
                    {
                        if (property.Name == nameof(Exception.InnerException) && reader.MoveToAttribute("namespace"))
                        {
                            objectType = Formatter.GetType($$"""{{reader.Value}}.{{exception}}""");
                            properties = objectType.GetProperties(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).ToList();
                            blueprints.Add(new MemberArgument("Type", objectType));
                            blueprints.Add(new MemberArgument(memberName, null));
                        }
                        else
                        {
                            reader.Read();
                            blueprints.Add(new MemberArgument(memberName, reader.Value));
                        }
                    }
                    else
                    {
                        if (memberName == nameof(Exception.InnerException) && reader.MoveToAttribute("namespace"))
                        {
                            objectType = Formatter.GetType($$"""{{reader.Value}}.{{exception}}""");
                            properties = objectType.GetProperties(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).ToList();
                            blueprints.Add(new MemberArgument("Type", objectType));
                            blueprints.Add(new MemberArgument(nameof(Exception.InnerException), null));
                        }
                    }
                    break;
            }

            lastException = exception;
        }

        if (blueprints.Count > 0) { stack.Push(blueprints); }

        return stack;
    }

    private static string MapOrDefault(string memberName)
    {
        switch (memberName.ToLowerInvariant())
        {
            case { } when memberName.EndsWith("Exception", StringComparison.Ordinal):
                return nameof(Exception.InnerException);
            case "Stack":
                return nameof(Exception.StackTrace);
            default:
                return memberName;
        }
    }

    private static void WriteExceptionCore(XmlWriter writer, Exception exception, bool includeStackTrace, bool includeData)
    {
        if (!string.IsNullOrEmpty(exception.Source))
        {
            writer.WriteElementString("Source", exception.Source);
        }

        if (!string.IsNullOrEmpty(exception.Message))
        {
            writer.WriteElementString("Message", exception.Message);
        }

        if (exception.StackTrace != null && includeStackTrace)
        {
            writer.WriteStartElement("Stack");
            var lines = exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                writer.WriteElementString("Frame", line.Trim());
            }
            writer.WriteEndElement();
        }

        if (includeData && exception.Data.Count > 0)
        {
            writer.WriteStartElement("Data");
            foreach (DictionaryEntry entry in exception.Data)
            {
                writer.WriteStartElement(Decorator.Enclose(entry.Key.ToString()).SanitizeXmlElementName());
                writer.WriteString(Decorator.Enclose(entry.Value?.ToString()).SanitizeXmlElementText());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>();
        foreach (var property in properties)
        {
            var value = property.GetValue(exception);
            if (value == null) { continue; }
            Decorator.Enclose(writer).WriteObject(value, value.GetType(), o => o.Settings.RootName = new XmlQualifiedEntity(property.Name));
        }

        WriteInnerExceptions(writer, exception, includeStackTrace, includeData);
    }

    private static void WriteInnerExceptions(XmlWriter writer, Exception exception, bool includeStackTrace, bool includeData)
    {
        var innerExceptions = new List<Exception>();
        if (exception is AggregateException aggregated)
        {
            innerExceptions.AddRange(aggregated.Flatten().InnerExceptions);
        }
        else
        {
            if (exception.InnerException != null) { innerExceptions.Add(exception.InnerException); }
        }
        if (innerExceptions.Count > 0)
        {
            var endElementsToWrite = 0;
            foreach (var inner in innerExceptions)
            {
                var exceptionType = inner.GetType();
                writer.WriteStartElement(Decorator.Enclose(exceptionType.Name).SanitizeXmlElementName());
                if (exceptionType.Namespace != null) { writer.WriteAttributeString("namespace", exceptionType.Namespace); }
                WriteExceptionCore(writer, inner, includeStackTrace, includeData);
                endElementsToWrite++;
            }
            for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndElement(); }
        }
    }
}
