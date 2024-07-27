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

    public void AssignAt(int depth, Token name, object? value)
    {
        // At this point we don't event check if variable is defined - we assume that Resolver did its job.
        Ancestor(depth)._values[name.Lexeme] = value;
    }

    public object? Get(Token name)
    {
        if (_values.TryGetValue(name.Lexeme, out object? value))
            return value;

        if (_parentEnvironment != null)
            return _parentEnvironment.Get(name);
            
        throw new RuntimeException(name, $"Undefined variable '{name.Lexeme}'.");
    }

    public object? GetAt(int depth, string name)
    {
        // At this point we don't event check if variable is defined - we assume that Resolver did its job.
        return Ancestor(depth)._values[name];
    }

    private Environment Ancestor(int depth)
    {
        var environment = this;
        // At this point we assume depth is correct value and dont check for null reference - we might want to change it later to some Lox internal error (as it's not user fault, but rather problem with our logic).
        for (int i = 0; i < depth; i++)
            environment = environment!._parentEnvironment;
        return environment!;
    }
}