using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;
using Cuemon.Security;

namespace Cuemon
{
    /// <summary>
    /// Provides a set of static methods for generating different types of values or sequences of values.
    /// </summary>
    /// <seealso cref="Eradicate"/>
    public static class Generate
    {
        private static readonly Hash Fnv1A = HashFactory.CreateFnv64(o => o.Algorithm = FowlerNollVoAlgorithm.Fnv1a);

        private static readonly ThreadLocal<Random> LocalRandomizer = new(() =>
        {
            var rnd = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(rnd);
                var seed = BitConverter.ToInt32(rnd, 0);
                return new Random(seed);
            }
        });

        /// <summary>
        /// Generates a portrayal of the specified <paramref name="instance"/> that might contain information about the instance state.
        /// </summary>
        /// <param name="instance">The instance of an <see cref="object"/> to convert.</param>
        /// <param name="setup">The <see cref="ObjectPortrayalOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> that represents the specified <paramref name="instance"/>.</returns>
        /// <remarks>
        /// When determining the representation of the specified <paramref name="instance"/>, these default rules applies:
        /// 1: if the <see cref="object.ToString"/> method has been overridden, any further processing is skipped (the assumption is, that a custom representation is already in place)
        /// 2: any public properties having index parameters is skipped
        /// 3: any public properties is appended to the result if <see cref="object.ToString"/> has not been overridden
        /// Note: do not call this method from an overridden ToString(..) method without setting <see cref="ObjectPortrayalOptions.BypassOverrideCheck"/> to <c>true</c>; otherwise a <see cref="StackOverflowException"/> will occur.
        /// </remarks>
        public static string ObjectPortrayal(object instance, Action<ObjectPortrayalOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            if (instance == null) { return options.NullValue; }

            var instanceType = instance.GetType();
            if (!options.BypassOverrideCheck)
            {
                var mi = instanceType.GetMethods().SingleOrDefault(m => m.Name == nameof(ToString) && m.GetParameters().Length == 0);
                if (Decorator.Enclose(mi).IsOverridden())
                {
                    var stringResult = instance.ToString();
                    return mi!.DeclaringType == typeof(bool) ? stringResult!.ToLowerInvariant() : stringResult;
                }
            }

            var instanceSignature = new StringBuilder(string.Format(options.FormatProvider, "{0}", Decorator.Enclose(instanceType).ToFriendlyName(o => o.FullName = true)));
            var properties = instanceType.GetRuntimeProperties().Where(options.PropertiesPredicate);
            instanceSignature.AppendFormat(options.FormatProvider, " {{ {0} }}", DelimitedString.Create(properties, o =>
            {
                o.Delimiter = options.Delimiter;
                o.StringConverter = pi => options.PropertyConverter(pi, instance, options.FormatProvider);
            }));
            return instanceSignature.ToString();
        }

        /// <summary>
        /// Generates a sequence of <typeparamref name="T"/> within a specified range.
        /// </summary>
        /// <typeparam name="T">The type of the elements to return.</typeparam>
        /// <param name="count">The number of <typeparamref name="T"/> to generate.</param>
        /// <param name="generator">The function delegate that will resolve the instance of <typeparamref name="T"/>; the parameter passed to the delegate represents the index (zero-based) of the element to return.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains a range of <typeparamref name="T"/> elements.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="generator"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is less than 0.
        /// </exception>
        public static IEnumerable<T> RangeOf<T>(int count, Func<int, T> generator)
        {
            Validator.ThrowIfNull(generator);
            Validator.ThrowIfLowerThan(count, 0, nameof(count));
            for (var i = 0; i < count; i++) { yield return generator(i); }
        }

        /// <summary>
        /// Generates a random integer that is within a specified range.
        /// </summary>
        /// <param name="maximumExclusive">The exclusive upper bound of the random number returned. <paramref name="maximumExclusive"/> must be greater than or equal to 0.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to 0 and less than <paramref name="maximumExclusive"/>; that is, the range of return values includes 0 but not <paramref name="maximumExclusive"/>.
        /// If 0 equals <paramref name="maximumExclusive"/>, 0 is returned.
        /// </returns>
        public static int RandomNumber(int maximumExclusive = int.MaxValue)
        {
            return RandomNumber(0, maximumExclusive);
        }

        /// <summary>
        /// Generates a random integer that is within a specified range.
        /// </summary>
        /// <param name="minimumInclusive">The inclusive lower bound of the random number returned.</param>
        /// <param name="maximumExclusive">The exclusive upper bound of the random number returned. <paramref name="maximumExclusive"/> must be greater than or equal to <paramref name="minimumInclusive"/>.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to <paramref name="minimumInclusive"/> and less than <paramref name="maximumExclusive"/>; that is, the range of return values includes <paramref name="minimumInclusive"/> but not <paramref name="maximumExclusive"/>.
        /// If <paramref name="minimumInclusive"/> equals <paramref name="maximumExclusive"/>, <paramref name="minimumInclusive"/> is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minimumInclusive" /> is greater than <paramref name="maximumExclusive"/>.
        /// </exception>
        public static int RandomNumber(int minimumInclusive, int maximumExclusive)
        {
            Validator.ThrowIfGreaterThan(minimumInclusive, maximumExclusive, nameof(minimumInclusive));
            return LocalRandomizer.Value.Next(minimumInclusive, maximumExclusive);
        }

        /// <summary>
        /// Generates a string from the specified Unicode character repeated until the specified length.
        /// </summary>
        /// <param name="c">A Unicode character.</param>
        /// <param name="count">The number of times <paramref name="c"/> occurs.</param>
        /// <returns>A <see cref="string"/> filled with the specified <paramref name="c"/> until the specified <paramref name="count"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is less than zero.
        /// </exception>
        public static string FixedString(char c, int count)
        {
            return new string(c, count);
        }

        /// <summary>
        /// Generates a random string with the specified length using values of <see cref="Alphanumeric.LettersAndNumbers"/>.
        /// </summary>
        /// <param name="length">The length of the random string to generate.</param>
        /// <returns>A random string from the values of <see cref="Alphanumeric.LettersAndNumbers"/>.</returns>
        public static string RandomString(int length)
        {
            return RandomString(length, Alphanumeric.LettersAndNumbers);
        }

        /// <summary>
        /// Generates a random string with the specified length from the provided values.
        /// </summary>
        /// <param name="length">The length of the random string to generate.</param>
        /// <param name="values">The values to use in the randomization process.</param>
        /// <returns>A random string from the values provided.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="values"/> contains no elements.
        /// </exception>
        public static string RandomString(int length, params string[] values)
        {
            Validator.ThrowIfSequenceNullOrEmpty(values, nameof(values));
            if (length <= 0) { return string.Empty; }

            var random = LocalRandomizer.Value;
            var buckets = values;
            var bucketCount = buckets.Length;
            var chars = new char[length];

            for (var i = 0; i < length; i++)
            {
                var bucketIndex = random.Next(bucketCount);
                var bucket = buckets[bucketIndex];
                var charIndex = random.Next(bucket.Length);
                chars[i] = bucket[charIndex];
            }

            return new string(chars);
        }

        /// <summary>
        /// Computes a suitable hash code from the variable number of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A variable number of objects implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 32-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        public static int HashCode32(params IConvertible[] convertibles)
        {
            return HashCode32(Arguments.ToEnumerableOf(convertibles));
        }

        /// <summary>
        /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A sequence of objects implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 32-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        public static int HashCode32(IEnumerable<IConvertible> convertibles)
        {
            return Fnv1A.ComputeHash(convertibles).To(bytes => BitConverter.ToInt32(bytes, 0));
        }

        /// <summary>
        /// Computes a suitable hash code from the variable number of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A variable number of objects implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 64-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        public static long HashCode64(params IConvertible[] convertibles)
        {
            return HashCode64(Arguments.ToEnumerableOf(convertibles));
        }

        /// <summary>
        /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A sequence of objects implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 64-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        public static long HashCode64(IEnumerable<IConvertible> convertibles)
        {
            return Fnv1A.ComputeHash(convertibles).To(bytes => BitConverter.ToInt64(bytes, 0));
        }
    }
}