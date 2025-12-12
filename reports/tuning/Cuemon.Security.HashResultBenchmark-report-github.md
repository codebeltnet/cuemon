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
| Method                              | Runtime   | Size | Mean           | Error       | StdDev      | Median         | Min            | Max            | Gen0   | Gen1   | Allocated |
|------------------------------------ |---------- |----- |---------------:|------------:|------------:|---------------:|---------------:|---------------:|-------:|-------:|----------:|
| **&#39;HashResult.GetBytes - copy bytes&#39;**  | **.NET 10.0** | **0**    |      **0.3081 ns** |   **0.0824 ns** |   **0.0916 ns** |      **0.2990 ns** |      **0.1514 ns** |      **0.5143 ns** |      **-** |      **-** |         **-** |
| HashResult.ToHexadecimalString      | .NET 10.0 | 0    |      8.2530 ns |   0.2758 ns |   0.3065 ns |      8.3525 ns |      7.2030 ns |      8.5634 ns |      - |      - |         - |
| HashResult.ToBase64String           | .NET 10.0 | 0    |      1.8415 ns |   0.1614 ns |   0.1859 ns |      1.8784 ns |      1.3136 ns |      2.0688 ns |      - |      - |         - |
| HashResult.ToUrlEncodedBase64String | .NET 10.0 | 0    |     12.8951 ns |   0.1479 ns |   0.1311 ns |     12.9058 ns |     12.5705 ns |     13.1165 ns | 0.0020 |      - |      32 B |
| HashResult.ToBinaryString           | .NET 10.0 | 0    |      5.2788 ns |   0.2434 ns |   0.2803 ns |      5.2655 ns |      4.6845 ns |      5.7648 ns |      - |      - |         - |
| &#39;HashResult.To&lt;string&gt; (converter)&#39; | .NET 10.0 | 0    |      2.4810 ns |   0.1438 ns |   0.1656 ns |      2.5167 ns |      2.0744 ns |      2.7984 ns |      - |      - |         - |
| &#39;HashResult.GetBytes - copy bytes&#39;  | .NET 9.0  | 0    |      1.1013 ns |   0.1009 ns |   0.0991 ns |      1.1202 ns |      0.8392 ns |      1.2559 ns |      - |      - |         - |
| HashResult.ToHexadecimalString      | .NET 9.0  | 0    |     10.4251 ns |   0.3300 ns |   0.3668 ns |     10.4798 ns |      9.3319 ns |     10.8308 ns |      - |      - |         - |
| HashResult.ToBase64String           | .NET 9.0  | 0    |      2.3065 ns |   0.1692 ns |   0.1949 ns |      2.3350 ns |      1.9197 ns |      2.7684 ns |      - |      - |         - |
| HashResult.ToUrlEncodedBase64String | .NET 9.0  | 0    |     20.2473 ns |   0.3475 ns |   0.3251 ns |     20.3253 ns |     19.7645 ns |     20.8662 ns | 0.0020 |      - |      32 B |
| HashResult.ToBinaryString           | .NET 9.0  | 0    |      8.9624 ns |   0.1725 ns |   0.1347 ns |      8.9710 ns |      8.6556 ns |      9.1649 ns |      - |      - |         - |
| &#39;HashResult.To&lt;string&gt; (converter)&#39; | .NET 9.0  | 0    |      2.5100 ns |   0.0633 ns |   0.0494 ns |      2.5175 ns |      2.4470 ns |      2.6110 ns |      - |      - |         - |
| **&#39;HashResult.GetBytes - copy bytes&#39;**  | **.NET 10.0** | **8**    |      **3.2229 ns** |   **0.1203 ns** |   **0.1235 ns** |      **3.2277 ns** |      **2.9727 ns** |      **3.4067 ns** | **0.0020** |      **-** |      **32 B** |
| HashResult.ToHexadecimalString      | .NET 10.0 | 8    |     19.0786 ns |   0.7386 ns |   0.8209 ns |     19.1042 ns |     17.6026 ns |     20.9303 ns | 0.0071 |      - |     112 B |
| HashResult.ToBase64String           | .NET 10.0 | 8    |      9.3121 ns |   0.2429 ns |   0.2599 ns |      9.2244 ns |      9.0330 ns |      9.9402 ns | 0.0030 |      - |      48 B |
| HashResult.ToUrlEncodedBase64String | .NET 10.0 | 8    |     31.4628 ns |   1.0524 ns |   1.1697 ns |     31.2177 ns |     30.2424 ns |     34.0435 ns | 0.0086 |      - |     136 B |
| HashResult.ToBinaryString           | .NET 10.0 | 8    |    134.7434 ns |  10.4299 ns |  12.0111 ns |    137.7178 ns |    117.6131 ns |    157.2830 ns | 0.0421 |      - |     664 B |
| &#39;HashResult.To&lt;string&gt; (converter)&#39; | .NET 10.0 | 8    |      9.8388 ns |   0.2520 ns |   0.2475 ns |      9.7202 ns |      9.5885 ns |     10.5373 ns | 0.0030 |      - |      48 B |
| &#39;HashResult.GetBytes - copy bytes&#39;  | .NET 9.0  | 8    |      4.9752 ns |   0.1337 ns |   0.1185 ns |      4.9724 ns |      4.7415 ns |      5.1787 ns | 0.0020 |      - |      32 B |
| HashResult.ToHexadecimalString      | .NET 9.0  | 8    |     22.9412 ns |   0.7908 ns |   0.8789 ns |     22.7765 ns |     21.8512 ns |     24.6682 ns | 0.0071 |      - |     112 B |
| HashResult.ToBase64String           | .NET 9.0  | 8    |     10.5079 ns |   0.1634 ns |   0.1448 ns |     10.5109 ns |     10.2437 ns |     10.7979 ns | 0.0030 |      - |      48 B |
| HashResult.ToUrlEncodedBase64String | .NET 9.0  | 8    |     35.4220 ns |   0.6964 ns |   0.5815 ns |     35.5249 ns |     34.4310 ns |     36.2857 ns | 0.0086 |      - |     136 B |
| HashResult.ToBinaryString           | .NET 9.0  | 8    |    141.2368 ns |   2.9172 ns |   3.1213 ns |    140.4155 ns |    137.6878 ns |    148.9139 ns | 0.0423 |      - |     664 B |
| &#39;HashResult.To&lt;string&gt; (converter)&#39; | .NET 9.0  | 8    |     11.3440 ns |   0.2816 ns |   0.3014 ns |     11.3818 ns |     10.8524 ns |     11.8204 ns | 0.0031 |      - |      48 B |
| **&#39;HashResult.GetBytes - copy bytes&#39;**  | **.NET 10.0** | **32**   |      **4.8382 ns** |   **0.6674 ns** |   **0.7686 ns** |      **5.0258 ns** |      **3.8634 ns** |      **5.8620 ns** | **0.0036** |      **-** |      **56 B** |
| HashResult.ToHexadecimalString      | .NET 10.0 | 32   |     29.2109 ns |   1.1493 ns |   1.2297 ns |     28.9061 ns |     27.8912 ns |     32.0415 ns | 0.0193 |      - |     304 B |
| HashResult.ToBase64String           | .NET 10.0 | 32   |     14.5346 ns |   0.5491 ns |   0.6324 ns |     14.2334 ns |     13.8828 ns |     15.6775 ns | 0.0071 |      - |     112 B |
| HashResult.ToUrlEncodedBase64String | .NET 10.0 | 32   |     61.5454 ns |   1.2715 ns |   1.3605 ns |     61.5718 ns |     59.4245 ns |     63.5087 ns | 0.0309 |      - |     488 B |
| HashResult.ToBinaryString           | .NET 10.0 | 32   |    462.5206 ns |  11.0808 ns |  11.8564 ns |    458.8116 ns |    443.3036 ns |    480.5018 ns | 0.1591 |      - |    2504 B |
| &#39;HashResult.To&lt;string&gt; (converter)&#39; | .NET 10.0 | 32   |     15.0599 ns |   0.4380 ns |   0.4869 ns |     14.9212 ns |     14.2127 ns |     16.0316 ns | 0.0071 |      - |     112 B |
| &#39;HashResult.GetBytes - copy bytes&#39;  | .NET 9.0  | 32   |      5.6659 ns |   0.1359 ns |   0.1205 ns |      5.6327 ns |      5.5139 ns |      5.9118 ns | 0.0035 |      - |      56 B |
| HashResult.ToHexadecimalString      | .NET 9.0  | 32   |     37.7492 ns |   3.5379 ns |   4.0742 ns |     35.5931 ns |     32.9668 ns |     43.0977 ns | 0.0193 |      - |     304 B |
| HashResult.ToBase64String           | .NET 9.0  | 32   |     16.9914 ns |   1.1334 ns |   1.3052 ns |     16.5715 ns |     15.7043 ns |     20.3899 ns | 0.0071 |      - |     112 B |
| HashResult.ToUrlEncodedBase64String | .NET 9.0  | 32   |     70.6395 ns |   1.6958 ns |   1.8848 ns |     70.7049 ns |     67.8201 ns |     74.1284 ns | 0.0310 |      - |     488 B |
| HashResult.ToBinaryString           | .NET 9.0  | 32   |    499.7174 ns |  12.1246 ns |  12.9731 ns |    497.6198 ns |    473.9336 ns |    524.6933 ns | 0.1588 |      - |    2504 B |
| &#39;HashResult.To&lt;string&gt; (converter)&#39; | .NET 9.0  | 32   |     16.9401 ns |   0.3920 ns |   0.4357 ns |     16.9590 ns |     15.9413 ns |     17.6572 ns | 0.0071 |      - |     112 B |
| **&#39;HashResult.GetBytes - copy bytes&#39;**  | **.NET 10.0** | **256**  |     **11.4939 ns** |   **0.3858 ns** |   **0.4288 ns** |     **11.4158 ns** |     **11.0227 ns** |     **12.5469 ns** | **0.0178** |      **-** |     **280 B** |
| HashResult.ToHexadecimalString      | .NET 10.0 | 256  |    138.3004 ns |   4.2830 ns |   4.5828 ns |    136.8480 ns |    133.0867 ns |    148.6847 ns | 0.1332 |      - |    2096 B |
| HashResult.ToBase64String           | .NET 10.0 | 256  |     39.7533 ns |   1.1674 ns |   1.2492 ns |     39.6671 ns |     38.0315 ns |     42.4357 ns | 0.0454 |      - |     712 B |
| HashResult.ToUrlEncodedBase64String | .NET 10.0 | 256  |    219.5320 ns |  19.7087 ns |  21.0880 ns |    226.8784 ns |    180.5383 ns |    242.1412 ns | 0.1843 | 0.0007 |    2896 B |
| HashResult.ToBinaryString           | .NET 10.0 | 256  |  3,539.5018 ns |  69.8744 ns |  58.3483 ns |  3,518.4114 ns |  3,466.0494 ns |  3,672.2965 ns | 1.2185 |      - |   19296 B |
| &#39;HashResult.To&lt;string&gt; (converter)&#39; | .NET 10.0 | 256  |     39.8802 ns |   1.1496 ns |   1.2300 ns |     39.9794 ns |     37.7491 ns |     41.9781 ns | 0.0453 |      - |     712 B |
| &#39;HashResult.GetBytes - copy bytes&#39;  | .NET 9.0  | 256  |     13.3596 ns |   0.3896 ns |   0.4330 ns |     13.2252 ns |     12.6234 ns |     14.0785 ns | 0.0178 |      - |     280 B |
| HashResult.ToHexadecimalString      | .NET 9.0  | 256  |    141.0557 ns |   6.4560 ns |   6.9078 ns |    138.8517 ns |    131.7096 ns |    157.0699 ns | 0.1333 |      - |    2096 B |
| HashResult.ToBase64String           | .NET 9.0  | 256  |     43.5091 ns |   1.8772 ns |   2.0865 ns |     42.6208 ns |     41.4190 ns |     48.7367 ns | 0.0453 |      - |     712 B |
| HashResult.ToUrlEncodedBase64String | .NET 9.0  | 256  |    188.2106 ns |   5.0923 ns |   5.6601 ns |    188.1370 ns |    177.2755 ns |    197.2722 ns | 0.1845 |      - |    2896 B |
| HashResult.ToBinaryString           | .NET 9.0  | 256  |  4,173.1435 ns |  76.6772 ns |  82.0437 ns |  4,173.7716 ns |  4,034.5332 ns |  4,356.8020 ns | 1.2268 |      - |   19296 B |
| &#39;HashResult.To&lt;string&gt; (converter)&#39; | .NET 9.0  | 256  |     51.0554 ns |   5.8049 ns |   6.6849 ns |     51.6895 ns |     42.6319 ns |     62.0578 ns | 0.0453 |      - |     712 B |
| **&#39;HashResult.GetBytes - copy bytes&#39;**  | **.NET 10.0** | **1024** |     **35.5398 ns** |   **1.5371 ns** |   **1.7084 ns** |     **35.0012 ns** |     **34.0142 ns** |     **39.8812 ns** | **0.0668** |      **-** |    **1048 B** |
| HashResult.ToHexadecimalString      | .NET 10.0 | 1024 |    785.5117 ns |  34.4237 ns |  36.8329 ns |    791.4329 ns |    706.7059 ns |    844.9607 ns | 0.5240 |      - |    8240 B |
| HashResult.ToBase64String           | .NET 10.0 | 1024 |    187.2059 ns |  11.5405 ns |  12.8272 ns |    187.5552 ns |    165.1308 ns |    213.2597 ns | 0.1755 |      - |    2760 B |
| HashResult.ToUrlEncodedBase64String | .NET 10.0 | 1024 |    593.8573 ns |  19.6008 ns |  21.7863 ns |    584.5445 ns |    556.0342 ns |    639.8617 ns | 0.7063 | 0.0138 |   11088 B |
| HashResult.ToBinaryString           | .NET 10.0 | 1024 | 15,381.0694 ns | 501.3670 ns | 536.4571 ns | 15,534.9627 ns | 14,184.9841 ns | 16,238.7977 ns | 4.8359 |      - |   76504 B |
| &#39;HashResult.To&lt;string&gt; (converter)&#39; | .NET 10.0 | 1024 |    141.7310 ns |   4.3182 ns |   4.2410 ns |    142.0612 ns |    136.4711 ns |    148.5489 ns | 0.1755 |      - |    2760 B |
| &#39;HashResult.GetBytes - copy bytes&#39;  | .NET 9.0  | 1024 |     80.2916 ns |   4.4464 ns |   5.1205 ns |     79.2496 ns |     72.3253 ns |     91.5821 ns | 0.0667 |      - |    1048 B |
| HashResult.ToHexadecimalString      | .NET 9.0  | 1024 |    851.1094 ns |  17.0267 ns |  18.2184 ns |    851.8305 ns |    809.7471 ns |    882.5554 ns | 0.5227 |      - |    8240 B |
| HashResult.ToBase64String           | .NET 9.0  | 1024 |    192.5303 ns |  13.7382 ns |  14.6998 ns |    192.2895 ns |    162.2138 ns |    218.0732 ns | 0.1759 |      - |    2760 B |
| HashResult.ToUrlEncodedBase64String | .NET 9.0  | 1024 |    900.7466 ns |  38.9886 ns |  43.3357 ns |    909.7328 ns |    813.3357 ns |    980.4328 ns | 0.7040 | 0.0116 |   11088 B |
| HashResult.ToBinaryString           | .NET 9.0  | 1024 | 20,039.3641 ns | 485.4824 ns | 559.0822 ns | 19,854.9229 ns | 19,113.9477 ns | 21,336.6594 ns | 4.8257 |      - |   76504 B |
| &#39;HashResult.To&lt;string&gt; (converter)&#39; | .NET 9.0  | 1024 |    189.3123 ns |  17.7942 ns |  20.4918 ns |    184.8754 ns |    153.5934 ns |    240.6874 ns | 0.1755 |      - |    2760 B |
