namespace Basmus.Parametrization;

public interface IProcessor
{
    void Process(object item, IProcessor processor);
}