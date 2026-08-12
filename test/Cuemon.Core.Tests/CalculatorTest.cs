using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class CalculatorTest : Test
{
    public CalculatorTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Calculate_ShouldGetExpectedResult()
    {
        Assert.Equal(10, Calculator.Calculate(5, AssignmentOperator.Addition, 5));
        Assert.Equal(5, Calculator.Calculate(5, AssignmentOperator.And, 5));
        Assert.Equal(5, Calculator.Calculate(5, AssignmentOperator.Assign, 5));
        Assert.Equal(1, Calculator.Calculate(5, AssignmentOperator.Division, 5));
        Assert.Equal(0, Calculator.Calculate(5, AssignmentOperator.ExclusiveOr, 5));
        Assert.Equal(160, Calculator.Calculate(5, AssignmentOperator.LeftShift, 5));
        Assert.Equal(25, Calculator.Calculate(5, AssignmentOperator.Multiplication, 5));
        Assert.Equal(5, Calculator.Calculate(5, AssignmentOperator.Or, 5));
        Assert.Equal(0, Calculator.Calculate(5, AssignmentOperator.Remainder, 5));
        Assert.Equal(0, Calculator.Calculate(5, AssignmentOperator.RightShift, 5));
        Assert.Equal(0, Calculator.Calculate(5, AssignmentOperator.Subtraction, 5));
    }
}
