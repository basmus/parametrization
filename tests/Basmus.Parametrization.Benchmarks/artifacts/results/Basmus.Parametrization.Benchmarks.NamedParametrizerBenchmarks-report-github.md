```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22624.1465)
Intel Core i5-10400 CPU 2.90GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.203
  [Host] : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2

Toolchain=InProcessNoEmitToolchain  UnrollFactor=1  

```
| Method            | Job        | InvocationCount | Mean       | Error     | StdDev     | Gen0    | Gen1   | Gen2   | Allocated |
|------------------ |----------- |---------------- |-----------:|----------:|-----------:|--------:|-------:|-------:|----------:|
| &#39;1000 parameters&#39; | Job-KOXPXP | 1000            | 352.170 μs | 6.9970 μs | 12.2547 μs | 28.0000 | 8.0000 | 1.0000 | 169.86 KB |
| &#39;100 parameters&#39;  | Job-REXTEZ | 10000           |  38.221 μs | 0.7526 μs |  0.7040 μs |  2.8000 | 0.8000 | 0.1000 |  17.12 KB |
| &#39;10 parameters&#39;   | Job-RIYMMI | 100000          |   4.655 μs | 0.0729 μs |  0.0682 μs |  0.3500 | 0.0900 |      - |   2.19 KB |
