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
| Method                                          | Runtime   | Mean      | Error     | StdDev    | Median    | Min       | Max       | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------------------ |---------- |----------:|----------:|----------:|----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| &#39;ContainsInterface - inline params[]&#39;           | .NET 10.0 | 15.758 ns | 0.2767 ns | 0.2453 ns | 15.713 ns | 15.487 ns | 16.309 ns |     ? |       ? | 0.0045 |      72 B |           ? |
| &#39;ContainsInterface - preallocated static[]&#39;     | .NET 10.0 | 13.361 ns | 1.0812 ns | 1.2451 ns | 12.966 ns | 12.023 ns | 16.123 ns |     ? |       ? | 0.0025 |      40 B |           ? |
| &#39;ContainsInterface - non-params Type overload&#39;  | .NET 10.0 | 13.014 ns | 1.0050 ns | 1.1573 ns | 12.799 ns | 11.453 ns | 14.979 ns |     ? |       ? | 0.0025 |      40 B |           ? |
| &#39;NotContainsInterface - inline params[]&#39;        | .NET 10.0 | 19.591 ns | 0.8266 ns | 0.8845 ns | 19.665 ns | 18.263 ns | 21.720 ns |     ? |       ? | 0.0081 |     128 B |           ? |
| &#39;NotContainsInterface - preallocated static[]&#39;  | .NET 10.0 | 16.996 ns | 0.7074 ns | 0.8146 ns | 16.600 ns | 16.231 ns | 18.826 ns |     ? |       ? | 0.0061 |      96 B |           ? |
|                                                 |           |           |           |           |           |           |           |       |         |        |           |             |
| &#39;ContainsInterface - inline params[]&#39;           | .NET 9.0  | 17.810 ns | 0.3508 ns | 0.3281 ns | 17.881 ns | 17.182 ns | 18.283 ns |     ? |       ? | 0.0046 |      72 B |           ? |
| &#39;ContainsInterface - preallocated static[]&#39;     | .NET 9.0  | 21.419 ns | 2.1929 ns | 2.5253 ns | 21.984 ns | 13.823 ns | 23.604 ns |     ? |       ? | 0.0025 |      40 B |           ? |
| &#39;ContainsInterface - non-params Type overload&#39;  | .NET 9.0  | 20.955 ns | 2.6607 ns | 3.0640 ns | 22.162 ns | 14.612 ns | 23.467 ns |     ? |       ? | 0.0025 |      40 B |           ? |
| &#39;NotContainsInterface - inline params[]&#39;        | .NET 9.0  | 31.210 ns | 3.5800 ns | 4.1227 ns | 33.947 ns | 25.143 ns | 35.325 ns |     ? |       ? | 0.0081 |     128 B |           ? |
| &#39;NotContainsInterface - preallocated static[]&#39;  | .NET 9.0  | 22.214 ns | 0.5246 ns | 0.6042 ns | 22.043 ns | 21.413 ns | 23.457 ns |     ? |       ? | 0.0061 |      96 B |           ? |
|                                                 |           |           |           |           |           |           |           |       |         |        |           |             |
| &#39;ThrowIfContainsInterface - generic&#39;            | .NET 10.0 | 13.625 ns | 0.2814 ns | 0.2633 ns | 13.588 ns | 13.142 ns | 14.016 ns |  1.00 |    0.03 | 0.0025 |      40 B |        1.00 |
| &#39;ThrowIfContainsInterface - generic message&#39;    | .NET 10.0 | 14.202 ns | 0.4195 ns | 0.4662 ns | 14.253 ns | 13.497 ns | 15.325 ns |  1.04 |    0.04 | 0.0025 |      40 B |        1.00 |
| &#39;ThrowIfContainsInterface - type&#39;               | .NET 10.0 | 13.965 ns | 0.3482 ns | 0.4010 ns | 13.965 ns | 13.393 ns | 14.767 ns |  1.03 |    0.03 | 0.0025 |      40 B |        1.00 |
| &#39;ThrowIfNotContainsInterface - generic&#39;         | .NET 10.0 | 17.803 ns | 0.3581 ns | 0.3174 ns | 17.881 ns | 17.186 ns | 18.171 ns |  1.31 |    0.03 | 0.0061 |      96 B |        2.40 |
| &#39;ThrowIfNotContainsInterface - generic message&#39; | .NET 10.0 | 16.879 ns | 0.3609 ns | 0.4011 ns | 16.850 ns | 16.276 ns | 17.719 ns |  1.24 |    0.04 | 0.0061 |      96 B |        2.40 |
| &#39;ThrowIfNotContainsInterface - type&#39;            | .NET 10.0 | 16.819 ns | 0.4873 ns | 0.5214 ns | 16.835 ns | 15.853 ns | 17.719 ns |  1.23 |    0.04 | 0.0061 |      96 B |        2.40 |
|                                                 |           |           |           |           |           |           |           |       |         |        |           |             |
| &#39;ThrowIfContainsInterface - generic&#39;            | .NET 9.0  | 14.600 ns | 0.1827 ns | 0.1526 ns | 14.558 ns | 14.382 ns | 14.859 ns |  1.00 |    0.01 | 0.0025 |      40 B |        1.00 |
| &#39;ThrowIfContainsInterface - generic message&#39;    | .NET 9.0  | 13.978 ns | 0.2039 ns | 0.1703 ns | 13.967 ns | 13.722 ns | 14.311 ns |  0.96 |    0.01 | 0.0025 |      40 B |        1.00 |
| &#39;ThrowIfContainsInterface - type&#39;               | .NET 9.0  | 13.253 ns | 0.2068 ns | 0.1615 ns | 13.201 ns | 13.089 ns | 13.670 ns |  0.91 |    0.01 | 0.0025 |      40 B |        1.00 |
| &#39;ThrowIfNotContainsInterface - generic&#39;         | .NET 9.0  | 19.631 ns | 0.2532 ns | 0.1977 ns | 19.638 ns | 19.298 ns | 19.946 ns |  1.34 |    0.02 | 0.0061 |      96 B |        2.40 |
| &#39;ThrowIfNotContainsInterface - generic message&#39; | .NET 9.0  | 21.053 ns | 0.2632 ns | 0.2462 ns | 21.024 ns | 20.578 ns | 21.618 ns |  1.44 |    0.02 | 0.0061 |      96 B |        2.40 |
| &#39;ThrowIfNotContainsInterface - type&#39;            | .NET 9.0  | 20.713 ns | 0.4220 ns | 0.4145 ns | 20.698 ns | 20.060 ns | 21.509 ns |  1.42 |    0.03 | 0.0061 |      96 B |        2.40 |
|                                                 |           |           |           |           |           |           |           |       |         |        |           |             |
| &#39;ThrowIfContainsType - object&#39;                  | .NET 10.0 |  9.311 ns | 0.1370 ns | 0.1282 ns |  9.374 ns |  9.125 ns |  9.487 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfContainsType - type&#39;                    | .NET 10.0 | 11.206 ns | 1.0214 ns | 1.1763 ns | 11.583 ns |  9.417 ns | 13.914 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfContainsType - generic&#39;                 | .NET 10.0 |  8.812 ns | 0.1120 ns | 0.1047 ns |  8.818 ns |  8.662 ns |  8.965 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfContainsType - generic message&#39;         | .NET 10.0 |  9.549 ns | 0.1390 ns | 0.1300 ns |  9.540 ns |  9.320 ns |  9.753 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfNotContainsType - type&#39;                 | .NET 10.0 | 12.265 ns | 0.1538 ns | 0.1438 ns | 12.224 ns | 12.027 ns | 12.477 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfNotContainsType - object&#39;               | .NET 10.0 | 69.293 ns | 2.8974 ns | 3.2205 ns | 68.082 ns | 65.630 ns | 76.254 ns |     ? |       ? | 0.0079 |     128 B |           ? |
| &#39;ThrowIfNotContainsType - generic&#39;              | .NET 10.0 | 12.482 ns | 0.4195 ns | 0.4831 ns | 12.223 ns | 12.100 ns | 13.764 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfNotContainsType - generic message&#39;      | .NET 10.0 | 11.491 ns | 0.2186 ns | 0.2045 ns | 11.418 ns | 11.230 ns | 11.858 ns |     ? |       ? |      - |         - |           ? |
|                                                 |           |           |           |           |           |           |           |       |         |        |           |             |
| &#39;ThrowIfContainsType - object&#39;                  | .NET 9.0  | 14.352 ns | 0.2013 ns | 0.1681 ns | 14.332 ns | 14.176 ns | 14.778 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfContainsType - type&#39;                    | .NET 9.0  | 14.217 ns | 0.1026 ns | 0.0959 ns | 14.211 ns | 14.078 ns | 14.393 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfContainsType - generic&#39;                 | .NET 9.0  | 14.625 ns | 0.4303 ns | 0.4783 ns | 14.382 ns | 14.214 ns | 15.822 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfContainsType - generic message&#39;         | .NET 9.0  | 13.995 ns | 0.0549 ns | 0.0513 ns | 13.998 ns | 13.899 ns | 14.085 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfNotContainsType - type&#39;                 | .NET 9.0  | 19.452 ns | 0.3213 ns | 0.2683 ns | 19.427 ns | 19.045 ns | 19.877 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfNotContainsType - object&#39;               | .NET 9.0  | 73.198 ns | 2.0043 ns | 1.9685 ns | 73.247 ns | 70.523 ns | 79.435 ns |     ? |       ? | 0.0081 |     128 B |           ? |
| &#39;ThrowIfNotContainsType - generic&#39;              | .NET 9.0  | 18.442 ns | 0.2489 ns | 0.2079 ns | 18.410 ns | 18.260 ns | 19.085 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfNotContainsType - generic message&#39;      | .NET 9.0  | 18.877 ns | 0.0916 ns | 0.0856 ns | 18.852 ns | 18.718 ns | 19.037 ns |     ? |       ? |      - |         - |           ? |
