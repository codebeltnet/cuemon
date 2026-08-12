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
| Method                                                | Runtime   | SiblingCount | Mean      | Error     | StdDev    | Median    | Min       | Max       | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
|------------------------------------------------------ |---------- |------------- |----------:|----------:|----------:|----------:|----------:|----------:|------:|--------:|--------:|----------:|------------:|
| **&#39;PhysicalFileProvider - unique missing paths&#39;**         | **.NET 10.0** | **5**            |  **21.13 μs** |  **1.649 μs** |  **1.899 μs** |  **20.85 μs** |  **17.45 μs** |  **24.02 μs** |  **1.01** |    **0.13** |       **-** |     **848 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - unique missing paths&#39; | .NET 10.0 | 5            | 301.41 μs | 16.898 μs | 18.782 μs | 302.02 μs | 237.95 μs | 326.51 μs | 14.38 |    1.56 |       - |    8512 B |       10.04 |
|                                                       |           |              |           |           |           |           |           |           |       |         |         |           |             |
| &#39;PhysicalFileProvider - unique missing paths&#39;         | .NET 9.0  | 5            |  25.76 μs |  0.806 μs |  0.928 μs |  25.98 μs |  23.97 μs |  27.99 μs |  1.00 |    0.05 |       - |     848 B |        1.00 |
| &#39;PortablePhysicalFileProvider - unique missing paths&#39; | .NET 9.0  | 5            | 170.03 μs |  9.160 μs | 10.549 μs | 170.87 μs | 153.00 μs | 190.48 μs |  6.61 |    0.46 |       - |    8480 B |       10.00 |
|                                                       |           |              |           |           |           |           |           |           |       |         |         |           |             |
| **&#39;PhysicalFileProvider - unique missing paths&#39;**         | **.NET 10.0** | **50**           |  **19.38 μs** |  **0.950 μs** |  **1.094 μs** |  **19.39 μs** |  **17.31 μs** |  **20.79 μs** |  **1.00** |    **0.08** |       **-** |     **848 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - unique missing paths&#39; | .NET 10.0 | 50           | 215.79 μs | 10.823 μs | 11.581 μs | 217.68 μs | 186.42 μs | 231.12 μs | 11.17 |    0.86 |  3.0864 |   55328 B |       65.25 |
|                                                       |           |              |           |           |           |           |           |           |       |         |         |           |             |
| &#39;PhysicalFileProvider - unique missing paths&#39;         | .NET 9.0  | 50           |  18.50 μs |  1.077 μs |  1.240 μs |  18.38 μs |  15.75 μs |  20.75 μs |  1.00 |    0.09 |       - |     848 B |        1.00 |
| &#39;PortablePhysicalFileProvider - unique missing paths&#39; | .NET 9.0  | 50           | 223.05 μs | 12.786 μs | 14.212 μs | 220.85 μs | 204.51 μs | 254.51 μs | 12.11 |    1.11 |  3.1646 |   55296 B |       65.21 |
|                                                       |           |              |           |           |           |           |           |           |       |         |         |           |             |
| **&#39;PhysicalFileProvider - unique missing paths&#39;**         | **.NET 10.0** | **500**          |  **20.31 μs** |  **1.272 μs** |  **1.465 μs** |  **20.18 μs** |  **18.17 μs** |  **23.08 μs** |  **1.00** |    **0.10** |       **-** |     **848 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - unique missing paths&#39; | .NET 10.0 | 500          | 689.18 μs | 30.681 μs | 31.507 μs | 692.74 μs | 650.20 μs | 759.69 μs | 34.10 |    2.80 | 32.5000 |  523328 B |      617.13 |
|                                                       |           |              |           |           |           |           |           |           |       |         |         |           |             |
| &#39;PhysicalFileProvider - unique missing paths&#39;         | .NET 9.0  | 500          |  20.67 μs |  0.885 μs |  1.019 μs |  20.57 μs |  19.29 μs |  23.03 μs |  1.00 |    0.07 |       - |     848 B |        1.00 |
| &#39;PortablePhysicalFileProvider - unique missing paths&#39; | .NET 9.0  | 500          | 683.95 μs | 50.819 μs | 56.485 μs | 672.79 μs | 600.86 μs | 796.18 μs | 33.17 |    3.09 | 31.2500 |  523296 B |      617.09 |
