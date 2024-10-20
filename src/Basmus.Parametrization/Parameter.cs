namespace Basmus.Parametrization;

public class Parameter
{
    private Placeholder? _placeholder;
    private readonly string? _name;

    public object Value { get; }

    public bool HasName => _name is not null;

    public string Name
    {
        get
        {
            ThrowIfNameIsNotDefined();
            return _name!;
        }
    }

    public bool HasPlaceholder => _placeholder is not null;

    public Placeholder Placeholder
    {
        get
        {
            ThrowIfPlaceholderIsNotSet();
            return _placeholder!.Value;
        }
    }

    public bool PlaceholderIsApplied { get; private set; }

    public Parameter(string? name, object value)
    {
        _name = name;
        Value = value;
    }

    public Parameter(object value)
    {
        Value = value;
    }

    public void SetPlaceholder(string placeholder)
    {
        ThrowIfPlaceholderIsSet();
        _placeholder = new Placeholder(placeholder);
    }

    public void SetPlaceholder(string placeholder, int index)
    {
        ThrowIfPlaceholderIsSet();
        _placeholder = new Placeholder(placeholder, index);
    }

    public void ApplyPlaceholder()
    {
        ThrowIfPlaceholderIsNotSet();
        ThrowIfPlaceholderIsApplied();
        PlaceholderIsApplied = true;
    }

    public override string ToString()
    {
        if (PlaceholderIsApplied)
        {
            return Placeholder.Value;
        }

        return Value.ToString() ?? string.Empty;

    }

    private void ThrowIfPlaceholderIsNotSet()
    {
        if (HasPlaceholder)
        {
            return;
        }

        throw new InvalidOperationException("Placeholder is not set");
    }

    private void ThrowIfPlaceholderIsSet()
    {
        if (!HasPlaceholder)
        {
            return;
        }

        throw new InvalidOperationException("Placeholder is already set");
    }

    private void ThrowIfNameIsNotDefined()
    {
        if (HasName)
        {
            return;
        }

        throw new InvalidOperationException("Name is not defined");
    }

    private void ThrowIfPlaceholderIsApplied()
    {
        if (!PlaceholderIsApplied)
        {
            return;
        }

        throw new InvalidOperationException("Placeholder is already applied");
    }
}