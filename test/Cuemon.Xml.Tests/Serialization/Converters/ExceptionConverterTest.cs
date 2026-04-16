using System;
using System.IO;
using System.Xml;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Xml.Serialization.Converters
{
    public class ExceptionConverterTest : Test
    {
        public ExceptionConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        private static string WriteException(Exception exception, bool includeStackTrace = false, bool includeData = false)
        {
            var sut = new ExceptionConverter(includeStackTrace, includeData);
            using var ms = new MemoryStream();
            using var writer = XmlWriter.Create(ms, new XmlWriterSettings { OmitXmlDeclaration = true });
            sut.WriteXml(writer, exception, null);
            writer.Flush();
            ms.Position = 0;
            return new StreamReader(ms).ReadToEnd();
        }

        [Fact]
        public void Ctor_ShouldHaveExpectedDefaults()
        {
            var sut = new ExceptionConverter();
            Assert.False(sut.IncludeStackTrace);
            Assert.False(sut.IncludeData);
        }

        [Fact]
        public void Ctor_WithParameters_ShouldSetProperties()
        {
            var sut = new ExceptionConverter(includeStackTrace: true, includeData: true);
            Assert.True(sut.IncludeStackTrace);
            Assert.True(sut.IncludeData);
        }

        [Fact]
        public void WriteXml_ShouldIncludeExceptionTypeAndNamespace()
        {
            var xml = WriteException(new InvalidOperationException("Oops"));
            TestOutput.WriteLine(xml);
            Assert.Contains("<InvalidOperationException namespace=\"System\">", xml);
        }

        [Fact]
        public void WriteXml_ShouldIncludeMessage_WhenNotEmpty()
        {
            var xml = WriteException(new Exception("My message"));
            TestOutput.WriteLine(xml);
            Assert.Contains("<Message>My message</Message>", xml);
        }

        [Fact]
        public void WriteXml_ShouldNotIncludeStack_WhenIncludeStackTraceIsFalse()
        {
            Exception caught = null;
            try { throw new Exception("x"); } catch (Exception ex) { caught = ex; }
            var xml = WriteException(caught, includeStackTrace: false);
            TestOutput.WriteLine(xml);
            Assert.DoesNotContain("<Stack>", xml);
        }

        [Fact]
        public void WriteXml_ShouldIncludeStack_WhenIncludeStackTraceIsTrue()
        {
            Exception caught = null;
            try { throw new Exception("x"); } catch (Exception ex) { caught = ex; }
            var xml = WriteException(caught, includeStackTrace: true);
            TestOutput.WriteLine(xml);
            Assert.Contains("<Stack>", xml);
            Assert.Contains("<Frame>", xml);
        }

        [Fact]
        public void WriteXml_ShouldNotIncludeData_WhenIncludeDataIsFalse()
        {
            var ex = new Exception("x");
            ex.Data.Add("Key", "Value");
            var xml = WriteException(ex, includeData: false);
            TestOutput.WriteLine(xml);
            Assert.DoesNotContain("<Data>", xml);
        }

        [Fact]
        public void WriteXml_ShouldIncludeData_WhenIncludeDataIsTrue()
        {
            var ex = new Exception("x");
            ex.Data.Add("MyKey", "MyValue");
            var xml = WriteException(ex, includeData: true);
            TestOutput.WriteLine(xml);
            Assert.Contains("<Data>", xml);
            Assert.Contains("<MyKey>MyValue</MyKey>", xml);
        }

        [Fact]
        public void WriteXml_ShouldIncludeInnerException()
        {
            var inner = new ArgumentNullException("param", "Inner message");
            var outer = new InvalidOperationException("Outer", inner);
            var xml = WriteException(outer);
            TestOutput.WriteLine(xml);
            Assert.Contains("<ArgumentNullException namespace=\"System\">", xml);
        }

        [Fact]
        public void WriteXml_ShouldIncludeAggregateExceptionInnerExceptions()
        {
            var agg = new AggregateException("Agg", new AccessViolationException("AV"), new ArithmeticException("Arith"));
            var outer = new InvalidOperationException("Outer", agg);
            var xml = WriteException(outer);
            TestOutput.WriteLine(xml);
            Assert.Contains("<AggregateException namespace=\"System\">", xml);
            Assert.Contains("<AccessViolationException namespace=\"System\">", xml);
            Assert.Contains("<ArithmeticException namespace=\"System\">", xml);
        }

        [Fact]
        public void ReadXml_ShouldDeserializeSimpleException()
        {
            var original = new InvalidOperationException("Round-trip message");
            var xml = WriteException(original);
            TestOutput.WriteLine(xml);

            var sut = new ExceptionConverter();
            using var reader = XmlReader.Create(new StringReader(xml));
            var result = (Exception)sut.ReadXml(reader, typeof(InvalidOperationException));

            Assert.IsAssignableFrom<Exception>(result);
            Assert.Contains("Round-trip message", result.Message);
        }

        [Fact]
        public void ReadXml_ShouldDeserializeExceptionWithInnerException()
        {
            var inner = new ArgumentNullException("param");
            var outer = new InvalidOperationException("Outer", inner);
            var xml = WriteException(outer);
            TestOutput.WriteLine(xml);

            var sut = new ExceptionConverter();
            using var reader = XmlReader.Create(new StringReader(xml));
            var result = (Exception)sut.ReadXml(reader, typeof(InvalidOperationException));

            Assert.IsAssignableFrom<Exception>(result);
            Assert.NotNull(result.InnerException);
        }

        [Fact]
        public void CanConvert_ShouldReturnTrueForExceptionTypes()
        {
            var sut = new ExceptionConverter();
            Assert.True(sut.CanConvert(typeof(Exception)));
            Assert.True(sut.CanConvert(typeof(InvalidOperationException)));
            Assert.True(sut.CanConvert(typeof(ArgumentNullException)));
        }

        [Fact]
        public void CanConvert_ShouldReturnFalseForNonExceptionTypes()
        {
            var sut = new ExceptionConverter();
            Assert.False(sut.CanConvert(typeof(string)));
            Assert.False(sut.CanConvert(typeof(int)));
        }
    }
}
