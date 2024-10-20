namespace Basmus.Parametrization.Queries;

public class DefaultNamedPlaceholderBuilder : INamedPlaceholderBuilder
{
    private readonly string _prefix;
    private readonly string _unnamedParameterPrefix;

    public DefaultNamedPlaceholderBuilder(string prefix, string unnamedParameterPrefix)
    {
        _prefix = prefix;
        _unnamedParameterPrefix = unnamedParameterPrefix;
    }

    public static INamedPlaceholderBuilder AtSymbol => new DefaultNamedPlaceholderBuilder("@", "p");
    public static INamedPlaceholderBuilder Colon => new DefaultNamedPlaceholderBuilder(":", "p");
    public static INamedPlaceholderBuilder Dollar => new DefaultNamedPlaceholderBuilder("$", "p");

    public string Build(string name, int? nameDuplicateIndex)
    {
        if (nameDuplicateIndex.HasValue)
        {
            return $"{_prefix}{name}{nameDuplicateIndex.Value}";
        }

        return $"{_prefix}{name}";
    }

    public string Build(int unnamedParameterPlaceholderIndex) => $"{_prefix}{_unnamedParameterPrefix}{unnamedParameterPlaceholderIndex}";
}