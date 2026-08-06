using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Cuemon.Extensions.FileProviders
{
    /// <summary>
    /// Measures steady-state successful file lookups after the portable provider cache has been primed.
    /// </summary>
    /// <remarks>
    /// Cache priming occurs in <see cref="GlobalSetup"/>. The measured methods reuse the same providers, files, and
    /// precomputed request strings so the results isolate warm-cache lookup overhead rather than setup work.
    /// </remarks>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class PortablePhysicalFileProviderWarmFileLookupBenchmark
    {
        private BenchmarkFileSystemScope _scope;
        private PhysicalFileProvider _physicalProvider;
        private PortablePhysicalFileProvider _portableProvider;
        private string _exactPath;
        private string _variedCasePath;

        [Params(LookupDepth.Shallow, LookupDepth.Deep)]
        public LookupDepth Depth { get; set; }

        [Params(5, 500)]
        public int SiblingCount { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var scenario = PortablePhysicalFileProviderBenchmarkScenarios.CreateFileLookupScenario(Depth, SiblingCount);
            _scope = new BenchmarkFileSystemScope($"warm-file-{Depth}-{SiblingCount}");
            _scope.CreateFileLookupScenario(scenario);
            _physicalProvider = new PhysicalFileProvider(_scope.RootPath);
            _portableProvider = new PortablePhysicalFileProvider(_scope.RootPath);
            _exactPath = scenario.ExactPath;
            _variedCasePath = scenario.VariedCasePath;

            if (!_portableProvider.GetFileInfo(_exactPath).Exists)
            {
                throw new InvalidOperationException($"Unable to prime the warm-cache file benchmark for '{scenario.Name}'.");
            }
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _portableProvider?.Dispose();
            _physicalProvider?.Dispose();
            _scope?.Dispose();
        }

        [Benchmark(Baseline = true, Description = "PhysicalFileProvider - exact casing")]
        [BenchmarkCategory("Warm file lookup")]
        public bool PhysicalFileProvider_ExactCasing()
        {
            return _physicalProvider.GetFileInfo(_exactPath).Exists;
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - exact casing (warm cache)")]
        [BenchmarkCategory("Warm file lookup")]
        public bool PortablePhysicalFileProvider_ExactCasing()
        {
            return _portableProvider.GetFileInfo(_exactPath).Exists;
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - varied casing (warm cache)")]
        [BenchmarkCategory("Warm file lookup")]
        public bool PortablePhysicalFileProvider_VariedCasing()
        {
            return _portableProvider.GetFileInfo(_variedCasePath).Exists;
        }
    }

    /// <summary>
    /// Measures steady-state successful directory lookups after the portable provider cache has been primed.
    /// </summary>
    /// <remarks>
    /// The portable provider path cache is primed in <see cref="GlobalSetup"/>, but each measured call still enumerates
    /// the resolved directory contents so the comparison stays aligned with the <see cref="PhysicalFileProvider"/> baseline.
    /// </remarks>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class PortablePhysicalFileProviderWarmDirectoryLookupBenchmark
    {
        private BenchmarkFileSystemScope _scope;
        private PhysicalFileProvider _physicalProvider;
        private PortablePhysicalFileProvider _portableProvider;
        private string _exactPath;
        private string _variedCasePath;
        private int _childEntryCount;

        [Params(LookupDepth.Shallow, LookupDepth.Deep)]
        public LookupDepth Depth { get; set; }

        [Params(5, 500)]
        public int SiblingCount { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var scenario = PortablePhysicalFileProviderBenchmarkScenarios.CreateDirectoryLookupScenario(Depth, SiblingCount);
            _scope = new BenchmarkFileSystemScope($"warm-directory-{Depth}-{SiblingCount}");
            _scope.CreateDirectoryLookupScenario(scenario);
            _physicalProvider = new PhysicalFileProvider(_scope.RootPath);
            _portableProvider = new PortablePhysicalFileProvider(_scope.RootPath);
            _exactPath = scenario.ExactPath;
            _variedCasePath = scenario.VariedCasePath;
            _childEntryCount = scenario.ChildEntryCount;

            if (BenchmarkFileSystemScope.CountEntries(_portableProvider.GetDirectoryContents(_exactPath)) != _childEntryCount)
            {
                throw new InvalidOperationException($"Unable to prime the warm-cache directory benchmark for '{scenario.Name}'.");
            }
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _portableProvider?.Dispose();
            _physicalProvider?.Dispose();
            _scope?.Dispose();
        }

        [Benchmark(Baseline = true, Description = "PhysicalFileProvider - exact casing")]
        [BenchmarkCategory("Warm directory lookup")]
        public int PhysicalFileProvider_ExactCasing()
        {
            return BenchmarkFileSystemScope.CountEntries(_physicalProvider.GetDirectoryContents(_exactPath));
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - exact casing (warm cache)")]
        [BenchmarkCategory("Warm directory lookup")]
        public int PortablePhysicalFileProvider_ExactCasing()
        {
            return BenchmarkFileSystemScope.CountEntries(_portableProvider.GetDirectoryContents(_exactPath));
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - varied casing (warm cache)")]
        [BenchmarkCategory("Warm directory lookup")]
        public int PortablePhysicalFileProvider_VariedCasing()
        {
            return BenchmarkFileSystemScope.CountEntries(_portableProvider.GetDirectoryContents(_variedCasePath));
        }
    }

    /// <summary>
    /// Measures successful file lookups with a fresh provider so each invocation starts without a portable path-cache entry.
    /// </summary>
    /// <remarks>
    /// The file-system layout is created once per scenario, but each iteration creates fresh providers and uses
    /// <see cref="InvocationCountAttribute"/> set to <c>1</c> so the measured call remains provider-cold.
    /// This is a cold-resolution benchmark, not an operating-system page-cache flush.
    /// </remarks>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [InvocationCount(1)]
    public class PortablePhysicalFileProviderColdFileLookupBenchmark
    {
        private BenchmarkFileSystemScope _scope;
        private PhysicalFileProvider _physicalProvider;
        private PortablePhysicalFileProvider _portableProvider;
        private string _exactPath;
        private string _variedCasePath;

        [Params(LookupDepth.Shallow, LookupDepth.Deep)]
        public LookupDepth Depth { get; set; }

        [Params(5, 500)]
        public int SiblingCount { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var scenario = PortablePhysicalFileProviderBenchmarkScenarios.CreateFileLookupScenario(Depth, SiblingCount);
            _scope = new BenchmarkFileSystemScope($"cold-file-{Depth}-{SiblingCount}");
            _scope.CreateFileLookupScenario(scenario);
            _exactPath = scenario.ExactPath;
            _variedCasePath = scenario.VariedCasePath;
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _physicalProvider = new PhysicalFileProvider(_scope.RootPath);
            _portableProvider = new PortablePhysicalFileProvider(_scope.RootPath);
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            _portableProvider?.Dispose();
            _physicalProvider?.Dispose();
            _portableProvider = null;
            _physicalProvider = null;
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _scope?.Dispose();
        }

        [Benchmark(Baseline = true, Description = "PhysicalFileProvider - exact casing")]
        [BenchmarkCategory("Cold file lookup")]
        public bool PhysicalFileProvider_ExactCasing()
        {
            return _physicalProvider.GetFileInfo(_exactPath).Exists;
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - exact casing (cold resolution)")]
        [BenchmarkCategory("Cold file lookup")]
        public bool PortablePhysicalFileProvider_ExactCasing()
        {
            return _portableProvider.GetFileInfo(_exactPath).Exists;
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - varied casing (cold resolution)")]
        [BenchmarkCategory("Cold file lookup")]
        public bool PortablePhysicalFileProvider_VariedCasing()
        {
            return _portableProvider.GetFileInfo(_variedCasePath).Exists;
        }
    }

    /// <summary>
    /// Measures successful directory lookups with a fresh provider so each invocation starts without a portable path-cache entry.
    /// </summary>
    /// <remarks>
    /// Each iteration recreates both providers while preserving the same directories and files, which keeps the filesystem work
    /// comparable while guaranteeing that the portable provider starts cold for the requested logical path.
    /// </remarks>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [InvocationCount(1)]
    public class PortablePhysicalFileProviderColdDirectoryLookupBenchmark
    {
        private BenchmarkFileSystemScope _scope;
        private PhysicalFileProvider _physicalProvider;
        private PortablePhysicalFileProvider _portableProvider;
        private string _exactPath;
        private string _variedCasePath;

        [Params(LookupDepth.Shallow, LookupDepth.Deep)]
        public LookupDepth Depth { get; set; }

        [Params(5, 500)]
        public int SiblingCount { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var scenario = PortablePhysicalFileProviderBenchmarkScenarios.CreateDirectoryLookupScenario(Depth, SiblingCount);
            _scope = new BenchmarkFileSystemScope($"cold-directory-{Depth}-{SiblingCount}");
            _scope.CreateDirectoryLookupScenario(scenario);
            _exactPath = scenario.ExactPath;
            _variedCasePath = scenario.VariedCasePath;
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _physicalProvider = new PhysicalFileProvider(_scope.RootPath);
            _portableProvider = new PortablePhysicalFileProvider(_scope.RootPath);
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            _portableProvider?.Dispose();
            _physicalProvider?.Dispose();
            _portableProvider = null;
            _physicalProvider = null;
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _scope?.Dispose();
        }

        [Benchmark(Baseline = true, Description = "PhysicalFileProvider - exact casing")]
        [BenchmarkCategory("Cold directory lookup")]
        public int PhysicalFileProvider_ExactCasing()
        {
            return BenchmarkFileSystemScope.CountEntries(_physicalProvider.GetDirectoryContents(_exactPath));
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - exact casing (cold resolution)")]
        [BenchmarkCategory("Cold directory lookup")]
        public int PortablePhysicalFileProvider_ExactCasing()
        {
            return BenchmarkFileSystemScope.CountEntries(_portableProvider.GetDirectoryContents(_exactPath));
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - varied casing (cold resolution)")]
        [BenchmarkCategory("Cold directory lookup")]
        public int PortablePhysicalFileProvider_VariedCasing()
        {
            return BenchmarkFileSystemScope.CountEntries(_portableProvider.GetDirectoryContents(_variedCasePath));
        }
    }

    /// <summary>
    /// Measures repeated lookup of the same missing file path across narrow and wide directories.
    /// </summary>
    /// <remarks>
    /// The same providers are reused for the full benchmark because misses are intentionally not cached. Each call therefore
    /// re-evaluates the unresolved path against the same file-system layout.
    /// </remarks>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class PortablePhysicalFileProviderRepeatedMissingFileBenchmark
    {
        private const string ExactMissingPath = "Assets/missing.svg";
        private const string VariedCaseMissingPath = "assets/missing.svg";

        private BenchmarkFileSystemScope _scope;
        private PhysicalFileProvider _physicalProvider;
        private PortablePhysicalFileProvider _portableProvider;

        [Params(5, 50, 500)]
        public int SiblingCount { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _scope = new BenchmarkFileSystemScope($"repeated-miss-{SiblingCount}");
            _scope.CreateMissingFileScenario("Assets", SiblingCount);
            _physicalProvider = new PhysicalFileProvider(_scope.RootPath);
            _portableProvider = new PortablePhysicalFileProvider(_scope.RootPath);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _portableProvider?.Dispose();
            _physicalProvider?.Dispose();
            _scope?.Dispose();
        }

        [Benchmark(Baseline = true, Description = "PhysicalFileProvider - same missing path")]
        [BenchmarkCategory("Repeated missing path")]
        public bool PhysicalFileProvider_ExactCasing()
        {
            return _physicalProvider.GetFileInfo(ExactMissingPath).Exists;
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - same missing path")]
        [BenchmarkCategory("Repeated missing path")]
        public bool PortablePhysicalFileProvider_VariedCasing()
        {
            return _portableProvider.GetFileInfo(VariedCaseMissingPath).Exists;
        }
    }

    /// <summary>
    /// Measures unique missing file paths under the same directory across narrow and wide layouts.
    /// </summary>
    /// <remarks>
    /// Request strings are precomputed in <see cref="GlobalSetup"/> so the benchmark reflects repeated unresolved-path
    /// evaluation instead of string construction.
    /// </remarks>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class PortablePhysicalFileProviderUniqueMissingFileBenchmark
    {
        private BenchmarkFileSystemScope _scope;
        private PhysicalFileProvider _physicalProvider;
        private PortablePhysicalFileProvider _portableProvider;
        private string[] _exactMissingPaths;
        private string[] _variedCaseMissingPaths;
        private int _nextExactPathIndex;
        private int _nextVariedPathIndex;

        [Params(5, 50, 500)]
        public int SiblingCount { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _scope = new BenchmarkFileSystemScope($"unique-miss-{SiblingCount}");
            _scope.CreateMissingFileScenario("Assets", SiblingCount);
            _physicalProvider = new PhysicalFileProvider(_scope.RootPath);
            _portableProvider = new PortablePhysicalFileProvider(_scope.RootPath);
            _exactMissingPaths = Enumerable.Range(0, 1_024).Select(i => $"Assets/missing-{i:D4}.svg").ToArray();
            _variedCaseMissingPaths = _exactMissingPaths.Select(path => path.ToLowerInvariant()).ToArray();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _portableProvider?.Dispose();
            _physicalProvider?.Dispose();
            _scope?.Dispose();
        }

        [Benchmark(Baseline = true, Description = "PhysicalFileProvider - unique missing paths")]
        [BenchmarkCategory("Unique missing paths")]
        public bool PhysicalFileProvider_ExactCasing()
        {
            return _physicalProvider.GetFileInfo(NextPath(_exactMissingPaths, ref _nextExactPathIndex)).Exists;
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - unique missing paths")]
        [BenchmarkCategory("Unique missing paths")]
        public bool PortablePhysicalFileProvider_VariedCasing()
        {
            return _portableProvider.GetFileInfo(NextPath(_variedCaseMissingPaths, ref _nextVariedPathIndex)).Exists;
        }

        private static string NextPath(IReadOnlyList<string> paths, ref int nextPathIndex)
        {
            var path = paths[nextPathIndex];
            nextPathIndex++;

            if (nextPathIndex == paths.Count)
            {
                nextPathIndex = 0;
            }

            return path;
        }
    }

    /// <summary>
    /// Measures repeated lookup of the same case-insensitive collision when the host filesystem can materialize both entries.
    /// </summary>
    /// <remarks>
    /// No <see cref="PhysicalFileProvider"/> baseline is provided because an exact-casing native lookup does not perform
    /// comparable ambiguity detection. When the temporary filesystem cannot host distinct case-only entries, the scenario
    /// source is empty and this benchmark is skipped.
    /// </remarks>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class PortablePhysicalFileProviderCollisionBenchmark
    {
        private const string CollisionRequestPath = "LOGO.SVG";
        private const string LowerCollisionFileName = "logo.svg";
        private const string UpperCollisionFileName = "Logo.svg";

        private BenchmarkFileSystemScope _scope;
        private PortablePhysicalFileProvider _portableProvider;

        [ParamsSource(nameof(SiblingCounts))]
        public int SiblingCount { get; set; }

        public static IEnumerable<int> SiblingCounts()
        {
            // Always return at least one value for BenchmarkDotNet discovery, even on case-insensitive filesystems.
            // The benchmark is skipped in GlobalSetup if the filesystem doesn't support case-distinct entries.
            return new[] { 50 };
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            // Skip this benchmark on filesystems that don't support case-distinct entries (e.g., Windows).
            if (!CaseDistinctEntryCapabilityDetector.IsSupported)
            {
                throw new NotSupportedException("This benchmark requires a case-sensitive filesystem.");
            }

            _scope = new BenchmarkFileSystemScope($"collision-{SiblingCount}");
            _scope.CreateCollisionFileScenario(LowerCollisionFileName, UpperCollisionFileName, SiblingCount);
            _portableProvider = new PortablePhysicalFileProvider(_scope.RootPath);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _portableProvider?.Dispose();
            _scope?.Dispose();
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - same casing collision")]
        [BenchmarkCategory("Repeated casing collision")]
        public bool PortablePhysicalFileProvider_SameCollisionPath()
        {
            return _portableProvider.GetFileInfo(CollisionRequestPath).Exists;
        }
    }

    /// <summary>
    /// Measures concurrent missing-file lookups under a wide directory using long-lived workers.
    /// </summary>
    /// <remarks>
    /// Each benchmark invocation coordinates four pre-created worker threads. The results therefore include provider work,
    /// filesystem enumeration, and the harness barrier synchronization required to release the batch, but exclude
    /// per-invocation task or thread creation overhead.
    /// </remarks>
    [MemoryDiagnoser]
    [ThreadingDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class PortablePhysicalFileProviderConcurrentMissingFileBenchmark
    {
        private const int WorkerCount = 4;

        private BenchmarkFileSystemScope _scope;
        private PhysicalFileProvider _physicalProvider;
        private PortablePhysicalFileProvider _portableProvider;
        private ConcurrentMissingPathHarness _physicalSamePathHarness;
        private ConcurrentMissingPathHarness _portableSamePathHarness;
        private ConcurrentMissingPathHarness _physicalDifferentPathsHarness;
        private ConcurrentMissingPathHarness _portableDifferentPathsHarness;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _scope = new BenchmarkFileSystemScope("concurrent-miss-500");
            _scope.CreateMissingFileScenario("Assets", 500);
            _physicalProvider = new PhysicalFileProvider(_scope.RootPath);
            _portableProvider = new PortablePhysicalFileProvider(_scope.RootPath);

            var exactSamePath = Enumerable.Repeat("Assets/missing.svg", WorkerCount).ToArray();
            var variedSamePath = Enumerable.Repeat("assets/missing.svg", WorkerCount).ToArray();
            var exactDifferentPaths = Enumerable.Range(0, WorkerCount).Select(i => $"Assets/missing-{i:D4}.svg").ToArray();
            var variedDifferentPaths = exactDifferentPaths.Select(path => path.ToLowerInvariant()).ToArray();

            _physicalSamePathHarness = new ConcurrentMissingPathHarness(_physicalProvider, exactSamePath);
            _portableSamePathHarness = new ConcurrentMissingPathHarness(_portableProvider, variedSamePath);
            _physicalDifferentPathsHarness = new ConcurrentMissingPathHarness(_physicalProvider, exactDifferentPaths);
            _portableDifferentPathsHarness = new ConcurrentMissingPathHarness(_portableProvider, variedDifferentPaths);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _portableDifferentPathsHarness?.Dispose();
            _physicalDifferentPathsHarness?.Dispose();
            _portableSamePathHarness?.Dispose();
            _physicalSamePathHarness?.Dispose();
            _portableProvider?.Dispose();
            _physicalProvider?.Dispose();
            _scope?.Dispose();
        }

        [Benchmark(Baseline = true, Description = "PhysicalFileProvider - same missing path", OperationsPerInvoke = WorkerCount)]
        [BenchmarkCategory("Concurrent same missing path")]
        public int PhysicalFileProvider_SameMissingPath()
        {
            return _physicalSamePathHarness.RunBatch();
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - same missing path", OperationsPerInvoke = WorkerCount)]
        [BenchmarkCategory("Concurrent same missing path")]
        public int PortablePhysicalFileProvider_SameMissingPath()
        {
            return _portableSamePathHarness.RunBatch();
        }

        [Benchmark(Baseline = true, Description = "PhysicalFileProvider - different missing paths", OperationsPerInvoke = WorkerCount)]
        [BenchmarkCategory("Concurrent different missing paths")]
        public int PhysicalFileProvider_DifferentMissingPaths()
        {
            return _physicalDifferentPathsHarness.RunBatch();
        }

        [Benchmark(Description = "PortablePhysicalFileProvider - different missing paths", OperationsPerInvoke = WorkerCount)]
        [BenchmarkCategory("Concurrent different missing paths")]
        public int PortablePhysicalFileProvider_DifferentMissingPaths()
        {
            return _portableDifferentPathsHarness.RunBatch();
        }
    }

    /// <summary>
    /// Defines the relative depth used by the portable file-provider benchmarks.
    /// </summary>
    public enum LookupDepth
    {
        Shallow,
        Deep
    }

    internal sealed class FileLookupScenario
    {
        public FileLookupScenario(string name, string[] physicalSegments, int[] siblingCounts)
        {
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("A benchmark scenario name is required.", nameof(name)) : name;
            PhysicalSegments = physicalSegments ?? throw new ArgumentNullException(nameof(physicalSegments));
            SiblingCounts = siblingCounts ?? throw new ArgumentNullException(nameof(siblingCounts));

            if (PhysicalSegments.Length == 0)
            {
                throw new ArgumentException("At least one physical segment is required.", nameof(physicalSegments));
            }

            if (PhysicalSegments.Length != SiblingCounts.Length)
            {
                throw new ArgumentException("The sibling-count array must contain one entry per physical segment.", nameof(siblingCounts));
            }

            ValidateSiblingCounts(SiblingCounts);

            ExactPath = string.Join("/", PhysicalSegments);
            VariedCasePath = ExactPath.ToLowerInvariant();
        }

        public string Name { get; }

        public string[] PhysicalSegments { get; }

        public int[] SiblingCounts { get; }

        public string ExactPath { get; }

        public string VariedCasePath { get; }

        public override string ToString() => Name;

        internal static void ValidateSiblingCounts(IReadOnlyList<int> siblingCounts)
        {
            for (var i = 0; i < siblingCounts.Count; i++)
            {
                if (siblingCounts[i] < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(siblingCounts), siblingCounts[i], "Sibling counts must be greater than zero.");
                }
            }
        }
    }

    internal sealed class DirectoryLookupScenario
    {
        public DirectoryLookupScenario(string name, string[] physicalSegments, int[] siblingCounts, int childEntryCount)
        {
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("A benchmark scenario name is required.", nameof(name)) : name;
            PhysicalSegments = physicalSegments ?? throw new ArgumentNullException(nameof(physicalSegments));
            SiblingCounts = siblingCounts ?? throw new ArgumentNullException(nameof(siblingCounts));

            if (PhysicalSegments.Length == 0)
            {
                throw new ArgumentException("At least one physical segment is required.", nameof(physicalSegments));
            }

            if (PhysicalSegments.Length != SiblingCounts.Length)
            {
                throw new ArgumentException("The sibling-count array must contain one entry per physical segment.", nameof(siblingCounts));
            }

            if (childEntryCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(childEntryCount), childEntryCount, "A directory benchmark requires at least one child entry.");
            }

            FileLookupScenario.ValidateSiblingCounts(SiblingCounts);
            ChildEntryCount = childEntryCount;
            ExactPath = string.Join("/", PhysicalSegments);
            VariedCasePath = ExactPath.ToLowerInvariant();
        }

        public string Name { get; }

        public string[] PhysicalSegments { get; }

        public int[] SiblingCounts { get; }

        public int ChildEntryCount { get; }

        public string ExactPath { get; }

        public string VariedCasePath { get; }

        public override string ToString() => Name;
    }

    internal static class PortablePhysicalFileProviderBenchmarkScenarios
    {
        public static FileLookupScenario CreateFileLookupScenario(LookupDepth depth, int siblingCount)
        {
            return depth == LookupDepth.Shallow
                ? new FileLookupScenario("shallow-file", new[] { "Assets", "Logo.svg" }, new[] { siblingCount, siblingCount })
                : new FileLookupScenario("deep-file", new[] { "Assets", "Images", "Branding", "Campaigns", "Logo.svg" }, new[] { siblingCount, siblingCount, siblingCount, siblingCount, siblingCount });
        }

        public static DirectoryLookupScenario CreateDirectoryLookupScenario(LookupDepth depth, int siblingCount)
        {
            return depth == LookupDepth.Shallow
                ? new DirectoryLookupScenario("shallow-directory", new[] { "Assets" }, new[] { siblingCount }, siblingCount)
                : new DirectoryLookupScenario("deep-directory", new[] { "Assets", "Images", "Branding", "Campaigns" }, new[] { siblingCount, siblingCount, siblingCount, siblingCount }, siblingCount);
        }
    }

    internal sealed class BenchmarkFileSystemScope : IDisposable
    {
        public BenchmarkFileSystemScope(string scenarioName)
        {
            RootPath = Path.Combine(Path.GetTempPath(), "cuemon", "portable-physical-file-provider-benchmarks", scenarioName, Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(RootPath);
        }

        public string RootPath { get; }

        public void CreateFileLookupScenario(FileLookupScenario scenario)
        {
            var currentPath = RootPath;

            for (var i = 0; i < scenario.PhysicalSegments.Length; i++)
            {
                var segment = scenario.PhysicalSegments[i];
                var siblingCount = scenario.SiblingCounts[i];
                var isFile = i == scenario.PhysicalSegments.Length - 1;

                if (isFile)
                {
                    CreateSiblingFiles(currentPath, siblingCount - 1);
                    File.WriteAllText(Path.Combine(currentPath, segment), "benchmark-content");
                    return;
                }

                CreateSiblingDirectories(currentPath, siblingCount - 1);
                currentPath = Path.Combine(currentPath, segment);
                Directory.CreateDirectory(currentPath);
            }
        }

        public void CreateDirectoryLookupScenario(DirectoryLookupScenario scenario)
        {
            var currentPath = RootPath;

            for (var i = 0; i < scenario.PhysicalSegments.Length; i++)
            {
                CreateSiblingDirectories(currentPath, scenario.SiblingCounts[i] - 1);
                currentPath = Path.Combine(currentPath, scenario.PhysicalSegments[i]);
                Directory.CreateDirectory(currentPath);
            }

            CreateSiblingFiles(currentPath, scenario.ChildEntryCount);
        }

        public void CreateMissingFileScenario(string directoryName, int siblingCount)
        {
            CreateSiblingDirectories(RootPath, siblingCount - 1);

            var directoryPath = Path.Combine(RootPath, directoryName);
            Directory.CreateDirectory(directoryPath);

            CreateSiblingFiles(directoryPath, siblingCount);
        }

        public void CreateCollisionFileScenario(string lowerFileName, string upperFileName, int siblingCount)
        {
            if (siblingCount < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(siblingCount), siblingCount, "A collision scenario requires at least two entries.");
            }

            CreateSiblingFiles(RootPath, siblingCount - 2);
            File.WriteAllText(Path.Combine(RootPath, lowerFileName), "lower");
            File.WriteAllText(Path.Combine(RootPath, upperFileName), "upper");
        }

        public static int CountEntries(IDirectoryContents contents)
        {
            var count = 0;

            foreach (var entry in contents)
            {
                _ = entry;
                count++;
            }

            return count;
        }

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, true);
            }
        }

        private static void CreateSiblingDirectories(string parentPath, int count)
        {
            Directory.CreateDirectory(parentPath);

            for (var i = 0; i < count; i++)
            {
                Directory.CreateDirectory(Path.Combine(parentPath, $"sibling-dir-{i:D4}"));
            }
        }

        private static void CreateSiblingFiles(string parentPath, int count)
        {
            Directory.CreateDirectory(parentPath);

            for (var i = 0; i < count; i++)
            {
                File.WriteAllText(Path.Combine(parentPath, $"sibling-file-{i:D4}.txt"), "sibling");
            }
        }
    }

    internal sealed class ConcurrentMissingPathHarness : IDisposable
    {
        private readonly Barrier _phaseBarrier;
        private readonly IFileProvider _provider;
        private readonly string[] _paths;
        private readonly Thread[] _threads;
        private Exception _capturedException;
        private int _existingCount;
        private bool _disposing;

        public ConcurrentMissingPathHarness(IFileProvider provider, string[] paths)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _paths = paths ?? throw new ArgumentNullException(nameof(paths));

            if (_paths.Length == 0)
            {
                throw new ArgumentException("At least one path is required.", nameof(paths));
            }

            _phaseBarrier = new Barrier(_paths.Length + 1);
            _threads = new Thread[_paths.Length];

            for (var i = 0; i < _threads.Length; i++)
            {
                var workerIndex = i;
                _threads[i] = new Thread(() => Worker(workerIndex))
                {
                    IsBackground = true,
                    Name = $"portable-physical-file-provider-benchmark-worker-{workerIndex:D2}"
                };
                _threads[i].Start();
            }
        }

        public int RunBatch()
        {
            _existingCount = 0;
            _capturedException = null;

            _phaseBarrier.SignalAndWait();
            _phaseBarrier.SignalAndWait();

            if (_capturedException is not null)
            {
                throw new InvalidOperationException("A concurrent benchmark worker failed.", _capturedException);
            }

            return _existingCount;
        }

        public void Dispose()
        {
            _disposing = true;
            _phaseBarrier.SignalAndWait();

            foreach (var thread in _threads)
            {
                thread.Join();
            }

            _phaseBarrier.Dispose();
        }

        private void Worker(int index)
        {
            while (true)
            {
                _phaseBarrier.SignalAndWait();

                if (_disposing)
                {
                    return;
                }

                try
                {
                    if (_provider.GetFileInfo(_paths[index]).Exists)
                    {
                        Interlocked.Increment(ref _existingCount);
                    }
                }
                catch (Exception ex)
                {
                    Interlocked.CompareExchange(ref _capturedException, ex, null);
                }

                _phaseBarrier.SignalAndWait();
            }
        }
    }

    internal static class CaseDistinctEntryCapabilityDetector
    {
        private static readonly Lazy<bool> SupportsDistinctCaseEntries = new(DetectSupportsDistinctCaseEntries);

        public static bool IsSupported => SupportsDistinctCaseEntries.Value;

        private static bool DetectSupportsDistinctCaseEntries()
        {
            var rootPath = Path.Combine(Path.GetTempPath(), "cuemon", "portable-physical-file-provider-benchmarks-probe", Guid.NewGuid().ToString("N"));
            var lowerPath = Path.Combine(rootPath, "probe");
            var upperPath = Path.Combine(rootPath, "PROBE");

            Directory.CreateDirectory(rootPath);

            try
            {
                using (File.Open(lowerPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                }

                try
                {
                    using (File.Open(upperPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                    {
                    }
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    return false;
                }

                var lowerExists = File.Exists(lowerPath);
                var upperExists = File.Exists(upperPath);
                return lowerExists && upperExists;
            }
            finally
            {
                if (File.Exists(lowerPath))
                {
                    File.Delete(lowerPath);
                }

                if (File.Exists(upperPath))
                {
                    File.Delete(upperPath);
                }

                if (Directory.Exists(rootPath))
                {
                    Directory.Delete(rootPath, true);
                }
            }
        }
    }
}
