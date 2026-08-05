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
| Method                               | Runtime   | scenario             | Mean       | Error      | StdDev     | Median     | Min        | Max        | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------------------- |---------- |--------------------- |-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|------:|--------:|----------:|------------:|
| **&#39;Cache hit (prime + lookup)&#39;**         | **.NET 10.0** | **?**                    |   **4.144 ms** |  **0.2804 ms** |  **0.3229 ms** |   **4.163 ms** |   **3.600 ms** |   **4.747 ms** |  **1.01** |    **0.11** |  **12.51 KB** |        **1.00** |
| &#39;Cache hit (varied case)&#39;            | .NET 10.0 | ?                    |   5.248 ms |  0.4290 ms |  0.4768 ms |   5.231 ms |   4.510 ms |   6.369 ms |  1.27 |    0.15 |  15.73 KB |        1.26 |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| &#39;Cache hit (prime + lookup)&#39;         | .NET 9.0  | ?                    |   4.167 ms |  0.2232 ms |  0.2480 ms |   4.151 ms |   3.811 ms |   4.802 ms |  1.00 |    0.08 |  12.45 KB |        1.00 |
| &#39;Cache hit (varied case)&#39;            | .NET 9.0  | ?                    |   4.520 ms |  0.2243 ms |  0.2493 ms |   4.500 ms |   4.138 ms |   4.986 ms |  1.09 |    0.08 |  15.66 KB |        1.26 |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| **&#39;Cache miss (deep {0})&#39;**              | **.NET 10.0** | **deep-2-segments**      |   **7.330 ms** |  **0.3883 ms** |  **0.4315 ms** |   **7.333 ms** |   **6.433 ms** |   **8.145 ms** |     **?** |       **?** |  **20.05 KB** |           **?** |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| &#39;Cache miss (deep {0})&#39;              | .NET 9.0  | deep-2-segments      |   7.908 ms |  0.7112 ms |  0.7905 ms |   7.666 ms |   6.998 ms |  10.126 ms |     ? |       ? |  19.98 KB |           ? |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| **&#39;Cache miss (deep {0})&#39;**              | **.NET 10.0** | **deep-3-segments**      |   **7.838 ms** |  **0.4741 ms** |  **0.4869 ms** |   **7.703 ms** |   **7.380 ms** |   **9.301 ms** |     **?** |       **?** |   **22.8 KB** |           **?** |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| &#39;Cache miss (deep {0})&#39;              | .NET 9.0  | deep-3-segments      |   7.301 ms |  0.4234 ms |  0.4706 ms |   7.174 ms |   6.693 ms |   8.288 ms |     ? |       ? |  22.72 KB |           ? |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| **&#39;Cache miss (deep {0})&#39;**              | **.NET 10.0** | **deep-5-segments**      |   **8.368 ms** |  **0.8745 ms** |  **0.9720 ms** |   **7.942 ms** |   **7.322 ms** |  **10.613 ms** |     **?** |       **?** |  **28.61 KB** |           **?** |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| &#39;Cache miss (deep {0})&#39;              | .NET 9.0  | deep-5-segments      |  13.940 ms |  0.8937 ms |  1.0292 ms |  13.836 ms |  12.312 ms |  16.019 ms |     ? |       ? |  28.49 KB |           ? |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| **&#39;Cache miss (shallow, {0} siblings)&#39;** | **.NET 10.0** | **shallow-5-siblings**   |   **4.332 ms** |  **0.2151 ms** |  **0.2301 ms** |   **4.386 ms** |   **3.933 ms** |   **4.838 ms** |     **?** |       **?** |  **12.03 KB** |           **?** |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| &#39;Cache miss (shallow, {0} siblings)&#39; | .NET 9.0  | shallow-5-siblings   |   4.328 ms |  0.1939 ms |  0.2155 ms |   4.336 ms |   3.955 ms |   4.657 ms |     ? |       ? |  11.98 KB |           ? |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| **&#39;Cache miss (shallow, {0} siblings)&#39;** | **.NET 10.0** | **shallow-50-siblings**  |  **29.290 ms** |  **0.8618 ms** |  **0.8850 ms** |  **29.265 ms** |  **27.921 ms** |  **30.844 ms** |     **?** |       **?** |  **61.91 KB** |           **?** |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| &#39;Cache miss (shallow, {0} siblings)&#39; | .NET 9.0  | shallow-50-siblings  |  29.552 ms |  1.0892 ms |  1.2543 ms |  29.235 ms |  27.636 ms |  32.824 ms |     ? |       ? |  63.03 KB |           ? |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| **&#39;Cache miss (shallow, {0} siblings)&#39;** | **.NET 10.0** | **shallow-500-siblings** | **313.986 ms** | **23.6054 ms** | **27.1840 ms** | **312.217 ms** | **279.042 ms** | **369.117 ms** |     **?** |       **?** | **561.13 KB** |           **?** |
|                                      |           |                      |            |            |            |            |            |            |       |         |           |             |
| &#39;Cache miss (shallow, {0} siblings)&#39; | .NET 9.0  | shallow-500-siblings | 277.705 ms | 22.7218 ms | 25.2552 ms | 266.627 ms | 237.723 ms | 325.514 ms |     ? |       ? | 561.08 KB |           ? |
