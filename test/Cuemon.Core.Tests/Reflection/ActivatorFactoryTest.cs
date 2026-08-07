using Xunit;

namespace Cuemon.Reflection;
public class ActivatorFactoryTest
{
    [Fact]
    public void CreateInstance_ShouldCreateInstanceUsingParameterlessConstructor()
    {
        var instance = ActivatorFactory.CreateInstance<TestClass>();
        Assert.NotNull(instance);
        Assert.IsType<TestClass>(instance);
    }

    [Fact]
    public void CreateInstance_WithOneParameter_ShouldCreateInstance()
    {
        var instance = ActivatorFactory.CreateInstance<int, TestClassWithOneParameter>(42);
        Assert.NotNull(instance);
        Assert.IsType<TestClassWithOneParameter>(instance);
        Assert.Equal(42, instance.Value);
    }

    [Fact]
    public void CreateInstance_WithTwoParameters_ShouldCreateInstance()
    {
        var instance = ActivatorFactory.CreateInstance<int, string, TestClassWithTwoParameters>(42, "Hello");
        Assert.NotNull(instance);
        Assert.IsType<TestClassWithTwoParameters>(instance);
        Assert.Equal(42, instance.Value1);
        Assert.Equal("Hello", instance.Value2);
    }

    [Fact]
    public void CreateInstance_WithThreeParameters_ShouldCreateInstance()
    {
        var instance = ActivatorFactory.CreateInstance<int, string, double, TestClassWithThreeParameters>(42, "Hello", 3.14);
        Assert.NotNull(instance);
        Assert.IsType<TestClassWithThreeParameters>(instance);
        Assert.Equal(42, instance.Value1);
        Assert.Equal("Hello", instance.Value2);
        Assert.Equal(3.14, instance.Value3);
    }

    [Fact]
    public void CreateInstance_WithFourParameters_ShouldCreateInstance()
    {
        var instance = ActivatorFactory.CreateInstance<int, string, double, bool, TestClassWithFourParameters>(42, "Hello", 3.14, true);
        Assert.NotNull(instance);
        Assert.IsType<TestClassWithFourParameters>(instance);
        Assert.Equal(42, instance.Value1);
        Assert.Equal("Hello", instance.Value2);
        Assert.Equal(3.14, instance.Value3);
        Assert.True(instance.Value4);
    }

    [Fact]
    public void CreateInstance_WithFiveParameters_ShouldCreateInstance()
    {
        var instance = ActivatorFactory.CreateInstance<int, string, double, bool, char, TestClassWithFiveParameters>(42, "Hello", 3.14, true, 'A');
        Assert.NotNull(instance);
        Assert.IsType<TestClassWithFiveParameters>(instance);
        Assert.Equal(42, instance.Value1);
        Assert.Equal("Hello", instance.Value2);
        Assert.Equal(3.14, instance.Value3);
        Assert.True(instance.Value4);
        Assert.Equal('A', instance.Value5);
    }

    private class TestClass
    {
    }

    private class TestClassWithOneParameter
    {
        public TestClassWithOneParameter(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    private class TestClassWithTwoParameters
    {
        public TestClassWithTwoParameters(int value1, string value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public int Value1 { get; }
        public string Value2 { get; }
    }

    private class TestClassWithThreeParameters
    {
        public TestClassWithThreeParameters(int value1, string value2, double value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public int Value1 { get; }
        public string Value2 { get; }
        public double Value3 { get; }
    }

    private class TestClassWithFourParameters
    {
        public TestClassWithFourParameters(int value1, string value2, double value3, bool value4)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
        }

        public int Value1 { get; }
        public string Value2 { get; }
        public double Value3 { get; }
        public bool Value4 { get; }
    }

    private class TestClassWithFiveParameters
    {
        public TestClassWithFiveParameters(int value1, string value2, double value3, bool value4, char value5)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
            Value5 = value5;
        }

        public int Value1 { get; }
        public string Value2 { get; }
        public double Value3 { get; }
        public bool Value4 { get; }
        public char Value5 { get; }
    }
}
