```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22624.1465)
Intel Core i5-10400 CPU 2.90GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.203
  [Host] : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2

Toolchain=InProcessNoEmitToolchain  UnrollFactor=1  

```
| Method            | Job        | InvocationCount | Mean       | Error     | StdDev    | Gen0    | Gen1   | Allocated |
|------------------ |----------- |---------------- |-----------:|----------:|----------:|--------:|-------:|----------:|
| &#39;1000 parameters&#39; | Job-KOXPXP | 1000            | 229.073 μs | 4.4601 μs | 4.7722 μs | 16.0000 | 4.0000 | 102.72 KB |
| &#39;100 parameters&#39;  | Job-REXTEZ | 10000           |  24.532 μs | 0.3160 μs | 0.2956 μs |  1.6000 | 0.4000 |  10.36 KB |
| &#39;10 parameters&#39;   | Job-RIYMMI | 100000          |   3.164 μs | 0.0408 μs | 0.0341 μs |  0.1700 | 0.0400 |   1.09 KB |
