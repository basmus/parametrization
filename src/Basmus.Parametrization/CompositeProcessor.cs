namespace Basmus.Parametrization;

public class CompositeProcessor : IProcessor
{
    private readonly List<IProcessor> _processors;

    public CompositeProcessor(IEnumerable<IProcessor> processors)
    {
        _processors = processors.ToList();
    }

    public void Process(object item, IProcessor processor)
    {
        foreach (var concreteProcessor in _processors)
        {
            concreteProcessor.Process(item, processor);
        }
    }
}