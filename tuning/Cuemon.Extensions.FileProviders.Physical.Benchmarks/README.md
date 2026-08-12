# PortablePhysicalFileProvider benchmarks

## Methodology

- **Warm-cache benchmarks** prime the `PortablePhysicalFileProvider` cache in `GlobalSetup`, then reuse the same provider instances, files, directories, and precomputed request strings in the measured methods. They measure steady-state successful cache hits, not setup work or string construction.
- **Cold-resolution benchmarks** create the filesystem once per scenario, then recreate both providers in `IterationSetup` and run with `InvocationCount = 1` and `UnrollFactor = 1`. "Cold" therefore means **provider-cache cold**, not operating-system page-cache cold or storage-device cold.
- **Repeated miss benchmarks** intentionally reuse the same providers because misses are not cached. Every measured call re-evaluates the unresolved path against the same directory topology.
- **Concurrent miss benchmarks** use four long-lived worker threads plus a barrier-coordinated start. Those numbers include provider work, filesystem enumeration, and synchronization needed to release a batch, but exclude per-invocation task or thread creation overhead.

## Parameters

- Successful lookup benchmarks cover **shallow** and **deep** paths plus **narrow** and **wide** directories.
- Missing-path benchmarks use directories with approximately **5**, **50**, and **500** siblings.
- Varied-casing request paths are created once during setup and reused during measurement.
- Collision benchmarks only materialize when the temporary filesystem supports distinct entries whose names differ only by casing.

## Baselines and interpretation

- `PhysicalFileProvider` baselines always use the **exact-casing** path against the same files and directories so ratios reflect the overhead required to add portable case-insensitive resolution.
- Directory lookup benchmarks force enumeration of the returned directory contents. File lookup benchmarks observe `IFileInfo.Exists`.
- No `PhysicalFileProvider` baseline is included for collision scenarios because an exact-casing native lookup does not perform equivalent ambiguity detection.
- Unresolved paths are intentionally re-evaluated on every call. Miss and collision benchmarks are therefore expected to remain much more expensive than warm-cache successful lookups, especially in wide directories.
