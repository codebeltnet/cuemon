using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml;
using Cuemon.Diagnostics;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.IO;
using Cuemon.Xml.Assets;
using Xunit;

namespace Cuemon.Xml.Serialization.Formatters
{
    public class XmlFormatterTest : Test
    {
        public XmlFormatterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DeserializeObject_ShouldBeEquivalentToOriginal_TimeSpan()
        {
            var sut1 = TimeSpan.Parse("01:12:05");

            TestOutput.WriteLine(sut1.ToString());

            var xml = XmlFormatter.SerializeObject(sut1);

            TestOutput.WriteLine(xml.ToEncodedString(o => o.LeaveOpen = true));

            var sut2 = XmlFormatter.DeserializeObject<TimeSpan>(xml);

            Assert.Equal(sut1, sut2);
        }

        [Fact]
        public void SerializeObject_ShouldBeEquivalentToOriginal_String()
        {
            var sut1 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><TimeSpan>01:12:05</TimeSpan>";

            TestOutput.WriteLine(sut1);

            var timeSpan = XmlFormatter.DeserializeObject<TimeSpan>(sut1.ToStream());

            TestOutput.WriteLine(timeSpan.ToString());

            var sut2 = XmlFormatter.SerializeObject(timeSpan);

            Assert.Equal(sut1, sut2.ToEncodedString());
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingExceptionConverter()
        {
            try
            {
                throw new OutOfMemoryException("First", new AggregateException(new AccessViolationException("I1"), new AbandonedMutexException("I2"), new ArithmeticException("I3")));
            }
            catch (Exception e)
            {
                e.Data.Add("Cuemon", "XmlFormatterTest");
                var f = new XmlFormatter(o =>
                {
                    o.SensitivityDetails = FaultSensitivityDetails.FailureWithStackTraceAndData;
                    o.Settings.Writer.Indent = true;
                });
                var r = f.Serialize(e);
                var x = new XmlDocument();
                x.Load(r);

                Assert.Contains(e.Data.Keys.Cast<string>(), s => s.Equals("Cuemon"));
                Assert.Contains(e.Data.Values.Cast<string>(), s => s.Equals("XmlFormatterTest"));
                Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
                Assert.Contains("<OutOfMemoryException namespace=\"System\">", x.OuterXml);
                Assert.Contains("<Source>Cuemon.Xml.Tests</Source>", x.OuterXml);
                Assert.Contains("<Message>First</Message>", x.OuterXml);
                Assert.Contains("<Stack>", x.OuterXml);
                Assert.Contains("<Frame>at Cuemon.Xml.Serialization.Formatters.XmlFormatterTest", x.OuterXml);
                Assert.Contains("<Cuemon>XmlFormatterTest</Cuemon>", x.OuterXml);
                Assert.Contains("<AggregateException namespace=\"System\">", x.OuterXml);
                Assert.Contains("<AccessViolationException namespace=\"System\">", x.OuterXml);
                Assert.Contains("<AbandonedMutexException namespace=\"System.Threading\">", x.OuterXml);
                Assert.Contains("<ArithmeticException namespace=\"System\">", x.OuterXml);

                TestOutput.WriteLine(Decorator.Enclose(r).ToEncodedString());
                r.Dispose();
            }
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingStringConverterWrappedInCData()
        {
            var sut = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var result = sut.Serialize("<html><title>Cuemon for .NET</title></html>");
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<String><![CDATA[<html><title>Cuemon for .NET</title></html>]]></String>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeWorldNodeHierarchy()
        {
            var world = new WorldNode
            {
                Code = "001",
                Name = "World",
                Kind = "World",
                Links = new WorldLinks
                {
                    Self = new Link { Href = "/", Title = "World" },
                    Children = new List<Link>
                    {
                        new Link { Href = "/regions/002", Title = "Africa" },
                        new Link { Href = "/regions/009", Title = "Oceania" },
                        new Link { Href = "/regions/010", Title = "Antarctica" },
                        new Link { Href = "/regions/019", Title = "Americas" },
                        new Link { Href = "/regions/142", Title = "Asia" },
                        new Link { Href = "/regions/150", Title = "Europe" }
                    }
                }
            };

            var sut = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var result = sut.Serialize(world);
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Contains("<Code>001</Code>", x.OuterXml);
            Assert.Contains("<Name>World</Name>", x.OuterXml);
            Assert.Contains("<Kind>World</Kind>", x.OuterXml);
            Assert.Contains("<Href>/</Href>", x.OuterXml);
            Assert.Contains("<Title>World</Title>", x.OuterXml);
            Assert.Contains("<Href>/regions/002</Href>", x.OuterXml);
            Assert.Contains("<Title>Africa</Title>", x.OuterXml);
            Assert.Contains("<Href>/regions/150</Href>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldProduce_PascalCase_Structure_ForWorldNode()
        {
            var world = new WorldNode
            {
                Code = "001",
                Name = "World",
                Kind = "World",
                Links = new WorldLinks
                {
                    Self = new Link { Href = "/", Title = "World" },
                    Children = new List<Link>
                    {
                        new Link { Href = "/regions/002", Title = "Africa" },
                        new Link { Href = "/regions/009", Title = "Oceania" },
                        new Link { Href = "/regions/010", Title = "Antarctica" },
                        new Link { Href = "/regions/019", Title = "Americas" },
                        new Link { Href = "/regions/142", Title = "Asia" },
                        new Link { Href = "/regions/150", Title = "Europe" }
                    }
                }
            };

            var sut = new XmlFormatter(o =>
            {
                o.Settings.Writer.Indent = true;
                o.Settings.FlattenCollectionItems = true;
            });
            var result = sut.Serialize(world);
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("WorldNode", x.DocumentElement.Name);
            Assert.Contains("<Code>001</Code>", x.OuterXml);
            Assert.Contains("<Name>World</Name>", x.OuterXml);
            Assert.Contains("<Kind>World</Kind>", x.OuterXml);
            Assert.Contains("<Links>", x.OuterXml);
            Assert.Contains("<Self>", x.OuterXml);
            Assert.Contains("<Href>/</Href>", x.OuterXml);
            Assert.Contains("<Title>World</Title>", x.OuterXml);
            Assert.DoesNotContain("<Item>", x.OuterXml);
            Assert.Contains("<Href>/regions/002</Href>", x.OuterXml);
            Assert.Contains("<Title>Africa</Title>", x.OuterXml);
            Assert.Contains("<Href>/regions/150</Href>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldProduce_PascalCase_Structure_ForWrapperResponse()
        {
            var wrapper = new WrapperResponse();

            var sut = new XmlFormatter(o =>
            {
                o.Settings.Writer.Indent = true;
                o.Settings.FlattenCollectionItems = true;
            });
            var result = sut.Serialize(wrapper);
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("WrapperResponse", x.DocumentElement.Name);
            Assert.Contains("<Links>", x.OuterXml);
            Assert.Contains("<Link>", x.OuterXml);
            Assert.Contains("<Href>https://example.com</Href>", x.OuterXml);
            Assert.Contains("<Rel>self</Rel>", x.OuterXml);
            Assert.Contains("<Href>https://example.com/other</Href>", x.OuterXml);
            Assert.Contains("<Rel>related</Rel>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingEnumerableConverter_DictionaryProperty()
        {
            var sut1 = new RegionStats
            {
                Name = "Africa",
                Indicators = new Dictionary<string, int>
                {
                    { "Population", 1400000000 },
                    { "Countries", 54 }
                }
            };

            var sut = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var result = sut.Serialize(sut1);
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("RegionStats", x.DocumentElement.Name);
            Assert.Contains("<Name>Africa</Name>", x.OuterXml);
            Assert.Contains("<Indicators>", x.OuterXml);
            Assert.Contains("<Item name=\"Population\">1400000000</Item>", x.OuterXml);
            Assert.Contains("<Item name=\"Countries\">54</Item>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingFlattenedConverter_DictionaryProperty()
        {
            var sut1 = new RegionStats
            {
                Name = "Africa",
                Indicators = new Dictionary<string, int>
                {
                    { "Population", 1400000000 },
                    { "Countries", 54 }
                }
            };

            var sut = new XmlFormatter(o =>
            {
                o.Settings.Writer.Indent = true;
                o.Settings.FlattenCollectionItems = true;
            });
            var result = sut.Serialize(sut1);
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("RegionStats", x.DocumentElement.Name);
            Assert.Contains("<Name>Africa</Name>", x.OuterXml);
            Assert.DoesNotContain("<Item", x.OuterXml);
            Assert.Contains("<Indicators>", x.OuterXml);
            Assert.Contains("<Population>1400000000</Population>", x.OuterXml);
            Assert.Contains("<Countries>54</Countries>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingStringConverter()
        {
            var sut = new XmlFormatter(o => o.Settings.Writer.OmitXmlDeclaration = true);
            var result = sut.Serialize("Cuemon for .NET");
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.NotEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<String>Cuemon for .NET</String>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingDateTimeConverter()
        {
            var sut = new XmlFormatter();
            var dt = DateTime.Parse("2021-03-14T14:33:00Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            var result = sut.Serialize(dt);
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.True(dt.Kind == DateTimeKind.Utc);
            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<DateTime>2021-03-14T14:33:00.0000000Z</DateTime>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingTimeSpanConverter()
        {
            var sut = new XmlFormatter();
            var result = sut.Serialize(DateTime.Parse("2021-03-14T15:33:00") - Decorator.Syntactic<DateTime>().GetUnixEpoch());
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<TimeSpan>18700.15:33:00</TimeSpan>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingUriConverter()
        {
            var sut = new XmlFormatter();
            var result = sut.Serialize(new Uri("https://docs.cuemon.net/"));
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<Uri>https://docs.cuemon.net/</Uri>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingDefaultConverter()
        {
            var sut = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var result = sut.Serialize(new WeatherForecast());
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<WeatherForecast>", x.OuterXml);
            Assert.Contains("<Date>", x.OuterXml);
            Assert.Contains("<TemperatureC>", x.OuterXml);
            Assert.Contains("<TemperatureF>", x.OuterXml);
            Assert.Contains("<Summary>Scorching</Summary>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingEnumerableConverter()
        {
            var sut = new XmlFormatter();
            var result = sut.Serialize(Generate.RangeOf(5, i => i + 1));
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<Enumerable><Item>1</Item><Item>2</Item><Item>3</Item><Item>4</Item><Item>5</Item></Enumerable>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingEnumerableConverter_List()
        {
            var sut = new XmlFormatter();
            var result = sut.Serialize(Generate.RangeOf(5, i => i + 1).ToList());
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<List><Item>1</Item><Item>2</Item><Item>3</Item><Item>4</Item><Item>5</Item></List>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingEnumerableConverter_Dictionary()
        {
            var keys = new[] { "A", "B", "C", "D", "E" };
            var sut = new XmlFormatter();
            var result = sut.Serialize(Generate.RangeOf(5, i => i + 1).ToDictionary(i => keys[i - 1], i => i));
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<Dictionary><Item name=\"A\">1</Item><Item name=\"B\">2</Item><Item name=\"C\">3</Item><Item name=\"D\">4</Item><Item name=\"E\">5</Item></Dictionary>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Deserialize_ShouldUseTimeSpanConverter()
        {
            var sut = new XmlFormatter();
            var result = sut.Deserialize<TimeSpan>(Decorator.Enclose("<TimeSpan>18700.15:33:00</TimeSpan>").ToStream());

            TestOutput.WriteLine(result.ToString());

            Assert.Equal(DateTime.Parse("2021-03-14T15:33:00") - Decorator.Syntactic<DateTime>().GetUnixEpoch(), result);
        }

        [Fact]
        public void Deserialize_ShouldUseUriConverter()
        {
            var sut = new XmlFormatter();
            var result = sut.Deserialize<Uri>(Decorator.Enclose("<Uri>https://docs.cuemon.net/</Uri>").ToStream());

            TestOutput.WriteLine(result.OriginalString);

            Assert.Equal(new Uri("https://docs.cuemon.net/"), result);
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingExceptionDescriptorConverter()
        {
            var sut = new XmlFormatter(o =>
            {
                o.SensitivityDetails = FaultSensitivityDetails.Failure | FaultSensitivityDetails.Evidence;
                o.Settings.Writer.Indent = true;
            });

            Exception catched = null;
            try
            {
                throw new OutOfMemoryException();
            }
            catch (Exception e)
            {
                catched = e;
            }

            var ed = new ExceptionDescriptor(catched, "NoMemory", "System halted; out of memory.", new Uri("https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html"));
            ed.AddEvidence("AnswerToEverything", 42, i => i);

            var result = sut.Serialize(ed);
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<ExceptionDescriptor>", x.OuterXml);
            Assert.Contains("<Error>", x.OuterXml);
            Assert.Contains("<Code>NoMemory</Code>", x.OuterXml);
            Assert.Contains("<Message>System halted; out of memory.</Message>", x.OuterXml);
            Assert.Contains("<HelpLink>https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html</HelpLink>", x.OuterXml);
            Assert.Contains("<Failure>", x.OuterXml);
            Assert.Contains("<OutOfMemoryException namespace=\"System\">", x.OuterXml);
            Assert.Contains("<Source>Cuemon.Xml.Tests</Source>", x.OuterXml);
            Assert.Contains("<Message>Insufficient memory to continue the execution of the program.</Message>", x.OuterXml);
            Assert.DoesNotContain("<Stack>", x.OuterXml);
            Assert.DoesNotContain("<Frame>", x.OuterXml);
            Assert.Contains("<Evidence>", x.OuterXml);
            Assert.Contains("<AnswerToEverything>42</AnswerToEverything>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingExceptionDescriptorConverter_IncludeStackTrace()
        {
            var sut = new XmlFormatter(o =>
            {
                o.SensitivityDetails = FaultSensitivityDetails.All;
                o.Settings.Writer.Indent = true;
            });

            Exception catched = null;
            try
            {
                throw new OutOfMemoryException();
            }
            catch (Exception e)
            {
                catched = e;
            }

            var ed = new ExceptionDescriptor(catched, "NoMemory", "System halted; out of memory.", new Uri("https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html"));
            ed.AddEvidence("AnswerToEverything", 42, i => i);

            var result = sut.Serialize(ed);
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<ExceptionDescriptor>", x.OuterXml);
            Assert.Contains("<Error>", x.OuterXml);
            Assert.Contains("<Code>NoMemory</Code>", x.OuterXml);
            Assert.Contains("<Message>System halted; out of memory.</Message>", x.OuterXml);
            Assert.Contains("<HelpLink>https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html</HelpLink>", x.OuterXml);
            Assert.Contains("<Failure>", x.OuterXml);
            Assert.Contains("<OutOfMemoryException namespace=\"System\">", x.OuterXml);
            Assert.Contains("<Source>Cuemon.Xml.Tests</Source>", x.OuterXml);
            Assert.Contains("<Message>Insufficient memory to continue the execution of the program.</Message>", x.OuterXml);
            Assert.Contains("<Stack>", x.OuterXml);
            Assert.Contains("<Frame>", x.OuterXml);
            Assert.Contains("<Evidence>", x.OuterXml);
            Assert.Contains("<AnswerToEverything>42</AnswerToEverything>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingExceptionDescriptorConverter_ExcludeFailure()
        {
            var sut = new XmlFormatter(o =>
            {
                o.SensitivityDetails = FaultSensitivityDetails.Evidence;
                o.Settings.Writer.Indent = true;
            });

            Exception catched = null;
            try
            {
                throw new OutOfMemoryException();
            }
            catch (Exception e)
            {
                catched = e;
            }

            var ed = new ExceptionDescriptor(catched, "NoMemory", "System halted; out of memory.", new Uri("https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html"));
            ed.AddEvidence("AnswerToEverything", 42, i => i);

            var result = sut.Serialize(ed);
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<ExceptionDescriptor>", x.OuterXml);
            Assert.Contains("<Error>", x.OuterXml);
            Assert.Contains("<Code>NoMemory</Code>", x.OuterXml);
            Assert.Contains("<Message>System halted; out of memory.</Message>", x.OuterXml);
            Assert.Contains("<HelpLink>https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html</HelpLink>", x.OuterXml);
            Assert.DoesNotContain("<Failure>", x.OuterXml);
            Assert.DoesNotContain("<OutOfMemoryException namespace=\"System\">", x.OuterXml);
            Assert.DoesNotContain("<Source>Cuemon.Xml.Tests</Source>", x.OuterXml);
            Assert.DoesNotContain("<Message>Insufficient memory to continue the execution of the program.</Message>", x.OuterXml);
            Assert.DoesNotContain("<Stack>", x.OuterXml);
            Assert.DoesNotContain("<Frame>", x.OuterXml);
            Assert.Contains("<Evidence>", x.OuterXml);
            Assert.Contains("<AnswerToEverything>42</AnswerToEverything>", x.OuterXml);

            result.Dispose();
        }

        [Fact]
        public void Serialize_ShouldSerializeUsingExceptionDescriptorConverter_ExcludeEvidence()
        {
            var sut = new XmlFormatter(o =>
            {
                o.SensitivityDetails = FaultSensitivityDetails.Failure;
                o.Settings.Writer.Indent = true;
            });

            Exception catched = null;
            try
            {
                throw new OutOfMemoryException();
            }
            catch (Exception e)
            {
                catched = e;
            }

            var ed = new ExceptionDescriptor(catched, "NoMemory", "System halted; out of memory.", new Uri("https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html"));
            ed.AddEvidence("AnswerToEverything", 42, i => i);

            var result = sut.Serialize(ed);
            var x = new XmlDocument();
            x.Load(result);

            TestOutput.WriteLine(Decorator.Enclose(result).ToEncodedString());

            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", x.FirstChild.OuterXml);
            Assert.Contains("<ExceptionDescriptor>", x.OuterXml);
            Assert.Contains("<Error>", x.OuterXml);
            Assert.Contains("<Code>NoMemory</Code>", x.OuterXml);
            Assert.Contains("<Message>System halted; out of memory.</Message>", x.OuterXml);
            Assert.Contains("<HelpLink>https://docs.cuemon.net/api/dotnet/Cuemon.Diagnostics.ExceptionDescriptor.html</HelpLink>", x.OuterXml);
            Assert.Contains("<Failure>", x.OuterXml);
            Assert.Contains("<OutOfMemoryException namespace=\"System\">", x.OuterXml);
            Assert.Contains("<Source>Cuemon.Xml.Tests</Source>", x.OuterXml);
            Assert.Contains("<Message>Insufficient memory to continue the execution of the program.</Message>", x.OuterXml);
            Assert.DoesNotContain("<Stack>", x.OuterXml);
            Assert.DoesNotContain("<Frame>", x.OuterXml);
            Assert.DoesNotContain("<Evidence>", x.OuterXml);
            Assert.DoesNotContain("<AnswerToEverything>500</AnswerToEverything>", x.OuterXml);

            result.Dispose();
        }
    }
}
