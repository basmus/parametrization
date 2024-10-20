namespace Basmus.Parametrization;

public interface INamedPlaceholderBuilder
{
    string Build(string name, int? nameDuplicateIndex);
    string Build(int unnamedParameterPlaceholderIndex);
}