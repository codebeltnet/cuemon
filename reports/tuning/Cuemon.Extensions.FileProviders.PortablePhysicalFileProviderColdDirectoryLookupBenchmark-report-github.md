```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8973/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i9-12900KF 3.20GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 11.0.100-preview.4.26230.115
  [Host]     : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  Job-UBZONI : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  Job-JFLVQI : .NET 9.0.18 (9.0.18, 9.0.1826.31522), X64 RyuJIT x86-64-v3

PowerPlanMode=00000000-0000-0000-0000-000000000000  InvocationCount=1  IterationTime=250ms  
MaxIterationCount=20  MinIterationCount=15  UnrollFactor=1  
WarmupCount=1  

```
| Method                                                           | Runtime   | Depth   | SiblingCount | Mean       | Error     | StdDev    | Median     | Min        | Max        | Ratio | RatioSD | Allocated  | Alloc Ratio |
|----------------------------------------------------------------- |---------- |-------- |------------- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|------:|--------:|-----------:|------------:|
| **&#39;PhysicalFileProvider - exact casing&#39;**                            | **.NET 10.0** | **Shallow** | **5**            |   **157.3 μs** |  **17.05 μs** |  **19.63 μs** |   **162.9 μs** |   **123.5 μs** |   **186.7 μs** |  **1.02** |    **0.18** |    **3.66 KB** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 10.0 | Shallow | 5            |   249.7 μs |  26.22 μs |  29.15 μs |   262.4 μs |   183.3 μs |   284.3 μs |  1.61 |    0.28 |    7.72 KB |        2.11 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 10.0 | Shallow | 5            |   251.3 μs |  26.82 μs |  29.82 μs |   253.0 μs |   203.0 μs |   321.8 μs |  1.62 |    0.28 |    7.72 KB |        2.11 |
|                                                                  |           |         |              |            |           |           |            |            |            |       |         |            |             |
| &#39;PhysicalFileProvider - exact casing&#39;                            | .NET 9.0  | Shallow | 5            |   162.9 μs |  21.86 μs |  25.18 μs |   164.3 μs |   124.7 μs |   209.4 μs |  1.02 |    0.22 |    3.64 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 9.0  | Shallow | 5            |   238.8 μs |  29.01 μs |  33.41 μs |   223.7 μs |   204.7 μs |   315.5 μs |  1.50 |    0.31 |    7.69 KB |        2.11 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 9.0  | Shallow | 5            |   243.1 μs |  32.11 μs |  36.98 μs |   239.6 μs |   185.5 μs |   313.8 μs |  1.53 |    0.33 |    7.69 KB |        2.11 |
|                                                                  |           |         |              |            |           |           |            |            |            |       |         |            |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                            | **.NET 10.0** | **Shallow** | **500**          |   **465.0 μs** |  **40.44 μs** |  **44.95 μs** |   **453.7 μs** |   **387.4 μs** |   **572.8 μs** |  **1.01** |    **0.13** |   **270.5 KB** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 10.0 | Shallow | 500          |   884.0 μs |  31.87 μs |  32.73 μs |   879.5 μs |   843.6 μs |   944.2 μs |  1.92 |    0.19 |  533.69 KB |        1.97 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 10.0 | Shallow | 500          |   858.5 μs |  55.31 μs |  59.18 μs |   872.1 μs |   732.9 μs |   962.6 μs |  1.86 |    0.21 |  533.69 KB |        1.97 |
|                                                                  |           |         |              |            |           |           |            |            |            |       |         |            |             |
| &#39;PhysicalFileProvider - exact casing&#39;                            | .NET 9.0  | Shallow | 500          |   509.5 μs |  35.56 μs |  36.52 μs |   511.1 μs |   455.0 μs |   596.5 μs |  1.00 |    0.10 |  270.48 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 9.0  | Shallow | 500          |   894.4 μs |  45.03 μs |  48.18 μs |   892.5 μs |   824.5 μs | 1,004.2 μs |  1.76 |    0.15 |  533.66 KB |        1.97 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 9.0  | Shallow | 500          |   902.2 μs |  39.41 μs |  40.47 μs |   906.1 μs |   829.0 μs |   972.4 μs |  1.78 |    0.14 |  533.66 KB |        1.97 |
|                                                                  |           |         |              |            |           |           |            |            |            |       |         |            |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                            | **.NET 10.0** | **Deep**    | **5**            |   **187.8 μs** |  **12.15 μs** |  **13.00 μs** |   **187.2 μs** |   **164.0 μs** |   **210.8 μs** |  **1.00** |    **0.10** |    **4.26 KB** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 10.0 | Deep    | 5            |   624.9 μs |  45.04 μs |  50.06 μs |   621.6 μs |   538.4 μs |   704.9 μs |  3.34 |    0.35 |   20.29 KB |        4.77 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 10.0 | Deep    | 5            |   702.1 μs | 135.38 μs | 155.90 μs |   651.6 μs |   517.3 μs | 1,044.4 μs |  3.76 |    0.86 |   20.29 KB |        4.77 |
|                                                                  |           |         |              |            |           |           |            |            |            |       |         |            |             |
| &#39;PhysicalFileProvider - exact casing&#39;                            | .NET 9.0  | Deep    | 5            |   166.9 μs |  23.73 μs |  27.33 μs |   165.8 μs |   127.6 μs |   239.2 μs |  1.02 |    0.23 |    4.24 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 9.0  | Deep    | 5            |   579.2 μs |  69.32 μs |  77.05 μs |   558.9 μs |   475.4 μs |   720.2 μs |  3.55 |    0.71 |   20.21 KB |        4.76 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 9.0  | Deep    | 5            |   564.2 μs |  38.23 μs |  39.26 μs |   564.5 μs |   491.2 μs |   658.1 μs |  3.46 |    0.57 |   20.21 KB |        4.76 |
|                                                                  |           |         |              |            |           |           |            |            |            |       |         |            |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                            | **.NET 10.0** | **Deep**    | **500**          |   **450.9 μs** |  **23.42 μs** |  **24.05 μs** |   **457.8 μs** |   **408.3 μs** |   **497.6 μs** |  **1.00** |    **0.07** |  **294.34 KB** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 10.0 | Deep    | 500          | 2,162.1 μs |  89.61 μs |  95.88 μs | 2,149.8 μs | 2,024.1 μs | 2,396.7 μs |  4.81 |    0.33 |  1377.8 KB |        4.68 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 10.0 | Deep    | 500          | 2,210.8 μs | 202.82 μs | 225.44 μs | 2,111.0 μs | 1,918.3 μs | 2,706.7 μs |  4.92 |    0.55 |  1377.8 KB |        4.68 |
|                                                                  |           |         |              |            |           |           |            |            |            |       |         |            |             |
| &#39;PhysicalFileProvider - exact casing&#39;                            | .NET 9.0  | Deep    | 500          |   445.3 μs |  32.56 μs |  37.49 μs |   457.1 μs |   378.4 μs |   490.9 μs |  1.01 |    0.12 |  294.32 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 9.0  | Deep    | 500          | 2,028.5 μs |  76.67 μs |  82.04 μs | 2,003.7 μs | 1,900.5 μs | 2,207.8 μs |  4.59 |    0.44 | 1377.73 KB |        4.68 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 9.0  | Deep    | 500          | 2,068.2 μs | 154.75 μs | 172.00 μs | 2,013.0 μs | 1,857.2 μs | 2,446.6 μs |  4.68 |    0.56 | 1377.73 KB |        4.68 |
