namespace Basmus.Parametrization;

public static class ParameterHelper
{
    public static IDictionary<string, object> Parametrize(FormattableString formattableString, INamedPlaceholderBuilder namedPlaceholderBuilder)
    {
        var parameters = new ParameterCollector()
            .Collect(formattableString)
            .ToArray();

        new NamedParametrizer(namedPlaceholderBuilder)
            .PreparePlaceholders(parameters);

        foreach (var parameter in parameters.ToArray())
        {
            parameter.ApplyPlaceholder();
        }

        return parameters.ToDictionary(x => x.Placeholder.Value, x => x.Value);
    }

    public static object[] Parametrize(FormattableString formattableString, IPositionalPlaceholderBuilder positionalPlaceholderBuilder)
    {
        var parameters = new ParameterCollector()
            .Collect(formattableString)
            .ToArray();

        new PositionalParametrizer(positionalPlaceholderBuilder)
            .PreparePlaceholders(parameters);

        foreach (var parameter in parameters)
        {
            parameter.ApplyPlaceholder();
        }

        return parameters
            .OrderBy(x => x.Placeholder.Index)
            .Select(x => x.Value)
            .ToArray();
    }
}