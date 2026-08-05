using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Cuemon.Extensions.FileProviders
{
    /// <summary>
    /// Performance benchmark for <see cref="PortablePhysicalFileProvider"/> path resolution and caching behavior.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This benchmark measures the cost of case-insensitive path resolution under various scenarios:
    /// - Cache hits: repeated lookups of previously resolved paths
    /// - Cache misses with varying directory sizes: resolution cost scales with entry enumeration
    /// - Deep path resolution: multiple segment-by-segment enumerations
    /// </para>
    /// <para>
    /// The provider resolves paths segment-by-segment using case-insensitive matching against physical directory entries,
    /// caching successful results. Misses (and collisions) force re-enumeration on each call.
    /// </para>
    /// </remarks>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class PortablePhysicalFileProviderBenchmark
    {
        /// <summary>
        /// Scenario object combining directory structure size and path to resolve.
        /// </summary>
        public class ResolutionScenario
        {
            public ResolutionScenario(string name, int siblingCount, string relativeFilePath, string requestedPath)
            {
                Name = name;
                SiblingCount = siblingCount;
                RelativeFilePath = relativeFilePath;
                RequestedPath = requestedPath;
            }

            public string Name { get; }
            public int SiblingCount { get; }
            public string RelativeFilePath { get; }
            public string RequestedPath { get; }

            public override string ToString() => Name;
        }

        private string _tempRootPath;
        private PortablePhysicalFileProvider _provider;

        // Scenario sources
        public IEnumerable<ResolutionScenario> ShallowResolutionScenarios()
        {
            return new[]
            {
                // Small directory: 5 siblings
                new ResolutionScenario("shallow-5-siblings", 5, "Assets/logo.svg", "assets/logo.svg"),
                // Medium directory: 50 siblings
                new ResolutionScenario("shallow-50-siblings", 50, "Assets/logo.svg", "assets/logo.svg"),
                // Large directory: 500 siblings
                new ResolutionScenario("shallow-500-siblings", 500, "Assets/logo.svg", "assets/logo.svg"),
            };
        }

        public IEnumerable<ResolutionScenario> DeepResolutionScenarios()
        {
            return new[]
            {
                // 2 segments (1 intermediate directory)
                new ResolutionScenario("deep-2-segments", 10, "A/B/logo.svg", "a/b/logo.svg"),
                // 3 segments (2 intermediate directories)
                new ResolutionScenario("deep-3-segments", 10, "A/B/C/logo.svg", "a/b/c/logo.svg"),
                // 5 segments (4 intermediate directories)
                new ResolutionScenario("deep-5-segments", 10, "A/B/C/D/E/logo.svg", "a/b/c/d/e/logo.svg"),
            };
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _tempRootPath = Path.Combine(Path.GetTempPath(), "cuemon-benchmark", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempRootPath);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _provider?.Dispose();
            if (Directory.Exists(_tempRootPath))
            {
                Directory.Delete(_tempRootPath, true);
            }
        }

        [IterationSetup]
        public void IterationSetup()
        {
            // Dispose previous provider for fresh state each iteration
            _provider?.Dispose();
            _provider = new PortablePhysicalFileProvider(_tempRootPath);
        }

        /// <summary>
        /// Benchmark: Cache hit on previously resolved path.
        /// The path is resolved once, cached, then measured on repeated lookup.
        /// </summary>
        [Benchmark(Baseline = true, Description = "Cache hit (prime + lookup)")]
        [BenchmarkCategory("CacheHit")]
        public void CacheHit_PrimeAndLookup()
        {
            using var scenario = new TempFileSystemScope(_tempRootPath, 5, "Assets/logo.svg");

            // Prime the cache: first call
            var first = _provider.GetFileInfo("assets/logo.svg");
            _ = first.Exists; // Force read

            // Measure: cached lookup (case-insensitive key, no enumeration)
            var cached = _provider.GetFileInfo("assets/logo.svg");
            _ = cached.Exists;

            // Verify: confirm cache hit by checking identical physical paths
            if (!first.Exists || first.PhysicalPath != cached.PhysicalPath)
            {
                throw new InvalidOperationException("Cache hit verification failed.");
            }
        }

        /// <summary>
        /// Benchmark: Cache miss requiring directory enumeration.
        /// Each call enumerates the directory to find the matching entry.
        /// Scenario parameter controls directory size (5, 50, 500 siblings).
        /// </summary>
        [Benchmark(Description = "Cache miss (shallow, {0} siblings)")]
        [BenchmarkCategory("CacheMiss_Shallow")]
        [ArgumentsSource(nameof(ShallowResolutionScenarios))]
        public void CacheMiss_Shallow(ResolutionScenario scenario)
        {
            using var scope = new TempFileSystemScope(_tempRootPath, scenario.SiblingCount, scenario.RelativeFilePath);

            // Measure: fresh lookup each call (no cache, full enumeration)
            var result = _provider.GetFileInfo(scenario.RequestedPath);
            _ = result.Exists;

            // Verify: path resolved correctly despite directory size
            if (!result.Exists)
            {
                throw new InvalidOperationException($"Expected file to exist at {scenario.RequestedPath}");
            }
        }

        /// <summary>
        /// Benchmark: Deep path resolution requiring multiple segment-by-segment enumerations.
        /// Each segment requires enumerating its parent directory.
        /// Scenario parameter controls path depth (2, 3, 5 segments).
        /// </summary>
        [Benchmark(Description = "Cache miss (deep {0})")]
        [BenchmarkCategory("CacheMiss_Deep")]
        [ArgumentsSource(nameof(DeepResolutionScenarios))]
        public void CacheMiss_Deep(ResolutionScenario scenario)
        {
            using var scope = new TempFileSystemScope(_tempRootPath, scenario.SiblingCount, scenario.RelativeFilePath);

            // Measure: fresh lookup each call (no cache, multiple enumerations per depth)
            var result = _provider.GetFileInfo(scenario.RequestedPath);
            _ = result.Exists;

            // Verify: deep path resolved correctly
            if (!result.Exists)
            {
                throw new InvalidOperationException($"Expected file to exist at {scenario.RequestedPath}");
            }
        }

        /// <summary>
        /// Benchmark: Mixed case lookup after cache priming.
        /// Verifies that case-insensitive key matching works correctly and efficiently.
        /// </summary>
        [Benchmark(Description = "Cache hit (varied case)")]
        [BenchmarkCategory("CacheHit")]
        public void CacheHit_VariedCase()
        {
            using var scenario = new TempFileSystemScope(_tempRootPath, 5, "Assets/Images/Logo.svg");

            // Prime with one casing
            var primed = _provider.GetFileInfo("assets/images/logo.svg");
            _ = primed.Exists;

            // Measure: lookup with different casing (should hit cache due to ordinal case-insensitive key)
            var variant = _provider.GetFileInfo("ASSETS/IMAGES/LOGO.SVG");
            _ = variant.Exists;

            // Verify: same physical path despite case variation
            if (!primed.Exists || !variant.Exists || primed.PhysicalPath != variant.PhysicalPath)
            {
                throw new InvalidOperationException("Case-insensitive cache hit verification failed.");
            }
        }

        /// <summary>
        /// Temporary file system scope: creates deterministic files and directories for benchmarking.
        /// </summary>
        private sealed class TempFileSystemScope : IDisposable
        {
            private readonly string _benchmarkRootPath;
            private readonly bool _created;

            /// <summary>
            /// Creates a temporary directory structure with the specified number of sibling entries
            /// and a target file at the given relative path.
            /// </summary>
            /// <param name="rootPath">Root directory for benchmark files.</param>
            /// <param name="siblingCount">Number of sibling entries to create in the directory containing the target file.</param>
            /// <param name="relativeFilePath">Relative path to the target file (e.g., "Assets/logo.svg").</param>
            public TempFileSystemScope(string rootPath, int siblingCount, string relativeFilePath)
            {
                _benchmarkRootPath = rootPath;

                try
                {
                    var segments = relativeFilePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    var targetFileName = segments[segments.Length - 1];
                    var dirPath = segments.Length > 1
                        ? Path.Combine(rootPath, Path.Combine(segments.Take(segments.Length - 1).ToArray()))
                        : rootPath;

                    Directory.CreateDirectory(dirPath);

                    // Create the target file
                    var targetPath = Path.Combine(dirPath, targetFileName);
                    File.WriteAllText(targetPath, "benchmark-content");

                    // Create sibling entries
                    for (var i = 0; i < siblingCount; i++)
                    {
                        var siblingPath = Path.Combine(dirPath, $"sibling-{i:D6}.dat");
                        File.WriteAllText(siblingPath, $"sibling-{i}");
                    }

                    _created = true;
                }
                catch
                {
                    _created = false;
                    throw;
                }
            }

            public void Dispose()
            {
                if (_created && Directory.Exists(_benchmarkRootPath))
                {
                    try
                    {
                        // Clean up only the benchmark-specific subdirectories, not the entire temp root
                        var benchmarkDir = _benchmarkRootPath;
                        if (Directory.Exists(benchmarkDir))
                        {
                            // Remove all contents
                            foreach (var item in Directory.EnumerateFileSystemEntries(benchmarkDir, "*", SearchOption.AllDirectories))
                            {
                                try
                                {
                                    if (File.Exists(item))
                                        File.Delete(item);
                                    else if (Directory.Exists(item))
                                        Directory.Delete(item);
                                }
                                catch
                                {
                                    // Ignore cleanup errors
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
            }
        }
    }
}
