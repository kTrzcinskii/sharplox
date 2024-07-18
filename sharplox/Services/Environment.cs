using sharplox.Errors;
using sharplox.Tokens;

namespace sharplox.Services;

public class Environment
{
    private readonly Dictionary<string, object?> _values = new Dictionary<string, object?>();

    public void Define(string name, object? value)
    {
        _values[name] = value;
    }

    public object? Get(Token name)
    {
        if (_values.TryGetValue(name.Lexeme, out object? value))
            return value;
        throw new RuntimeException(name, $"Undefined variable '{name.Lexeme}'.");
    }
}