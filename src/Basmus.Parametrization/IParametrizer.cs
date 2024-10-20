namespace Basmus.Parametrization;

public interface IParametrizer
{
    void PreparePlaceholders(IEnumerable<Parameter> parameters);
}