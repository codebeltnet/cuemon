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
| Method             | Runtime   | Size  | Mean        | Error       | StdDev      | Median      | Min         | Max         | Gen0   | Allocated |
|------------------- |---------- |------ |------------:|------------:|------------:|------------:|------------:|------------:|-------:|----------:|
| **AesCryptor.Encrypt** | **.NET 10.0** | **128**   |    **755.1 ns** |    **12.70 ns** |     **9.92 ns** |    **754.3 ns** |    **740.6 ns** |    **775.8 ns** | **0.0453** |     **752 B** |
| AesCryptor.Decrypt | .NET 10.0 | 128   |    754.5 ns |    11.05 ns |     9.23 ns |    752.6 ns |    738.8 ns |    776.7 ns | 0.0457 |     744 B |
| AesCryptor.Encrypt | .NET 9.0  | 128   |    668.1 ns |    10.12 ns |     8.45 ns |    669.6 ns |    658.0 ns |    683.2 ns | 0.0529 |     848 B |
| AesCryptor.Decrypt | .NET 9.0  | 128   |    747.3 ns |    12.37 ns |    11.57 ns |    745.5 ns |    731.0 ns |    768.6 ns | 0.0511 |     840 B |
| **AesCryptor.Encrypt** | **.NET 10.0** | **1024**  |  **1,245.1 ns** |    **17.09 ns** |    **15.15 ns** |  **1,243.5 ns** |  **1,222.2 ns** |  **1,273.6 ns** | **0.1015** |    **1648 B** |
| AesCryptor.Decrypt | .NET 10.0 | 1024  |  1,299.6 ns |   147.32 ns |   157.63 ns |  1,366.2 ns |    915.7 ns |  1,389.7 ns | 0.1040 |    1640 B |
| AesCryptor.Encrypt | .NET 9.0  | 1024  |  1,613.0 ns |   170.95 ns |   196.87 ns |  1,707.7 ns |  1,206.2 ns |  1,770.3 ns | 0.1109 |    1744 B |
| AesCryptor.Decrypt | .NET 9.0  | 1024  |  1,202.2 ns |   162.13 ns |   186.71 ns |  1,300.5 ns |    872.8 ns |  1,392.8 ns | 0.1075 |    1736 B |
| **AesCryptor.Encrypt** | **.NET 10.0** | **65536** | **47,992.7 ns** | **4,700.84 ns** | **5,413.49 ns** | **51,240.1 ns** | **39,618.5 ns** | **52,582.1 ns** | **4.1560** |   **66162 B** |
| AesCryptor.Decrypt | .NET 10.0 | 65536 | 21,379.7 ns | 1,450.45 ns | 1,670.34 ns | 21,705.7 ns | 14,695.4 ns | 22,730.5 ns | 4.1868 |   66154 B |
| AesCryptor.Encrypt | .NET 9.0  | 65536 | 51,121.5 ns | 3,129.53 ns | 3,348.56 ns | 52,150.6 ns | 40,544.9 ns | 53,011.5 ns | 4.1139 |   66258 B |
| AesCryptor.Decrypt | .NET 9.0  | 65536 | 19,881.6 ns | 2,983.27 ns | 3,315.89 ns | 21,329.2 ns | 12,927.3 ns | 22,819.1 ns | 4.1632 |   66250 B |
