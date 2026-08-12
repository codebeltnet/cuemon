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
| Method                                             | Runtime   | SiblingCount | Mean      | Error     | StdDev    | Median    | Min       | Max       | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
|--------------------------------------------------- |---------- |------------- |----------:|----------:|----------:|----------:|----------:|----------:|------:|--------:|--------:|----------:|------------:|
| **&#39;PhysicalFileProvider - same missing path&#39;**         | **.NET 10.0** | **5**            |  **14.49 μs** |  **0.749 μs** |  **0.833 μs** |  **14.32 μs** |  **13.34 μs** |  **16.16 μs** |  **1.00** |    **0.08** |       **-** |     **824 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - same missing path&#39; | .NET 10.0 | 5            | 203.47 μs | 13.824 μs | 15.365 μs | 204.72 μs | 182.00 μs | 241.75 μs | 14.08 |    1.29 |       - |    8496 B |       10.31 |
|                                                    |           |              |           |           |           |           |           |           |       |         |         |           |             |
| &#39;PhysicalFileProvider - same missing path&#39;         | .NET 9.0  | 5            |  14.00 μs |  0.393 μs |  0.437 μs |  13.89 μs |  13.06 μs |  14.67 μs |  1.00 |    0.04 |       - |     824 B |        1.00 |
| &#39;PortablePhysicalFileProvider - same missing path&#39; | .NET 9.0  | 5            |  34.38 μs |  2.097 μs |  2.244 μs |  34.18 μs |  31.31 μs |  39.04 μs |  2.46 |    0.17 |       - |    1408 B |        1.71 |
|                                                    |           |              |           |           |           |           |           |           |       |         |         |           |             |
| **&#39;PhysicalFileProvider - same missing path&#39;**         | **.NET 10.0** | **50**           |  **14.99 μs** |  **1.163 μs** |  **1.292 μs** |  **14.56 μs** |  **13.56 μs** |  **18.00 μs** |  **1.01** |    **0.12** |       **-** |     **824 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - same missing path&#39; | .NET 10.0 | 50           | 247.19 μs | 25.498 μs | 29.363 μs | 245.33 μs | 197.98 μs | 306.81 μs | 16.60 |    2.32 |  3.3333 |   56088 B |       68.07 |
|                                                    |           |              |           |           |           |           |           |           |       |         |         |           |             |
| &#39;PhysicalFileProvider - same missing path&#39;         | .NET 9.0  | 50           |  15.01 μs |  0.667 μs |  0.768 μs |  15.09 μs |  13.56 μs |  16.32 μs |  1.00 |    0.07 |       - |     824 B |        1.00 |
| &#39;PortablePhysicalFileProvider - same missing path&#39; | .NET 9.0  | 50           | 219.96 μs | 15.890 μs | 17.002 μs | 218.92 μs | 183.90 μs | 256.51 μs | 14.69 |    1.33 |  3.0864 |   56056 B |       68.03 |
|                                                    |           |              |           |           |           |           |           |           |       |         |         |           |             |
| **&#39;PhysicalFileProvider - same missing path&#39;**         | **.NET 10.0** | **500**          |  **18.82 μs** |  **1.629 μs** |  **1.810 μs** |  **19.11 μs** |  **15.86 μs** |  **22.22 μs** |  **1.01** |    **0.14** |       **-** |     **824 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - same missing path&#39; | .NET 10.0 | 500          | 784.11 μs | 72.429 μs | 83.410 μs | 766.96 μs | 660.56 μs | 952.56 μs | 42.03 |    5.96 | 31.2500 |  531304 B |      644.79 |
|                                                    |           |              |           |           |           |           |           |           |       |         |         |           |             |
| &#39;PhysicalFileProvider - same missing path&#39;         | .NET 9.0  | 500          |  16.74 μs |  1.200 μs |  1.382 μs |  17.08 μs |  14.68 μs |  19.66 μs |  1.01 |    0.11 |       - |     824 B |        1.00 |
| &#39;PortablePhysicalFileProvider - same missing path&#39; | .NET 9.0  | 500          | 735.07 μs | 46.760 μs | 50.033 μs | 728.85 μs | 647.75 μs | 818.09 μs | 44.20 |    4.59 | 32.5000 |  531272 B |      644.75 |
