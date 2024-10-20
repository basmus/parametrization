namespace Basmus.Parametrization;

public class PositionalParametrizer : IParametrizer
{
    private readonly IPositionalPlaceholderBuilder _positionalPlaceholderBuilder;

    public PositionalParametrizer(IPositionalPlaceholderBuilder positionalPlaceholderBuilder)
    {
        _positionalPlaceholderBuilder = positionalPlaceholderBuilder;
    }

    public void PreparePlaceholders(IEnumerable<Parameter> parameters)
    {
        HashSet<string> usedPlaceholder = new();

        var index = 0;
        foreach (var parameter in parameters)
        {
            var placeholder = _positionalPlaceholderBuilder.Build(index);

            parameter.SetPlaceholder(placeholder, index);

            usedPlaceholder.Add(placeholder);
            index++;
        }
    }
}