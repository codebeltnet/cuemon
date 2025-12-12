using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using Codebelt.Extensions.BenchmarkDotNet;
using Codebelt.Extensions.BenchmarkDotNet.Console;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkProgram.Run(args, o =>
        {
            o.AllowDebugBuild = BenchmarkProgram.IsDebugBuild;
            o.ConfigureBenchmarkDotNet(c =>
            {
                var slimJob = BenchmarkWorkspaceOptions.Slim;
                return c.AddJob(slimJob.WithRuntime(CoreRuntime.Core90))
                    .AddJob(slimJob.WithRuntime(CoreRuntime.Core10_0));
            });
        });
    }
}
