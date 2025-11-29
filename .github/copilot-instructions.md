---
description: 'Writing Unit Tests in Cuemon'
applyTo: "**/*.{cs,csproj}"
---

# Writing Unit Tests in Cuemon
This document provides instructions for writing unit tests in the Cuemon codebase. Please follow these guidelines to ensure consistency and maintainability.

## 1. Base Class

**Always inherit from the `Test` base class** for all unit test classes.  
This ensures consistent setup, teardown, and output handling across all tests.

> Important: Do NOT add `using Xunit.Abstractions`. xUnit v3 no longer exposes that namespace; including it is incorrect and will cause compilation errors. Use the `Codebelt.Extensions.Xunit` Test base class and `using Xunit;` as shown in the examples below. If you need access to test output, rely on the Test base class (which accepts the appropriate output helper) rather than importing `Xunit.Abstractions`.

```csharp
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Your.Namespace
{
    public class YourTestClass : Test
    {
        public YourTestClass(ITestOutputHelper output) : base(output)
        {
        }

        // Your tests here
    }
}
```

## 2. Test Method Attributes

- Use `[Fact]` for standard unit tests.
- Use `[Theory]` with `[InlineData]` or other data sources for parameterized tests.

## 3. Naming Conventions

- **Test classes**: End with `Test` (e.g., `DateSpanTest`).
- **Test methods**: Use descriptive names that state the expected behavior (e.g., `ShouldReturnTrue_WhenConditionIsMet`).

## 4. Assertions

- Use `Assert` methods from xUnit for all assertions.
- Prefer explicit and expressive assertions (e.g., `Assert.Equal`, `Assert.NotNull`, `Assert.Contains`).

## 5. File and Namespace Organization

- Place test files in the appropriate test project and folder structure.
- Use namespaces that mirror the source code structure. The namespace of a test file MUST match the namespace of the System Under Test (SUT). Do NOT append ".Tests", ".Benchmarks" or similar suffixes to the namespace. Only the assembly/project name should indicate that the file is a test/benchmark (for example: Cuemon.Foo.Tests assembly, but namespace Cuemon.Foo).
  - Example: If the SUT class is declared as:
    ```csharp
    namespace Cuemon.Foo.Bar
    {
        public class Zoo { /* ... */ }
    }
    ```
    then the corresponding unit test class must use the exact same namespace:
    ```csharp
    namespace Cuemon.Foo.Bar
    {
        public class ZooTest : Test { /* ... */ }
    }
    ```
  - Do NOT use:
    ```csharp
    namespace Cuemon.Foo.Bar.Tests { /* ... */ } // ❌
    namespace Cuemon.Foo.Bar.Benchmarks { /* ... */ } // ❌
    ```
- The unit tests for the Cuemon.Foo assembly live in the Cuemon.Foo.Tests assembly.
- The functional tests for the Cuemon.Foo assembly live in the Cuemon.Foo.FunctionalTests assembly.
- Test class names end with Test and live in the same namespace as the class being tested, e.g., the unit tests for the Boo class that resides in the Cuemon.Foo assembly would be named BooTest and placed in the Cuemon.Foo namespace in the Cuemon.Foo.Tests assembly.
- Modify the associated .csproj file to override the root namespace so the compiled namespace matches the SUT. Example:
  ```xml
  <PropertyGroup>
    <RootNamespace>Cuemon.Foo</RootNamespace>
  </PropertyGroup>
  ```
- When generating test scaffolding automatically, resolve the SUT's namespace from the source file (or project/assembly metadata) and use that exact namespace in the test file header.

- Notes:
  - This rule ensures type discovery and XML doc links behave consistently and reduces confusion when reading tests.
  - Keep folder structure aligned with the production code layout to make locating SUT <-> test pairs straightforward.

## 6. Example Test

```csharp
using System;
using System.Globalization;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon
{
    /// <summary>
    /// Tests for the <see cref="DefaultCommand"/> class.
    /// </summary>
    public class DateSpanTest : Test
    {
        public DateSpanTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Parse_ShouldGetOneMonthOfDifference_UsingIso8601String()
        {
            var start = new DateTime(2021, 3, 5).ToString("O");
            var end = new DateTime(2021, 4, 5).ToString("O");

            var span = DateSpan.Parse(start, end);

            Assert.Equal("0:01:31:00:00:00.0", span.ToString());
            Assert.Equal(0, span.Years);
            Assert.Equal(1, span.Months);
            Assert.Equal(31, span.Days);
            Assert.Equal(0, span.Hours);
            Assert.Equal(0, span.Minutes);
            Assert.Equal(0, span.Seconds);
            Assert.Equal(0, span.Milliseconds);

            Assert.Equal(0.08493150684931507, span.TotalYears);
            Assert.Equal(1, span.TotalMonths);
            Assert.Equal(31, span.TotalDays);
            Assert.Equal(744, span.TotalHours);
            Assert.Equal(44640, span.TotalMinutes);
            Assert.Equal(2678400, span.TotalSeconds);
            Assert.Equal(2678400000, span.TotalMilliseconds);

            Assert.Equal(6, span.GetWeeks());
            Assert.Equal(-1566296493, span.GetHashCode());

            TestOutput.WriteLine(span.ToString());
        }
    }
}
```

## 7. Additional Guidelines

- Keep tests focused and isolated.
- Do not rely on external systems except for xUnit itself and Codebelt.Extensions.Xunit (and derived from this).
- Ensure tests are deterministic and repeatable.

## 8. Test Doubles

- Preferred test doubles include dummies, fakes, stubs and spies if and when the design allows it.
- Under special circumstances, mock can be used (using Moq library).
- Before overriding methods, verify that the method is virtual or abstract; this rule also applies to mocks.
- Never mock IMarshaller; always use a new instance of JsonMarshaller.

For further examples, refer to existing test files such as  
[`test/Cuemon.Core.Tests/DisposableTest.cs`](test/Cuemon.Core.Tests/DisposableTest.cs) and  [`test/Cuemon.Core.Tests/Security/HashFactoryTest.cs`](test/Cuemon.Core.Tests/Security/HashFactoryTest.cs).

---
description: 'Writing Performance Tests in Cuemon'
applyTo: "tuning/**, **/*Benchmark*.cs"
---

# Writing Performance Tests in Cuemon
This document provides guidance for writing performance tests (benchmarks) in the Cuemon codebase using BenchmarkDotNet. Follow these guidelines to keep benchmarks consistent, readable, and comparable.

## 1. Naming and Placement

- Place micro- and component-benchmarks under the `tuning/` folder or in projects named `*.Benchmarks`.
- Place benchmark files in the appropriate benchmark project and folder structure.
- Use namespaces that mirror the source code structure, e.g. do not suffix with `Benchmarks`.
- Namespace rule: DO NOT append `.Benchmarks` to the namespace. Benchmarks must live in the same namespace as the production assembly. Example: if the production assembly uses `namespace Cuemon.Security.Cryptography`, the benchmark file should also use:
  ```
  namespace Cuemon.Security.Cryptography
  {
      public class Sha512256Benchmark { /* ... */ }
  }
  ```
The class name may end with `Benchmark`, but the namespace must match the assembly (no `.Benchmarks` suffix).
- The benchmarks for the Cuemon.Bar assembly live in the Cuemon.Bar.Benchmarks assembly.
- Test class names end with Benchmark and live in the same namespace as the class being measured, e.g., the benchmarks for the Zoo class that resides in the Cuemon.Bar assembly would be named BooBenchmark and placed in the Cuemon.Bar namespace in the Cuemon.Bar.Benchmarks assembly.
- Modify the associated .csproj file to override the root namespace, e.g., <RootNamespace>Cuemon.Bar</RootNamespace>.

## 2. Attributes and Configuration

- Use `BenchmarkDotNet` attributes to express intent and collect relevant metrics:
    - `[MemoryDiagnoser]` to capture memory allocations.
    - `[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]` to group related benchmarks.
    - `[Params]` for input sizes or variations to exercise multiple scenarios.
    - `[GlobalSetup]` for one-time initialization that's not part of measured work.
    - `[Benchmark]` on methods representing measured operations; consider `Baseline = true` and `Description` to improve report clarity.
- Keep benchmark configuration minimal and explicit; prefer in-class attributes over large shared configs unless re-used widely.

## 3. Structure and Best Practices

- Keep benchmarks focused: each `Benchmark` method should measure a single logical operation.
- Avoid doing expensive setup work inside a measured method; use `[GlobalSetup]`, `[IterationSetup]`, or cached fields instead.
- Use `Params` to cover micro, mid and macro input sizes (for example: small, medium, large) and verify performance trends across them.
- Use small, deterministic data sets and avoid external systems (network, disk, DB). If external systems are necessary, mark them clearly and do not include them in CI benchmark runs by default.
- Capture results that are meaningful: time, allocations, and if needed custom counters. Prefer `MemoryDiagnoser` and descriptive `Description` values.

## 4. Naming Conventions for Methods

- Method names should be descriptive and indicate the scenario, e.g., `Parse_Short`, `ComputeHash_Large`.
- When comparing implementations, mark one method with `Baseline = true` and use similar names so reports are easy to read.

## 5. Example Benchmark

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class SampleOperationBenchmark
    {
        [Params(8, 256, 4096)]
        public int Count { get; set; }

        private byte[] _payload;

        [GlobalSetup]
        public void Setup()
        {
            _payload = new byte[Count];
            // deterministic initialization
        }

        [Benchmark(Baseline = true, Description = "Operation - baseline")]
        public int Operation_Baseline() => SampleOperation.Process(_payload);

        [Benchmark(Description = "Operation - optimized")]
        public int Operation_Optimized() => SampleOperation.ProcessOptimized(_payload);
    }
}
```

## 6. Reporting and CI

- Benchmarks are primarily for local and tuning runs; be cautious about running heavy BenchmarkDotNet workloads in CI. Prefer targeted runs or harnesses for CI where appropriate.
- Keep benchmark projects isolated (e.g., `tuning/*.csproj`) so they don't affect package builds or production artifacts.

## 7. Additional Guidelines

- Keep benchmarks readable and well-documented; add comments explaining non-obvious choices.
- If a benchmark exposes regressions or optimizations, add a short note in the benchmark file referencing the relevant issue or PR.
- For any shared helpers for benchmarking, prefer small utility classes inside the `tuning` projects rather than cross-cutting changes to production code.

For further examples, refer to the benchmark files under the `tuning/` folder such as `tuning/Cuemon.Core.Benchmarks/DateSpanBenchmark.cs` and `tuning/Cuemon.Security.Cryptography.Benchmarks/Sha512256Benchmark.cs`.

---
description: 'Writing XML documentation in Cuemon'
applyTo: "**/*.cs"
---

# Writing XML documentation in Cuemon

This document provides instructions for writing XML documentation.

## 1. Documentation Style

- Use the same documentation style as found throughout the codebase.
- Add XML doc comments to public and protected classes and methods where appropriate.
- Example:

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Cuemon.Collections.Generic;
using Cuemon.Configuration;
using Cuemon.IO;
using Cuemon.Text;

namespace Cuemon.Security
{
    /// <summary>
    /// Represents the base class from which all implementations of hash algorithms and checksums should derive.
    /// </summary>
    /// <typeparam name="TOptions">The type of the configured options.</typeparam>
    /// <seealso cref="ConvertibleOptions"/>
    /// <seealso cref="IConfigurable{TOptions}" />
    /// <seealso cref="IHash" />
    public abstract class Hash<TOptions> : Hash, IConfigurable<TOptions> where TOptions : ConvertibleOptions, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hash{TOptions}"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
        protected Hash(Action<TOptions> setup)
        {
            Options = Patterns.Configure(setup);
        }

        /// <summary>
        /// Gets the configured options of this instance.
        /// </summary>
        /// <value>The configured options of this instance.</value>
        public TOptions Options { get; }


        /// <summary>
        /// The endian-initializer of this instance.
        /// </summary>
        /// <param name="options">An instance of the configured options.</param>
        protected sealed override void EndianInitializer(EndianOptions options)
        {
            options.ByteOrder = Options.ByteOrder;
        }
    }

    /// <summary>
    /// Represents the base class that defines the public facing structure to expose.
    /// </summary>
    /// <seealso cref="IHash" />
    public abstract class Hash : IHash
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hash"/> class.
        /// </summary>
        protected Hash()
        {
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="bool"/>.
        /// </summary>
        /// <param name="input">The <see cref="bool"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(bool input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="byte"/>.
        /// </summary>
        /// <param name="input">The <see cref="byte"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(byte input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="char"/>.
        /// </summary>
        /// <param name="input">The <see cref="char"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(char input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="input">The <see cref="DateTime"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(DateTime input)
        {
            return ComputeHash(Convertible.GetBytes(input));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="DBNull"/>.
        /// </summary>
        /// <param name="input">The <see cref="DBNull"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(DBNull input)
        {
            return ComputeHash(Convertible.GetBytes(input));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="decimal"/>.
        /// </summary>
        /// <param name="input">The <see cref="decimal"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(decimal input)
        {
            return ComputeHash(Convertible.GetBytes(input));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="double"/>.
        /// </summary>
        /// <param name="input">The <see cref="double"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(double input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="short"/>.
        /// </summary>
        /// <param name="input">The <see cref="short"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(short input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="int"/>.
        /// </summary>
        /// <param name="input">The <see cref="int"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(int input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="long"/>.
        /// </summary>
        /// <param name="input">The <see cref="long"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(long input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="sbyte"/>.
        /// </summary>
        /// <param name="input">The <see cref="sbyte"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(sbyte input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="float"/>.
        /// </summary>
        /// <param name="input">The <see cref="float"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(float input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="ushort"/>.
        /// </summary>
        /// <param name="input">The <see cref="ushort"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(ushort input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="uint"/>.
        /// </summary>
        /// <param name="input">The <see cref="uint"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(uint input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="ulong"/>.
        /// </summary>
        /// <param name="input">The <see cref="ulong"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(ulong input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="string"/>.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to compute the hash code for.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(string input, Action<EncodingOptions> setup = null)
        {
            return ComputeHash(Convertible.GetBytes(input, setup));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="Enum"/>.
        /// </summary>
        /// <param name="input">The <see cref="Enum"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(Enum input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="T:IConvertible[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="T:IConvertible[]"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(params IConvertible[] input)
        {
            return ComputeHash(Arguments.ToEnumerableOf(input));
        }

        /// <summary>
        /// Computes the hash value for the specified sequence of <see cref="IConvertible"/>.
        /// </summary>
        /// <param name="input">The sequence of <see cref="IConvertible"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(IEnumerable<IConvertible> input)
        {
            return ComputeHash(Convertible.GetBytes(input));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public abstract HashResult ComputeHash(byte[] input);

        /// <summary>
        /// Computes the hash value for the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(Stream input)
        {
            return ComputeHash(Patterns.SafeInvoke(() => new MemoryStream(), destination =>
            {
                Decorator.Enclose(input).CopyStream(destination);
                return destination;
            }).ToArray());
        }

        /// <summary>
        /// Defines the initializer that <see cref="Hash{TOptions}"/> must implement.
        /// </summary>
        /// <param name="options">An instance of the configured options.</param>
        protected abstract void EndianInitializer(EndianOptions options);
    }
}

namespace Cuemon.Security
{
    /// <summary>
    /// Configuration options for <see cref="FowlerNollVoHash"/>.
    /// </summary>
    public class FowlerNollVoOptions : ConvertibleOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FowlerNollVoOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="FowlerNollVoOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="EndianOptions.ByteOrder"/></term>
        ///         <description><see cref="Endianness.BigEndian"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Algorithm"/></term>
        ///         <description><see cref="FowlerNollVoAlgorithm.Fnv1a"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FowlerNollVoOptions()
        {
            Algorithm = FowlerNollVoAlgorithm.Fnv1a;
            ByteOrder = Endianness.BigEndian;
        }

        /// <summary>
        /// Gets or sets the algorithm of the Fowler-Noll-Vo hash function.
        /// </summary>
        /// <value>The algorithm of the Fowler-Noll-Vo hash function.</value>
        public FowlerNollVoAlgorithm Algorithm { get; set; }
    }
}
```
