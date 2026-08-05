using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Xml;
using Cuemon.Extensions.Xml.Serialization.Converters;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Converters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Cuemon.Extensions.AspNetCore.Xml.Converters
{
    /// <summary>
    /// Extension methods for the <see cref=" XmlConverter"/> class.
    /// </summary>
    public static class XmlConverterExtensions
    {
        /// <summary>
        /// Adds a <see cref="ProblemDetails"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddProblemDetailsConverter(this IList<XmlConverter> converters)
        {
            return converters
                .AddXmlConverter<ProblemDetails>((writer, pd, _) => WriteProblemDetails(writer, pd))
                .AddXmlConverter<IDecorator<ProblemDetails>>((writer, dpd, _) => WriteProblemDetails(writer, dpd.Inner));
        }

        private static void WriteProblemDetails(XmlWriter writer, ProblemDetails pd)
        {
            writer.WriteStartElement(nameof(ProblemDetails));
            if (pd.Type != null) { writer.WriteElementString(nameof(ProblemDetails.Type), pd.Type); }
            if (pd.Title != null) { writer.WriteElementString(nameof(ProblemDetails.Title), pd.Title); }
            if (pd.Status.HasValue) { writer.WriteElementString(nameof(ProblemDetails.Status), pd.Status.Value.ToString(CultureInfo.InvariantCulture)); }
            if (pd.Detail != null) { writer.WriteElementString(nameof(ProblemDetails.Detail), pd.Detail); }
            if (pd.Instance != null) { writer.WriteElementString(nameof(ProblemDetails.Instance), pd.Instance); }

            foreach (var extension in pd.Extensions.Where(kvp => kvp.Value != null))
            {
                writer.WriteObject(extension.Value, extension.Value.GetType(), o => o.Settings.RootName = new XmlQualifiedEntity(extension.Key));
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Adds an <see cref="ExceptionDescriptor"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="IList{XmlConverter}" /> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddHttpExceptionDescriptorConverter(this IList<XmlConverter> converters, Action<ExceptionDescriptorOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            return converters.AddXmlConverter<HttpExceptionDescriptor>((writer, descriptor, _) =>
            {
                writer.WriteStartElement("HttpExceptionDescriptor");
                writer.WriteStartElement("Error");
                if (descriptor.Instance != null) { writer.WriteElementString("Instance", descriptor.Instance.OriginalString); }
                writer.WriteElementString("Status", descriptor.StatusCode.ToString(CultureInfo.InvariantCulture));
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
                        writer.WriteObject(evidence.Value, evidence.Value.GetType(), o => o.Settings.RootName = new XmlQualifiedEntity(evidence.Key));
                    }
                    writer.WriteEndElement();
                }
                if (!string.IsNullOrWhiteSpace(descriptor.CorrelationId)) { writer.WriteElementString("CorrelationId", descriptor.CorrelationId); }
                if (!string.IsNullOrWhiteSpace(descriptor.RequestId)) { writer.WriteElementString("RequestId", descriptor.RequestId); }
                if (!string.IsNullOrWhiteSpace(descriptor.TraceId)) { writer.WriteElementString("TraceId", descriptor.TraceId); }
                writer.WriteEndElement();
            });
        }

        /// <summary>
        /// Adds an <see cref="StringValues"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddStringValuesConverter(this IList<XmlConverter> converters)
        {
            return converters.InsertXmlConverter<StringValues>(0, (writer, values, _) =>
            {
                if (values.Count <= 1)
                {
                    writer.WriteValue(values.ToString());
                }
                else
                {
                    foreach (var value in values) { writer.WriteElementString("Value", value); }
                }
            });
        }

        /// <summary>
        /// Adds an <see cref="IHeaderDictionary"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddHeaderDictionaryConverter(this IList<XmlConverter> converters)
        {
            return converters.InsertXmlConverter<IHeaderDictionary>(0, (writer, headers, _) =>
            {
                foreach (var header in headers)
                {
                    writer.WriteStartElement("Header");
                    writer.WriteAttributeString("name", header.Key);
                    writer.WriteObject(header.Value);
                    writer.WriteEndElement();
                }
            });
        }

        /// <summary>
        /// Adds an <see cref="IQueryCollection"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddQueryCollectionConverter(this IList<XmlConverter> converters)
        {
            return converters.InsertXmlConverter<IQueryCollection>(0, (writer, collection, _) =>
            {
                foreach (var pair in collection)
                {
                    writer.WriteStartElement("Field");
                    writer.WriteAttributeString("key", pair.Key);
                    writer.WriteObject(pair.Value);
                    writer.WriteEndElement();
                }
            });
        }

        /// <summary>
        /// Adds an <see cref="IFormCollection"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddFormCollectionConverter(this IList<XmlConverter> converters)
        {
            return converters.InsertXmlConverter<IFormCollection>(0, (writer, collection, _) =>
            {
                foreach (var pair in collection)
                {
                    writer.WriteStartElement("Field");
                    writer.WriteAttributeString("key", pair.Key);
                    writer.WriteObject(pair.Value);
                    writer.WriteEndElement();
                }
            });
        }

        /// <summary>
        /// Adds an <see cref="IRequestCookieCollection"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddCookieCollectionConverter(this IList<XmlConverter> converters)
        {
            return converters.InsertXmlConverter<IRequestCookieCollection>(0, (writer, collection, _) =>
            {
                foreach (var pair in collection)
                {
                    writer.WriteStartElement("Field");
                    writer.WriteAttributeString("key", pair.Key);
                    writer.WriteValue(pair.Value);
                    writer.WriteEndElement();
                }
            });
        }
    }
}
