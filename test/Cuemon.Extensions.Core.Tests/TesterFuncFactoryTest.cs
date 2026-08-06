using System.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class TesterFuncFactoryTest : Test
{
    public TesterFuncFactoryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Create_ShouldExecuteWrappedTesterFunctions_WhenCreatingFactoriesFromZeroToFifteenArguments()
    {
        var results = new List<string>();
        var successes = new List<bool>();

        successes.Add(TesterFuncFactory.Create<string, bool>((out string result) => { result = "0"; return true; }).ExecuteMethod(out var r0));
        results.Add(r0);
        successes.Add(TesterFuncFactory.Create((int a1, out string result) => { result = $"{a1}"; return true; }, 1).ExecuteMethod(out var r1));
        results.Add(r1);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, out string result) => { result = $"{a1},{a2}"; return true; }, 1, 2).ExecuteMethod(out var r2));
        results.Add(r2);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, out string result) => { result = $"{a1},{a2},{a3}"; return true; }, 1, 2, 3).ExecuteMethod(out var r3));
        results.Add(r3);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, out string result) => { result = $"{a1},{a2},{a3},{a4}"; return true; }, 1, 2, 3, 4).ExecuteMethod(out var r4));
        results.Add(r4);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, int a5, out string result) => { result = $"{a1},{a2},{a3},{a4},{a5}"; return true; }, 1, 2, 3, 4, 5).ExecuteMethod(out var r5));
        results.Add(r5);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, out string result) => { result = $"{a1},{a2},{a3},{a4},{a5},{a6}"; return true; }, 1, 2, 3, 4, 5, 6).ExecuteMethod(out var r6));
        results.Add(r6);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, out string result) => { result = $"{a1},{a2},{a3},{a4},{a5},{a6},{a7}"; return true; }, 1, 2, 3, 4, 5, 6, 7).ExecuteMethod(out var r7));
        results.Add(r7);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, out string result) => { result = $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8}"; return true; }, 1, 2, 3, 4, 5, 6, 7, 8).ExecuteMethod(out var r8));
        results.Add(r8);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, out string result) => { result = $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9}"; return true; }, 1, 2, 3, 4, 5, 6, 7, 8, 9).ExecuteMethod(out var r9));
        results.Add(r9);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, out string result) => { result = $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10}"; return true; }, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ExecuteMethod(out var r10));
        results.Add(r10);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, out string result) => { result = $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11}"; return true; }, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ExecuteMethod(out var r11));
        results.Add(r11);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, out string result) => { result = $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12}"; return true; }, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ExecuteMethod(out var r12));
        results.Add(r12);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, out string result) => { result = $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12},{a13}"; return true; }, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ExecuteMethod(out var r13));
        results.Add(r13);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, out string result) => { result = $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12},{a13},{a14}"; return true; }, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ExecuteMethod(out var r14));
        results.Add(r14);
        successes.Add(TesterFuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15, out string result) => { result = $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12},{a13},{a14},{a15}"; return true; }, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ExecuteMethod(out var r15));
        results.Add(r15);

        Assert.DoesNotContain(false, successes);
        Assert.Equal(new[]
        {
            "0",
            "1",
            "1,2",
            "1,2,3",
            "1,2,3,4",
            "1,2,3,4,5",
            "1,2,3,4,5,6",
            "1,2,3,4,5,6,7",
            "1,2,3,4,5,6,7,8",
            "1,2,3,4,5,6,7,8,9",
            "1,2,3,4,5,6,7,8,9,10",
            "1,2,3,4,5,6,7,8,9,10,11",
            "1,2,3,4,5,6,7,8,9,10,11,12",
            "1,2,3,4,5,6,7,8,9,10,11,12,13",
            "1,2,3,4,5,6,7,8,9,10,11,12,13,14",
            "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15"
        }, results);
    }

    [Fact]
    public void Invoke_ShouldExecuteTesterFunction_WhenTupleIsProvided()
    {
        var success = TesterFuncFactory.Invoke((MutableTuple<int, int, int> tuple, out string result) =>
        {
            result = $"{tuple.Arg1}:{tuple.Arg2}:{tuple.Arg3}";
            return true;
        }, MutableTupleFactory.CreateThree(1, 2, 3), out var result);

        Assert.True(success);
        Assert.Equal("1:2:3", result);
    }
}
