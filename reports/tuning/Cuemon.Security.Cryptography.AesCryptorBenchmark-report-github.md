```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i9-12900KF 3.20GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 10.0.101
  [Host]     : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3
  Job-LDLMHG : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3
  Job-IOAYXE : .NET 9.0.11 (9.0.11, 9.0.1125.51716), X64 RyuJIT x86-64-v3

PowerPlanMode=00000000-0000-0000-0000-000000000000  IterationTime=250ms  MaxIterationCount=20  
MinIterationCount=15  WarmupCount=1  

```
| Method             | Runtime   | Size  | Mean        | Error     | StdDev    | Median      | Min         | Max         | Gen0   | Allocated |
|------------------- |---------- |------ |------------:|----------:|----------:|------------:|------------:|------------:|-------:|----------:|
| **AesCryptor.Encrypt** | **.NET 10.0** | **128**   |    **750.2 ns** |  **13.44 ns** |  **11.22 ns** |    **747.4 ns** |    **737.9 ns** |    **777.6 ns** | **0.0474** |     **752 B** |
| AesCryptor.Decrypt | .NET 10.0 | 128   |    802.0 ns |   9.40 ns |   8.79 ns |    802.1 ns |    783.0 ns |    814.1 ns | 0.0446 |     744 B |
| AesCryptor.Encrypt | .NET 9.0  | 128   |    658.3 ns |   8.56 ns |   8.01 ns |    656.7 ns |    646.6 ns |    669.8 ns | 0.0525 |     848 B |
| AesCryptor.Decrypt | .NET 9.0  | 128   |    731.6 ns |   8.72 ns |   7.73 ns |    729.9 ns |    721.5 ns |    748.9 ns | 0.0529 |     840 B |
| **AesCryptor.Encrypt** | **.NET 10.0** | **1024**  |  **1,261.4 ns** |  **13.44 ns** |  **11.92 ns** |  **1,259.5 ns** |  **1,245.5 ns** |  **1,280.9 ns** | **0.1049** |    **1648 B** |
| AesCryptor.Decrypt | .NET 10.0 | 1024  |    902.8 ns |  19.64 ns |  21.83 ns |    897.4 ns |    872.8 ns |    949.3 ns | 0.1027 |    1640 B |
| AesCryptor.Encrypt | .NET 9.0  | 1024  |  1,252.4 ns |  16.27 ns |  15.22 ns |  1,246.7 ns |  1,225.4 ns |  1,277.5 ns | 0.1084 |    1744 B |
| AesCryptor.Decrypt | .NET 9.0  | 1024  |    863.4 ns |  17.37 ns |  19.31 ns |    867.0 ns |    824.3 ns |    896.3 ns | 0.1093 |    1736 B |
| **AesCryptor.Encrypt** | **.NET 10.0** | **65536** | **39,608.6 ns** | **601.37 ns** | **562.52 ns** | **39,525.0 ns** | **38,927.0 ns** | **40,607.4 ns** | **4.0796** |   **66162 B** |
| AesCryptor.Decrypt | .NET 10.0 | 65536 | 12,640.4 ns | 267.65 ns | 297.49 ns | 12,583.9 ns | 12,263.6 ns | 13,368.0 ns | 4.1701 |   66154 B |
| AesCryptor.Encrypt | .NET 9.0  | 65536 | 39,731.4 ns | 703.00 ns | 657.59 ns | 39,500.4 ns | 38,913.3 ns | 40,837.5 ns | 4.0932 |   66258 B |
| AesCryptor.Decrypt | .NET 9.0  | 65536 | 12,374.1 ns | 122.62 ns | 102.39 ns | 12,383.7 ns | 12,205.0 ns | 12,571.0 ns | 4.1631 |   66250 B |
