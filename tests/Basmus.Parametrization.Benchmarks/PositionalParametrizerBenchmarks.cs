using BenchmarkDotNet.Attributes;

namespace Basmus.Parametrization.Benchmarks;

[Config(typeof(BenchmarkConfig))]
[MemoryDiagnoser]
public class PositionalParametrizerBenchmarks
{
    private PositionalParametrizer _parametrizer;
    private Parameter[][] _parameters;
    private int _invocationIndex;

    [GlobalSetup]
    public void Setup()
    {
        var builder = new TestPositionalPlaceholderBuilder();
        _parametrizer = new PositionalParametrizer(builder);
    }

    [IterationSetup(Target = nameof(SmallParameterSet))]
    public void SetupSmall()
    {
        _parameters = GenerateParameters(100000, 10);
        _invocationIndex = 0;
    }

    [Benchmark(Description = "10 parameters")]
    [InvocationCount(100000)]
    public void SmallParameterSet()
    {
        _parametrizer.PreparePlaceholders(_parameters[_invocationIndex]);
        _invocationIndex++;
    }

    [IterationSetup(Target = nameof(MediumParameterSet))]
    public void SetupMedium()
    {
        _parameters = GenerateParameters(10000, 100);
        _invocationIndex = 0;
    }

    [Benchmark(Description = "100 parameters")]
    [InvocationCount(10000)]
    public void MediumParameterSet()
    {
        _parametrizer.PreparePlaceholders(_parameters[_invocationIndex]);
        _invocationIndex++;
    }

    [IterationSetup(Target = nameof(LargeParameterSet))]
    public void SetupLarge()
    {
        _parameters = GenerateParameters(1000, 1000);
        _invocationIndex = 0;
    }

    [Benchmark(Description = "1000 parameters")]
    [InvocationCount(1000)]
    public void LargeParameterSet()
    {
        _parametrizer.PreparePlaceholders(_parameters[_invocationIndex]);
        _invocationIndex++;
    }

    private Parameter[][] GenerateParameters(int invocationCount, int parameterCountPerInvocation)
    {
        return Enumerable
            .Range(0, invocationCount)
            .Select(_ => GenerateParameters(parameterCountPerInvocation))
            .ToArray();
    }

    private static Parameter[] GenerateParameters(int count)
    {
        var parameters = new Parameter[count];
        for (var i = 0; i < count; i++)
        {
            parameters[i] = new Parameter(i);
        }
        return parameters;
    }
} 