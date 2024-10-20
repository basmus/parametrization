namespace Basmus.Parametrization;

public class ParameterProcessor : IProcessor
{
    private readonly List<Parameter> _parameters;

    public ParameterProcessor(List<Parameter> parameters)
    {
        _parameters = parameters;
    }

    public void Process(object item, IProcessor processor)
    {
        if (item is not Parameter parameter)
        {
            return;
        }

        _parameters.Add(parameter);
    }
}