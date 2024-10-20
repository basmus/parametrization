namespace Basmus.Parametrization.Queries;

public class DefaultPositionalPlaceholderBuilder : IPositionalPlaceholderBuilder
{
    private readonly string _symbol;
    private readonly bool _asPrefix;

    public DefaultPositionalPlaceholderBuilder(string symbol, bool asPrefix)
    {
        _symbol = symbol;
        _asPrefix = asPrefix;
    }

    public static IPositionalPlaceholderBuilder ColonPrefix => new DefaultPositionalPlaceholderBuilder(":", true);
    public static IPositionalPlaceholderBuilder DollarPrefix => new DefaultPositionalPlaceholderBuilder("$", true);
    public static IPositionalPlaceholderBuilder QuestionMarkPrefix => new DefaultPositionalPlaceholderBuilder("?", true);
    public static IPositionalPlaceholderBuilder QuestionMark => new DefaultPositionalPlaceholderBuilder("?", false);

    public string Build(int index) => _asPrefix ? $"{_symbol}{index}" : _symbol;
}