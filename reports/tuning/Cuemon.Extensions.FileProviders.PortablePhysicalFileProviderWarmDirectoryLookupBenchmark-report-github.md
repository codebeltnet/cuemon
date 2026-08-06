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
| Method                                                      | Runtime   | Depth   | SiblingCount | Mean      | Error     | StdDev    | Median    | Min       | Max       | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
|------------------------------------------------------------ |---------- |-------- |------------- |----------:|----------:|----------:|----------:|----------:|----------:|------:|--------:|--------:|----------:|------------:|
| **&#39;PhysicalFileProvider - exact casing&#39;**                       | **.NET 10.0** | **Shallow** | **5**            |  **65.16 μs** |  **3.506 μs** |  **4.037 μs** |  **64.99 μs** |  **58.51 μs** |  **72.09 μs** |  **1.00** |    **0.09** |       **-** |   **3.66 KB** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 10.0 | Shallow | 5            |  71.43 μs |  7.472 μs |  7.995 μs |  69.23 μs |  61.59 μs |  90.21 μs |  1.10 |    0.14 |       - |   3.76 KB |        1.03 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 10.0 | Shallow | 5            |  71.16 μs |  5.819 μs |  6.702 μs |  73.00 μs |  59.52 μs |  81.49 μs |  1.10 |    0.12 |       - |   3.76 KB |        1.03 |
|                                                             |           |         |              |           |           |           |           |           |           |       |         |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                       | .NET 9.0  | Shallow | 5            |  66.60 μs |  4.679 μs |  5.388 μs |  65.82 μs |  60.29 μs |  77.17 μs |  1.01 |    0.11 |       - |   3.64 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 9.0  | Shallow | 5            |  71.14 μs |  3.302 μs |  3.802 μs |  71.11 μs |  64.32 μs |  76.96 μs |  1.07 |    0.10 |       - |   3.74 KB |        1.03 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 9.0  | Shallow | 5            |  69.33 μs |  5.427 μs |  6.032 μs |  68.51 μs |  60.66 μs |  80.82 μs |  1.05 |    0.12 |       - |   3.74 KB |        1.03 |
|                                                             |           |         |              |           |           |           |           |           |           |       |         |         |           |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                       | **.NET 10.0** | **Shallow** | **500**          | **306.39 μs** | **18.839 μs** | **20.940 μs** | **297.72 μs** | **281.12 μs** | **344.59 μs** |  **1.00** |    **0.09** | **17.5000** |  **270.5 KB** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 10.0 | Shallow | 500          | 301.39 μs | 20.065 μs | 22.303 μs | 294.88 μs | 277.06 μs | 356.19 μs |  0.99 |    0.10 | 17.2872 |  270.6 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 10.0 | Shallow | 500          | 312.41 μs | 24.610 μs | 27.354 μs | 304.20 μs | 279.26 μs | 380.71 μs |  1.02 |    0.11 | 17.3611 |  270.6 KB |        1.00 |
|                                                             |           |         |              |           |           |           |           |           |           |       |         |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                       | .NET 9.0  | Shallow | 500          | 287.41 μs | 11.835 μs | 12.154 μs | 287.58 μs | 271.94 μs | 314.79 μs |  1.00 |    0.06 | 17.5439 | 270.48 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 9.0  | Shallow | 500          | 324.51 μs | 31.270 μs | 32.112 μs | 328.18 μs | 274.08 μs | 383.24 μs |  1.13 |    0.12 | 17.2414 | 270.59 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 9.0  | Shallow | 500          | 370.20 μs | 33.885 μs | 39.022 μs | 378.00 μs | 298.07 μs | 422.31 μs |  1.29 |    0.14 | 16.9271 | 270.59 KB |        1.00 |
|                                                             |           |         |              |           |           |           |           |           |           |       |         |         |           |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                       | **.NET 10.0** | **Deep**    | **5**            |  **70.22 μs** |  **7.934 μs** |  **9.136 μs** |  **68.14 μs** |  **57.74 μs** |  **87.44 μs** |  **1.02** |    **0.18** |  **0.2706** |   **4.26 KB** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 10.0 | Deep    | 5            |  79.83 μs |  7.095 μs |  8.170 μs |  80.38 μs |  64.33 μs |  98.10 μs |  1.15 |    0.18 |  0.2648 |   4.36 KB |        1.02 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 10.0 | Deep    | 5            |  76.06 μs |  4.989 μs |  5.745 μs |  75.95 μs |  66.23 μs |  86.71 μs |  1.10 |    0.16 |       - |   4.36 KB |        1.02 |
|                                                             |           |         |              |           |           |           |           |           |           |       |         |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                       | .NET 9.0  | Deep    | 5            |  83.00 μs | 13.364 μs | 14.855 μs |  80.66 μs |  63.28 μs | 115.33 μs |  1.03 |    0.25 |  0.2615 |   4.24 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 9.0  | Deep    | 5            | 106.44 μs | 14.816 μs | 17.062 μs | 110.82 μs |  70.57 μs | 129.43 μs |  1.32 |    0.30 |  0.2815 |   4.34 KB |        1.02 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 9.0  | Deep    | 5            |  84.12 μs |  8.758 μs | 10.086 μs |  84.41 μs |  69.85 μs | 100.04 μs |  1.04 |    0.21 |       - |   4.34 KB |        1.02 |
|                                                             |           |         |              |           |           |           |           |           |           |       |         |         |           |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                       | **.NET 10.0** | **Deep**    | **500**          | **347.23 μs** | **28.864 μs** | **33.239 μs** | **338.12 μs** | **309.84 μs** | **434.41 μs** |  **1.01** |    **0.13** | **17.5781** | **294.34 KB** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 10.0 | Deep    | 500          | 345.18 μs | 27.631 μs | 31.820 μs | 347.47 μs | 300.74 μs | 413.82 μs |  1.00 |    0.13 | 18.8953 | 294.44 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 10.0 | Deep    | 500          | 327.87 μs | 13.182 μs | 15.181 μs | 332.32 μs | 307.01 μs | 356.19 μs |  0.95 |    0.09 | 18.7500 | 294.44 KB |        1.00 |
|                                                             |           |         |              |           |           |           |           |           |           |       |         |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                       | .NET 9.0  | Deep    | 500          | 300.19 μs | 11.194 μs | 11.496 μs | 295.34 μs | 285.89 μs | 328.42 μs |  1.00 |    0.05 | 18.1818 | 294.32 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 9.0  | Deep    | 500          | 313.91 μs | 22.162 μs | 23.714 μs | 314.55 μs | 282.24 μs | 372.87 μs |  1.05 |    0.09 | 18.7500 | 294.42 KB |        1.00 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 9.0  | Deep    | 500          | 330.29 μs | 24.074 μs | 27.724 μs | 324.81 μs | 286.52 μs | 390.66 μs |  1.10 |    0.10 | 18.3824 | 294.42 KB |        1.00 |
