using System;
using System.ComponentModel;
using System.Globalization;

namespace Cuemon;
/// <summary>
/// Provides a set of static methods for generic arithmetic assignment operations.
/// </summary>
public static class Calculator
{
    /// <summary>
    /// Performs a binary addition of the two specified values.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The first value to add.</param>
    /// <param name="y">The second value to add.</param>
    /// <returns>The sum of <paramref name="x"/> and <paramref name="y"/>.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="float"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum of <paramref name="x"/> and <paramref name="y"/> is less than or greater than the by <typeparamref name="T"/> valid <c>MinValue</c> and <c>MaxValue</c>.
    /// </exception>
    public static T Add<T>(T x, T y) where T : struct, IConvertible
    {
        return CalculateCore(x, y, AssignmentOperator.Addition);
    }

    /// <summary>
    /// Performs a a bitwise logical conjunction (AND) operation of the two specified values.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The first value to AND.</param>
    /// <param name="y">The second value to AND.</param>
    /// <returns>The result of <paramref name="x"/> AND <paramref name="y"/>.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    public static T And<T>(T x, T y) where T : struct, IConvertible
    {
        return CalculateCore(x, y, AssignmentOperator.And);
    }

    /// <summary>
    /// Performs an assignment of the right-hand operand to the left-hand operand.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The left-hand operand.</param>
    /// <param name="y">The right-hand operand.</param>
    /// <returns>The value of <paramref name="y"/>.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="float"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    public static T Assign<T>(T x, T y) where T : struct, IConvertible
    {
        return CalculateCore(x, y, AssignmentOperator.Assign);
    }

    /// <summary>
    /// Performs a binary division of the two specified values.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The dividend.</param>
    /// <param name="y">The divisor.</param>
    /// <returns>The result of dividing <paramref name="x"/> by <paramref name="y"/>.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="float"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="y"/> is zero.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum of <paramref name="x"/> and <paramref name="y"/> is less than or greater than the by <typeparamref name="T"/> valid <c>MinValue</c> and <c>MaxValue</c>.
    /// </exception>
    public static T Divide<T>(T x, T y) where T : struct, IConvertible
    {
        return CalculateCore(x, y, AssignmentOperator.Division);
    }

    /// <summary>
    /// Performs a bitwise exclusive or (XOR) operation of the two specified values.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The first value to XOR.</param>
    /// <param name="y">The second value to XOR.</param>
    /// <returns>The result of <paramref name="x"/> XOR <paramref name="y"/>.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    public static T ExclusiveOr<T>(T x, T y) where T : struct, IConvertible
    {
        return CalculateCore(x, y, AssignmentOperator.ExclusiveOr);
    }

    /// <summary>
    /// Performs an arithmetic left shift (&lt;&lt;) operation.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The bit pattern to be shifted.</param>
    /// <param name="y">The number of bits to shift the bit pattern.</param>
    /// <returns>The result of shifting the bit pattern.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>, <see cref="sbyte"/>, <see cref="ushort"/>.
    /// </exception>
    public static T LeftShift<T>(T x, T y) where T : struct, IConvertible
    {
        return CalculateCore(x, y, AssignmentOperator.LeftShift);
    }

    /// <summary>
    /// Performs a binary multiplication of the two specified values.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The multiplicand.</param>
    /// <param name="y">The multiplier.</param>
    /// <returns>The result of multiplying <paramref name="x"/> and <paramref name="y"/>.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="float"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum of <paramref name="x"/> and <paramref name="y"/> is less than or greater than the by <typeparamref name="T"/> valid <c>MinValue</c> and <c>MaxValue</c>.
    /// </exception>
    public static T Multiply<T>(T x, T y) where T : struct, IConvertible
    {
        return CalculateCore(x, y, AssignmentOperator.Multiplication);
    }

    /// <summary>
    /// Performs a bitwise logical disjunction (OR) operation of the two specified values.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The first value to OR.</param>
    /// <param name="y">The second value to OR.</param>
    /// <returns>The result of <paramref name="x"/> OR <paramref name="y"/>.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    public static T Or<T>(T x, T y) where T : struct, IConvertible
    {
        return CalculateCore(x, y, AssignmentOperator.Or);
    }

    /// <summary>
    /// Performs a binary division of the two specified values and computes the remainder hereof.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The dividend.</param>
    /// <param name="y">The divisor.</param>
    /// <returns>The remainder after dividing <paramref name="x"/> by <paramref name="y"/>.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="float"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    /// <exception cref="DivideByZeroException">
    /// <paramref name="y"/> is zero.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum of <paramref name="x"/> and <paramref name="y"/> is less than or greater than the by <typeparamref name="T"/> valid <c>MinValue</c> and <c>MaxValue</c>.
    /// </exception>
    public static T Remainder<T>(T x, T y) where T : struct, IConvertible
    {
        return CalculateCore(x, y, AssignmentOperator.Remainder);
    }

    /// <summary>
    /// Performs an arithmetic right shift (&gt;&gt;) operation.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The bit pattern to be shifted.</param>
    /// <param name="y">The number of bits to shift the bit pattern.</param>
    /// <returns>The result of shifting the bit pattern.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>, <see cref="sbyte"/>, <see cref="ushort"/>.
    /// </exception>
    public static T RightShift<T>(T x, T y) where T : struct, IConvertible
    {
        return CalculateCore(x, y, AssignmentOperator.RightShift);
    }

    /// <summary>
    /// Performs a binary subtraction of the two specified values.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The minuend.</param>
    /// <param name="y">The subtrahend.</param>
    /// <returns>The result of subtracting <paramref name="y"/> from <paramref name="x"/>.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="float"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The sum of <paramref name="x"/> and <paramref name="y"/> is less than or greater than the by <typeparamref name="T"/> valid <c>MinValue</c> and <c>MaxValue</c>.
    /// </exception>
    public static T Subtract<T>(T x, T y) where T : struct, IConvertible
    {
        return CalculateCore(x, y, AssignmentOperator.Subtraction);
    }

    /// <summary>
    /// Performs a calculation following the <paramref name="assignment"/> of the two specified values.
    /// </summary>
    /// <typeparam name="T">The type of the values for the operand operation.</typeparam>
    /// <param name="x">The value to calculate with <paramref name="y"/>.</param>
    /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the assignment operator of <paramref name="x"/> and <paramref name="y"/>.</param>
    /// <param name="y">The value to calculate with <paramref name="x"/>.</param>
    /// <returns>The result of the <paramref name="assignment"/> for <paramref name="x"/> and <paramref name="y"/>.</returns>
    /// <exception cref="TypeArgumentException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="float"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    public static T Calculate<T>(T x, AssignmentOperator assignment, T y) where T : struct, IConvertible
    {
        switch (assignment)
        {
            case AssignmentOperator.Addition:
                return Add(x, y);
            case AssignmentOperator.And:
                return And(x, y);
            case AssignmentOperator.Assign:
                return Assign(x, y);
            case AssignmentOperator.Division:
                return Divide(x, y);
            case AssignmentOperator.ExclusiveOr:
                return ExclusiveOr(x, y);
            case AssignmentOperator.LeftShift:
                return LeftShift(x, y);
            case AssignmentOperator.Multiplication:
                return Multiply(x, y);
            case AssignmentOperator.Or:
                return Or(x, y);
            case AssignmentOperator.Remainder:
                return Remainder(x, y);
            case AssignmentOperator.RightShift:
                return RightShift(x, y);
            case AssignmentOperator.Subtraction:
                return Subtract(x, y);
            default:
                throw new InvalidEnumArgumentException(nameof(assignment), (int)assignment, typeof(AssignmentOperator));
        }
    }

    private static T CalculateCore<T>(T x, T y, AssignmentOperator assignment) where T : struct, IConvertible
    {
        var provider = CultureInfo.InvariantCulture;
        var assignmentCode = x.GetTypeCode();
        switch (assignmentCode)
        {
            case TypeCode.Byte:
                switch (assignment)
                {
                    case AssignmentOperator.Addition:
                        return Decorator.Enclose<object>(x.ToByte(provider) + y.ToByte(provider)).ChangeType<T>();
                    case AssignmentOperator.And:
                        return Decorator.Enclose<object>(x.ToByte(provider) & y.ToByte(provider)).ChangeType<T>();
                    case AssignmentOperator.Assign:
                        return y;
                    case AssignmentOperator.Division:
                        return Decorator.Enclose<object>(x.ToByte(provider) / y.ToByte(provider)).ChangeType<T>();
                    case AssignmentOperator.ExclusiveOr:
                        return Decorator.Enclose<object>(x.ToByte(provider) ^ y.ToByte(provider)).ChangeType<T>();
                    case AssignmentOperator.LeftShift:
                        return Decorator.Enclose<object>(x.ToByte(provider) << y.ToByte(provider)).ChangeType<T>();
                    case AssignmentOperator.Multiplication:
                        return Decorator.Enclose<object>(x.ToByte(provider) * y.ToByte(provider)).ChangeType<T>();
                    case AssignmentOperator.Or:
                        return Decorator.Enclose<object>(x.ToByte(provider) | y.ToByte(provider)).ChangeType<T>();
                    case AssignmentOperator.Remainder:
                        return Decorator.Enclose<object>(x.ToByte(provider) % y.ToByte(provider)).ChangeType<T>();
                    case AssignmentOperator.RightShift:
                        return Decorator.Enclose<object>(x.ToByte(provider) >> y.ToByte(provider)).ChangeType<T>();
                    case AssignmentOperator.Subtraction:
                        return Decorator.Enclose<object>(x.ToByte(provider) - y.ToByte(provider)).ChangeType<T>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assignment));
                }
            case TypeCode.Decimal:
                switch (assignment)
                {
                    case AssignmentOperator.Addition:
                        return Decorator.Enclose<object>(x.ToDecimal(provider) + y.ToDecimal(provider)).ChangeType<T>();
                    case AssignmentOperator.And:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x &= y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Assign:
                        return y;
                    case AssignmentOperator.Division:
                        return Decorator.Enclose<object>(x.ToDecimal(provider) / y.ToDecimal(provider)).ChangeType<T>();
                    case AssignmentOperator.ExclusiveOr:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x ^= y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.LeftShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x << y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Multiplication:
                        return Decorator.Enclose<object>(x.ToDecimal(provider) * y.ToDecimal(provider)).ChangeType<T>();
                    case AssignmentOperator.Or:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x |= y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Remainder:
                        return Decorator.Enclose<object>(x.ToDecimal(provider) % y.ToDecimal(provider)).ChangeType<T>();
                    case AssignmentOperator.RightShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x >> y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Subtraction:
                        return Decorator.Enclose<object>(x.ToDecimal(provider) - y.ToDecimal(provider)).ChangeType<T>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assignment));
                }
            case TypeCode.Double:
                switch (assignment)
                {
                    case AssignmentOperator.Addition:
                        return Decorator.Enclose<object>(x.ToDouble(provider) + y.ToDouble(provider)).ChangeType<T>();
                    case AssignmentOperator.And:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x &= y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Assign:
                        return y;
                    case AssignmentOperator.Division:
                        return Decorator.Enclose<object>(x.ToDouble(provider) / y.ToDouble(provider)).ChangeType<T>();
                    case AssignmentOperator.ExclusiveOr:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x ^= y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.LeftShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x << y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Multiplication:
                        return Decorator.Enclose<object>(x.ToDouble(provider) * y.ToDouble(provider)).ChangeType<T>();
                    case AssignmentOperator.Or:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x |= y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Remainder:
                        return Decorator.Enclose<object>(x.ToDouble(provider) % y.ToDouble(provider)).ChangeType<T>();
                    case AssignmentOperator.RightShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x >> y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Subtraction:
                        return Decorator.Enclose<object>(x.ToDouble(provider) - y.ToDouble(provider)).ChangeType<T>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assignment));
                }
            case TypeCode.Int16:
                switch (assignment)
                {
                    case AssignmentOperator.Addition:
                        return Decorator.Enclose<object>(x.ToInt16(provider) + y.ToInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.And:
                        return Decorator.Enclose<object>(x.ToInt16(provider) & y.ToInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.Assign:
                        return y;
                    case AssignmentOperator.Division:
                        return Decorator.Enclose<object>(x.ToInt16(provider) / y.ToInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.ExclusiveOr:
                        return Decorator.Enclose<object>(x.ToInt16(provider) ^ y.ToInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.LeftShift:
                        return Decorator.Enclose<object>(x.ToInt16(provider) << y.ToInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.Multiplication:
                        return Decorator.Enclose<object>(x.ToInt16(provider) * y.ToInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.Or:
                        return Decorator.Enclose<object>(x.ToInt16(provider) | y.ToInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.Remainder:
                        return Decorator.Enclose<object>(x.ToInt16(provider) % y.ToInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.RightShift:
                        return Decorator.Enclose<object>(x.ToInt16(provider) >> y.ToInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.Subtraction:
                        return Decorator.Enclose<object>(x.ToInt16(provider) - y.ToInt16(provider)).ChangeType<T>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assignment));
                }
            case TypeCode.Int32:
                switch (assignment)
                {
                    case AssignmentOperator.Addition:
                        return Decorator.Enclose<object>(x.ToInt32(provider) + y.ToInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.And:
                        return Decorator.Enclose<object>(x.ToInt32(provider) & y.ToInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.Assign:
                        return y;
                    case AssignmentOperator.Division:
                        return Decorator.Enclose<object>(x.ToInt32(provider) / y.ToInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.ExclusiveOr:
                        return Decorator.Enclose<object>(x.ToInt32(provider) ^ y.ToInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.LeftShift:
                        return Decorator.Enclose<object>(x.ToInt32(provider) << y.ToInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.Multiplication:
                        return Decorator.Enclose<object>(x.ToInt32(provider) * y.ToInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.Or:
                        return Decorator.Enclose<object>(x.ToInt32(provider) | y.ToInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.Remainder:
                        return Decorator.Enclose<object>(x.ToInt32(provider) % y.ToInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.RightShift:
                        return Decorator.Enclose<object>(x.ToInt32(provider) >> y.ToInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.Subtraction:
                        return Decorator.Enclose<object>(x.ToInt32(provider) - y.ToInt32(provider)).ChangeType<T>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assignment));
                }
            case TypeCode.Int64:
                switch (assignment)
                {
                    case AssignmentOperator.Addition:
                        return Decorator.Enclose<object>(x.ToInt64(provider) + y.ToInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.And:
                        return Decorator.Enclose<object>(x.ToInt64(provider) & y.ToInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.Assign:
                        return y;
                    case AssignmentOperator.Division:
                        return Decorator.Enclose<object>(x.ToInt64(provider) / y.ToInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.ExclusiveOr:
                        return Decorator.Enclose<object>(x.ToInt64(provider) ^ y.ToInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.LeftShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x << y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Multiplication:
                        return Decorator.Enclose<object>(x.ToInt64(provider) * y.ToInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.Or:
                        return Decorator.Enclose<object>(x.ToInt64(provider) | y.ToInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.Remainder:
                        return Decorator.Enclose<object>(x.ToInt64(provider) % y.ToInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.RightShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x >> y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Subtraction:
                        return Decorator.Enclose<object>(x.ToInt64(provider) - y.ToInt64(provider)).ChangeType<T>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assignment));
                }
            case TypeCode.SByte:
                switch (assignment)
                {
                    case AssignmentOperator.Addition:
                        return Decorator.Enclose<object>(x.ToSByte(provider) + y.ToSByte(provider)).ChangeType<T>();
                    case AssignmentOperator.And:
                        return Decorator.Enclose<object>(x.ToSByte(provider) & y.ToSByte(provider)).ChangeType<T>();
                    case AssignmentOperator.Assign:
                        return y;
                    case AssignmentOperator.Division:
                        return Decorator.Enclose<object>(x.ToSByte(provider) / y.ToSByte(provider)).ChangeType<T>();
                    case AssignmentOperator.ExclusiveOr:
                        return Decorator.Enclose<object>(x.ToSByte(provider) ^ y.ToSByte(provider)).ChangeType<T>();
                    case AssignmentOperator.LeftShift:
                        return Decorator.Enclose<object>(x.ToSByte(provider) << y.ToSByte(provider)).ChangeType<T>();
                    case AssignmentOperator.Multiplication:
                        return Decorator.Enclose<object>(x.ToSByte(provider) * y.ToSByte(provider)).ChangeType<T>();
                    case AssignmentOperator.Or:
                        return Decorator.Enclose<object>(x.ToSByte(provider) | y.ToSByte(provider)).ChangeType<T>();
                    case AssignmentOperator.Remainder:
                        return Decorator.Enclose<object>(x.ToSByte(provider) % y.ToSByte(provider)).ChangeType<T>();
                    case AssignmentOperator.RightShift:
                        return Decorator.Enclose<object>(x.ToSByte(provider) >> y.ToSByte(provider)).ChangeType<T>();
                    case AssignmentOperator.Subtraction:
                        return Decorator.Enclose<object>(x.ToSByte(provider) - y.ToSByte(provider)).ChangeType<T>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assignment));
                }
            case TypeCode.Single:
                switch (assignment)
                {
                    case AssignmentOperator.Addition:
                        return Decorator.Enclose<object>(x.ToSingle(provider) + y.ToSingle(provider)).ChangeType<T>();
                    case AssignmentOperator.And:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x &= y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Assign:
                        return y;
                    case AssignmentOperator.Division:
                        return Decorator.Enclose<object>(x.ToSingle(provider) / y.ToSingle(provider)).ChangeType<T>();
                    case AssignmentOperator.ExclusiveOr:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x ^= y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.LeftShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x << y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Multiplication:
                        return Decorator.Enclose<object>(x.ToSingle(provider) * y.ToSingle(provider)).ChangeType<T>();
                    case AssignmentOperator.Or:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x |= y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Remainder:
                        return Decorator.Enclose<object>(x.ToSingle(provider) % y.ToSingle(provider)).ChangeType<T>();
                    case AssignmentOperator.RightShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x >> y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Subtraction:
                        return Decorator.Enclose<object>(x.ToSingle(provider) - y.ToSingle(provider)).ChangeType<T>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assignment));
                }
            case TypeCode.UInt16:
                switch (assignment)
                {
                    case AssignmentOperator.Addition:
                        return Decorator.Enclose<object>(x.ToUInt16(provider) + y.ToUInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.And:
                        return Decorator.Enclose<object>(x.ToUInt16(provider) & y.ToUInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.Assign:
                        return y;
                    case AssignmentOperator.Division:
                        return Decorator.Enclose<object>(x.ToUInt16(provider) / y.ToUInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.ExclusiveOr:
                        return Decorator.Enclose<object>(x.ToUInt16(provider) ^ y.ToUInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.LeftShift:
                        return Decorator.Enclose<object>(x.ToUInt16(provider) << y.ToUInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.Multiplication:
                        return Decorator.Enclose<object>(x.ToUInt16(provider) * y.ToUInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.Or:
                        return Decorator.Enclose<object>(x.ToUInt16(provider) | y.ToUInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.Remainder:
                        return Decorator.Enclose<object>(x.ToUInt16(provider) % y.ToUInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.RightShift:
                        return Decorator.Enclose<object>(x.ToUInt16(provider) >> y.ToUInt16(provider)).ChangeType<T>();
                    case AssignmentOperator.Subtraction:
                        return Decorator.Enclose<object>(x.ToUInt16(provider) - y.ToUInt16(provider)).ChangeType<T>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assignment));
                }
            case TypeCode.UInt32:
                switch (assignment)
                {
                    case AssignmentOperator.Addition:
                        return Decorator.Enclose<object>(x.ToUInt32(provider) + y.ToUInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.And:
                        return Decorator.Enclose<object>(x.ToUInt32(provider) & y.ToUInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.Assign:
                        return y;
                    case AssignmentOperator.Division:
                        return Decorator.Enclose<object>(x.ToUInt32(provider) / y.ToUInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.ExclusiveOr:
                        return Decorator.Enclose<object>(x.ToUInt32(provider) ^ y.ToUInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.LeftShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x << y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Multiplication:
                        return Decorator.Enclose<object>(x.ToUInt32(provider) * y.ToUInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.Or:
                        return Decorator.Enclose<object>(x.ToUInt32(provider) | y.ToUInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.Remainder:
                        return Decorator.Enclose<object>(x.ToUInt32(provider) % y.ToUInt32(provider)).ChangeType<T>();
                    case AssignmentOperator.RightShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x >> y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Subtraction:
                        return Decorator.Enclose<object>(x.ToUInt32(provider) - y.ToUInt32(provider)).ChangeType<T>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assignment));
                }
            case TypeCode.UInt64:
                switch (assignment)
                {
                    case AssignmentOperator.Addition:
                        return Decorator.Enclose<object>(x.ToUInt64(provider) + y.ToUInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.And:
                        return Decorator.Enclose<object>(x.ToUInt64(provider) & y.ToUInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.Assign:
                        return y;
                    case AssignmentOperator.Division:
                        return Decorator.Enclose<object>(x.ToUInt64(provider) / y.ToUInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.ExclusiveOr:
                        return Decorator.Enclose<object>(x.ToUInt64(provider) ^ y.ToUInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.LeftShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x << y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Multiplication:
                        return Decorator.Enclose<object>(x.ToUInt64(provider) * y.ToUInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.Or:
                        return Decorator.Enclose<object>(x.ToUInt64(provider) | y.ToUInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.Remainder:
                        return Decorator.Enclose<object>(x.ToUInt64(provider) % y.ToUInt64(provider)).ChangeType<T>();
                    case AssignmentOperator.RightShift:
                        throw new ArgumentOutOfRangeException(nameof(assignment), string.Format(CultureInfo.InvariantCulture, "Cannot apply assignment operator '{0}' (x >> y) to operands of type '{1}'.", Enum.GetName(typeof(AssignmentOperator), assignment), typeof(T).Name));
                    case AssignmentOperator.Subtraction:
                        return Decorator.Enclose<object>(x.ToUInt64(provider) - y.ToUInt64(provider)).ChangeType<T>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assignment));
                }
            default:
                throw new TypeArgumentException("T", string.Format(CultureInfo.InvariantCulture, "T appears to contain an invalid type. Expected type is numeric and must be one of the following: Byte, Decimal, Double, Int16, Int32, Int64, SByte, Single, UInt16, UInt32 or UInt64. Actually type was {0}.", typeof(T).Name));
        }
    }

    /// <summary>
    /// Validates if the specified <typeparamref name="T"/> is within the allowed range of numeric operands.
    /// </summary>
    /// <typeparam name="T">The type of the value for an operand operation.</typeparam>
    /// <exception cref="TypeArgumentOutOfRangeException">
    /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="float"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    public static void ValidAsNumericOperand<T>() where T : struct, IComparable<T>, IEquatable<T>, IConvertible
    {
        var valueType = typeof(T);
        var valueCode = Type.GetTypeCode(valueType);
        switch (valueCode)
        {
            case TypeCode.Byte:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.SByte:
            case TypeCode.Single:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                break;
            default:
                throw new TypeArgumentOutOfRangeException("T", string.Format(CultureInfo.InvariantCulture, "T appears to contain an invalid type. Expected type is numeric and must be one of the following: Byte, Decimal, Double, Int16, Int32, Int64, SByte, Single, UInt16, UInt32 or UInt64. Actually type was {0}.", valueType));
        }
    }
}
