using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains;
using Perfolizer.Horology;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cuemon;

public class Program
{
    // Inspiration sources:
    // https://github.com/dotnet/performance/blob/main/src/harness/BenchmarkDotNet.Extensions/RecommendedConfig.cs
    // https://github.com/dotnet/performance/blob/main/docs/microbenchmark-design-guidelines.md

    public static void Main(string[] args)
    {
        var projectDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var artifactsPath = Path.Combine(projectDir, "..", "..", "..", "reports");

        var job = Job.Default
            .WithWarmupCount(1) // 1 warmup is enough for our purpose
            .WithIterationTime(TimeInterval.FromMilliseconds(250)) // the default is 0.5s per iteration, which is slighlty too much for us
            .WithMinIterationCount(15)
            .WithMaxIterationCount(20) // we don't want to run more that 20 iterations
            .DontEnforcePowerPlan(); // make sure BDN does not try to enforce High Performance power plan on Windows

        var config = ManualConfig.CreateEmpty()
            .WithBuildTimeout(TimeSpan.FromMinutes(15)) // for slow machines
            .AddLogger(ConsoleLogger.Default) // log output to console
            .AddValidator(DefaultConfig.Instance.GetValidators().ToArray()) // copy default validators
            .AddAnalyser(DefaultConfig.Instance.GetAnalysers().ToArray()) // copy default analysers
            .AddExporter(MarkdownExporter.GitHub) // export to GitHub markdown
            .AddColumnProvider(DefaultColumnProviders.Instance) // display default columns (method name, args etc)
            .AddJob(job.AsDefault()) // tell BDN that this are our default settings
            .WithArtifactsPath(artifactsPath)
            .AddDiagnoser(MemoryDiagnoser.Default) // MemoryDiagnoser is enabled by default
            .AddExporter(JsonExporter.Full) // make sure we export to Json
            .AddColumn(StatisticColumn.Median, StatisticColumn.Min, StatisticColumn.Max)
            .WithSummaryStyle(SummaryStyle.Default.WithMaxParameterColumnWidth(36)); // the default is 20 and trims too aggressively some benchmark results

        var finalArgs = args.Length == 0
            ? new[] { "--filter", "*", "--runtimes", "net9.0", "net10.0" }
            : args;

        BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(finalArgs, config);
    }
}
