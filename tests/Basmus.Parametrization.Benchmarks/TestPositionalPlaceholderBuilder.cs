namespace Basmus.Parametrization.Benchmarks;

internal class TestPositionalPlaceholderBuilder : IPositionalPlaceholderBuilder
{
    public string Build(int index)
    {
        return $":{index}";
    }
}