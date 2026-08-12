```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8973/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i9-12900KF 3.20GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 11.0.100-preview.4.26230.115
  [Host]     : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  Job-LDLMHG : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  Job-IOAYXE : .NET 9.0.18 (9.0.18, 9.0.1826.31522), X64 RyuJIT x86-64-v3

PowerPlanMode=00000000-0000-0000-0000-000000000000  IterationTime=250ms  MaxIterationCount=20  
MinIterationCount=15  WarmupCount=1  

```
| Method                                                 | Runtime   | SiblingCount | Mean | Error | Median | Min | Max |
|------------------------------------------------------- |---------- |------------- |-----:|------:|-------:|----:|----:|
| &#39;PortablePhysicalFileProvider - same casing collision&#39; | .NET 10.0 | 50           |   NA |    NA |     NA |  NA |  NA |
| &#39;PortablePhysicalFileProvider - same casing collision&#39; | .NET 9.0  | 50           |   NA |    NA |     NA |  NA |  NA |

Benchmarks with issues:
  PortablePhysicalFileProviderCollisionBenchmark.'PortablePhysicalFileProvider - same casing collision': Job-LDLMHG(PowerPlanMode=00000000-0000-0000-0000-000000000000, Runtime=.NET 10.0, IterationTime=250ms, MaxIterationCount=20, MinIterationCount=15, WarmupCount=1) [SiblingCount=50]
  PortablePhysicalFileProviderCollisionBenchmark.'PortablePhysicalFileProvider - same casing collision': Job-IOAYXE(PowerPlanMode=00000000-0000-0000-0000-000000000000, Runtime=.NET 9.0, IterationTime=250ms, MaxIterationCount=20, MinIterationCount=15, WarmupCount=1) [SiblingCount=50]
