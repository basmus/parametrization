namespace Basmus.Parametrization;

public readonly struct Placeholder
{
    private readonly int? _index;

    public string Value { get; }

    public bool HasIndex => _index.HasValue;

    public int Index => HasIndex
        ? _index!.Value
        : throw new InvalidOperationException();

    public Placeholder(string value)
    {
        Value = value;
    }

    public Placeholder(string value, int index)
    {
        Value = value;
        _index = index;
    }
}