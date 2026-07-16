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
| Method        | Runtime   | Length | Scenario       | Mean         | Error        | StdDev        | Median       | Min          | Max          | Gen0   | Gen1   | Allocated |
|-------------- |---------- |------- |--------------- |-------------:|-------------:|--------------:|-------------:|-------------:|-------------:|-------:|-------:|----------:|
| **HasDifference** | **.NET 10.0** | **16**     | **Equivalent**     |     **29.63 ns** |     **0.055 ns** |      **0.049 ns** |     **29.62 ns** |     **29.58 ns** |     **29.75 ns** |      **-** |      **-** |         **-** |
| HasDifference | .NET 9.0  | 16     | Equivalent     |     30.14 ns |     0.628 ns |      0.587 ns |     29.91 ns |     29.51 ns |     31.38 ns |      - |      - |         - |
| **HasDifference** | **.NET 10.0** | **16**     | **Reordered**      |     **29.96 ns** |     **0.399 ns** |      **0.333 ns** |     **29.79 ns** |     **29.68 ns** |     **30.76 ns** |      **-** |      **-** |         **-** |
| HasDifference | .NET 9.0  | 16     | Reordered      |     30.48 ns |     0.635 ns |      0.594 ns |     30.31 ns |     29.84 ns |     32.19 ns |      - |      - |         - |
| **HasDifference** | **.NET 10.0** | **16**     | **DuplicateHeavy** |     **11.18 ns** |     **0.247 ns** |      **0.243 ns** |     **11.14 ns** |     **10.95 ns** |     **11.98 ns** |      **-** |      **-** |         **-** |
| HasDifference | .NET 9.0  | 16     | DuplicateHeavy |     10.66 ns |     0.259 ns |      0.254 ns |     10.58 ns |     10.44 ns |     11.44 ns |      - |      - |         - |
| **HasDifference** | **.NET 10.0** | **16**     | **DiffAtStart**    |     **61.62 ns** |     **1.039 ns** |      **0.921 ns** |     **61.41 ns** |     **60.46 ns** |     **63.45 ns** | **0.0188** |      **-** |     **296 B** |
| HasDifference | .NET 9.0  | 16     | DiffAtStart    |     61.34 ns |     1.200 ns |      1.179 ns |     61.35 ns |     58.09 ns |     63.73 ns | 0.0187 |      - |     296 B |
| **HasDifference** | **.NET 10.0** | **16**     | **DiffAtMiddle**   |     **61.69 ns** |     **1.016 ns** |      **0.848 ns** |     **61.79 ns** |     **60.65 ns** |     **63.73 ns** | **0.0187** |      **-** |     **296 B** |
| HasDifference | .NET 9.0  | 16     | DiffAtMiddle   |     62.27 ns |     0.573 ns |      0.508 ns |     62.32 ns |     61.41 ns |     63.20 ns | 0.0187 |      - |     296 B |
| **HasDifference** | **.NET 10.0** | **16**     | **DiffAtEnd**      |     **63.34 ns** |     **0.546 ns** |      **0.510 ns** |     **63.33 ns** |     **62.66 ns** |     **64.48 ns** | **0.0188** |      **-** |     **296 B** |
| HasDifference | .NET 9.0  | 16     | DiffAtEnd      |     64.73 ns |     0.577 ns |      0.512 ns |     64.82 ns |     63.69 ns |     65.71 ns | 0.0187 |      - |     296 B |
| **HasDifference** | **.NET 10.0** | **16**     | **MostlyUnique**   |    **164.26 ns** |     **1.730 ns** |      **1.444 ns** |    **164.02 ns** |    **162.65 ns** |    **166.62 ns** | **0.0421** |      **-** |     **664 B** |
| HasDifference | .NET 9.0  | 16     | MostlyUnique   |    185.11 ns |     1.522 ns |      1.349 ns |    185.00 ns |    182.65 ns |    187.78 ns | 0.0423 |      - |     664 B |
| **HasDifference** | **.NET 10.0** | **256**    | **Equivalent**     |    **493.56 ns** |     **0.601 ns** |      **0.469 ns** |    **493.65 ns** |    **492.78 ns** |    **494.09 ns** |      **-** |      **-** |         **-** |
| HasDifference | .NET 9.0  | 256    | Equivalent     |    492.48 ns |     0.374 ns |      0.292 ns |    492.47 ns |    491.87 ns |    492.87 ns |      - |      - |         - |
| **HasDifference** | **.NET 10.0** | **256**    | **Reordered**      |    **494.48 ns** |     **0.519 ns** |      **0.460 ns** |    **494.40 ns** |    **493.89 ns** |    **495.18 ns** |      **-** |      **-** |         **-** |
| HasDifference | .NET 9.0  | 256    | Reordered      |    493.12 ns |     1.425 ns |      1.263 ns |    492.62 ns |    491.60 ns |    495.17 ns |      - |      - |         - |
| **HasDifference** | **.NET 10.0** | **256**    | **DuplicateHeavy** |    **124.25 ns** |     **2.342 ns** |      **2.405 ns** |    **124.57 ns** |    **120.35 ns** |    **129.00 ns** |      **-** |      **-** |         **-** |
| HasDifference | .NET 9.0  | 256    | DuplicateHeavy |    118.22 ns |     0.355 ns |      0.332 ns |    118.25 ns |    117.68 ns |    118.74 ns |      - |      - |         - |
| **HasDifference** | **.NET 10.0** | **256**    | **DiffAtStart**    |    **537.08 ns** |     **1.568 ns** |      **1.309 ns** |    **536.75 ns** |    **535.49 ns** |    **540.36 ns** | **0.0173** |      **-** |     **296 B** |
| HasDifference | .NET 9.0  | 256    | DiffAtStart    |    520.63 ns |     1.161 ns |      1.086 ns |    520.65 ns |    519.10 ns |    522.25 ns | 0.0189 |      - |     296 B |
| **HasDifference** | **.NET 10.0** | **256**    | **DiffAtMiddle**   |    **596.67 ns** |     **4.290 ns** |      **4.013 ns** |    **597.40 ns** |    **587.90 ns** |    **602.00 ns** | **0.0168** |      **-** |     **296 B** |
| HasDifference | .NET 9.0  | 256    | DiffAtMiddle   |    545.56 ns |     3.571 ns |      3.340 ns |    545.36 ns |    539.00 ns |    550.91 ns | 0.0176 |      - |     296 B |
| **HasDifference** | **.NET 10.0** | **256**    | **DiffAtEnd**      |    **535.93 ns** |     **6.062 ns** |      **5.671 ns** |    **537.03 ns** |    **526.99 ns** |    **545.36 ns** | **0.0172** |      **-** |     **296 B** |
| HasDifference | .NET 9.0  | 256    | DiffAtEnd      |    530.36 ns |     3.650 ns |      3.236 ns |    530.90 ns |    524.94 ns |    536.12 ns | 0.0171 |      - |     296 B |
| **HasDifference** | **.NET 10.0** | **256**    | **MostlyUnique**   |  **2,684.94 ns** |    **40.869 ns** |     **38.229 ns** |  **2,684.82 ns** |  **2,630.73 ns** |  **2,745.38 ns** | **0.8160** | **0.0215** |   **12952 B** |
| HasDifference | .NET 9.0  | 256    | MostlyUnique   |  2,817.49 ns |    34.840 ns |     32.589 ns |  2,830.46 ns |  2,758.76 ns |  2,865.28 ns | 0.8212 | 0.0228 |   12952 B |
| **HasDifference** | **.NET 10.0** | **4096**   | **Equivalent**     |  **7,876.05 ns** |    **16.417 ns** |     **13.709 ns** |  **7,873.97 ns** |  **7,859.59 ns** |  **7,901.70 ns** |      **-** |      **-** |         **-** |
| HasDifference | .NET 9.0  | 4096   | Equivalent     |  7,856.98 ns |     8.910 ns |      7.440 ns |  7,857.84 ns |  7,847.71 ns |  7,871.61 ns |      - |      - |         - |
| **HasDifference** | **.NET 10.0** | **4096**   | **Reordered**      |  **7,871.06 ns** |     **8.013 ns** |      **6.692 ns** |  **7,872.37 ns** |  **7,861.78 ns** |  **7,880.96 ns** |      **-** |      **-** |         **-** |
| HasDifference | .NET 9.0  | 4096   | Reordered      |  7,855.42 ns |     6.838 ns |      6.062 ns |  7,854.88 ns |  7,846.31 ns |  7,866.68 ns |      - |      - |         - |
| **HasDifference** | **.NET 10.0** | **4096**   | **DuplicateHeavy** |  **1,714.30 ns** |     **4.995 ns** |      **4.428 ns** |  **1,714.63 ns** |  **1,707.74 ns** |  **1,723.22 ns** |      **-** |      **-** |         **-** |
| HasDifference | .NET 9.0  | 4096   | DuplicateHeavy |  1,697.37 ns |     4.575 ns |      4.055 ns |  1,698.92 ns |  1,691.05 ns |  1,702.80 ns |      - |      - |         - |
| **HasDifference** | **.NET 10.0** | **4096**   | **DiffAtStart**    |  **8,235.33 ns** |    **45.189 ns** |     **40.059 ns** |  **8,235.92 ns** |  **8,167.80 ns** |  **8,301.60 ns** |      **-** |      **-** |     **296 B** |
| HasDifference | .NET 9.0  | 4096   | DiffAtStart    |  7,898.97 ns |    18.350 ns |     15.323 ns |  7,901.69 ns |  7,871.24 ns |  7,927.67 ns |      - |      - |     296 B |
| **HasDifference** | **.NET 10.0** | **4096**   | **DiffAtMiddle**   |  **8,028.09 ns** |    **64.858 ns** |     **60.668 ns** |  **8,011.30 ns** |  **7,947.35 ns** |  **8,158.36 ns** |      **-** |      **-** |     **296 B** |
| HasDifference | .NET 9.0  | 4096   | DiffAtMiddle   |  7,904.49 ns |    12.562 ns |     11.136 ns |  7,906.29 ns |  7,876.43 ns |  7,924.96 ns |      - |      - |     296 B |
| **HasDifference** | **.NET 10.0** | **4096**   | **DiffAtEnd**      |  **7,911.52 ns** |    **23.094 ns** |     **20.473 ns** |  **7,906.20 ns** |  **7,891.26 ns** |  **7,954.35 ns** |      **-** |      **-** |     **296 B** |
| HasDifference | .NET 9.0  | 4096   | DiffAtEnd      |  7,918.53 ns |    16.922 ns |     14.130 ns |  7,915.00 ns |  7,900.43 ns |  7,948.04 ns |      - |      - |     296 B |
| **HasDifference** | **.NET 10.0** | **4096**   | **MostlyUnique**   | **30,124.44 ns** |   **597.038 ns** |    **558.470 ns** | **29,955.25 ns** | **29,446.96 ns** | **31,279.33 ns** | **7.7975** | **1.6795** |  **123504 B** |
| HasDifference | .NET 9.0  | 4096   | MostlyUnique   | 43,509.77 ns | 8,995.085 ns | 10,358.753 ns | 43,107.81 ns | 30,969.54 ns | 57,113.05 ns | 7.8125 | 1.6447 |  123504 B |
