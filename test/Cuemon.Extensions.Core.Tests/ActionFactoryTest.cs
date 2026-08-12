using System.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class ActionFactoryTest : Test
{
    public ActionFactoryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Create_ShouldExecuteWrappedActions_WhenCreatingFactoriesFromZeroToFifteenArguments()
    {
        var executed = new List<string>();

        ActionFactory.Create(() => executed.Add("0")).ExecuteMethod();
        ActionFactory.Create((int a1) => executed.Add($"{a1}"), 1).ExecuteMethod();
        ActionFactory.Create((int a1, int a2) => executed.Add($"{a1},{a2}"), 1, 2).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3) => executed.Add($"{a1},{a2},{a3}"), 1, 2, 3).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4) => executed.Add($"{a1},{a2},{a3},{a4}"), 1, 2, 3, 4).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4, int a5) => executed.Add($"{a1},{a2},{a3},{a4},{a5}"), 1, 2, 3, 4, 5).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6) => executed.Add($"{a1},{a2},{a3},{a4},{a5},{a6}"), 1, 2, 3, 4, 5, 6).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7) => executed.Add($"{a1},{a2},{a3},{a4},{a5},{a6},{a7}"), 1, 2, 3, 4, 5, 6, 7).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8) => executed.Add($"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8}"), 1, 2, 3, 4, 5, 6, 7, 8).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9) => executed.Add($"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9}"), 1, 2, 3, 4, 5, 6, 7, 8, 9).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10) => executed.Add($"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10}"), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11) => executed.Add($"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11}"), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12) => executed.Add($"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12}"), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13) => executed.Add($"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12},{a13}"), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14) => executed.Add($"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12},{a13},{a14}"), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ExecuteMethod();
        ActionFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15) => executed.Add($"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12},{a13},{a14},{a15}"), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ExecuteMethod();

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
        }, executed);
    }

    [Fact]
    public void Invoke_ShouldExecuteAction_WhenTupleIsProvided()
    {
        var result = string.Empty;

        ActionFactory.Invoke((MutableTuple<int, int, int> tuple) => result = $"{tuple.Arg1}:{tuple.Arg2}:{tuple.Arg3}", MutableTupleFactory.CreateThree(1, 2, 3));

        Assert.Equal("1:2:3", result);
    }
}
