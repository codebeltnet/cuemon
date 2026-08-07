using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class FuncFactoryTest : Test
{
    public FuncFactoryTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Create_ShouldExecuteWrappedFunctions_WhenCreatingFactoriesFromZeroToFifteenArguments()
    {
        var actual = new[]
        {
            FuncFactory.Create(() => "0").ExecuteMethod(),
            FuncFactory.Create((int a1) => $"{a1}", 1).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2) => $"{a1},{a2}", 1, 2).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3) => $"{a1},{a2},{a3}", 1, 2, 3).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4) => $"{a1},{a2},{a3},{a4}", 1, 2, 3, 4).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4, int a5) => $"{a1},{a2},{a3},{a4},{a5}", 1, 2, 3, 4, 5).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6) => $"{a1},{a2},{a3},{a4},{a5},{a6}", 1, 2, 3, 4, 5, 6).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7) => $"{a1},{a2},{a3},{a4},{a5},{a6},{a7}", 1, 2, 3, 4, 5, 6, 7).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8) => $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8}", 1, 2, 3, 4, 5, 6, 7, 8).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9) => $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9}", 1, 2, 3, 4, 5, 6, 7, 8, 9).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10) => $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11) => $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12) => $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13) => $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12},{a13}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14) => $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12},{a13},{a14}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ExecuteMethod(),
            FuncFactory.Create((int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, int a11, int a12, int a13, int a14, int a15) => $"{a1},{a2},{a3},{a4},{a5},{a6},{a7},{a8},{a9},{a10},{a11},{a12},{a13},{a14},{a15}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ExecuteMethod()
        };

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
        }, actual);
    }

    [Fact]
    public void Invoke_ShouldExecuteFunction_WhenTupleIsProvided()
    {
        var result = FuncFactory.Invoke((MutableTuple<int, int, int> tuple) => $"{tuple.Arg1}:{tuple.Arg2}:{tuple.Arg3}", MutableTupleFactory.CreateThree(1, 2, 3));

        Assert.Equal("1:2:3", result);
    }
}
