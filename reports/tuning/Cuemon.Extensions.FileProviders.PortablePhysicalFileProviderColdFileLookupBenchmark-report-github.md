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
| Method                                                           | Runtime   | Depth   | SiblingCount | Mean        | Error      | StdDev     | Median      | Min         | Max         | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------------------------------------------------------------- |---------- |-------- |------------- |------------:|-----------:|-----------:|------------:|------------:|------------:|------:|--------:|----------:|------------:|
| **&#39;PhysicalFileProvider - exact casing&#39;**                            | **.NET 10.0** | **Shallow** | **5**            |    **78.02 μs** |  **10.631 μs** |  **11.375 μs** |    **77.95 μs** |    **60.00 μs** |    **94.90 μs** |  **1.02** |    **0.21** |     **816 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 10.0 | Shallow | 5            |   307.21 μs |  51.437 μs |  57.172 μs |   287.50 μs |   234.00 μs |   436.80 μs |  4.02 |    0.94 |    8640 B |       10.59 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 10.0 | Shallow | 5            |   286.23 μs |  26.877 μs |  28.758 μs |   279.20 μs |   244.15 μs |   359.55 μs |  3.74 |    0.65 |    8640 B |       10.59 |
|                                                                  |           |         |              |             |            |            |             |             |             |       |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                            | .NET 9.0  | Shallow | 5            |    62.27 μs |  13.589 μs |  15.649 μs |    54.55 μs |    45.90 μs |    96.80 μs |  1.05 |    0.35 |     816 B |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 9.0  | Shallow | 5            |   293.16 μs |  33.046 μs |  36.730 μs |   297.20 μs |   232.80 μs |   373.90 μs |  4.97 |    1.24 |    8608 B |       10.55 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 9.0  | Shallow | 5            |   358.86 μs |  63.221 μs |  70.270 μs |   349.70 μs |   255.20 μs |   513.70 μs |  6.08 |    1.77 |    8608 B |       10.55 |
|                                                                  |           |         |              |             |            |            |             |             |             |       |         |           |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                            | **.NET 10.0** | **Shallow** | **500**          |    **94.56 μs** |  **16.100 μs** |  **18.541 μs** |    **88.85 μs** |    **66.25 μs** |   **134.95 μs** |  **1.03** |    **0.27** |     **832 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 10.0 | Shallow | 500          |   936.54 μs |  53.076 μs |  58.994 μs |   930.40 μs |   837.10 μs | 1,078.50 μs | 10.25 |    1.96 |  539384 B |      648.30 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 10.0 | Shallow | 500          |   972.82 μs |  54.922 μs |  61.045 μs |   973.30 μs |   858.70 μs | 1,087.90 μs | 10.65 |    2.03 |  539384 B |      648.30 |
|                                                                  |           |         |              |             |            |            |             |             |             |       |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                            | .NET 9.0  | Shallow | 500          |    77.13 μs |   9.132 μs |   9.771 μs |    82.00 μs |    55.70 μs |    87.10 μs |  1.02 |    0.19 |     832 B |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 9.0  | Shallow | 500          |   864.16 μs |  54.557 μs |  60.640 μs |   848.90 μs |   780.10 μs |   999.20 μs | 11.41 |    1.83 |  539352 B |      648.26 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 9.0  | Shallow | 500          |   898.28 μs |  56.787 μs |  63.119 μs |   899.40 μs |   787.90 μs | 1,038.30 μs | 11.86 |    1.90 |  539352 B |      648.26 |
|                                                                  |           |         |              |             |            |            |             |             |             |       |         |           |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                            | **.NET 10.0** | **Deep**    | **5**            |   **101.22 μs** |  **17.633 μs** |  **19.599 μs** |    **99.75 μs** |    **62.85 μs** |   **131.55 μs** |  **1.04** |    **0.30** |     **912 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 10.0 | Deep    | 5            |   627.11 μs |  68.214 μs |  75.819 μs |   638.30 μs |   504.10 μs |   756.60 μs |  6.44 |    1.55 |   21440 B |       23.51 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 10.0 | Deep    | 5            |   606.24 μs |  77.086 μs |  82.481 μs |   595.45 μs |   507.90 μs |   793.00 μs |  6.23 |    1.55 |   21440 B |       23.51 |
|                                                                  |           |         |              |             |            |            |             |             |             |       |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                            | .NET 9.0  | Deep    | 5            |   104.89 μs |  18.083 μs |  19.349 μs |   104.30 μs |    70.40 μs |   135.40 μs |  1.03 |    0.27 |     912 B |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 9.0  | Deep    | 5            |   548.66 μs |  41.953 μs |  44.890 μs |   548.60 μs |   448.40 μs |   642.80 μs |  5.41 |    1.12 |   21360 B |       23.42 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 9.0  | Deep    | 5            |   644.29 μs |  80.436 μs |  92.630 μs |   631.80 μs |   503.00 μs |   856.30 μs |  6.35 |    1.51 |   21360 B |       23.42 |
|                                                                  |           |         |              |             |            |            |             |             |             |       |         |           |             |
| **&#39;PhysicalFileProvider - exact casing&#39;**                            | **.NET 10.0** | **Deep**    | **500**          |    **71.87 μs** |   **7.633 μs** |   **7.839 μs** |    **72.20 μs** |    **57.80 μs** |    **84.80 μs** |  **1.01** |    **0.16** |     **912 B** |        **1.00** |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 10.0 | Deep    | 500          | 2,077.64 μs |  82.356 μs |  84.573 μs | 2,083.40 μs | 1,898.40 μs | 2,227.90 μs | 29.25 |    3.44 | 1391784 B |    1,526.08 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 10.0 | Deep    | 500          | 2,103.57 μs |  65.254 μs |  72.530 μs | 2,118.40 μs | 1,984.00 μs | 2,232.10 μs | 29.61 |    3.42 | 1391784 B |    1,526.08 |
|                                                                  |           |         |              |             |            |            |             |             |             |       |         |           |             |
| &#39;PhysicalFileProvider - exact casing&#39;                            | .NET 9.0  | Deep    | 500          |    57.86 μs |   9.326 μs |  10.366 μs |    55.40 μs |    44.30 μs |    80.10 μs |  1.03 |    0.24 |     912 B |        1.00 |
| &#39;PortablePhysicalFileProvider - exact casing (cold resolution)&#39;  | .NET 9.0  | Deep    | 500          | 2,289.69 μs | 268.347 μs | 298.267 μs | 2,180.30 μs | 1,972.10 μs | 3,002.40 μs | 40.68 |    8.30 | 1391704 B |    1,525.99 |
| &#39;PortablePhysicalFileProvider - varied casing (cold resolution)&#39; | .NET 9.0  | Deep    | 500          | 2,067.39 μs |  73.665 μs |  75.649 μs | 2,071.80 μs | 1,926.50 μs | 2,181.60 μs | 36.73 |    5.97 | 1391704 B |    1,525.99 |
