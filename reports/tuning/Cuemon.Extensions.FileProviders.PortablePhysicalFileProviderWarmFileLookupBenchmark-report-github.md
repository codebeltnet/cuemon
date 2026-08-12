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
| Method                                                      | Runtime   | Depth   | SiblingCount | Mean     | Error    | StdDev   | Median   | Min      | Max      | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------------------------------------------ |---------- |-------- |------------- |---------:|---------:|---------:|---------:|---------:|---------:|------:|--------:|----------:|------------:|
| **&#39;PhysicalFileProvider - exact casing&#39;**                       | **.NET 10.0** | **Shallow** | **5**            | **27.88 μs** | **4.256 μs** | **4.901 μs** | **27.40 μs** | **21.70 μs** | **37.28 μs** |  **1.03** |    **0.25** |     **816 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 10.0 | Shallow | 5            | 26.84 μs | 5.735 μs | 6.604 μs | 24.58 μs | 18.81 μs | 42.10 μs |  0.99 |    0.29 |     920 B |        1.13 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 10.0 | Shallow | 5            | 27.65 μs | 4.051 μs | 4.665 μs | 28.01 μs | 19.85 μs | 33.81 μs |  1.02 |    0.24 |     920 B |        1.13 |
|                                                             |           |         |              |          |          |          |          |          |          |       |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                       | .NET 9.0  | Shallow | 5            | 21.96 μs | 1.522 μs | 1.692 μs | 21.71 μs | 19.54 μs | 25.02 μs |  1.01 |    0.11 |     816 B |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 9.0  | Shallow | 5            | 21.92 μs | 1.523 μs | 1.754 μs | 21.70 μs | 19.36 μs | 25.82 μs |  1.00 |    0.11 |     920 B |        1.13 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 9.0  | Shallow | 5            | 22.19 μs | 1.410 μs | 1.568 μs | 21.85 μs | 19.95 μs | 25.44 μs |  1.02 |    0.10 |     920 B |        1.13 |
|                                                             |           |         |              |          |          |          |          |          |          |       |         |           |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                       | **.NET 10.0** | **Shallow** | **500**          | **22.49 μs** | **2.398 μs** | **2.665 μs** | **21.85 μs** | **19.21 μs** | **29.30 μs** |  **1.01** |    **0.16** |     **832 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 10.0 | Shallow | 500          | 20.76 μs | 1.361 μs | 1.398 μs | 20.59 μs | 19.02 μs | 23.65 μs |  0.93 |    0.12 |     936 B |        1.12 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 10.0 | Shallow | 500          | 27.96 μs | 2.686 μs | 3.093 μs | 28.03 μs | 21.51 μs | 34.13 μs |  1.26 |    0.19 |     936 B |        1.12 |
|                                                             |           |         |              |          |          |          |          |          |          |       |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                       | .NET 9.0  | Shallow | 500          | 22.27 μs | 1.761 μs | 1.885 μs | 22.17 μs | 19.40 μs | 25.92 μs |  1.01 |    0.12 |     832 B |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 9.0  | Shallow | 500          | 22.05 μs | 2.398 μs | 2.566 μs | 21.14 μs | 18.52 μs | 28.38 μs |  1.00 |    0.14 |     936 B |        1.12 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 9.0  | Shallow | 500          | 21.33 μs | 1.077 μs | 1.197 μs | 21.21 μs | 18.90 μs | 23.79 μs |  0.96 |    0.10 |     936 B |        1.12 |
|                                                             |           |         |              |          |          |          |          |          |          |       |         |           |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                       | **.NET 10.0** | **Deep**    | **5**            | **24.70 μs** | **2.343 μs** | **2.698 μs** | **25.32 μs** | **20.03 μs** | **29.43 μs** |  **1.01** |    **0.16** |     **912 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 10.0 | Deep    | 5            | 22.32 μs | 2.227 μs | 2.475 μs | 21.97 μs | 19.21 μs | 27.04 μs |  0.91 |    0.14 |    1016 B |        1.11 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 10.0 | Deep    | 5            | 21.07 μs | 1.788 μs | 2.060 μs | 20.00 μs | 18.98 μs | 25.47 μs |  0.86 |    0.13 |    1016 B |        1.11 |
|                                                             |           |         |              |          |          |          |          |          |          |       |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                       | .NET 9.0  | Deep    | 5            | 20.63 μs | 1.208 μs | 1.292 μs | 20.45 μs | 18.41 μs | 23.12 μs |  1.00 |    0.09 |     912 B |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 9.0  | Deep    | 5            | 21.05 μs | 0.769 μs | 0.886 μs | 21.07 μs | 19.68 μs | 22.98 μs |  1.02 |    0.07 |    1016 B |        1.11 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 9.0  | Deep    | 5            | 20.64 μs | 1.338 μs | 1.487 μs | 20.46 μs | 18.82 μs | 24.04 μs |  1.00 |    0.09 |    1016 B |        1.11 |
|                                                             |           |         |              |          |          |          |          |          |          |       |         |           |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                       | **.NET 10.0** | **Deep**    | **500**          | **20.89 μs** | **1.385 μs** | **1.539 μs** | **20.46 μs** | **18.99 μs** | **24.42 μs** |  **1.00** |    **0.10** |     **912 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 10.0 | Deep    | 500          | 20.31 μs | 1.289 μs | 1.323 μs | 20.19 μs | 18.99 μs | 23.34 μs |  0.98 |    0.09 |    1016 B |        1.11 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 10.0 | Deep    | 500          | 22.47 μs | 2.148 μs | 2.387 μs | 21.78 μs | 18.95 μs | 27.68 μs |  1.08 |    0.13 |    1016 B |        1.11 |
|                                                             |           |         |              |          |          |          |          |          |          |       |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                       | .NET 9.0  | Deep    | 500          | 22.84 μs | 1.826 μs | 2.103 μs | 22.79 μs | 19.55 μs | 26.90 μs |  1.01 |    0.13 |     912 B |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (warm cache)&#39;  | .NET 9.0  | Deep    | 500          | 20.92 μs | 1.256 μs | 1.446 μs | 20.52 μs | 19.01 μs | 23.59 μs |  0.92 |    0.10 |    1016 B |        1.11 |
| &#39;PortablePhysicalFileProvider - varied casing (warm cache)&#39; | .NET 9.0  | Deep    | 500          | 21.12 μs | 1.315 μs | 1.514 μs | 21.48 μs | 19.02 μs | 23.62 μs |  0.93 |    0.11 |    1016 B |        1.11 |
