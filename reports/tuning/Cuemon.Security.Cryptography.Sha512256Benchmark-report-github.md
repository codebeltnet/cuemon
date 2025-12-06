```

BenchmarkDotNet v0.15.6, Windows 11 (10.0.26200.7296)
12th Gen Intel Core i9-12900KF 3.20GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 10.0.100
  [Host]    : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.11 (9.0.11, 9.0.1125.51716), X64 RyuJIT x86-64-v3

PowerPlanMode=00000000-0000-0000-0000-000000000000  IterationTime=250ms  MaxIterationCount=20  
MinIterationCount=15  WarmupCount=1  

```
| Method                                                             | Job       | Runtime   | Variant          | Mean           | Error        | StdDev       | Median         | Min            | Max            | Ratio    | RatioSD  | Gen0   | Allocated | Alloc Ratio |
|------------------------------------------------------------------- |---------- |---------- |----------------- |---------------:|-------------:|-------------:|---------------:|---------------:|---------------:|---------:|---------:|-------:|----------:|------------:|
| **&#39;Custom SHA-512/256 — small (64 bytes)&#39;**                            | **.NET 10.0** | **.NET 10.0** | **CustomSHA512_256** |       **396.9 ns** |      **8.53 ns** |      **9.82 ns** |       **393.4 ns** |       **383.9 ns** |       **417.4 ns** |     **0.88** |     **0.20** | **0.0704** |    **1104 B** |        **1.00** |
| &#39;Custom SHA-512/256 — large (1 MB)&#39;                                | .NET 10.0 | .NET 10.0 | CustomSHA512_256 | 2,215,370.3 ns | 30,618.15 ns | 27,142.19 ns | 2,200,658.2 ns | 2,187,930.5 ns | 2,263,451.6 ns | 4,910.01 | 1,093.90 |      - |    1104 B |        1.00 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — small (64 bytes)&#39;             | .NET 10.0 | .NET 10.0 | CustomSHA512_256 |       441.2 ns |      8.58 ns |      8.42 ns |       438.1 ns |       432.7 ns |       454.7 ns |     0.98 |     0.22 | 0.0233 |     368 B |        0.33 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — large (1 MB)&#39;                 | .NET 10.0 | .NET 10.0 | CustomSHA512_256 | 1,346,672.3 ns | 12,525.60 ns | 11,103.62 ns | 1,344,033.3 ns | 1,335,707.8 ns | 1,371,581.2 ns | 2,984.68 |   664.42 |      - |     369 B |        0.33 |
| &#39;Param-based: ComputeHash (selects algorithm by [Params] Variant)&#39; | .NET 10.0 | .NET 10.0 | CustomSHA512_256 |       552.2 ns |    111.87 ns |    128.83 ns |       634.6 ns |       380.5 ns |       687.4 ns |     1.22 |     0.39 | 0.0696 |    1104 B |        1.00 |
| &#39;Custom SHA-512/256 — small (64 bytes)&#39;                            | .NET 9.0  | .NET 9.0  | CustomSHA512_256 |       479.0 ns |    112.17 ns |    129.18 ns |       400.1 ns |       373.3 ns |       691.4 ns |     1.06 |     0.37 | 0.0695 |    1104 B |        1.00 |
| &#39;Custom SHA-512/256 — large (1 MB)&#39;                                | .NET 9.0  | .NET 9.0  | CustomSHA512_256 | 2,236,045.5 ns | 10,136.27 ns |  9,481.47 ns | 2,232,607.1 ns | 2,224,303.6 ns | 2,255,956.2 ns | 4,955.83 | 1,102.54 |      - |    1104 B |        1.00 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — small (64 bytes)&#39;             | .NET 9.0  | .NET 9.0  | CustomSHA512_256 |       441.4 ns |      9.21 ns |     10.23 ns |       440.7 ns |       422.6 ns |       460.3 ns |     0.98 |     0.22 | 0.0229 |     368 B |        0.33 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — large (1 MB)&#39;                 | .NET 9.0  | .NET 9.0  | CustomSHA512_256 | 1,342,722.2 ns | 16,476.57 ns | 15,412.19 ns | 1,341,708.3 ns | 1,321,103.1 ns | 1,367,723.4 ns | 2,975.92 |   662.81 |      - |     369 B |        0.33 |
| &#39;Param-based: ComputeHash (selects algorithm by [Params] Variant)&#39; | .NET 9.0  | .NET 9.0  | CustomSHA512_256 |       400.3 ns |      5.96 ns |      5.58 ns |       400.3 ns |       391.5 ns |       412.5 ns |     0.89 |     0.20 | 0.0702 |    1104 B |        1.00 |
|                                                                    |           |           |                  |                |              |              |                |                |                |          |          |        |           |             |
| **&#39;Custom SHA-512/256 — small (64 bytes)&#39;**                            | **.NET 10.0** | **.NET 10.0** | **SHA512_Truncated** |       **371.4 ns** |     **23.52 ns** |     **27.08 ns** |       **359.7 ns** |       **345.1 ns** |       **425.9 ns** |     **1.02** |     **0.07** | **0.0701** |    **1104 B** |        **1.00** |
| &#39;Custom SHA-512/256 — large (1 MB)&#39;                                | .NET 10.0 | .NET 10.0 | SHA512_Truncated | 2,208,794.2 ns | 30,979.07 ns | 28,977.85 ns | 2,208,998.2 ns | 2,148,959.8 ns | 2,249,233.9 ns | 6,051.29 |   120.02 |      - |    1104 B |        1.00 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — small (64 bytes)&#39;             | .NET 10.0 | .NET 10.0 | SHA512_Truncated |       427.5 ns |      8.48 ns |      9.08 ns |       424.4 ns |       418.1 ns |       449.6 ns |     1.17 |     0.03 | 0.0224 |     368 B |        0.33 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — large (1 MB)&#39;                 | .NET 10.0 | .NET 10.0 | SHA512_Truncated | 1,343,433.9 ns | 18,287.12 ns | 17,105.78 ns | 1,342,228.6 ns | 1,315,239.6 ns | 1,371,437.5 ns | 3,680.52 |    72.13 |      - |     369 B |        0.33 |
| &#39;Param-based: ComputeHash (selects algorithm by [Params] Variant)&#39; | .NET 10.0 | .NET 10.0 | SHA512_Truncated |       418.4 ns |      7.87 ns |      6.97 ns |       417.8 ns |       407.8 ns |       434.0 ns |     1.15 |     0.03 | 0.0231 |     368 B |        0.33 |
| &#39;Custom SHA-512/256 — small (64 bytes)&#39;                            | .NET 9.0  | .NET 9.0  | SHA512_Truncated |       365.1 ns |      5.89 ns |      5.78 ns |       364.6 ns |       358.5 ns |       374.9 ns |     1.00 |     0.02 | 0.0693 |    1104 B |        1.00 |
| &#39;Custom SHA-512/256 — large (1 MB)&#39;                                | .NET 9.0  | .NET 9.0  | SHA512_Truncated | 2,159,919.2 ns | 27,489.49 ns | 24,368.72 ns | 2,152,612.9 ns | 2,132,135.2 ns | 2,197,020.3 ns | 5,917.39 |   110.84 |      - |    1104 B |        1.00 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — small (64 bytes)&#39;             | .NET 9.0  | .NET 9.0  | SHA512_Truncated |       443.3 ns |      8.29 ns |      7.75 ns |       442.5 ns |       432.7 ns |       461.5 ns |     1.21 |     0.03 | 0.0233 |     368 B |        0.33 |
| &#39;Built-in SHA-512 truncated -&gt; 256 — large (1 MB)&#39;                 | .NET 9.0  | .NET 9.0  | SHA512_Truncated | 1,352,995.6 ns | 26,185.39 ns | 25,717.56 ns | 1,353,562.5 ns | 1,312,020.6 ns | 1,403,619.0 ns | 3,706.71 |    88.66 |      - |     369 B |        0.33 |
| &#39;Param-based: ComputeHash (selects algorithm by [Params] Variant)&#39; | .NET 9.0  | .NET 9.0  | SHA512_Truncated |       447.2 ns |      8.90 ns |      8.33 ns |       445.7 ns |       436.6 ns |       466.0 ns |     1.23 |     0.03 | 0.0231 |     368 B |        0.33 |
