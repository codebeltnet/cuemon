```

BenchmarkDotNet v0.15.6, Windows 11 (10.0.26200.7462)
12th Gen Intel Core i9-12900KF 3.20GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 10.0.101
  [Host]     : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3
  Job-LDLMHG : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3
  Job-IOAYXE : .NET 9.0.11 (9.0.11, 9.0.1125.51716), X64 RyuJIT x86-64-v3

PowerPlanMode=00000000-0000-0000-0000-000000000000  IterationTime=250ms  MaxIterationCount=20  
MinIterationCount=15  WarmupCount=1  

```
| Method                                                             | Runtime   | Variant          | Mean           | Error         | StdDev        | Median         | Min            | Max            | Ratio    | RatioSD  | Gen0   | Allocated | Alloc Ratio |
|------------------------------------------------------------------- |---------- |----------------- |---------------:|--------------:|--------------:|---------------:|---------------:|---------------:|---------:|---------:|-------:|----------:|------------:|
| **&#39;Custom SHA-512/256 — small (64 bytes)&#39;**                            | **.NET 10.0** | **CustomSHA512_256** |       **584.7 ns** |      **71.22 ns** |      **82.02 ns** |       **622.8 ns** |       **369.8 ns** |       **633.9 ns** |     **1.03** |     **0.24** | **0.0700** |    **1104 B** |        **1.00** |
| &#39;Custom SHA-512/256 — large (1 MB)&#39;                                | .NET 10.0 | CustomSHA512_256 | 3,012,093.2 ns | 640,274.57 ns | 737,341.15 ns | 3,633,838.4 ns | 2,184,795.5 ns | 3,694,461.6 ns | 5,287.69 | 1,631.80 |      - |    1104 B |        1.00 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — small (64 bytes)&#39;             | .NET 10.0 | CustomSHA512_256 |       717.5 ns |      64.97 ns |      69.52 ns |       732.3 ns |       439.9 ns |       747.1 ns |     1.26 |     0.27 | 0.0228 |     368 B |        0.33 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — large (1 MB)&#39;                 | .NET 10.0 | CustomSHA512_256 | 1,822,488.4 ns | 391,107.58 ns | 450,400.07 ns | 2,119,414.1 ns | 1,321,180.2 ns | 2,282,040.6 ns | 3,199.35 |   993.21 |      - |     369 B |        0.33 |
| &#39;Param-based: ComputeHash (selects algorithm by [Params] Variant)&#39; | .NET 10.0 | CustomSHA512_256 |       574.9 ns |      81.87 ns |      94.28 ns |       618.0 ns |       360.2 ns |       628.0 ns |     1.01 |     0.25 | 0.0698 |    1104 B |        1.00 |
|                                                                    |           |                  |                |               |               |                |                |                |          |          |        |           |             |
| &#39;Custom SHA-512/256 — small (64 bytes)&#39;                            | .NET 9.0  | CustomSHA512_256 |       649.4 ns |      60.29 ns |      69.43 ns |       665.5 ns |       402.8 ns |       694.8 ns |     1.02 |     0.19 | 0.0704 |    1104 B |        1.00 |
| &#39;Custom SHA-512/256 — large (1 MB)&#39;                                | .NET 9.0  | CustomSHA512_256 | 3,157,187.5 ns | 737,751.94 ns | 849,596.23 ns | 3,741,927.2 ns | 2,184,308.9 ns | 3,989,937.5 ns | 4,938.82 | 1,508.34 |      - |    1104 B |        1.00 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — small (64 bytes)&#39;             | .NET 9.0  | CustomSHA512_256 |       726.6 ns |      14.55 ns |      13.61 ns |       727.5 ns |       687.2 ns |       743.0 ns |     1.14 |     0.17 | 0.0228 |     368 B |        0.33 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — large (1 MB)&#39;                 | .NET 9.0  | CustomSHA512_256 | 1,857,814.6 ns | 389,831.12 ns | 448,930.09 ns | 2,184,678.1 ns | 1,333,799.0 ns | 2,331,946.4 ns | 2,906.20 |   820.01 |      - |     369 B |        0.33 |
| &#39;Param-based: ComputeHash (selects algorithm by [Params] Variant)&#39; | .NET 9.0  | CustomSHA512_256 |       630.3 ns |      75.70 ns |      87.18 ns |       661.1 ns |       375.7 ns |       667.1 ns |     0.99 |     0.20 | 0.0699 |    1104 B |        1.00 |
|                                                                    |           |                  |                |               |               |                |                |                |          |          |        |           |             |
| **&#39;Custom SHA-512/256 — small (64 bytes)&#39;**                            | **.NET 10.0** | **SHA512_Truncated** |       **593.5 ns** |      **76.55 ns** |      **85.09 ns** |       **619.5 ns** |       **358.9 ns** |       **649.3 ns** |     **1.03** |     **0.26** | **0.0703** |    **1104 B** |        **1.00** |
| &#39;Custom SHA-512/256 — large (1 MB)&#39;                                | .NET 10.0 | SHA512_Truncated | 2,647,853.6 ns | 554,834.97 ns | 638,948.77 ns | 2,252,851.3 ns | 2,187,259.8 ns | 3,664,383.9 ns | 4,593.82 | 1,459.62 |      - |    1104 B |        1.00 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — small (64 bytes)&#39;             | .NET 10.0 | SHA512_Truncated |       442.3 ns |       8.17 ns |       7.24 ns |       439.1 ns |       434.3 ns |       457.1 ns |     0.77 |     0.16 | 0.0227 |     368 B |        0.33 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — large (1 MB)&#39;                 | .NET 10.0 | SHA512_Truncated | 1,345,192.4 ns |  16,030.80 ns |  14,210.88 ns | 1,340,056.5 ns | 1,328,410.4 ns | 1,367,594.8 ns | 2,333.80 |   485.43 |      - |     369 B |        0.33 |
| &#39;Param-based: ComputeHash (selects algorithm by [Params] Variant)&#39; | .NET 10.0 | SHA512_Truncated |       432.6 ns |       8.57 ns |       7.60 ns |       431.1 ns |       421.2 ns |       444.7 ns |     0.75 |     0.16 | 0.0227 |     368 B |        0.33 |
|                                                                    |           |                  |                |               |               |                |                |                |          |          |        |           |             |
| &#39;Custom SHA-512/256 — small (64 bytes)&#39;                            | .NET 9.0  | SHA512_Truncated |       392.7 ns |      12.65 ns |      14.06 ns |       389.8 ns |       370.9 ns |       427.4 ns |     1.00 |     0.05 | 0.0701 |    1104 B |        1.00 |
| &#39;Custom SHA-512/256 — large (1 MB)&#39;                                | .NET 9.0  | SHA512_Truncated | 2,245,427.1 ns |  20,433.14 ns |  18,113.45 ns | 2,247,822.8 ns | 2,221,083.9 ns | 2,281,519.6 ns | 5,724.90 |   200.49 |      - |    1104 B |        1.00 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — small (64 bytes)&#39;             | .NET 9.0  | SHA512_Truncated |       446.7 ns |       7.18 ns |       6.72 ns |       445.9 ns |       438.4 ns |       459.6 ns |     1.14 |     0.04 | 0.0230 |     368 B |        0.33 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — large (1 MB)&#39;                 | .NET 9.0  | SHA512_Truncated | 1,347,728.0 ns |  10,251.91 ns |   9,088.05 ns | 1,346,210.2 ns | 1,337,494.3 ns | 1,369,357.8 ns | 3,436.14 |   119.44 |      - |     369 B |        0.33 |
| &#39;Param-based: ComputeHash (selects algorithm by [Params] Variant)&#39; | .NET 9.0  | SHA512_Truncated |       442.9 ns |       6.39 ns |       5.66 ns |       442.8 ns |       436.8 ns |       454.1 ns |     1.13 |     0.04 | 0.0232 |     368 B |        0.33 |
