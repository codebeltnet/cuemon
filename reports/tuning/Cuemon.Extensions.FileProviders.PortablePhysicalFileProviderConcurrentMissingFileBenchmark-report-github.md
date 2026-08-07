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
| Method                                                   | Runtime   | Mean      | Error     | StdDev    | Median    | Min       | Max       | Ratio | RatioSD | Gen0    | Completed Work Items | Lock Contentions | Allocated | Alloc Ratio |
|--------------------------------------------------------- |---------- |----------:|----------:|----------:|----------:|----------:|----------:|------:|--------:|--------:|---------------------:|-----------------:|----------:|------------:|
| &#39;PhysicalFileProvider - different missing paths&#39;         | .NET 10.0 |  14.89 μs |  1.334 μs |  1.428 μs |  14.37 μs |  13.42 μs |  18.12 μs |  1.01 |    0.13 |  0.0556 |                    - |           0.0544 |     864 B |        1.00 |
| &#39;PortablePhysicalFileProvider - different missing paths&#39; | .NET 10.0 | 449.11 μs | 25.613 μs | 27.406 μs | 446.85 μs | 397.00 μs | 510.76 μs | 30.40 |    3.16 | 32.9861 |                    - |           0.1285 |  531368 B |      615.01 |
|                                                          |           |           |           |           |           |           |           |       |         |         |                      |                  |           |             |
| &#39;PhysicalFileProvider - different missing paths&#39;         | .NET 9.0  |  15.37 μs |  1.387 μs |  1.484 μs |  15.17 μs |  13.39 μs |  18.49 μs |  1.01 |    0.13 |       - |                    - |           0.0492 |     864 B |        1.00 |
| &#39;PortablePhysicalFileProvider - different missing paths&#39; | .NET 9.0  | 479.54 μs | 43.322 μs | 49.890 μs | 462.52 μs | 410.94 μs | 575.57 μs | 31.46 |    4.28 | 32.8125 |                    - |           0.1547 |  531336 B |      614.97 |
|                                                          |           |           |           |           |           |           |           |       |         |         |                      |                  |           |             |
| &#39;PhysicalFileProvider - same missing path&#39;               | .NET 10.0 |  17.94 μs |  2.306 μs |  2.563 μs |  17.65 μs |  14.29 μs |  23.58 μs |  1.02 |    0.20 |       - |                    - |           0.0913 |     840 B |        1.00 |
| &#39;PortablePhysicalFileProvider - same missing path&#39;       | .NET 10.0 | 465.33 μs | 41.451 μs | 47.735 μs | 453.29 μs | 395.65 μs | 552.86 μs | 26.42 |    4.43 | 32.9861 |                    - |           0.1458 |  531336 B |      632.54 |
|                                                          |           |           |           |           |           |           |           |       |         |         |                      |                  |           |             |
| &#39;PhysicalFileProvider - same missing path&#39;               | .NET 9.0  |  17.05 μs |  2.417 μs |  2.587 μs |  16.69 μs |  14.05 μs |  23.36 μs |  1.02 |    0.20 |       - |                    - |           0.0725 |     840 B |        1.00 |
| &#39;PortablePhysicalFileProvider - same missing path&#39;       | .NET 9.0  | 466.20 μs | 29.858 μs | 33.187 μs | 466.83 μs | 413.22 μs | 523.48 μs | 27.88 |    4.16 | 33.2031 |                    - |           0.2480 |  531304 B |      632.50 |
