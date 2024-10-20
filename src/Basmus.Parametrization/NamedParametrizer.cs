namespace Basmus.Parametrization;

public class NamedParametrizer : IParametrizer
{
    private readonly INamedPlaceholderBuilder _namedPlaceholderBuilder;

    public NamedParametrizer(INamedPlaceholderBuilder namedPlaceholderBuilder)
    {
        _namedPlaceholderBuilder = namedPlaceholderBuilder;
    }

    public void PreparePlaceholders(IEnumerable<Parameter> parameters)
    {
        var parametersArray = parameters
            .Distinct()
            .ToArray();

        var namedParameters = parametersArray
            .Where(x => x.HasName)
            .ToArray();

        var namedParameterPlaceholders = SetPlaceholdersToNamedParameters(namedParameters);

        var unnamedParameters = parametersArray.Where(p => !p.HasName);
        SetPlaceholdersToUnnamedParameters(unnamedParameters, namedParameterPlaceholders);
    }

    private HashSet<string> SetPlaceholdersToNamedParameters(Parameter[] namedParameters)
    {
        var nameDuplicates = new Dictionary<string, int>();

        var names = new HashSet<string>();

        foreach (var namedParameter in namedParameters)
        {
            var name = namedParameter.Name;
            if (!names.Add(name))
            {
                nameDuplicates.TryAdd(name, 0);
            }
        }

        HashSet<string> usedPlaceholder = new();

        foreach (var parameter in namedParameters)
        {
            int? index = null;
            if (nameDuplicates.ContainsKey(parameter.Name))
            {
                index = nameDuplicates[parameter.Name];
                nameDuplicates[parameter.Name]++;
            }

            var placeholder = _namedPlaceholderBuilder.Build(parameter.Name, index);

            ThrowIfNamedPlaceholderIsUsed(placeholder, usedPlaceholder, parameter.Name, index);

            parameter.SetPlaceholder(placeholder);
            usedPlaceholder.Add(placeholder);
        }

        return usedPlaceholder;
    }

    private void SetPlaceholdersToUnnamedParameters(IEnumerable<Parameter> unnamedParameters, IReadOnlySet<string> reservedPlaceholder)
    {
        HashSet<string> usedPlaceholder = new();
        var allowedConflictNumber = reservedPlaceholder.Count;
        var placeholderIndex = -1;

        foreach (var parameter in unnamedParameters)
        {
            placeholderIndex++;
            var placeholder = _namedPlaceholderBuilder.Build(placeholderIndex);

            while (reservedPlaceholder.Contains(placeholder))
            {
                ThrowIfPlaceholderConflictNumberExceeded(placeholder, allowedConflictNumber);

                allowedConflictNumber--;
                placeholderIndex++;
                placeholder = _namedPlaceholderBuilder.Build(placeholderIndex);
            }

            ThrowIfUnnamedPlaceholderIsUsed(placeholder, usedPlaceholder, placeholderIndex);

            parameter.SetPlaceholder(placeholder);
            usedPlaceholder.Add(placeholder);
        }
    }

    private static void ThrowIfNamedPlaceholderIsUsed(
        string placeholder,
        HashSet<string> usedPlaceholders,
        string name,
        int? index)
    {
        if (!usedPlaceholders.Contains(placeholder))
        {
            return;
        }

        var message = $"Named placeholder '{placeholder}' is already in use. Name: '{name}', Index: '{index}'";
        throw new ImproperPlaceholderBuilderException(message);
    }

    private static void ThrowIfUnnamedPlaceholderIsUsed(
        string placeholder,
        IReadOnlySet<string> usedPlaceholder,
        int index)
    {
        if (!usedPlaceholder.Contains(placeholder))
        {
            return;
        }

        var message = $"Unnamed placeholder '{placeholder}' is already in use. Index: '{index}'";
        throw new ImproperPlaceholderBuilderException(message);
    }

    private static void ThrowIfPlaceholderConflictNumberExceeded(string placeholder, int allowedConflictNumber)
    {
        if (allowedConflictNumber != 0)
        {
            return;
        }

        var message = $"Placeholder conflict was not resolved in the expected number of attempts. The last unsuccessful placeholder is '{placeholder}'";
        throw new ImproperPlaceholderBuilderException(message);
    }
}