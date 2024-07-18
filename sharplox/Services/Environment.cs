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

    public void Assign(Token name, object? value)
    {
        if (_values.ContainsKey(name.Lexeme))
        {
            _values[name.Lexeme] = value;
            return;
        }

        throw new RuntimeException(name, $"Undefined variable '{name.Lexeme}'");
    }

    public object? Get(Token name)
    {
        if (_values.TryGetValue(name.Lexeme, out object? value))
            return value;
        throw new RuntimeException(name, $"Undefined variable '{name.Lexeme}'.");
    }
}