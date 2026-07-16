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
| Method                         | Runtime   | Mean        | Error     | StdDev    | Median      | Min         | Max         | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------- |---------- |------------:|----------:|----------:|------------:|------------:|------------:|------:|--------:|-------:|----------:|------------:|
| ThrowIfNotBase64String         | .NET 10.0 |   2.8934 ns | 0.0103 ns | 0.0092 ns |   2.8946 ns |   2.8770 ns |   2.9081 ns |     ? |       ? |      - |         - |           ? |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfNotBase64String         | .NET 9.0  |   2.6816 ns | 0.0212 ns | 0.0177 ns |   2.6844 ns |   2.6520 ns |   2.7142 ns |     ? |       ? |      - |         - |           ? |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfNotBinaryDigits         | .NET 10.0 |   2.6126 ns | 0.0193 ns | 0.0171 ns |   2.6095 ns |   2.5913 ns |   2.6493 ns |     ? |       ? |      - |         - |           ? |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfNotBinaryDigits         | .NET 9.0  |   2.8414 ns | 0.0182 ns | 0.0161 ns |   2.8443 ns |   2.8037 ns |   2.8622 ns |     ? |       ? |      - |         - |           ? |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfEmailAddress            | .NET 10.0 | 114.1496 ns | 1.0902 ns | 0.9104 ns | 114.6724 ns | 112.6790 ns | 115.1337 ns |  1.00 |    0.01 |      - |         - |          NA |
| ThrowIfNotEmailAddress         | .NET 10.0 |  99.5773 ns | 0.6011 ns | 0.5020 ns |  99.6990 ns |  98.1583 ns | 100.0400 ns |  0.87 |    0.01 |      - |         - |          NA |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfEmailAddress            | .NET 9.0  | 146.4382 ns | 0.4364 ns | 0.3868 ns | 146.4981 ns | 145.3997 ns | 147.0229 ns |  1.00 |    0.00 |      - |         - |          NA |
| ThrowIfNotEmailAddress         | .NET 9.0  | 114.6492 ns | 0.5217 ns | 0.4880 ns | 114.7519 ns | 113.6430 ns | 115.3961 ns |  0.78 |    0.00 |      - |         - |          NA |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfEnum                    | .NET 10.0 |  19.6780 ns | 0.3353 ns | 0.2972 ns |  19.6265 ns |  19.1935 ns |  20.2613 ns |  1.00 |    0.02 | 0.0030 |      48 B |        1.00 |
| ThrowIfNotEnum                 | .NET 10.0 |  62.9211 ns | 0.4876 ns | 0.4561 ns |  62.9361 ns |  61.9052 ns |  63.6236 ns |  3.20 |    0.05 | 0.0043 |      72 B |        1.50 |
| &#39;ThrowIfEnumType - type&#39;       | .NET 10.0 |   0.2194 ns | 0.0105 ns | 0.0098 ns |   0.2210 ns |   0.2061 ns |   0.2415 ns |  0.01 |    0.00 |      - |         - |        0.00 |
| &#39;ThrowIfEnumType - generic&#39;    | .NET 10.0 |   0.2260 ns | 0.0138 ns | 0.0129 ns |   0.2287 ns |   0.1955 ns |   0.2429 ns |  0.01 |    0.00 |      - |         - |        0.00 |
| &#39;ThrowIfNotEnumType - generic&#39; | .NET 10.0 |   0.2158 ns | 0.0112 ns | 0.0105 ns |   0.2126 ns |   0.1990 ns |   0.2313 ns |  0.01 |    0.00 |      - |         - |        0.00 |
| &#39;ThrowIfNotEnumType - type&#39;    | .NET 10.0 |   0.2162 ns | 0.0085 ns | 0.0066 ns |   0.2192 ns |   0.2031 ns |   0.2223 ns |  0.01 |    0.00 |      - |         - |        0.00 |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfEnum                    | .NET 9.0  |  23.5041 ns | 0.9247 ns | 1.0278 ns |  23.3222 ns |  22.2512 ns |  26.1332 ns |  1.00 |    0.06 | 0.0071 |     112 B |        1.00 |
| ThrowIfNotEnum                 | .NET 9.0  |  76.5258 ns | 1.0131 ns | 0.8981 ns |  76.5165 ns |  75.0281 ns |  78.3661 ns |  3.26 |    0.14 | 0.0085 |     136 B |        1.21 |
| &#39;ThrowIfEnumType - type&#39;       | .NET 9.0  |   1.0023 ns | 0.0088 ns | 0.0082 ns |   1.0064 ns |   0.9827 ns |   1.0132 ns |  0.04 |    0.00 |      - |         - |        0.00 |
| &#39;ThrowIfEnumType - generic&#39;    | .NET 9.0  |   0.8024 ns | 0.0088 ns | 0.0082 ns |   0.8044 ns |   0.7805 ns |   0.8125 ns |  0.03 |    0.00 |      - |         - |        0.00 |
| &#39;ThrowIfNotEnumType - generic&#39; | .NET 9.0  |   0.8115 ns | 0.0098 ns | 0.0087 ns |   0.8083 ns |   0.7958 ns |   0.8278 ns |  0.03 |    0.00 |      - |         - |        0.00 |
| &#39;ThrowIfNotEnumType - type&#39;    | .NET 9.0  |   1.0126 ns | 0.0178 ns | 0.0167 ns |   1.0111 ns |   0.9828 ns |   1.0424 ns |  0.04 |    0.00 |      - |         - |        0.00 |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfGuid                    | .NET 10.0 |   2.5814 ns | 0.0263 ns | 0.0246 ns |   2.5715 ns |   2.5380 ns |   2.6331 ns |  1.00 |    0.01 |      - |         - |          NA |
| ThrowIfNotGuid                 | .NET 10.0 |  13.0563 ns | 0.1379 ns | 0.1289 ns |  13.0234 ns |  12.8819 ns |  13.2565 ns |  5.06 |    0.07 |      - |         - |          NA |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfGuid                    | .NET 9.0  |   3.5593 ns | 0.0419 ns | 0.0350 ns |   3.5514 ns |   3.5149 ns |   3.6471 ns |  1.00 |    0.01 |      - |         - |          NA |
| ThrowIfNotGuid                 | .NET 9.0  |  16.4935 ns | 0.1087 ns | 0.0964 ns |  16.4600 ns |  16.3548 ns |  16.6890 ns |  4.63 |    0.05 |      - |         - |          NA |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfHex                     | .NET 10.0 |   0.6292 ns | 0.0106 ns | 0.0100 ns |   0.6292 ns |   0.6012 ns |   0.6431 ns |  1.00 |    0.02 |      - |         - |          NA |
| ThrowIfNotHex                  | .NET 10.0 |   1.2484 ns | 0.0367 ns | 0.0343 ns |   1.2517 ns |   1.1799 ns |   1.3085 ns |  1.98 |    0.06 |      - |         - |          NA |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfHex                     | .NET 9.0  |   1.9705 ns | 0.0228 ns | 0.0202 ns |   1.9605 ns |   1.9466 ns |   2.0186 ns |  1.00 |    0.01 |      - |         - |          NA |
| ThrowIfNotHex                  | .NET 9.0  |   3.0287 ns | 0.0591 ns | 0.0553 ns |   3.0308 ns |   2.9102 ns |   3.1144 ns |  1.54 |    0.03 |      - |         - |          NA |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfNumber                  | .NET 10.0 |  15.3265 ns | 0.1239 ns | 0.1159 ns |  15.3359 ns |  15.1471 ns |  15.5166 ns |  1.00 |    0.01 |      - |         - |          NA |
| ThrowIfNotNumber               | .NET 10.0 |  19.2189 ns | 0.1935 ns | 0.1716 ns |  19.1601 ns |  19.0282 ns |  19.6298 ns |  1.25 |    0.01 |      - |         - |          NA |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfNumber                  | .NET 9.0  |  20.9156 ns | 0.0847 ns | 0.0792 ns |  20.9219 ns |  20.7625 ns |  21.0299 ns |  1.00 |    0.01 |      - |         - |          NA |
| ThrowIfNotNumber               | .NET 9.0  |  25.4723 ns | 0.1644 ns | 0.1538 ns |  25.4569 ns |  25.2596 ns |  25.8007 ns |  1.22 |    0.01 |      - |         - |          NA |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfUri                     | .NET 10.0 |  78.8806 ns | 2.8760 ns | 3.3120 ns |  78.3785 ns |  74.0347 ns |  87.1510 ns |  1.00 |    0.06 | 0.0204 |     320 B |        1.00 |
| ThrowIfNotUri                  | .NET 10.0 |  90.3678 ns | 2.2567 ns | 2.5083 ns |  90.6735 ns |  86.8278 ns |  94.7976 ns |  1.15 |    0.06 | 0.0237 |     376 B |        1.18 |
|                                |           |             |           |           |             |             |             |       |         |        |           |             |
| ThrowIfUri                     | .NET 9.0  |  88.0244 ns | 2.4042 ns | 2.7687 ns |  88.6392 ns |  82.4452 ns |  91.4258 ns |  1.00 |    0.04 | 0.0201 |     320 B |        1.00 |
| ThrowIfNotUri                  | .NET 9.0  | 125.3698 ns | 5.3087 ns | 6.1135 ns | 124.5573 ns | 113.6706 ns | 136.7812 ns |  1.43 |    0.08 | 0.0237 |     376 B |        1.18 |
