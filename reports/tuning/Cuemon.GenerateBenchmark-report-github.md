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
| Method                           | Runtime   | Count | Mean          | Error          | StdDev         | Median         | Min           | Max            | Gen0   | Allocated |
|--------------------------------- |---------- |------ |--------------:|---------------:|---------------:|---------------:|--------------:|---------------:|-------:|----------:|
| **&#39;RangeOf - enumerate&#39;**            | **.NET 10.0** | **8**     |     **13.752 ns** |      **0.3084 ns** |      **0.2884 ns** |      **13.665 ns** |     **13.336 ns** |      **14.343 ns** | **0.0036** |      **56 B** |
| &#39;RandomNumber - default&#39;         | .NET 10.0 | 8     |      4.631 ns |      0.4425 ns |      0.5096 ns |       4.783 ns |      2.678 ns |       5.064 ns |      - |         - |
| &#39;RandomNumber - bounded&#39;         | .NET 10.0 | 8     |      5.379 ns |      0.1240 ns |      0.1274 ns |       5.371 ns |      5.100 ns |       5.649 ns |      - |         - |
| FixedString                      | .NET 10.0 | 8     |      8.546 ns |      0.3617 ns |      0.4020 ns |       8.434 ns |      7.932 ns |       9.362 ns | 0.0025 |      40 B |
| &#39;RandomString - letters/numbers&#39; | .NET 10.0 | 8     |    155.307 ns |     22.8554 ns |     26.3203 ns |     169.168 ns |    107.429 ns |     173.531 ns | 0.0048 |      80 B |
| &#39;ObjectPortrayal - basic object&#39; | .NET 10.0 | 8     |  1,116.025 ns |    229.0145 ns |    245.0430 ns |     983.155 ns |    904.655 ns |   1,543.517 ns | 0.1835 |    3057 B |
| &#39;HashCode32 - params&#39;            | .NET 10.0 | 8     |    274.385 ns |     26.9432 ns |     31.0279 ns |     282.782 ns |    188.446 ns |     311.292 ns | 0.0737 |    1160 B |
| &#39;HashCode32 - enumerable&#39;        | .NET 10.0 | 8     |    576.608 ns |     72.6174 ns |     80.7140 ns |     610.751 ns |    393.246 ns |     628.250 ns | 0.1253 |    1984 B |
| &#39;HashCode64 - params&#39;            | .NET 10.0 | 8     |    266.472 ns |     39.5951 ns |     44.0099 ns |     284.667 ns |    168.383 ns |     307.818 ns | 0.0733 |    1160 B |
| &#39;HashCode64 - enumerable&#39;        | .NET 10.0 | 8     |    580.757 ns |     50.6152 ns |     58.2885 ns |     597.811 ns |    394.791 ns |     630.709 ns | 0.1260 |    1984 B |
| &#39;RangeOf - enumerate&#39;            | .NET 9.0  | 8     |     25.364 ns |      3.1801 ns |      3.6622 ns |      26.547 ns |     14.559 ns |      27.277 ns | 0.0036 |      56 B |
| &#39;RandomNumber - default&#39;         | .NET 9.0  | 8     |     11.774 ns |      0.2125 ns |      0.1988 ns |      11.794 ns |     11.454 ns |      12.179 ns |      - |         - |
| &#39;RandomNumber - bounded&#39;         | .NET 9.0  | 8     |     12.490 ns |      0.8992 ns |      0.9621 ns |      12.732 ns |      8.690 ns |      13.048 ns |      - |         - |
| FixedString                      | .NET 9.0  | 8     |     10.354 ns |      0.3191 ns |      0.3546 ns |      10.368 ns |      9.358 ns |      11.065 ns | 0.0025 |      40 B |
| &#39;RandomString - letters/numbers&#39; | .NET 9.0  | 8     |    246.538 ns |     41.4913 ns |     46.1175 ns |     264.655 ns |    119.373 ns |     271.581 ns | 0.0049 |      80 B |
| &#39;ObjectPortrayal - basic object&#39; | .NET 9.0  | 8     |  1,077.889 ns |     45.0992 ns |     51.9363 ns |   1,066.710 ns |  1,019.233 ns |   1,199.251 ns | 0.2324 |    3809 B |
| &#39;HashCode32 - params&#39;            | .NET 9.0  | 8     |    341.248 ns |     43.4503 ns |     50.0375 ns |     364.476 ns |    217.524 ns |     381.623 ns | 0.0742 |    1168 B |
| &#39;HashCode32 - enumerable&#39;        | .NET 9.0  | 8     |    717.778 ns |     74.2399 ns |     85.4947 ns |     734.604 ns |    422.259 ns |     806.599 ns | 0.1267 |    2008 B |
| &#39;HashCode64 - params&#39;            | .NET 9.0  | 8     |    337.140 ns |     56.1433 ns |     64.6547 ns |     367.770 ns |    218.481 ns |     397.841 ns | 0.0736 |    1168 B |
| &#39;HashCode64 - enumerable&#39;        | .NET 9.0  | 8     |    663.564 ns |    113.1051 ns |    121.0212 ns |     723.730 ns |    414.056 ns |     740.452 ns | 0.1261 |    1984 B |
| **&#39;RangeOf - enumerate&#39;**            | **.NET 10.0** | **256**   |    **342.777 ns** |      **6.6366 ns** |      **6.5181 ns** |     **340.232 ns** |    **337.576 ns** |     **359.620 ns** | **0.0028** |      **56 B** |
| &#39;RandomNumber - default&#39;         | .NET 10.0 | 256   |      3.559 ns |      1.3191 ns |      1.5191 ns |       3.131 ns |      2.026 ns |       5.534 ns |      - |         - |
| &#39;RandomNumber - bounded&#39;         | .NET 10.0 | 256   |      3.365 ns |      0.6758 ns |      0.7512 ns |       3.032 ns |      2.823 ns |       5.315 ns |      - |         - |
| FixedString                      | .NET 10.0 | 256   |     46.265 ns |      5.8475 ns |      6.7339 ns |      44.579 ns |     34.073 ns |      60.192 ns | 0.0341 |     536 B |
| &#39;RandomString - letters/numbers&#39; | .NET 10.0 | 256   |  4,345.017 ns |    542.1830 ns |    624.3787 ns |   4,602.454 ns |  2,892.183 ns |   4,732.553 ns | 0.0578 |    1072 B |
| &#39;ObjectPortrayal - basic object&#39; | .NET 10.0 | 256   |  1,123.323 ns |    282.6378 ns |    314.1511 ns |     941.517 ns |    904.634 ns |   1,929.718 ns | 0.1844 |    3057 B |
| &#39;HashCode32 - params&#39;            | .NET 10.0 | 256   |    265.279 ns |     37.9712 ns |     42.2049 ns |     284.814 ns |    176.163 ns |     307.581 ns | 0.0722 |    1136 B |
| &#39;HashCode32 - enumerable&#39;        | .NET 10.0 | 256   |    575.823 ns |     70.9475 ns |     78.8580 ns |     608.621 ns |    373.591 ns |     628.460 ns | 0.1262 |    1984 B |
| &#39;HashCode64 - params&#39;            | .NET 10.0 | 256   |    278.682 ns |     30.4411 ns |     35.0560 ns |     290.453 ns |    179.047 ns |     307.092 ns | 0.0723 |    1136 B |
| &#39;HashCode64 - enumerable&#39;        | .NET 10.0 | 256   |    611.716 ns |     68.1448 ns |     78.4756 ns |     630.702 ns |    360.546 ns |     668.071 ns | 0.1259 |    1984 B |
| &#39;RangeOf - enumerate&#39;            | .NET 9.0  | 256   |    379.536 ns |     25.4274 ns |     28.2624 ns |     388.060 ns |    319.260 ns |     412.371 ns | 0.0026 |      56 B |
| &#39;RandomNumber - default&#39;         | .NET 9.0  | 256   |     11.568 ns |      0.2541 ns |      0.2495 ns |      11.570 ns |     11.167 ns |      11.933 ns |      - |         - |
| &#39;RandomNumber - bounded&#39;         | .NET 9.0  | 256   |     12.196 ns |      1.3966 ns |      1.6084 ns |      12.688 ns |      6.052 ns |      13.445 ns |      - |         - |
| FixedString                      | .NET 9.0  | 256   |     42.005 ns |      1.0933 ns |      1.2152 ns |      41.727 ns |     40.256 ns |      44.115 ns | 0.0341 |     536 B |
| &#39;RandomString - letters/numbers&#39; | .NET 9.0  | 256   |  7,217.224 ns |    140.6265 ns |    161.9457 ns |   7,272.145 ns |  6,796.457 ns |   7,471.423 ns | 0.0605 |    1072 B |
| &#39;ObjectPortrayal - basic object&#39; | .NET 9.0  | 256   |  1,128.344 ns |     49.4649 ns |     48.5812 ns |   1,111.535 ns |  1,066.252 ns |   1,268.186 ns | 0.2422 |    3809 B |
| &#39;HashCode32 - params&#39;            | .NET 9.0  | 256   |    367.516 ns |     40.1032 ns |     46.1829 ns |     385.965 ns |    242.714 ns |     401.743 ns | 0.0738 |    1168 B |
| &#39;HashCode32 - enumerable&#39;        | .NET 9.0  | 256   |    714.650 ns |     72.0194 ns |     82.9377 ns |     742.707 ns |    450.357 ns |     766.006 ns | 0.1247 |    1984 B |
| &#39;HashCode64 - params&#39;            | .NET 9.0  | 256   |    345.928 ns |     44.8874 ns |     51.6924 ns |     369.129 ns |    229.526 ns |     403.120 ns | 0.0739 |    1168 B |
| &#39;HashCode64 - enumerable&#39;        | .NET 9.0  | 256   |    679.573 ns |     79.5320 ns |     91.5892 ns |     712.927 ns |    410.320 ns |     743.815 ns | 0.1250 |    1984 B |
| **&#39;RangeOf - enumerate&#39;**            | **.NET 10.0** | **4096**  |  **4,845.266 ns** |     **93.1043 ns** |     **77.7463 ns** |   **4,826.403 ns** |  **4,783.734 ns** |   **5,080.302 ns** |      **-** |      **56 B** |
| &#39;RandomNumber - default&#39;         | .NET 10.0 | 4096  |      6.315 ns |      0.1756 ns |      0.1803 ns |       6.304 ns |      5.810 ns |       6.577 ns |      - |         - |
| &#39;RandomNumber - bounded&#39;         | .NET 10.0 | 4096  |      6.831 ns |      0.0691 ns |      0.0577 ns |       6.829 ns |      6.698 ns |       6.944 ns |      - |         - |
| FixedString                      | .NET 10.0 | 4096  |    630.344 ns |     27.6595 ns |     29.5954 ns |     632.391 ns |    525.400 ns |     669.824 ns | 0.5234 |    8216 B |
| &#39;RandomString - letters/numbers&#39; | .NET 10.0 | 4096  | 68,699.329 ns |  9,692.4837 ns | 11,161.8786 ns |  73,975.763 ns | 44,693.966 ns |  76,344.037 ns | 0.8980 |   16432 B |
| &#39;ObjectPortrayal - basic object&#39; | .NET 10.0 | 4096  |    999.553 ns |     95.9858 ns |    102.7038 ns |     961.020 ns |    903.164 ns |   1,206.619 ns | 0.1903 |    3057 B |
| &#39;HashCode32 - params&#39;            | .NET 10.0 | 4096  |    277.088 ns |     36.7918 ns |     40.8939 ns |     295.769 ns |    187.439 ns |     314.978 ns | 0.0722 |    1136 B |
| &#39;HashCode32 - enumerable&#39;        | .NET 10.0 | 4096  |    560.948 ns |     86.6765 ns |     99.8168 ns |     607.525 ns |    369.753 ns |     656.535 ns | 0.1265 |    1984 B |
| &#39;HashCode64 - params&#39;            | .NET 10.0 | 4096  |    278.117 ns |     31.3709 ns |     34.8687 ns |     291.652 ns |    189.096 ns |     296.214 ns | 0.0719 |    1136 B |
| &#39;HashCode64 - enumerable&#39;        | .NET 10.0 | 4096  |    628.799 ns |     48.6570 ns |     56.0334 ns |     642.454 ns |    422.388 ns |     676.304 ns | 0.1250 |    1984 B |
| &#39;RangeOf - enumerate&#39;            | .NET 9.0  | 4096  |  5,703.199 ns |    309.7010 ns |    344.2319 ns |   5,869.904 ns |  5,068.472 ns |   6,035.801 ns |      - |      56 B |
| &#39;RandomNumber - default&#39;         | .NET 9.0  | 4096  |     11.640 ns |      0.4271 ns |      0.4919 ns |      11.695 ns |     10.017 ns |      12.320 ns |      - |         - |
| &#39;RandomNumber - bounded&#39;         | .NET 9.0  | 4096  |     12.426 ns |      0.3414 ns |      0.3506 ns |      12.526 ns |     11.827 ns |      13.128 ns |      - |         - |
| FixedString                      | .NET 9.0  | 4096  |    617.267 ns |     80.6150 ns |     86.2571 ns |     638.831 ns |    322.960 ns |     698.689 ns | 0.5233 |    8216 B |
| &#39;RandomString - letters/numbers&#39; | .NET 9.0  | 4096  | 98,723.980 ns | 25,927.6097 ns | 29,858.2739 ns | 116,603.855 ns | 48,780.958 ns | 120,045.444 ns | 0.9735 |   16432 B |
| &#39;ObjectPortrayal - basic object&#39; | .NET 9.0  | 4096  |  1,204.026 ns |    165.4206 ns |    176.9982 ns |   1,115.564 ns |  1,032.866 ns |   1,574.951 ns | 0.2342 |    3809 B |
| &#39;HashCode32 - params&#39;            | .NET 9.0  | 4096  |    339.094 ns |     48.8290 ns |     54.2733 ns |     365.069 ns |    223.956 ns |     375.490 ns | 0.0756 |    1192 B |
| &#39;HashCode32 - enumerable&#39;        | .NET 9.0  | 4096  |    722.856 ns |     31.2564 ns |     33.4439 ns |     730.860 ns |    593.169 ns |     743.857 ns | 0.1249 |    1984 B |
| &#39;HashCode64 - params&#39;            | .NET 9.0  | 4096  |    357.981 ns |     42.3492 ns |     47.0710 ns |     376.756 ns |    237.628 ns |     390.329 ns | 0.0742 |    1168 B |
| &#39;HashCode64 - enumerable&#39;        | .NET 9.0  | 4096  |    662.406 ns |    118.8854 ns |    136.9087 ns |     730.940 ns |    407.273 ns |     781.566 ns | 0.1258 |    1984 B |
