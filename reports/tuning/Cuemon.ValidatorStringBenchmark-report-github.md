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
| Method                                              | Runtime   | StringLength | Mean          | Error      | StdDev     | Median        | Min           | Max           | Ratio     | RatioSD  | Allocated | Alloc Ratio |
|---------------------------------------------------- |---------- |------------- |--------------:|-----------:|-----------:|--------------:|--------------:|--------------:|----------:|---------:|----------:|------------:|
| **&#39;ThrowIfNullOrWhitespace - text&#39;**                    | **.NET 10.0** | **16**           |     **0.2090 ns** |  **0.0081 ns** |  **0.0072 ns** |     **0.2100 ns** |     **0.1960 ns** |     **0.2232 ns** |      **1.00** |     **0.05** |         **-** |          **NA** |
| &#39;ThrowIfContainsAny - no match (Ordinal)&#39;           | .NET 10.0 | 16           |     3.8545 ns |  0.0391 ns |  0.0366 ns |     3.8639 ns |     3.7861 ns |     3.9058 ns |     18.46 |     0.64 |         - |          NA |
| &#39;ThrowIfContainsAny - no match (OrdinalIgnoreCase)&#39; | .NET 10.0 | 16           |     5.7693 ns |  0.0624 ns |  0.0584 ns |     5.7478 ns |     5.6901 ns |     5.8837 ns |     27.63 |     0.96 |         - |          NA |
| &#39;ThrowIfDifferent - equivalent values&#39;              | .NET 10.0 | 16           |    30.6741 ns |  0.1957 ns |  0.1831 ns |    30.7397 ns |    30.3970 ns |    30.9082 ns |    146.92 |     4.96 |         - |          NA |
| &#39;ThrowIfNotHex - hexadecimal text&#39;                  | .NET 10.0 | 16           |     6.0493 ns |  0.0747 ns |  0.0624 ns |     6.0375 ns |     5.9675 ns |     6.1874 ns |     28.98 |     1.01 |         - |          NA |
| &#39;ThrowIfNotBase64String - base-64 text&#39;             | .NET 10.0 | 16           |     3.1983 ns |  0.3948 ns |  0.4388 ns |     2.8952 ns |     2.8690 ns |     4.3662 ns |     15.32 |     2.11 |         - |          NA |
|                                                     |           |              |               |            |            |               |               |               |           |          |           |             |
| &#39;ThrowIfNullOrWhitespace - text&#39;                    | .NET 9.0  | 16           |     1.2279 ns |  0.0057 ns |  0.0051 ns |     1.2276 ns |     1.2176 ns |     1.2390 ns |      1.00 |     0.01 |         - |          NA |
| &#39;ThrowIfContainsAny - no match (Ordinal)&#39;           | .NET 9.0  | 16           |     5.1475 ns |  0.0380 ns |  0.0337 ns |     5.1422 ns |     5.1090 ns |     5.2131 ns |      4.19 |     0.03 |         - |          NA |
| &#39;ThrowIfContainsAny - no match (OrdinalIgnoreCase)&#39; | .NET 9.0  | 16           |    15.4126 ns |  0.0755 ns |  0.0630 ns |    15.4270 ns |    15.3050 ns |    15.5037 ns |     12.55 |     0.07 |         - |          NA |
| &#39;ThrowIfDifferent - equivalent values&#39;              | .NET 9.0  | 16           |    30.0680 ns |  0.0687 ns |  0.0574 ns |    30.0808 ns |    29.9612 ns |    30.1483 ns |     24.49 |     0.11 |         - |          NA |
| &#39;ThrowIfNotHex - hexadecimal text&#39;                  | .NET 9.0  | 16           |     6.7588 ns |  0.0837 ns |  0.0699 ns |     6.7307 ns |     6.6753 ns |     6.9089 ns |      5.50 |     0.06 |         - |          NA |
| &#39;ThrowIfNotBase64String - base-64 text&#39;             | .NET 9.0  | 16           |     2.4632 ns |  0.0167 ns |  0.0139 ns |     2.4589 ns |     2.4454 ns |     2.4944 ns |      2.01 |     0.01 |         - |          NA |
|                                                     |           |              |               |            |            |               |               |               |           |          |           |             |
| **&#39;ThrowIfNullOrWhitespace - text&#39;**                    | **.NET 10.0** | **256**          |     **0.2053 ns** |  **0.0164 ns** |  **0.0137 ns** |     **0.1992 ns** |     **0.1854 ns** |     **0.2346 ns** |      **1.00** |     **0.09** |         **-** |          **NA** |
| &#39;ThrowIfContainsAny - no match (Ordinal)&#39;           | .NET 10.0 | 256          |    14.7533 ns |  0.2636 ns |  0.2589 ns |    14.6884 ns |    14.4309 ns |    15.4377 ns |     72.15 |     4.67 |         - |          NA |
| &#39;ThrowIfContainsAny - no match (OrdinalIgnoreCase)&#39; | .NET 10.0 | 256          |    15.8052 ns |  0.0953 ns |  0.0891 ns |    15.8130 ns |    15.6791 ns |    15.9556 ns |     77.29 |     4.85 |         - |          NA |
| &#39;ThrowIfDifferent - equivalent values&#39;              | .NET 10.0 | 256          |   500.6127 ns |  5.2231 ns |  4.6301 ns |   500.0484 ns |   492.8398 ns |   508.6385 ns |  2,448.09 |   154.60 |         - |          NA |
| &#39;ThrowIfNotHex - hexadecimal text&#39;                  | .NET 10.0 | 256          |    96.6366 ns |  0.6012 ns |  0.4694 ns |    96.6694 ns |    95.6843 ns |    97.5523 ns |    472.57 |    29.64 |         - |          NA |
| &#39;ThrowIfNotBase64String - base-64 text&#39;             | .NET 10.0 | 256          |     7.1094 ns |  0.0359 ns |  0.0336 ns |     7.1076 ns |     7.0179 ns |     7.1504 ns |     34.77 |     2.18 |         - |          NA |
|                                                     |           |              |               |            |            |               |               |               |           |          |           |             |
| &#39;ThrowIfNullOrWhitespace - text&#39;                    | .NET 9.0  | 256          |     1.2236 ns |  0.0075 ns |  0.0066 ns |     1.2241 ns |     1.2050 ns |     1.2329 ns |      1.00 |     0.01 |         - |          NA |
| &#39;ThrowIfContainsAny - no match (Ordinal)&#39;           | .NET 9.0  | 256          |    13.0390 ns |  0.0984 ns |  0.0872 ns |    13.0467 ns |    12.8598 ns |    13.1827 ns |     10.66 |     0.09 |         - |          NA |
| &#39;ThrowIfContainsAny - no match (OrdinalIgnoreCase)&#39; | .NET 9.0  | 256          |    24.4683 ns |  0.0937 ns |  0.0877 ns |    24.4806 ns |    24.3493 ns |    24.6330 ns |     20.00 |     0.13 |         - |          NA |
| &#39;ThrowIfDifferent - equivalent values&#39;              | .NET 9.0  | 256          |   495.6196 ns |  1.0700 ns |  0.8935 ns |   495.6281 ns |   493.9982 ns |   497.3675 ns |    405.05 |     2.25 |         - |          NA |
| &#39;ThrowIfNotHex - hexadecimal text&#39;                  | .NET 9.0  | 256          |    97.6221 ns |  0.3138 ns |  0.2935 ns |    97.5774 ns |    96.9968 ns |    98.2066 ns |     79.78 |     0.48 |         - |          NA |
| &#39;ThrowIfNotBase64String - base-64 text&#39;             | .NET 9.0  | 256          |     6.7426 ns |  0.0442 ns |  0.0413 ns |     6.7452 ns |     6.6333 ns |     6.7896 ns |      5.51 |     0.04 |         - |          NA |
|                                                     |           |              |               |            |            |               |               |               |           |          |           |             |
| **&#39;ThrowIfNullOrWhitespace - text&#39;**                    | **.NET 10.0** | **4096**         |     **0.2014 ns** |  **0.0068 ns** |  **0.0057 ns** |     **0.2027 ns** |     **0.1909 ns** |     **0.2080 ns** |      **1.00** |     **0.04** |         **-** |          **NA** |
| &#39;ThrowIfContainsAny - no match (Ordinal)&#39;           | .NET 10.0 | 4096         |   178.7936 ns |  2.9975 ns |  2.5031 ns |   177.3768 ns |   176.4823 ns |   184.2122 ns |    888.56 |    27.30 |         - |          NA |
| &#39;ThrowIfContainsAny - no match (OrdinalIgnoreCase)&#39; | .NET 10.0 | 4096         |   202.2872 ns |  0.6133 ns |  0.5122 ns |   202.3260 ns |   201.3919 ns |   203.2353 ns |  1,005.32 |    27.86 |         - |          NA |
| &#39;ThrowIfDifferent - equivalent values&#39;              | .NET 10.0 | 4096         | 7,904.7501 ns | 85.8306 ns | 76.0866 ns | 7,892.1977 ns | 7,757.0875 ns | 8,078.8161 ns | 39,284.79 | 1,144.04 |         - |          NA |
| &#39;ThrowIfNotHex - hexadecimal text&#39;                  | .NET 10.0 | 4096         | 1,366.0532 ns |  5.3093 ns |  4.9663 ns | 1,367.1497 ns | 1,355.2667 ns | 1,372.5650 ns |  6,788.97 |   188.83 |         - |          NA |
| &#39;ThrowIfNotBase64String - base-64 text&#39;             | .NET 10.0 | 4096         |    97.6195 ns |  0.6136 ns |  0.5124 ns |    97.8061 ns |    96.5087 ns |    98.1168 ns |    485.15 |    13.61 |         - |          NA |
|                                                     |           |              |               |            |            |               |               |               |           |          |           |             |
| &#39;ThrowIfNullOrWhitespace - text&#39;                    | .NET 9.0  | 4096         |     1.2299 ns |  0.0121 ns |  0.0113 ns |     1.2321 ns |     1.2063 ns |     1.2470 ns |      1.00 |     0.01 |         - |          NA |
| &#39;ThrowIfContainsAny - no match (Ordinal)&#39;           | .NET 9.0  | 4096         |   182.8157 ns |  0.2953 ns |  0.2466 ns |   182.7091 ns |   182.5392 ns |   183.3806 ns |    148.65 |     1.34 |         - |          NA |
| &#39;ThrowIfContainsAny - no match (OrdinalIgnoreCase)&#39; | .NET 9.0  | 4096         |   214.6854 ns |  1.0387 ns |  0.9716 ns |   214.3181 ns |   213.0358 ns |   216.5681 ns |    174.56 |     1.74 |         - |          NA |
| &#39;ThrowIfDifferent - equivalent values&#39;              | .NET 9.0  | 4096         | 7,817.4053 ns | 17.8227 ns | 16.6714 ns | 7,812.4374 ns | 7,787.5782 ns | 7,845.9189 ns |  6,356.43 |    58.25 |         - |          NA |
| &#39;ThrowIfNotHex - hexadecimal text&#39;                  | .NET 9.0  | 4096         | 1,365.1147 ns |  4.8334 ns |  4.5211 ns | 1,366.0036 ns | 1,357.2074 ns | 1,373.2712 ns |  1,109.99 |    10.53 |         - |          NA |
| &#39;ThrowIfNotBase64String - base-64 text&#39;             | .NET 9.0  | 4096         |    95.5167 ns |  0.2984 ns |  0.2492 ns |    95.4892 ns |    95.0800 ns |    96.0121 ns |     77.67 |     0.72 |         - |          NA |
