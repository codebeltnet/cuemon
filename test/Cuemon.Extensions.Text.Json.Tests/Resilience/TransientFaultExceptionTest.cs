using System;
using System.Collections.Generic;
using System.Reflection;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Text.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Resilience;
public class TransientFaultExceptionTest : Test
{
    public TransientFaultExceptionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [MemberData(nameof(GetRandomString))]
    public void TransientFaultException_ShouldBeSerializable_Json(string random)
    {
        var sut1 = new TransientFaultException(random, new TransientFaultEvidence(10, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(2), MethodDescriptor.Create(MethodBase.GetCurrentMethod()).AppendRuntimeArguments(random)));
        var sut2 = new JsonFormatter();
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<TransientFaultException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal($$"""
                           {
                             "type": "Cuemon.Resilience.TransientFaultException",
                             "message": "{{random}}",
                             "evidence": {
                               "attempts": 10,
                               "recoveryWaitTime": "00:00:05",
                               "totalRecoveryWaitTime": "00:00:15",
                               "latency": "00:00:02",
                               "descriptor": {
                                 "caller": "Cuemon.Resilience.TransientFaultExceptionTest",
                                 "methodName": "TransientFaultException_ShouldBeSerializable_Json",
                                 "parameters": [
                                   "random"
                                 ],
                                 "arguments": [
                                   "{{random}}"
                                 ]
                               }
                             }
                           }
                           """.ReplaceLineEndings(), sut4);
    }

    [Fact]
    public void TransientFaultException_WithInnerException_ShouldBeSerializable_Json()
    {
        var sut1 = new TransientFaultException("The transient operation has failed.", new ArithmeticException(), new TransientFaultEvidence(10, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(2), MethodDescriptor.Create(MethodBase.GetCurrentMethod())));
        var sut2 = new JsonFormatter();
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<TransientFaultException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal("""
                           {
                             "type": "Cuemon.Resilience.TransientFaultException",
                             "message": "The transient operation has failed.",
                             "evidence": {
                               "attempts": 10,
                               "recoveryWaitTime": "00:00:05",
                               "totalRecoveryWaitTime": "00:00:15",
                               "latency": "00:00:02",
                               "descriptor": {
                                 "caller": "Cuemon.Resilience.TransientFaultExceptionTest",
                                 "methodName": "TransientFaultException_WithInnerException_ShouldBeSerializable_Json",
                                 "parameters": [],
                                 "arguments": []
                               }
                             },
                             "inner": {
                               "type": "System.ArithmeticException",
                               "message": "Overflow or underflow in the arithmetic operation."
                             }
                           }
                           """.ReplaceLineEndings(), sut4);
    }

    public static IEnumerable<object[]> GetRandomString()
    {
        var parameters = new List<object[]>()
        {
            new object[]
            {
                Generate.RandomString(25)
            }
        };
        return parameters;
    }
}
