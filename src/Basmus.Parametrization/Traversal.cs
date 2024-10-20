namespace Basmus.Parametrization;

public class Traversal
{
    private readonly List<IProcessor> _processors;

    private Traversal(IProcessor[] processors)
    {
        _processors = new List<IProcessor>(processors);
    }

    public static Traversal Create(params IProcessor[] processors)
    {
        return new Traversal(processors);
    }

    public Traversal With(params IProcessor[] processors)
    {
        _processors.AddRange(processors);
        return this;
    }

    public void Traverse(object item)
    {
        var compositeProcessor = new CompositeProcessor(_processors);
        compositeProcessor.Process(item, compositeProcessor);
    }
}