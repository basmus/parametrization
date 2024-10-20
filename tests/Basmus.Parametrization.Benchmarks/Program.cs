using BenchmarkDotNet.Running;

namespace Basmus.Parametrization.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<NamedParametrizerBenchmarks>();
        BenchmarkRunner.Run<PositionalParametrizerBenchmarks>();
    }
} 