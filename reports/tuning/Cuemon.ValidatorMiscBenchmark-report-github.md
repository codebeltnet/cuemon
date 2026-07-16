```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8737/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i9-12900KF 3.20GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 11.0.100-preview.4.26230.115
  [Host]     : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  Job-MTVXEA : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  Job-HYQNZL : .NET 9.0.18 (9.0.18, 9.0.1826.31522), X64 RyuJIT x86-64-v3

PowerPlanMode=00000000-0000-0000-0000-000000000000  IterationTime=250ms  MaxIterationCount=20  
MinIterationCount=15  WarmupCount=3  

```
| Method                                              | Runtime   | Mean         | Error      | StdDev     | Median       | Min          | Max          | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------------------------------- |---------- |-------------:|-----------:|-----------:|-------------:|-------------:|-------------:|-------:|--------:|-------:|----------:|------------:|
| &#39;ThrowIfContainsAny - no match&#39;                     | .NET 10.0 |     6.986 ns |  0.0616 ns |  0.0576 ns |     6.964 ns |     6.913 ns |     7.123 ns |   1.00 |    0.01 |      - |         - |          NA |
| &#39;ThrowIfNotContainsAny - match&#39;                     | .NET 10.0 |     1.884 ns |  0.0135 ns |  0.0119 ns |     1.886 ns |     1.864 ns |     1.902 ns |   0.27 |    0.00 |      - |         - |          NA |
| &#39;ThrowIfContainsAny - match (throws)&#39;               | .NET 10.0 | 1,354.627 ns | 15.1791 ns | 14.1985 ns | 1,355.466 ns | 1,322.130 ns | 1,377.824 ns | 193.93 |    2.50 | 0.1020 |    1664 B |          NA |
| &#39;ThrowIfNotContainsAny - no match (throws)&#39;         | .NET 10.0 | 1,241.701 ns | 10.5299 ns |  9.8497 ns | 1,242.677 ns | 1,213.809 ns | 1,254.369 ns | 177.76 |    1.96 | 0.0443 |     760 B |          NA |
|                                                     |           |              |            |            |              |              |              |        |         |        |           |             |
| &#39;ThrowIfContainsAny - no match&#39;                     | .NET 9.0  |    17.115 ns |  0.0488 ns |  0.0408 ns |    17.128 ns |    17.041 ns |    17.164 ns |   1.00 |    0.00 |      - |         - |          NA |
| &#39;ThrowIfNotContainsAny - match&#39;                     | .NET 9.0  |     5.549 ns |  0.0214 ns |  0.0190 ns |     5.552 ns |     5.510 ns |     5.583 ns |   0.32 |    0.00 |      - |         - |          NA |
| &#39;ThrowIfContainsAny - match (throws)&#39;               | .NET 9.0  | 1,945.985 ns | 32.3847 ns | 30.2927 ns | 1,934.675 ns | 1,910.775 ns | 2,022.311 ns | 113.70 |    1.73 | 0.1007 |    1664 B |          NA |
| &#39;ThrowIfNotContainsAny - no match (throws)&#39;         | .NET 9.0  | 1,950.613 ns | 19.7315 ns | 18.4569 ns | 1,945.772 ns | 1,920.392 ns | 1,984.450 ns | 113.97 |    1.08 | 0.0469 |     760 B |          NA |
|                                                     |           |              |            |            |              |              |              |        |         |        |           |             |
| ThrowWhen                                           | .NET 10.0 |     6.329 ns |  0.2048 ns |  0.2358 ns |     6.366 ns |     5.838 ns |     6.670 ns |      ? |       ? | 0.0015 |      24 B |           ? |
|                                                     |           |              |            |            |              |              |              |        |         |        |           |             |
| ThrowWhen                                           | .NET 9.0  |     9.314 ns |  0.1063 ns |  0.0942 ns |     9.302 ns |     9.151 ns |     9.495 ns |      ? |       ? | 0.0015 |      24 B |           ? |
|                                                     |           |              |            |            |              |              |              |        |         |        |           |             |
| &#39;ThrowIfContainsReservedKeyword - default comparer&#39; | .NET 10.0 |     4.541 ns |  0.0322 ns |  0.0269 ns |     4.531 ns |     4.502 ns |     4.586 ns |   1.00 |    0.01 |      - |         - |          NA |
| &#39;ThrowIfContainsReservedKeyword - custom comparer&#39;  | .NET 10.0 |     4.115 ns |  0.0158 ns |  0.0140 ns |     4.110 ns |     4.096 ns |     4.139 ns |   0.91 |    0.01 |      - |         - |          NA |
|                                                     |           |              |            |            |              |              |              |        |         |        |           |             |
| &#39;ThrowIfContainsReservedKeyword - default comparer&#39; | .NET 9.0  |     5.457 ns |  0.0461 ns |  0.0431 ns |     5.473 ns |     5.378 ns |     5.511 ns |   1.00 |    0.01 |      - |         - |          NA |
| &#39;ThrowIfContainsReservedKeyword - custom comparer&#39;  | .NET 9.0  |     5.936 ns |  0.0334 ns |  0.0296 ns |     5.934 ns |     5.885 ns |     5.987 ns |   1.09 |    0.01 |      - |         - |          NA |
