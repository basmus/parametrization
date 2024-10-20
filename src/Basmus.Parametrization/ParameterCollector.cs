namespace Basmus.Parametrization;

public class ParameterCollector
{
    public IEnumerable<Parameter> Collect(FormattableString formattableString)
    {
        var parameters = new List<Parameter>();

        Traversal
            .Create()
            .With(new FormattableStringProcessor())
            .With(new ParameterProcessor(parameters))
            .Traverse(formattableString);

        return parameters;
    }
}