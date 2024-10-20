using Basmus.Parametrization;

namespace Basmus.Parameterization.Examples;

public class Program
{
    public static void Main(string[] args)
    {
        RunNamedParameterExample();
        RunPositionalParameterExample();
        Console.ReadLine();
    }

    private static void RunNamedParameterExample()
    {
        var id = 1;
        var name = "John";
        var date = DateTime.Now;

        FormattableString fs = $"User {name.AsParameter("name")} with ID {id.AsParameter("id")} was created on {date.AsParameter()}";
        Console.WriteLine($"Original: {fs.ToString()}");

        var parameters = ParameterHelper.Parametrize(fs, new MyNamedPlaceholderBuilder());

        Console.WriteLine($"Parametrized: {fs.ToString()}");
        Console.WriteLine("Parameters:");
        foreach (var parameter in parameters)
        {
            Console.WriteLine($"{parameter.Key}: {parameter.Value}");
        }
    }

    private static void RunPositionalParameterExample()
    {
        var id = 1;
        var name = "John";

        FormattableString fs = $"User {name.AsParameter()} with ID {id.AsParameter()}";
        Console.WriteLine($"Original: {fs.ToString()}");

        var parameters = ParameterHelper.Parametrize(fs, new MyPositionalPlaceholderBuilder());

        Console.WriteLine($"Parametrized: {fs.ToString()}");
        Console.WriteLine("Parameters:");
        for (int i = 0; i < parameters.Length; i++)
        {
            Console.WriteLine($"[{i}]: {parameters[i]}");
        }
    }

    private class MyNamedPlaceholderBuilder : INamedPlaceholderBuilder
    {
        public string Build(string name, int? nameDuplicateIndex)
        {
            return nameDuplicateIndex.HasValue ? $"@{name}{nameDuplicateIndex}" : $"@{name}";
        }

        public string Build(int unnamedParameterPlaceholderIndex)
        {
            return $"@p{unnamedParameterPlaceholderIndex}";
        }
    }

    private class MyPositionalPlaceholderBuilder : IPositionalPlaceholderBuilder
    {
        public string Build(int index)
        {
            return $"${index}";
        }
    }
}
