using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Xml.Serialization.Converters;
using Cuemon.Xml.Serialization.Formatters;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Xml.Converters
{
    public class XmlConverterExtensionsTest : Test
    {
        public XmlConverterExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddProblemDetailsConverter_ShouldAddTwoConverters()
        {
            var sut = new List<XmlConverter>();
            sut.AddProblemDetailsConverter();
            Assert.Equal(2, sut.Count);
        }

        [Fact]
        public void AddProblemDetailsConverter_ShouldSerializeProblemDetails_WithAllFields()
        {
            var formatter = new XmlFormatter(o => o.Settings.Converters.AddProblemDetailsConverter());
            var pd = new ProblemDetails { Type = "https://example.com/error", Title = "Bad Request", Status = 400, Detail = "Something went wrong", Instance = "/api/test" };
            using var stream = formatter.Serialize(pd, typeof(ProblemDetails));
            var xml = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
            TestOutput.WriteLine(xml);
            Assert.Contains("ProblemDetails", xml);
            Assert.Contains("Bad Request", xml);
            Assert.Contains("400", xml);
            Assert.Contains("Something went wrong", xml);
        }

        [Fact]
        public void AddProblemDetailsConverter_ShouldSerializeProblemDetails_WithNullOptionalFields()
        {
            var formatter = new XmlFormatter(o => o.Settings.Converters.AddProblemDetailsConverter());
            var pd = new ProblemDetails { Status = 500 };
            using var stream = formatter.Serialize(pd, typeof(ProblemDetails));
            var xml = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
            TestOutput.WriteLine(xml);
            Assert.Contains("ProblemDetails", xml);
            Assert.Contains("500", xml);
            Assert.DoesNotContain("<Type>", xml);
        }

        [Fact]
        public void AddHttpExceptionDescriptorConverter_ShouldAddOneConverter()
        {
            var sut = new List<XmlConverter>();
            sut.AddHttpExceptionDescriptorConverter();
            Assert.Single(sut);
        }

        [Fact]
        public void AddHttpExceptionDescriptorConverter_ShouldSerializeHttpExceptionDescriptor()
        {
            var formatter = new XmlFormatter(o => o.Settings.Converters.AddHttpExceptionDescriptorConverter());
            var descriptor = new HttpExceptionDescriptor(new InvalidOperationException("Test error"), 400, "BadRequest", "A bad request occurred");
            using var stream = formatter.Serialize(descriptor, typeof(HttpExceptionDescriptor));
            var xml = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
            TestOutput.WriteLine(xml);
            Assert.Contains("HttpExceptionDescriptor", xml);
            Assert.Contains("BadRequest", xml);
            Assert.Contains("A bad request occurred", xml);
        }

        [Fact]
        public void AddHttpExceptionDescriptorConverter_ShouldIncludeFailure_WhenSensitivityDetailsHasFailureFlag()
        {
            var formatter = new XmlFormatter(o => o.Settings.Converters.AddHttpExceptionDescriptorConverter(s => s.SensitivityDetails = FaultSensitivityDetails.Failure));
            var descriptor = new HttpExceptionDescriptor(new InvalidOperationException("Test error"), 500, "InternalServerError", "An error occurred");
            using var stream = formatter.Serialize(descriptor, typeof(HttpExceptionDescriptor));
            var xml = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
            TestOutput.WriteLine(xml);
            Assert.Contains("Failure", xml);
            Assert.Contains("Test error", xml);
        }

        [Fact]
        public void AddStringValuesConverter_ShouldAddOneConverter()
        {
            var sut = new List<XmlConverter>();
            sut.AddStringValuesConverter();
            Assert.Single(sut);
        }

        [Fact]
        public void AddStringValuesConverter_ShouldSerializeSingleValue()
        {
            var formatter = new XmlFormatter(o => o.Settings.Converters.AddStringValuesConverter());
            var xml = SerializeInsideRoot(formatter, new StringValues("hello"), typeof(StringValues));
            TestOutput.WriteLine(xml);
            Assert.Contains("hello", xml);
        }

        [Fact]
        public void AddStringValuesConverter_ShouldSerializeMultipleValues()
        {
            var formatter = new XmlFormatter(o => o.Settings.Converters.AddStringValuesConverter());
            var values = new StringValues(new[] { "val1", "val2", "val3" });
            var xml = SerializeInsideRoot(formatter, values, typeof(StringValues));
            TestOutput.WriteLine(xml);
            Assert.Contains("val1", xml);
            Assert.Contains("val2", xml);
            Assert.Contains("val3", xml);
            Assert.Contains("<Value>", xml);
        }

        [Fact]
        public void AddHeaderDictionaryConverter_ShouldAddOneConverter()
        {
            var sut = new List<XmlConverter>();
            sut.AddHeaderDictionaryConverter();
            Assert.Single(sut);
        }

        [Fact]
        public void AddHeaderDictionaryConverter_ShouldSerializeHeaders()
        {
            var formatter = new XmlFormatter(o =>
            {
                o.Settings.Converters.AddHeaderDictionaryConverter();
                o.Settings.Converters.AddStringValuesConverter();
            });
            var headers = new HeaderDictionary { { "Content-Type", "application/xml" }, { "X-Custom", "test-value" } };
            var xml = SerializeInsideRoot(formatter, headers, typeof(IHeaderDictionary));
            TestOutput.WriteLine(xml);
            Assert.Contains("Header", xml);
            Assert.Contains("Content-Type", xml);
            Assert.Contains("application/xml", xml);
        }

        [Fact]
        public void AddQueryCollectionConverter_ShouldAddOneConverter()
        {
            var sut = new List<XmlConverter>();
            sut.AddQueryCollectionConverter();
            Assert.Single(sut);
        }

        [Fact]
        public void AddQueryCollectionConverter_ShouldSerializeQueryCollection()
        {
            var formatter = new XmlFormatter(o =>
            {
                o.Settings.Converters.AddQueryCollectionConverter();
                o.Settings.Converters.AddStringValuesConverter();
            });
            var query = new QueryCollection(new Dictionary<string, StringValues> { { "id", new StringValues("42") }, { "name", new StringValues("test") } });
            var xml = SerializeInsideRoot(formatter, query, typeof(IQueryCollection));
            TestOutput.WriteLine(xml);
            Assert.Contains("Field", xml);
            Assert.Contains("id", xml);
            Assert.Contains("42", xml);
        }

        [Fact]
        public void AddFormCollectionConverter_ShouldAddOneConverter()
        {
            var sut = new List<XmlConverter>();
            sut.AddFormCollectionConverter();
            Assert.Single(sut);
        }

        [Fact]
        public void AddFormCollectionConverter_ShouldSerializeFormCollection()
        {
            var formatter = new XmlFormatter(o =>
            {
                o.Settings.Converters.AddFormCollectionConverter();
                o.Settings.Converters.AddStringValuesConverter();
            });
            var form = new FormCollection(new Dictionary<string, StringValues> { { "username", new StringValues("alice") } });
            var xml = SerializeInsideRoot(formatter, form, typeof(IFormCollection));
            TestOutput.WriteLine(xml);
            Assert.Contains("Field", xml);
            Assert.Contains("username", xml);
            Assert.Contains("alice", xml);
        }

        [Fact]
        public void AddCookieCollectionConverter_ShouldAddOneConverter()
        {
            var sut = new List<XmlConverter>();
            sut.AddCookieCollectionConverter();
            Assert.Single(sut);
        }

        [Fact]
        public void AddCookieCollectionConverter_ShouldSerializeCookieCollection()
        {
            var formatter = new XmlFormatter(o => o.Settings.Converters.AddCookieCollectionConverter());
            var cookies = new FakeCookieCollection(new Dictionary<string, string> { { "session", "abc123" }, { "pref", "dark" } });
            var xml = SerializeInsideRoot(formatter, cookies, typeof(IRequestCookieCollection));
            TestOutput.WriteLine(xml);
            Assert.Contains("Field", xml);
            Assert.Contains("session", xml);
            Assert.Contains("abc123", xml);
        }

        // These converters write child elements, designed to be called from within a parent element.
        // This helper wraps the serialization in a root element so the converter lambdas work correctly.
        private static string SerializeInsideRoot(XmlFormatter formatter, object value, Type type)
        {
            using var ms = new MemoryStream();
            var writerSettings = new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Fragment };
            using (var xmlWriter = XmlWriter.Create(ms, writerSettings))
            {
                xmlWriter.WriteStartElement("Root");
                formatter.SerializeToWriter(xmlWriter, value, type);
                xmlWriter.WriteEndElement();
            }
            ms.Position = 0;
            return new StreamReader(ms, Encoding.UTF8).ReadToEnd();
        }

        private class FakeCookieCollection : IRequestCookieCollection
        {
            private readonly Dictionary<string, string> _cookies;

            public FakeCookieCollection(Dictionary<string, string> cookies) => _cookies = cookies;

            public string this[string key] => _cookies.TryGetValue(key, out var v) ? v : null;
            public int Count => _cookies.Count;
            public ICollection<string> Keys => _cookies.Keys;
            public bool ContainsKey(string key) => _cookies.ContainsKey(key);
            public bool TryGetValue(string key, out string value) => _cookies.TryGetValue(key, out value);
            public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _cookies.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => _cookies.GetEnumerator();
        }
    }
}
