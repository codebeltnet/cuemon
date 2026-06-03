using System;
using System.Collections.Generic;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Cuemon.Diagnostics;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Converters;
using Xunit;

namespace Cuemon.Extensions.Xml.Serialization.Converters
{
    public class CoverageTest : Test
    {
        public CoverageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void XmlConverterExtensions_ShouldAddAndLocateConverters()
        {
            IList<XmlConverter> converters = new List<XmlConverter>();

            converters.InsertXmlConverter<string>(0, (writer, value, entity) => writer.WriteElementString(entity?.LocalName ?? "Value", value), (reader, type) => "alpha", type => type == typeof(string), new XmlQualifiedEntity("String"));
            converters.AddXmlConverter<int>((writer, value, entity) => writer.WriteElementString(entity?.LocalName ?? "Value", value.ToString()), (reader, type) => 42, type => type == typeof(int), new XmlQualifiedEntity("Int32"));
            converters.AddEnumerableConverter();
            converters.AddExceptionDescriptorConverter(o => { });
            converters.AddUriConverter();
            converters.AddDateTimeConverter();
            converters.AddTimeSpanConverter();
            converters.AddStringConverter();
            converters.AddExceptionConverter(true, true);
            converters.AddFailureConverter();

            Assert.NotNull(converters.FirstOrDefaultWriterConverter(typeof(string)));
            Assert.NotNull(converters.FirstOrDefaultReaderConverter(typeof(string)));
            Assert.NotNull(converters.FirstOrDefaultWriterConverter(typeof(int)));
            Assert.NotNull(converters.FirstOrDefaultReaderConverter(typeof(int)));
            Assert.NotNull(converters.FirstOrDefaultWriterConverter(typeof(List<string>)));
            Assert.NotNull(converters.FirstOrDefaultReaderConverter(typeof(List<string>)));
            Assert.NotNull(converters.FirstOrDefaultWriterConverter(typeof(Uri)));
            Assert.NotNull(converters.FirstOrDefaultReaderConverter(typeof(Uri)));
            Assert.NotNull(converters.FirstOrDefaultWriterConverter(typeof(DateTime)));
            Assert.NotNull(converters.FirstOrDefaultReaderConverter(typeof(DateTime)));
            Assert.NotNull(converters.FirstOrDefaultWriterConverter(typeof(TimeSpan)));
            Assert.NotNull(converters.FirstOrDefaultReaderConverter(typeof(TimeSpan)));
            Assert.NotNull(converters.FirstOrDefaultWriterConverter(typeof(Failure)));
            Assert.NotNull(converters.FirstOrDefaultWriterConverter(typeof(Exception)));
            Assert.NotNull(converters.FirstOrDefaultWriterConverter(typeof(ExceptionDescriptor)));
            Assert.Throws<ArgumentNullException>(() => XmlConverterExtensions.FirstOrDefaultWriterConverter(null, typeof(string)));
        }
    }
}
