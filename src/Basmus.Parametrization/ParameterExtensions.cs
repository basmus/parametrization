namespace Basmus.Parametrization;

public static class ParameterExtensions
{
    public static Parameter AsParameter(this object value, string? name = null)
    {
        return new Parameter(name, value);
    }
}