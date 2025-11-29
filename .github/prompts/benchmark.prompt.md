---
mode: agent
description: 'Writing Performance Benchmarks in Cuemon'
---

# Benchmark Fixture Prompt (Cuemon Tuning Benchmarks)

This prompt defines how to generate performance tests (“benchmarks”) for the Cuemon codebase using BenchmarkDotNet.  
Benchmarks in Cuemon are *not* unit tests — they are micro- or component-level performance measurements that belong under the `tuning/` directory and follow strict conventions.

Copilot must follow these guidelines when generating benchmark fixtures.

---

## 1. Naming and Placement

- All benchmark projects live under the `tuning/` folder.  
  Examples:
  - `tuning/Cuemon.Core.Benchmarks/`
  - `tuning/Cuemon.Security.Benchmarks/`

- **Namespaces must NOT end with `.Benchmarks`.**  
  They must mirror the production assembly’s namespace.

  Example:  
  If benchmarking a type inside `Cuemon.Security.Cryptography`, then:

  ```csharp
  namespace Cuemon.Security.Cryptography
  {
      public class Sha512256Benchmark { … }
  }
````

* **Benchmark class names must end with `Benchmark`.**
  Example: `DateSpanBenchmark`, `FowlerNollVoBenchmark`.

* Benchmark files should be located in the matching benchmark project
  (e.g., benchmarks for `Cuemon.Security.Cryptography` go in `Cuemon.Security.Cryptography.Benchmarks.csproj`).

* In the `.csproj` for each benchmark project, set the root namespace to the production namespace:

  ```xml
  <RootNamespace>Cuemon.Security.Cryptography</RootNamespace>
  ```

---

## 2. Attributes and Configuration

Each benchmark class should use:

```csharp
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
```

Optional but strongly recommended where meaningful:

* `[Params(...)]` — define small, medium, large input sizes.
* `[GlobalSetup]` — deterministic initialization of benchmark data.
* `[Benchmark(Description = "...")]` — always add descriptions.
* `[Benchmark(Baseline = true)]` — when comparing two implementations.

Avoid complex global configs; prefer explicit attributes inside the class.

---

## 3. Structure and Best Practices

A benchmark fixture must:

* Measure a **single logical operation** per benchmark method.
* Avoid I/O, networking, disk access, logging, or side effects.
* Avoid expensive setup inside `[Benchmark]` methods.
* Use deterministic data (e.g., seeded RNG or predefined constants).
* Use `[GlobalSetup]` to allocate buffers, random payloads, or reusable test data only once.
* Avoid shared mutable state unless reset per iteration.

Use representative input sizes such as:

```csharp
[Params(8, 256, 4096)]
public int Count { get; set; }
```

BenchmarkDotNet will run each benchmark for each parameter value.

---

## 4. Method Naming Conventions

Use descriptive names that communicate intent:

* `Parse_Short`
* `Parse_Long`
* `ComputeHash_Small`
* `ComputeHash_Large`
* `Serialize_Optimized`
* `Serialize_Baseline`

When comparing approaches, always list them clearly and tag one as the baseline.

---

## 5. Example Benchmark Fixture

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

---

## 6. Reporting and CI

* Benchmark projects live exclusively under `tuning/`. They must not affect production builds.
* Heavy BenchmarkDotNet runs should *not* run in CI unless explicitly configured.
* Reports are produced by the Cuemon benchmark runner and stored under its configured artifacts directory.

---

## 7. Additional Guidelines

* Keep benchmark fixtures focused and readable.
* Document non-obvious reasoning in short comments.
* Prefer realistic but deterministic data sets.
* When benchmarks reveal regressions or improvements, reference the associated PR or issue in a comment.
* Shared benchmark helpers belong in `tuning/` projects, not in production code.

---

## Final Notes

* Benchmarks are performance tests, not unit tests.
* Use `[Benchmark]` only for pure performance measurement.
* Avoid `MethodImplOptions.NoInlining` unless absolutely necessary.
* Use small sets of meaningful benchmark scenarios — avoid combinatorial explosion.

