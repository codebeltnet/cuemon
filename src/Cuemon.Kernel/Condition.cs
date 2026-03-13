using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Provide ways to verify conditions a generic way for countless scenarios using true/false propositions.
    /// </summary>
    public sealed class Condition
    {
        private static readonly Condition ExtendedCondition = new();

        private static readonly Regex RegExEmailAddressValidator = new(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
        RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, TimeSpan.FromSeconds(2));

        /// <summary>
        /// Gets the singleton instance of the Condition functionality allowing for extensions methods like: <c>Condition.Query.IsTrue()</c>.
        /// </summary>
        /// <value>The singleton instance of the Condition functionality.</value>
        public static Condition Query { get; } = ExtendedCondition;

        /// <summary>
        /// Determines whether the two specified <paramref name="x"/> and <paramref name="y"/> are equal by using the default equality operator from <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> are equal to <paramref name="y"/>; otherwise <c>false</c>.</returns>
        public static bool AreEqual<T>(T x, T y)
        {
            return AreEqual(x, y, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Determines whether the two specified <paramref name="x"/> and <paramref name="y"/> are equal by using the equality operator.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <returns><c>true</c> if <paramref name="x"/> are equal to <paramref name="y"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer"/> is null.
        /// </exception>
        public static bool AreEqual<T>(T x, T y, IEqualityComparer<T> comparer)
        {
            Validator.ThrowIfNull(comparer);
            return comparer.Equals(x, y);
        }

        /// <summary>
        /// Determines whether the two specified <paramref name="x"/> and <paramref name="y"/> are different by using the default equality operator from <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> are different from <paramref name="y"/>; otherwise <c>false</c>.</returns>
        public static bool AreNotEqual<T>(T x, T y)
        {
            return AreNotEqual(x, y, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Determines whether the two specified <paramref name="x"/> and <paramref name="y"/> are different by using the equality operator.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <returns><c>true</c> if <paramref name="x"/> are different from <paramref name="y"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer"/> is null.
        /// </exception>
        public static bool AreNotEqual<T>(T x, T y, IEqualityComparer<T> comparer)
        {
            Validator.ThrowIfNull(comparer);
            return !AreEqual(x, y, comparer);
        }

        /// <summary>
        /// Determines whether the two specified <paramref name="x"/> object are not of the same instance as the <paramref name="y"/> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> object are not of the same instance as the <paramref name="y"/> object; otherwise <c>false</c>.</returns>
        public static bool AreNotSame<T>(T x, T y)
        {
            return !AreSame(x, y);
        }

        /// <summary>
        /// Determines whether the two specified <paramref name="x"/> object are of the same instance as the <paramref name="y"/> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> object are of the same instance as the <paramref name="y"/> object; otherwise <c>false</c>.</returns>
        public static bool AreSame<T>(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        /// <summary>
        /// Invokes one of two expressions depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked; when <c>false</c>, the <paramref name="secondExpression"/> is invoked.</param>
        /// <param name="firstExpression">The delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        public static void FlipFlop(bool condition, Action firstExpression, Action secondExpression)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            if (IsTrue(condition)) { firstExpression(); }
            if (IsFalse(condition)) { secondExpression(); }
        }

        /// <summary>
        /// Invokes one of two expressions depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked; when <c>false</c>, the <paramref name="secondExpression"/> is invoked.</param>
        /// <param name="firstExpression">The delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <param name="arg">The parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        public static void FlipFlop<T>(bool condition, Action<T> firstExpression, Action<T> secondExpression, T arg)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            if (IsTrue(condition)) { firstExpression(arg); }
            if (IsFalse(condition)) { secondExpression(arg); }
        }

        /// <summary>
        /// Invokes one of two expressions depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked; when <c>false</c>, the <paramref name="secondExpression"/> is invoked.</param>
        /// <param name="firstExpression">The delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <param name="arg1">The first parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg2">The second parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        public static void FlipFlop<T1, T2>(bool condition, Action<T1, T2> firstExpression, Action<T1, T2> secondExpression, T1 arg1, T2 arg2)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            if (IsTrue(condition)) { firstExpression(arg1, arg2); }
            if (IsFalse(condition)) { secondExpression(arg1, arg2); }
        }

        /// <summary>
        /// Invokes one of two expressions depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked; when <c>false</c>, the <paramref name="secondExpression"/> is invoked.</param>
        /// <param name="firstExpression">The delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <param name="arg1">The first parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg2">The second parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg3">The third parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        public static void FlipFlop<T1, T2, T3>(bool condition, Action<T1, T2, T3> firstExpression, Action<T1, T2, T3> secondExpression, T1 arg1, T2 arg2, T3 arg3)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            if (IsTrue(condition)) { firstExpression(arg1, arg2, arg3); }
            if (IsFalse(condition)) { secondExpression(arg1, arg2, arg3); }
        }

        /// <summary>
        /// Invokes one of two expressions depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked; when <c>false</c>, the <paramref name="secondExpression"/> is invoked.</param>
        /// <param name="firstExpression">The delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <param name="arg1">The first parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg2">The second parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg3">The third parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        public static void FlipFlop<T1, T2, T3, T4>(bool condition, Action<T1, T2, T3, T4> firstExpression, Action<T1, T2, T3, T4> secondExpression, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            if (IsTrue(condition)) { firstExpression(arg1, arg2, arg3, arg4); }
            if (IsFalse(condition)) { secondExpression(arg1, arg2, arg3, arg4); }
        }

        /// <summary>
        /// Invokes one of two expressions depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked; when <c>false</c>, the <paramref name="secondExpression"/> is invoked.</param>
        /// <param name="firstExpression">The delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <param name="arg1">The first parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg2">The second parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg3">The third parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        public static void FlipFlop<T1, T2, T3, T4, T5>(bool condition, Action<T1, T2, T3, T4, T5> firstExpression, Action<T1, T2, T3, T4, T5> secondExpression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            if (IsTrue(condition)) { firstExpression(arg1, arg2, arg3, arg4, arg5); }
            if (IsFalse(condition)) { secondExpression(arg1, arg2, arg3, arg4, arg5); }
        }

        /// <summary>
        /// Invokes one of two expressions depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked; when <c>false</c>, the <paramref name="secondExpression"/> is invoked.</param>
        /// <param name="firstExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task FlipFlopAsync(bool condition, Func<Task> firstExpression, Func<Task> secondExpression)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            return condition ? firstExpression() : secondExpression();
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> contains at least one of the succession <paramref name="characters"/> of <paramref name="length"/>.
        /// </summary>
        /// <param name="value">The value to test for consecutive characters.</param>
        /// <param name="characters">The character to locate with the specified <paramref name="length"/>.</param>
        /// <param name="length">The number of characters in succession.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> contains at least one of the succession <paramref name="characters"/> of <paramref name="length"/>; otherwise, <c>false</c>.</returns>
        public static bool HasConsecutiveCharacters(string value, IEnumerable<char> characters, int length = 2)
        {
            if (string.IsNullOrWhiteSpace(value)) { return false; }
            if (value.Length == 1) { return false; }
            if (characters is null) { return false; }
            foreach (var sc in characters)
            {
                if (HasConsecutiveCharacters(value, sc, length)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> contains a succession <paramref name="character"/> of <paramref name="length"/>.
        /// </summary>
        /// <param name="value">The value to test for consecutive characters.</param>
        /// <param name="character">The characters to locate with the specified <paramref name="length"/>.</param>
        /// <param name="length">The number of characters in succession.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> contains a succession <paramref name="character"/> of <paramref name="length"/>; otherwise, <c>false</c>.</returns>
        public static bool HasConsecutiveCharacters(string value, char character, int length = 2)
        {
            if (length < 2) { length = 2; }
            if (string.IsNullOrWhiteSpace(value)) { return false; }
            if (value.Length == 1) { return false; }
            return value.Contains(new string(character, length));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> matches a base-64 structure.
        /// </summary>
        /// <param name="value">The value to test for a Base64 structure.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> matches a base-64 structure; otherwise, <c>false</c>.</returns>
        public static bool IsBase64(string value)
        {
            if (string.IsNullOrEmpty(value)) { return false; }
            return Patterns.TryInvoke(() => Convert.FromBase64String(value), out _);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> consists only of binary digits.
        /// </summary>
        /// <param name="value">The string to verify consist only of binary digits.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> consists only of binary digits; otherwise, <c>false</c>.</returns>
        public static bool IsBinaryDigits(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return false; }
            return !value.Any(ch => ch < '0' || ch > '1');
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence).
        /// </summary>
        /// <param name="source">The value to test for a sequence of countable characters.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence); otherwise, <c>false</c>.</returns>
        public static bool IsCountableSequence(IEnumerable<int> source)
        {
            return IsCountableSequence(source.Select(Convert.ToInt64));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence).
        /// </summary>
        /// <param name="source">The value to test for a sequence of countable characters.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence); otherwise, <c>false</c>.</returns>
        public static bool IsCountableSequence(IEnumerable<long> source)
        {
            if (source is null) { return false; }
            var numbers = new List<long>(source);

            var x = numbers[0];
            var y = numbers[1];

            var difference = y - x;
            for (var i = 2; i < numbers.Count; i++)
            {
                x = numbers[i];
                y = numbers[i - 1];
                if ((x - y) != difference) { return false; }
            }
            return true;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is a sequence of countable characters (hence, characters being either incremented or decremented with the same cardinality through out the sequence).
        /// </summary>
        /// <param name="value">The value to test for a sequence of countable characters.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is a sequence of countable characters (hence, characters being either incremented or decremented with the same cardinality through out the sequence); otherwise, <c>false</c>.</returns>
        public static bool IsCountableSequence(string value)
        {
            if (string.IsNullOrEmpty(value)) { return false; }
            if (value.Length < 2) { return false; }
            return IsCountableSequence(value.Select(Convert.ToInt64));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> has its initial default value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The object to verify has its initial default value.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> has its initial default value; otherwise, <c>false</c>.</returns>
        public static bool IsDefault<T>(T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> has a valid format of an email address.
        /// </summary>
        /// <param name="value">The string to verify has a valid format of an email address.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> has a valid format of an email address; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// In my search for the most comprehensive and up-to-date regular expression for email address validation, this was the article I choose to implement: http://blog.trojanhunter.com/2012/09/26/the-best-regex-to-validate-an-email-address/.
        /// </remarks>
        public static bool IsEmailAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return false; }
            return (RegExEmailAddressValidator.Match(value).Length > 0);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is empty ("").
        /// </summary>
        /// <param name="value">The string to verify is empty.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is empty; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty(string value)
        {
            if (value is null) { return false; }
            return (value.Length == 0);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is valid by attempting to construct an enumeration of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration to validate.</typeparam>
        /// <param name="value">The <see cref="string"/> containing the name or value used to attempt to construct an <see cref="Enum"/>.</param>
        /// <param name="setup">The <see cref="EnumStringOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is a valid enumeration; otherwise, <c>false</c>.</returns>
        public static bool IsEnum<T>(string value, Action<EnumStringOptions> setup = null) where T : struct, IConvertible
        {
            if (string.IsNullOrWhiteSpace(value)) { return false; }
            var enumType = typeof(T);
            if (!enumType.GetTypeInfo().IsEnum) { return false; }
            try
            {
                var options = Patterns.Configure(setup);
                var hasFlags = enumType.GetTypeInfo().IsDefined(typeof(FlagsAttribute), false);
                var result = Enum.Parse(enumType, value, options.IgnoreCase);
                if (hasFlags && value.IndexOf(',') != -1) { return true; }
                return Enum.IsDefined(enumType, result);
            }
            catch (Exception e) when (Patterns.IsRecoverableException(e))
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is an even number.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is an even number; otherwise, <c>false</c>.</returns>
        public static bool IsEven(int value)
        {
            return ((value % 2) == 0);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is <c>false</c>.
        /// </summary>
        /// <param name="value">The value to verify is <c>false</c>.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is <c>false</c>; otherwise, <c>false</c>.</returns>
        public static bool IsFalse(bool value)
        {
            return !value;
        }

        /// <summary>
        /// Invokes the delegate <paramref name="expression"/> when value of <paramref name="condition"/> is <c>false</c>.
        /// </summary>
        /// <param name="condition">When <c>false</c>, the <paramref name="expression"/> delegate is invoked.</param>
        /// <param name="expression">The delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        public static void IsFalse(bool condition, Action expression)
        {
            Validator.ThrowIfNull(expression);
            if (IsFalse(condition)) { expression(); }
        }

        /// <summary>
        /// Determines whether the specified <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is greater than <paramref name="y"/>; otherwise <c>false</c>.</returns>
        public static bool IsGreaterThan<T>(T x, T y) where T : struct, IConvertible
        {
            return Comparer<T>.Default.Compare(x, y) > 0;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="x"/> is greater than or equal to <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is greater than or equal to <paramref name="y"/>; otherwise <c>false</c>.</returns>
        public static bool IsGreaterThanOrEqual<T>(T x, T y) where T : struct, IConvertible
        {
            return (IsGreaterThan(x, y) || AreEqual(x, y));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> has a valid format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The string to verify has a valid format of a <see cref="Guid"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> has a format of a <see cref="Guid"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This implementation only evaluates for GUID formats of: <see cref="GuidFormats.D"/> | <see cref="GuidFormats.B"/> | <see cref="GuidFormats.P"/>, eg. 32 digits separated by hyphens; 32 digits separated by hyphens, enclosed in brackets and 32 digits separated by hyphens, enclosed in parentheses.<br/>
        /// The reason not to include <see cref="GuidFormats.N"/>, eg. 32 digits is the possible unintended GUID result of a MD5 string representation.
        /// </remarks>
        public static bool IsGuid(string value)
        {
            return IsGuid(value, GuidFormats.B | GuidFormats.D | GuidFormats.P);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> has a valid format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The string to verify has a valid format of a <see cref="Guid"/>.</param>
        /// <param name="format">A bitmask comprised of one or more <see cref="GuidFormats"/> that specify how the GUID parsing is conducted.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> has a format of a <see cref="Guid"/>; otherwise, <c>false</c>.</returns>
        public static bool IsGuid(string value, GuidFormats format)
        {
            if (string.IsNullOrWhiteSpace(value)) { return false; }
            return TryParseGuid(value, format);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is hexadecimal.
        /// </summary>
        /// <param name="value">The string to verify is hexadecimal.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is hexadecimal; otherwise, <c>false</c>.</returns>
        public static bool IsHex(string value)
        {
            if (string.IsNullOrEmpty(value)) { return false; }
            if (!IsEven(value.Length)) { return false; }
            using (var reader = new StringReader(value))
            {
                var even = value.Length / 2;
                for (var i = 0; i < even; ++i)
                {
                    var char1 = (char)reader.Read();
                    var char2 = (char)reader.Read();
                    if (!IsHexDigit(char1) || !IsHexDigit(char2)) { return false; }
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is hexadecimal.
        /// </summary>
        /// <param name="value">The character to verify is hexadecimal.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is hexadecimal; otherwise, <c>false</c>.</returns>
        public static bool IsHex(char value)
        {
            return IsHexDigit(value);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="x"/> is lower than <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is lower than <paramref name="y"/>; otherwise <c>false</c>.</returns>
        public static bool IsLowerThan<T>(T x, T y) where T : struct, IConvertible
        {
            return Comparer<T>.Default.Compare(x, y) < 0;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="x"/> is lower than or equal to <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is lower than or equal to <paramref name="y"/>; otherwise <c>false</c>.</returns>
        public static bool IsLowerThanOrEqual<T>(T x, T y) where T : struct, IConvertible
        {
            return (IsLowerThan(x, y) || AreEqual(x, y));
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> does not have its initial default value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The object to verify does not have its initial default value.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> does not have its initial default value; otherwise, <c>false</c>.</returns>
        public static bool IsNotDefault<T>(T value)
        {
            return !IsDefault(value);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is not null.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The object to verify is not null.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is not null; otherwise, <c>false</c>.</returns>
        public static bool IsNotNull<T>(T value)
        {
            return !IsNull(value);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="x"/> is outside the range of <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The object to compare.</param>
        /// <param name="min">The minimum value of <paramref name="x"/>.</param>
        /// <param name="max">The maximum value of <paramref name="x"/>.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is outside the range of <paramref name="min"/> and <paramref name="max"/>; otherwise <c>false</c>.</returns>
        public static bool IsNotWithinRange<T>(T x, T min, T max) where T : struct, IConvertible
        {
            return !IsWithinRange(x, min, max);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is null.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The object to verify is null.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is null; otherwise, <c>false</c>.</returns>
        public static bool IsNull<T>(T value)
        {
            return (value is null);
        }

        /// <summary>
        /// Determines whether the specified value can be evaluated as a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about <paramref name="value"/>.</param>
        /// <returns><c>true</c> if the specified value can be evaluated as a number; otherwise, <c>false</c>.</returns>
        public static bool IsNumeric(string value, NumberStyles styles = NumberStyles.Number, IFormatProvider provider = null)
        {
            if (string.IsNullOrWhiteSpace(value)) { return false; }
            if (string.Equals(value, "NaN", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (string.Equals(value, "Infinity", StringComparison.OrdinalIgnoreCase)) { return false; }
            provider ??= CultureInfo.InvariantCulture;
            return double.TryParse(value, styles, provider, out _);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is an odd number.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is an odd number; otherwise, <c>false</c>.</returns>
        public static bool IsOdd(int value)
        {
            return !IsEven(value);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is a prime number.
        /// </summary>
        /// <param name="value">The positive integer to determine whether is a prime number.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is a prime number; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> has a value smaller than 0.
        /// </exception>
        public static bool IsPrime(int value)
        {
            if (value < 0) { throw new ArgumentException("Value must have a value equal or higher than 0.", nameof(value)); }
            if ((value & 1) == 0) { return value == 2; }
            for (long i = 3; (i * i) <= value; i += 2)
            {
                if ((value % i) == 0) { return false; }
            }
            return value != 1;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is valid by attempting to construct a URI.
        /// </summary>
        /// <param name="value">The <see cref="string"/> used to attempt to construct a <see cref="Uri"/>.</param>
        /// <param name="setup">The <see cref="ProtocolRelativeUriStringOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is a protocol relative URI; otherwise, <c>false</c>.</returns>
        public static bool IsProtocolRelativeUrl(string value, Action<ProtocolRelativeUriStringOptions> setup = null)
        {
            if (string.IsNullOrWhiteSpace(value)) { return false; }
            try
            {
                var options = Patterns.Configure(setup, validator: o =>
                {
                    Validator.ThrowIfFalse(value.StartsWith(o.RelativeReference, StringComparison.OrdinalIgnoreCase), nameof(value), FormattableString.Invariant($"The specified input did not start with the expected input of: {o.RelativeReference}."));
                });
                var relativeReferenceLength = options.RelativeReference.Length;
                var candidate = value.Remove(0, relativeReferenceLength).Insert(0, FormattableString.Invariant($"{CreateUriScheme(options.Protocol)}://"));
                return Uri.TryCreate(candidate, UriKind.Absolute, out _);
            }
            catch (Exception e) when (Patterns.IsRecoverableException(e))
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is <c>true</c>.
        /// </summary>
        /// <param name="value">The value to verify is <c>true</c>.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool IsTrue(bool value)
        {
            return value;
        }

        /// <summary>
        /// Invokes the delegate <paramref name="expression"/> when value of <paramref name="condition"/> is <c>true</c>.
        /// </summary>
        /// <param name="condition">When <c>true</c>, the <paramref name="expression"/> delegate is invoked.</param>
        /// <param name="expression">The delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        public static void IsTrue(bool condition, Action expression)
        {
            Validator.ThrowIfNull(expression);
            if (IsTrue(condition)) { expression(); }
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is valid by attempting to construct a URI.
        /// </summary>
        /// <param name="value">The <see cref="string"/> used to attempt to construct a <see cref="Uri"/>.</param>
        /// <param name="setup">The <see cref="UriStringOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is a valid URI; otherwise, <c>false</c>.</returns>
        public static bool IsUri(string value, Action<UriStringOptions> setup = null)
        {
            if (string.IsNullOrWhiteSpace(value)) { return false; }
            try
            {
                Validator.ThrowIfInvalidConfigurator(setup, out UriStringOptions options);
                var isValid = options.Kind == UriKind.Relative;
                if (!isValid)
                {
                    foreach (var scheme in options.Schemes)
                    {
                        switch (scheme)
                        {
                            case UriScheme.Undefined:
                                break;
                            case UriScheme.File:
                            case UriScheme.Ftp:
                            case UriScheme.Sftp:
                            case UriScheme.Gopher:
                            case UriScheme.Http:
                            case UriScheme.Https:
                            case UriScheme.Mailto:
                            case UriScheme.NetPipe:
                            case UriScheme.NetTcp:
                            case UriScheme.News:
                            case UriScheme.Nntp:
                                isValid = value.StartsWith(CreateUriScheme(scheme), StringComparison.OrdinalIgnoreCase);
                                break;
                            default:
                                throw new InvalidEnumArgumentException(nameof(setup), (int)scheme, typeof(UriScheme));
                        }
                        if (isValid) { break; }
                    }
                }
                return isValid && Uri.TryCreate(value, options.Kind, out _);
            }
            catch (Exception e) when (Patterns.IsRecoverableException(e))
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> consist only of white-space characters.
        /// </summary>
        /// <param name="value">The string to verify consist only of white-space characters.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> consist only of white-space characters; otherwise, <c>false</c>.</returns>
        public static bool IsWhiteSpace(string value)
        {
            if (value is null) { return false; }
            return value.All(char.IsWhiteSpace);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="x"/> is within range of <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The object to compare.</param>
        /// <param name="min">The minimum value of <paramref name="x"/>.</param>
        /// <param name="max">The maximum value of <paramref name="x"/>.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is within range of <paramref name="min"/> and <paramref name="max"/>; otherwise <c>false</c>.</returns>
        public static bool IsWithinRange<T>(T x, T min, T max) where T : struct, IConvertible
        {
            return (IsGreaterThanOrEqual(x, min) && IsLowerThanOrEqual(x, max));
        }

        /// <summary>
        /// Returns one of two values depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked and becomes the result; when <c>false</c>, the <paramref name="secondExpression"/> is invoked and becomes the result.</param>
        /// <param name="firstExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <returns>The result of either function delegate <paramref name="firstExpression"/> or function delegate <paramref name="secondExpression"/>.</returns>
        public static TResult TernaryIf<TResult>(bool condition, Func<TResult> firstExpression, Func<TResult> secondExpression)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            return condition ? firstExpression() : secondExpression();
        }

        /// <summary>
        /// Returns one of two values depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked and becomes the result; when <c>false</c>, the <paramref name="secondExpression"/> is invoked and becomes the result.</param>
        /// <param name="firstExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <param name="arg">The parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <returns>The result of either function delegate <paramref name="firstExpression"/> or function delegate <paramref name="secondExpression"/>.</returns>
        public static TResult TernaryIf<T, TResult>(bool condition, Func<T, TResult> firstExpression, Func<T, TResult> secondExpression, T arg)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            return condition ? firstExpression(arg) : secondExpression(arg);
        }

        /// <summary>
        /// Returns one of two values depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked and becomes the result; when <c>false</c>, the <paramref name="secondExpression"/> is invoked and becomes the result.</param>
        /// <param name="firstExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <param name="arg1">The first parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg2">The second parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <returns>The result of either function delegate <paramref name="firstExpression"/> or function delegate <paramref name="secondExpression"/>.</returns>
        public static TResult TernaryIf<T1, T2, TResult>(bool condition, Func<T1, T2, TResult> firstExpression, Func<T1, T2, TResult> secondExpression, T1 arg1, T2 arg2)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            return condition ? firstExpression(arg1, arg2) : secondExpression(arg1, arg2);
        }

        /// <summary>
        /// Returns one of two values depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked and becomes the result; when <c>false</c>, the <paramref name="secondExpression"/> is invoked and becomes the result.</param>
        /// <param name="firstExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <param name="arg1">The first parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg2">The second parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg3">The third parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <returns>The result of either function delegate <paramref name="firstExpression"/> or function delegate <paramref name="secondExpression"/>.</returns>
        public static TResult TernaryIf<T1, T2, T3, TResult>(bool condition, Func<T1, T2, T3, TResult> firstExpression, Func<T1, T2, T3, TResult> secondExpression, T1 arg1, T2 arg2, T3 arg3)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            return condition ? firstExpression(arg1, arg2, arg3) : secondExpression(arg1, arg2, arg3);
        }

        /// <summary>
        /// Returns one of two values depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked and becomes the result; when <c>false</c>, the <paramref name="secondExpression"/> is invoked and becomes the result.</param>
        /// <param name="firstExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <param name="arg1">The first parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg2">The second parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg3">The third parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <returns>The result of either function delegate <paramref name="firstExpression"/> or function delegate <paramref name="secondExpression"/>.</returns>
        public static TResult TernaryIf<T1, T2, T3, T4, TResult>(bool condition, Func<T1, T2, T3, T4, TResult> firstExpression, Func<T1, T2, T3, T4, TResult> secondExpression, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            return condition ? firstExpression(arg1, arg2, arg3, arg4) : secondExpression(arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Returns one of two values depending on the value of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="condition">When <c>true</c>, the <paramref name="firstExpression"/> is invoked and becomes the result; when <c>false</c>, the <paramref name="secondExpression"/> is invoked and becomes the result.</param>
        /// <param name="firstExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The function delegate that is invoked when <paramref name="condition"/> is <c>false</c>.</param>
        /// <param name="arg1">The first parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg2">The second parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg3">The third parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegates <paramref name="firstExpression"/> and <paramref name="secondExpression"/>.</param>
        /// <returns>The result of either function delegate <paramref name="firstExpression"/> or function delegate <paramref name="secondExpression"/>.</returns>
        public static TResult TernaryIf<T1, T2, T3, T4, T5, TResult>(bool condition, Func<T1, T2, T3, T4, T5, TResult> firstExpression, Func<T1, T2, T3, T4, T5, TResult> secondExpression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Validator.ThrowIfNull(firstExpression);
            Validator.ThrowIfNull(secondExpression);
            return condition ? firstExpression(arg1, arg2, arg3, arg4, arg5) : secondExpression(arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Determines whether there is a set difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </summary>
        /// <param name="first">The value where characters that are not also in <paramref name="second"/> will be returned.</param>
        /// <param name="second">The value to compare with <paramref name="first"/>.</param>
        /// <param name="difference">The set difference between <paramref name="second"/> and <paramref name="first"/> or <see cref="string.Empty"/> if no difference.</param>
        /// <returns>
        /// 	<c>true</c> if there is a set difference between <paramref name="second"/> and <paramref name="first"/>; otherwise <c>false</c>.
        /// </returns>
        public static bool HasDifference(string first, string second, out string difference)
        {
            first ??= string.Empty;
            second ??= string.Empty;
            difference = string.Concat(second.Except(first));
            return difference.Length != 0;
        }

        private static bool IsHexDigit(char character)
        {
            if (character >= 48 && character <= 57 || character >= 65 && character <= 70)
                return true;
            if (character >= 97)
                return character <= 102;
            return false;
        }

        private static string CreateUriScheme(UriScheme scheme)
        {
            return scheme switch
            {
                UriScheme.File => "file",
                UriScheme.Ftp => "ftp",
                UriScheme.Gopher => "gopher",
                UriScheme.Http => "http",
                UriScheme.Https => "https",
                UriScheme.Mailto => "mailto",
                UriScheme.NetPipe => "net.pipe",
                UriScheme.NetTcp => "net.tcp",
                UriScheme.News => "news",
                UriScheme.Nntp => "nntp",
                UriScheme.Sftp => "sftp",
                _ => UriScheme.Undefined.ToString()
            };
        }

        private static bool TryParseGuid(string value, GuidFormats format)
        {
            if (format.HasFlag(GuidFormats.Any)) { return Guid.TryParse(value, out _); }

            var hasHyphens = value.IndexOf('-') != -1;
            var hasBraces = value.StartsWith("{", StringComparison.OrdinalIgnoreCase) && value.EndsWith("}", StringComparison.OrdinalIgnoreCase);
            var hasParentheses = value.StartsWith("(", StringComparison.OrdinalIgnoreCase) && value.EndsWith(")", StringComparison.OrdinalIgnoreCase);
            var hasHexadecimalStructure = hasBraces && value.Split(',').Length == 11;

            return (!hasHyphens && format.HasFlag(GuidFormats.N) && Guid.TryParseExact(value, "N", out _))
                   || (hasHyphens && format.HasFlag(GuidFormats.D) && Guid.TryParseExact(value, "D", out _))
                   || (hasBraces && hasHyphens && format.HasFlag(GuidFormats.B) && Guid.TryParseExact(value, "B", out _))
                   || (hasParentheses && hasHyphens && format.HasFlag(GuidFormats.P) && Guid.TryParseExact(value, "P", out _))
                   || (hasHexadecimalStructure && format.HasFlag(GuidFormats.X) && Guid.TryParseExact(value, "X", out _));
        }
    }
}
