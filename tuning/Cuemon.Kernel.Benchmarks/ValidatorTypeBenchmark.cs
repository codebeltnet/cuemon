using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon
{
    /// <summary>
    /// Measures the interface and type-members of <see cref="Validator"/>.
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class ValidatorTypeBenchmark
    {
        private const string ParamName = "argument";

        private readonly Type[] _baseTypes = { typeof(Exception) };
        private readonly Type[] _interfaceTypes = { typeof(IConvertible) };

        /// <summary>
        /// Measures the generic interface inclusion guard.
        /// </summary>
        [Benchmark(Baseline = true, Description = "ThrowIfContainsInterface - generic")]
        [BenchmarkCategory("Interface guards")]
        public void ThrowIfContainsInterface_Generic()
        {
            Validator.ThrowIfContainsInterface<Stream>(ParamName, _interfaceTypes);
        }

        /// <summary>
        /// Measures the generic interface inclusion guard with a custom message.
        /// </summary>
        [Benchmark(Description = "ThrowIfContainsInterface - generic message")]
        [BenchmarkCategory("Interface guards")]
        public void ThrowIfContainsInterface_GenericMessage()
        {
            Validator.ThrowIfContainsInterface<Stream>(ParamName, "message", _interfaceTypes);
        }

        /// <summary>
        /// Measures the type interface inclusion guard.
        /// </summary>
        [Benchmark(Description = "ThrowIfContainsInterface - type")]
        [BenchmarkCategory("Interface guards")]
        public void ThrowIfContainsInterface_Type()
        {
            Validator.ThrowIfContainsInterface(typeof(Stream), _interfaceTypes);
        }

        /// <summary>
        /// Measures the generic interface exclusion guard.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotContainsInterface - generic")]
        [BenchmarkCategory("Interface guards")]
        public void ThrowIfNotContainsInterface_Generic()
        {
            Validator.ThrowIfNotContainsInterface<string>(ParamName, _interfaceTypes);
        }

        /// <summary>
        /// Measures the generic interface exclusion guard with a custom message.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotContainsInterface - generic message")]
        [BenchmarkCategory("Interface guards")]
        public void ThrowIfNotContainsInterface_GenericMessage()
        {
            Validator.ThrowIfNotContainsInterface<string>(ParamName, "message", _interfaceTypes);
        }

        /// <summary>
        /// Measures the type interface exclusion guard.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotContainsInterface - type")]
        [BenchmarkCategory("Interface guards")]
        public void ThrowIfNotContainsInterface_Type()
        {
            Validator.ThrowIfNotContainsInterface(typeof(string), _interfaceTypes);
        }

        /// <summary>
        /// Measures the object type inclusion guard.
        /// </summary>
        [Benchmark(Description = "ThrowIfContainsType - object")]
        [BenchmarkCategory("Type guards")]
        public void ThrowIfContainsType_Object()
        {
            Validator.ThrowIfContainsType("Cuemon", _baseTypes);
        }

        /// <summary>
        /// Measures the type inclusion guard.
        /// </summary>
        [Benchmark(Description = "ThrowIfContainsType - type")]
        [BenchmarkCategory("Type guards")]
        public void ThrowIfContainsType_Type()
        {
            Validator.ThrowIfContainsType(typeof(string), _baseTypes);
        }

        /// <summary>
        /// Measures the generic type inclusion guard.
        /// </summary>
        [Benchmark(Description = "ThrowIfContainsType - generic")]
        [BenchmarkCategory("Type guards")]
        public void ThrowIfContainsType_Generic()
        {
            Validator.ThrowIfContainsType<string>(ParamName, _baseTypes);
        }

        /// <summary>
        /// Measures the generic type inclusion guard with a custom message.
        /// </summary>
        [Benchmark(Description = "ThrowIfContainsType - generic message")]
        [BenchmarkCategory("Type guards")]
        public void ThrowIfContainsType_GenericMessage()
        {
            Validator.ThrowIfContainsType<string>(ParamName, "message", _baseTypes);
        }

        /// <summary>
        /// Measures the type exclusion guard.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotContainsType - type")]
        [BenchmarkCategory("Type guards")]
        public void ThrowIfNotContainsType_Type()
        {
            Validator.ThrowIfNotContainsType(typeof(ArgumentNullException), _baseTypes);
        }

        /// <summary>
        /// Measures the object type exclusion guard.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotContainsType - object")]
        [BenchmarkCategory("Type guards")]
        public void ThrowIfNotContainsType_Object()
        {
            Validator.ThrowIfNotContainsType(new ArgumentNullException(), _baseTypes);
        }

        /// <summary>
        /// Measures the generic type exclusion guard.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotContainsType - generic")]
        [BenchmarkCategory("Type guards")]
        public void ThrowIfNotContainsType_Generic()
        {
            Validator.ThrowIfNotContainsType<ArgumentNullException>(ParamName, _baseTypes);
        }

        /// <summary>
        /// Measures the generic type exclusion guard with a custom message.
        /// </summary>
        [Benchmark(Description = "ThrowIfNotContainsType - generic message")]
        [BenchmarkCategory("Type guards")]
        public void ThrowIfNotContainsType_GenericMessage()
        {
            Validator.ThrowIfNotContainsType<ArgumentNullException>(ParamName, "message", _baseTypes);
        }
    }
}
