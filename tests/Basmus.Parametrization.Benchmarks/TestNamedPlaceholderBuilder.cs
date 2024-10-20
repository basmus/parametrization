namespace Basmus.Parametrization.Benchmarks;

internal class TestNamedPlaceholderBuilder : INamedPlaceholderBuilder
{
    public string Build(string name, int? nameDuplicateIndex)
    {
        return nameDuplicateIndex.HasValue ? $"@{name}{nameDuplicateIndex}" : $"@{name}";
    }

    public string Build(int unnamedParameterPlaceholderIndex)
    {
        return $"@p{unnamedParameterPlaceholderIndex}";
    }
}