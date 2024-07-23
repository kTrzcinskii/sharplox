using sharplox.Exceptions;
using sharplox.Tokens;

namespace sharplox.Services;

public class Environment
{
    private readonly Environment? _parentEnvironment;
    private readonly Dictionary<string, object?> _values = new Dictionary<string, object?>();

    public Environment()
    {
        _parentEnvironment = null;
    }

    public Environment(Environment? parentEnvironment)
    {
        _parentEnvironment = parentEnvironment;
    }

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

        if (_parentEnvironment != null)
        {
            _parentEnvironment.Assign(name, value);
            return;
        }

        throw new RuntimeException(name, $"Undefined variable '{name.Lexeme}'");
    }

    public object? Get(Token name)
    {
        if (_values.TryGetValue(name.Lexeme, out object? value))
            return value;

        if (_parentEnvironment != null)
            return _parentEnvironment.Get(name);
            
        throw new RuntimeException(name, $"Undefined variable '{name.Lexeme}'.");
    }
}