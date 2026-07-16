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
| Method                        | Runtime   | Mean       | Error     | StdDev    | Median     | Min        | Max        | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------ |---------- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| &#39;ThrowIfFalse - Boolean&#39;      | .NET 10.0 |  0.0009 ns | 0.0023 ns | 0.0021 ns |  0.0000 ns |  0.0000 ns |  0.0067 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfFalse - predicate&#39;    | .NET 10.0 |  1.4518 ns | 0.0515 ns | 0.0481 ns |  1.4378 ns |  1.4088 ns |  1.5522 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfTrue - Boolean&#39;       | .NET 10.0 |  0.0047 ns | 0.0112 ns | 0.0094 ns |  0.0000 ns |  0.0000 ns |  0.0272 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfTrue - predicate&#39;     | .NET 10.0 |  1.4287 ns | 0.0064 ns | 0.0050 ns |  1.4299 ns |  1.4176 ns |  1.4343 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| &#39;ThrowIfFalse - Boolean&#39;      | .NET 9.0  |  0.0001 ns | 0.0002 ns | 0.0002 ns |  0.0000 ns |  0.0000 ns |  0.0006 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfFalse - predicate&#39;    | .NET 9.0  |  2.0409 ns | 0.0144 ns | 0.0128 ns |  2.0446 ns |  2.0047 ns |  2.0510 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfTrue - Boolean&#39;       | .NET 9.0  |  0.0012 ns | 0.0016 ns | 0.0013 ns |  0.0010 ns |  0.0000 ns |  0.0047 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfTrue - predicate&#39;     | .NET 9.0  |  1.6382 ns | 0.0113 ns | 0.0105 ns |  1.6401 ns |  1.6112 ns |  1.6498 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| ThrowIfSequenceEmpty          | .NET 10.0 |  0.2113 ns | 0.0093 ns | 0.0087 ns |  0.2081 ns |  0.2023 ns |  0.2287 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfSequenceNullOrEmpty    | .NET 10.0 |  0.2172 ns | 0.0134 ns | 0.0126 ns |  0.2181 ns |  0.2036 ns |  0.2440 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| ThrowIfSequenceEmpty          | .NET 9.0  |  0.8215 ns | 0.0130 ns | 0.0115 ns |  0.8245 ns |  0.8060 ns |  0.8471 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfSequenceNullOrEmpty    | .NET 9.0  |  1.0223 ns | 0.0154 ns | 0.0136 ns |  1.0205 ns |  0.9999 ns |  1.0473 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| ThrowIfSame                   | .NET 10.0 |  0.0008 ns | 0.0012 ns | 0.0011 ns |  0.0000 ns |  0.0000 ns |  0.0033 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfNotSame                | .NET 10.0 |  0.0000 ns | 0.0001 ns | 0.0001 ns |  0.0000 ns |  0.0000 ns |  0.0005 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfEqual                  | .NET 10.0 |  0.2047 ns | 0.0046 ns | 0.0041 ns |  0.2061 ns |  0.1968 ns |  0.2095 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfNotEqual               | .NET 10.0 |  0.0015 ns | 0.0018 ns | 0.0017 ns |  0.0013 ns |  0.0000 ns |  0.0049 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| ThrowIfSame                   | .NET 9.0  |  1.0214 ns | 0.0101 ns | 0.0089 ns |  1.0237 ns |  1.0035 ns |  1.0311 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfNotSame                | .NET 9.0  |  0.8193 ns | 0.0114 ns | 0.0101 ns |  0.8229 ns |  0.7977 ns |  0.8326 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfEqual                  | .NET 9.0  |  0.8207 ns | 0.0079 ns | 0.0074 ns |  0.8213 ns |  0.7973 ns |  0.8306 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfNotEqual               | .NET 9.0  |  1.0271 ns | 0.0059 ns | 0.0055 ns |  1.0270 ns |  1.0159 ns |  1.0352 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| Constructor                   | .NET 10.0 |  2.8861 ns | 0.1873 ns | 0.2157 ns |  2.8157 ns |  2.6196 ns |  3.2732 ns |  1.01 |    0.10 | 0.0015 |      24 B |        1.00 |
| &#39;ThrowIf property&#39;            | .NET 10.0 |  0.7834 ns | 0.0436 ns | 0.0408 ns |  0.7789 ns |  0.7030 ns |  0.8309 ns |  0.27 |    0.02 |      - |         - |        0.00 |
| &#39;CheckParameter - action&#39;     | .NET 10.0 |  2.1364 ns | 0.0328 ns | 0.0291 ns |  2.1472 ns |  2.0662 ns |  2.1695 ns |  0.74 |    0.05 |      - |         - |        0.00 |
| &#39;CheckParameter - function&#39;   | .NET 10.0 |  1.1941 ns | 0.0061 ns | 0.0054 ns |  1.1933 ns |  1.1856 ns |  1.2051 ns |  0.42 |    0.03 |      - |         - |        0.00 |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| Constructor                   | .NET 9.0  |  2.7773 ns | 0.1900 ns | 0.2188 ns |  2.7491 ns |  2.4768 ns |  3.2375 ns |  1.01 |    0.11 | 0.0015 |      24 B |        1.00 |
| &#39;ThrowIf property&#39;            | .NET 9.0  |  0.5976 ns | 0.0172 ns | 0.0161 ns |  0.5971 ns |  0.5715 ns |  0.6280 ns |  0.22 |    0.02 |      - |         - |        0.00 |
| &#39;CheckParameter - action&#39;     | .NET 9.0  |  2.2499 ns | 0.0226 ns | 0.0200 ns |  2.2503 ns |  2.1952 ns |  2.2783 ns |  0.81 |    0.06 |      - |         - |        0.00 |
| &#39;CheckParameter - function&#39;   | .NET 9.0  |  1.2386 ns | 0.0100 ns | 0.0084 ns |  1.2404 ns |  1.2141 ns |  1.2470 ns |  0.45 |    0.03 |      - |         - |        0.00 |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| &#39;ThrowIfNull - decorator&#39;     | .NET 10.0 |  0.4113 ns | 0.0054 ns | 0.0045 ns |  0.4135 ns |  0.4033 ns |  0.4178 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfNull - object&#39;        | .NET 10.0 |  0.2025 ns | 0.0049 ns | 0.0044 ns |  0.2044 ns |  0.1931 ns |  0.2069 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| &#39;ThrowIfNull - decorator&#39;     | .NET 9.0  |  8.5274 ns | 0.0357 ns | 0.0316 ns |  8.5215 ns |  8.4763 ns |  8.5915 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfNull - object&#39;        | .NET 9.0  |  0.0122 ns | 0.0054 ns | 0.0047 ns |  0.0132 ns |  0.0006 ns |  0.0190 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| ThrowIfInvalidConfigurator    | .NET 10.0 |  7.4054 ns | 0.1820 ns | 0.2095 ns |  7.3917 ns |  7.1096 ns |  7.8605 ns |     ? |       ? | 0.0015 |      24 B |           ? |
| ThrowIfInvalidOptions         | .NET 10.0 |  1.2375 ns | 0.0168 ns | 0.0157 ns |  1.2410 ns |  1.1848 ns |  1.2535 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| ThrowIfInvalidConfigurator    | .NET 9.0  | 10.2685 ns | 0.1600 ns | 0.1497 ns | 10.2251 ns | 10.0541 ns | 10.5256 ns |     ? |       ? | 0.0015 |      24 B |           ? |
| ThrowIfInvalidOptions         | .NET 9.0  |  0.8304 ns | 0.0147 ns | 0.0131 ns |  0.8283 ns |  0.8165 ns |  0.8560 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| ThrowIfGreaterThan            | .NET 10.0 |  0.0021 ns | 0.0042 ns | 0.0039 ns |  0.0000 ns |  0.0000 ns |  0.0129 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfGreaterThanOrEqual     | .NET 10.0 |  0.0003 ns | 0.0006 ns | 0.0006 ns |  0.0000 ns |  0.0000 ns |  0.0019 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfLowerThan              | .NET 10.0 |  0.0007 ns | 0.0011 ns | 0.0010 ns |  0.0000 ns |  0.0000 ns |  0.0027 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfLowerThanOrEqual       | .NET 10.0 |  0.0002 ns | 0.0005 ns | 0.0005 ns |  0.0000 ns |  0.0000 ns |  0.0019 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| ThrowIfGreaterThan            | .NET 9.0  |  0.4234 ns | 0.0187 ns | 0.0175 ns |  0.4137 ns |  0.4089 ns |  0.4605 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfGreaterThanOrEqual     | .NET 9.0  |  0.4364 ns | 0.0284 ns | 0.0266 ns |  0.4288 ns |  0.4100 ns |  0.4946 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfLowerThan              | .NET 9.0  |  0.4056 ns | 0.0089 ns | 0.0079 ns |  0.4058 ns |  0.3948 ns |  0.4210 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfLowerThanOrEqual       | .NET 9.0  |  0.4126 ns | 0.0110 ns | 0.0103 ns |  0.4095 ns |  0.3989 ns |  0.4274 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| &#39;ThrowIfInvalidState - valid&#39; | .NET 10.0 |  0.0004 ns | 0.0011 ns | 0.0010 ns |  0.0000 ns |  0.0000 ns |  0.0029 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfDisposed - object&#39;    | .NET 10.0 |  0.8202 ns | 0.0065 ns | 0.0057 ns |  0.8223 ns |  0.8086 ns |  0.8277 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfDisposed - type&#39;      | .NET 10.0 |  0.0007 ns | 0.0009 ns | 0.0008 ns |  0.0006 ns |  0.0000 ns |  0.0028 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| &#39;ThrowIfInvalidState - valid&#39; | .NET 9.0  |  0.0005 ns | 0.0008 ns | 0.0008 ns |  0.0001 ns |  0.0000 ns |  0.0025 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfDisposed - object&#39;    | .NET 9.0  |  1.0207 ns | 0.0060 ns | 0.0047 ns |  1.0231 ns |  1.0077 ns |  1.0238 ns |     ? |       ? |      - |         - |           ? |
| &#39;ThrowIfDisposed - type&#39;      | .NET 9.0  |  0.0015 ns | 0.0018 ns | 0.0016 ns |  0.0013 ns |  0.0000 ns |  0.0059 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| ThrowIfEmpty                  | .NET 10.0 |  0.0002 ns | 0.0003 ns | 0.0003 ns |  0.0000 ns |  0.0000 ns |  0.0010 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfWhiteSpace             | .NET 10.0 |  0.2050 ns | 0.0070 ns | 0.0059 ns |  0.2056 ns |  0.1962 ns |  0.2177 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfNullOrEmpty            | .NET 10.0 |  0.0085 ns | 0.0085 ns | 0.0076 ns |  0.0078 ns |  0.0000 ns |  0.0208 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfNullOrWhitespace       | .NET 10.0 |  0.2049 ns | 0.0075 ns | 0.0067 ns |  0.2050 ns |  0.1958 ns |  0.2144 ns |     ? |       ? |      - |         - |           ? |
|                               |           |            |           |           |            |            |            |       |         |        |           |             |
| ThrowIfEmpty                  | .NET 9.0  |  0.0016 ns | 0.0032 ns | 0.0030 ns |  0.0000 ns |  0.0000 ns |  0.0108 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfWhiteSpace             | .NET 9.0  |  0.8203 ns | 0.0031 ns | 0.0027 ns |  0.8195 ns |  0.8170 ns |  0.8249 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfNullOrEmpty            | .NET 9.0  |  0.0008 ns | 0.0011 ns | 0.0010 ns |  0.0005 ns |  0.0000 ns |  0.0026 ns |     ? |       ? |      - |         - |           ? |
| ThrowIfNullOrWhitespace       | .NET 9.0  |  0.8230 ns | 0.0032 ns | 0.0028 ns |  0.8222 ns |  0.8184 ns |  0.8283 ns |     ? |       ? |      - |         - |           ? |
