using System;
using System.Runtime.InteropServices;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Text.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
    public class TypeArgumentOutOfRangeExceptionTest : Test
    {
        public TypeArgumentOutOfRangeExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TypeArgumentOutOfRangeException_ShouldBeSerializable_Json()
        {
            var randomParamName = Generate.RandomString(10);
            var actualValue = 42;
            var randomMessage = Generate.RandomString(50);
            var sut1 = new TypeArgumentOutOfRangeException(randomParamName, 42, randomMessage);
            var sut2 = new JsonFormatter();
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<TypeArgumentOutOfRangeException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.ParamName, original.ParamName);
            Assert.Equal(sut1.ActualValue!.ToString(), original.ActualValue!.ToString());
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(sut1.ToString(), original.ToString());
#if NET48_OR_GREATER
            Assert.Equal($$"""
                         {
                           "type": "Cuemon.TypeArgumentOutOfRangeException",
                           "message": "{{randomMessage}}\r\nParameter name: {{randomParamName}}\r\nActual value was {{actualValue}}.",
                           "actualValue": {{actualValue}},
                           "paramName": "{{randomParamName}}"
                         }
                         """, sut4);
#else
            var newline = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"\r\n" : @"\n";
            Assert.Equal($$"""
                         {
                           "type": "Cuemon.TypeArgumentOutOfRangeException",
                           "message": "{{randomMessage}} (Parameter '{{randomParamName}}'){{newline}}Actual value was {{actualValue}}.",
                           "actualValue": {{actualValue}},
                           "paramName": "{{randomParamName}}"
                         }
                         """.ReplaceLineEndings(), sut4);
#endif
        }
    }
}
