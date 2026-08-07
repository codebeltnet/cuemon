using System;
using System.Threading;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Xml.Serialization.Formatters;
using Xunit;

namespace Cuemon;
public class TypeArgumentExceptionTest : Test
{
    public TypeArgumentExceptionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ArgumentException_ShouldBeSerializable_Json()
    {
        var sut1 = new ArgumentException("My fancy message.", "myArg");
        var sut2 = new JsonFormatter();
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<ArgumentException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.ParamName, original.ParamName);
        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(sut1.ToString(), original.ToString());

#if NET48_OR_GREATER
        Assert.Equal("""
                     {
                       "type": "System.ArgumentException",
                       "message": "My fancy message.\r\nParameter name: myArg",
                       "paramName": "myArg"
                     }
                     """.ReplaceLineEndings(), sut4);
#else
        Assert.Equal("""
                         {
                           "type": "System.ArgumentException",
                           "message": "My fancy message. (Parameter 'myArg')",
                           "paramName": "myArg"
                         }
                         """.ReplaceLineEndings(), sut4);
#endif
    }

    [Fact]
    public void TypeArgumentException_ShouldBeSerializable_Json()
    {
        var random = Generate.RandomString(10);
        var sut1 = new TypeArgumentException(random);
        var sut2 = new JsonFormatter();
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<TypeArgumentException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.ParamName, original.ParamName);
        Assert.Equal(sut1.Message.ReplaceLineEndings(), original.Message.ReplaceLineEndings());
        Assert.Equal(sut1.ToString().ReplaceLineEndings(), original.ToString().ReplaceLineEndings());

#if NET48_OR_GREATER
        Assert.Equal($$"""
                     {
                       "type": "Cuemon.TypeArgumentException",
                       "message": "Value does not fall within the expected range.\r\nParameter name: {{random}}",
                       "paramName": "{{random}}"
                     }
                     """.ReplaceLineEndings(), sut4);
#else
        Assert.Equal($$"""
                         {
                           "type": "Cuemon.TypeArgumentException",
                           "message": "Value does not fall within the expected range. (Parameter '{{random}}')",
                           "paramName": "{{random}}"
                         }
                         """.ReplaceLineEndings(), sut4);
#endif
    }

    [Fact]
    public void TypeArgumentException_WithInnerException_ShouldBeSerializable_Json()
    {
        var sut1 = new TypeArgumentException("Should have IE.", new ArgumentReservedKeywordException("Test", new AbandonedMutexException(20, null)));
        var sut2 = new JsonFormatter();
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var sut5 = sut2.Deserialize<TypeArgumentException>(sut3);
        var sut6 = sut2.Serialize(sut5).ToEncodedString();

        sut3.Dispose();

        Assert.Equal(sut1.ParamName, sut5.ParamName);
        Assert.Equal(sut1.Message, sut5.Message);
        Assert.Equal(sut1.ToString(), sut5.ToString());
        Assert.Equal(sut4, sut6);

        Assert.Equal(@"{
  ""type"": ""Cuemon.TypeArgumentException"",
  ""message"": ""Should have IE."",
  ""inner"": {
    ""type"": ""Cuemon.ArgumentReservedKeywordException"",
    ""message"": ""Test"",
    ""inner"": {
      ""type"": ""System.Threading.AbandonedMutexException"",
      ""message"": ""The wait completed due to an abandoned mutex."",
      ""mutexIndex"": 20
    }
  }
}".ReplaceLineEndings(), sut4);
    }

    [Fact]
    public void TypeArgumentException_ShouldBeSerializable_Xml()
    {
        var random = Generate.RandomString(10);
        var sut1 = new TypeArgumentException(random);
        var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<TypeArgumentException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.ParamName, original.ParamName);
        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(sut1.ToString(), original.ToString());

#if NET48_OR_GREATER
        Assert.Equal($$"""
                     <?xml version="1.0" encoding="utf-8"?>
                     <TypeArgumentException namespace="Cuemon">
                     	<Message>Value does not fall within the expected range.
                     Parameter name: {{random}}</Message>
                     	<ParamName>{{random}}</ParamName>
                     </TypeArgumentException>
                     """.ReplaceLineEndings(), sut4);
#else
        Assert.Equal($$"""
                           <?xml version="1.0" encoding="utf-8"?>
                           <TypeArgumentException namespace="Cuemon">
                           	<Message>Value does not fall within the expected range. (Parameter '{{random}}')</Message>
                           	<ParamName>{{random}}</ParamName>
                           </TypeArgumentException>
                           """.ReplaceLineEndings(), sut4);
#endif
    }

    [Fact]
    public void TypeArgumentException_WithInnerException_ShouldBeSerializable_Xml()
    {
        var sut1 = new TypeArgumentException("Should have IE.", new ArgumentReservedKeywordException("Test", new AbandonedMutexException(20, null)));
        var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<TypeArgumentException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.ParamName, original.ParamName);
        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal("""
                         <?xml version="1.0" encoding="utf-8"?>
                         <TypeArgumentException namespace="Cuemon">
                         	<Message>Should have IE.</Message>
                         	<ArgumentReservedKeywordException namespace="Cuemon">
                         		<Message>Test</Message>
                         		<AbandonedMutexException namespace="System.Threading">
                         			<Message>The wait completed due to an abandoned mutex.</Message>
                         			<MutexIndex>20</MutexIndex>
                         		</AbandonedMutexException>
                         	</ArgumentReservedKeywordException>
                         </TypeArgumentException>
                         """.ReplaceLineEndings(), sut4);
    }
}
