namespace Basmus.Parametrization;

public class FormattableStringProcessor : IProcessor
{
    public void Process(object item, IProcessor processor)
    {
        if (item is not FormattableString fs)
        {
            return;
        }

        foreach (var argument in fs.GetArguments())
        {
            if (argument == null)
            {
                continue;
            }

            processor.Process(argument, processor);
        }
    }
}