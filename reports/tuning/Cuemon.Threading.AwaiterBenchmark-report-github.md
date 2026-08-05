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
| Method                                           | Runtime   | Mean           | Error         | StdDev        | Median         | Min            | Max            | Ratio     | RatioSD  | Gen0   | Allocated | Alloc Ratio |
|------------------------------------------------- |---------- |---------------:|--------------:|--------------:|---------------:|---------------:|---------------:|----------:|---------:|-------:|----------:|------------:|
| &#39;Direct await - immediate success&#39;               | .NET 10.0 |      1.1686 ns |     0.4213 ns |     0.4851 ns |      0.9187 ns |      0.6406 ns |      2.0159 ns |      1.18 |     0.68 |      - |         - |          NA |
| &#39;Awaiter - immediate success&#39;                    | .NET 10.0 |     41.2368 ns |     1.4858 ns |     1.5898 ns |     40.8783 ns |     38.9826 ns |     45.4328 ns |     41.49 |    15.81 | 0.0081 |     128 B |          NA |
| &#39;Awaiter - 1 unsuccessful result then success&#39;   | .NET 10.0 |     75.6987 ns |     3.3230 ns |     3.8268 ns |     74.5604 ns |     70.3202 ns |     83.1604 ns |     76.17 |    29.14 | 0.0080 |     128 B |          NA |
| &#39;Awaiter - 10 unsuccessful results then success&#39; | .NET 10.0 |    402.3537 ns |    17.4538 ns |    19.3999 ns |    403.2956 ns |    366.3224 ns |    436.3778 ns |    404.86 |   154.75 | 0.0074 |     128 B |          NA |
| &#39;Awaiter - 1 exception then success&#39;             | .NET 10.0 |  2,848.7940 ns |    45.6752 ns |    42.7246 ns |  2,836.4864 ns |  2,789.2363 ns |  2,936.8668 ns |  2,866.52 | 1,087.48 | 0.0316 |     496 B |          NA |
| &#39;Awaiter - 2 exceptions then success&#39;            | .NET 10.0 |  3,852.8681 ns |   243.5870 ns |   280.5152 ns |  3,774.3764 ns |  3,503.9328 ns |  4,454.6288 ns |  3,876.84 | 1,498.16 | 0.0562 |     952 B |          NA |
| &#39;Awaiter - 10 exceptions then success&#39;           | .NET 10.0 | 18,373.1467 ns |   669.1400 ns |   715.9723 ns | 18,298.3352 ns | 17,432.0542 ns | 19,666.4715 ns | 18,487.48 | 7,045.80 | 0.2116 |    4136 B |          NA |
|                                                  |           |                |               |               |                |                |                |           |          |        |           |             |
| &#39;Direct await - immediate success&#39;               | .NET 9.0  |      0.6436 ns |     0.1225 ns |     0.1411 ns |      0.5888 ns |      0.4502 ns |      0.9046 ns |      1.04 |     0.31 |      - |         - |          NA |
| &#39;Awaiter - immediate success&#39;                    | .NET 9.0  |     43.7514 ns |     1.8304 ns |     1.9585 ns |     43.7521 ns |     41.0902 ns |     48.9031 ns |     71.03 |    14.93 | 0.0080 |     128 B |          NA |
| &#39;Awaiter - 1 unsuccessful result then success&#39;   | .NET 9.0  |     78.2367 ns |     2.6698 ns |     2.8567 ns |     77.9068 ns |     74.1319 ns |     84.9456 ns |    127.02 |    26.50 | 0.0079 |     128 B |          NA |
| &#39;Awaiter - 10 unsuccessful results then success&#39; | .NET 9.0  |    391.0464 ns |     6.3003 ns |     4.9189 ns |    390.3881 ns |    382.2525 ns |    400.4781 ns |    634.90 |   130.76 | 0.0078 |     128 B |          NA |
| &#39;Awaiter - 1 exception then success&#39;             | .NET 9.0  |  2,854.6606 ns |    94.1330 ns |    96.6677 ns |  2,828.0311 ns |  2,741.3313 ns |  3,126.1184 ns |  4,634.81 |   964.87 | 0.0229 |     496 B |          NA |
| &#39;Awaiter - 2 exceptions then success&#39;            | .NET 9.0  |  5,711.0810 ns |   177.2485 ns |   197.0112 ns |  5,735.0608 ns |  5,430.3575 ns |  6,067.3631 ns |  9,272.47 | 1,931.15 | 0.0438 |     952 B |          NA |
| &#39;Awaiter - 10 exceptions then success&#39;           | .NET 9.0  | 28,370.2022 ns | 1,625.1880 ns | 1,738.9329 ns | 27,654.3457 ns | 26,701.1068 ns | 32,713.2704 ns | 46,061.64 | 9,869.61 | 0.2170 |    4136 B |          NA |
