using sharplox.Exceptions;
using sharplox.Tokens;

namespace sharplox.BuiltIns;

public class LoxInstance
{
    private LoxClass _loxClass;
    private readonly Dictionary<string, object?> _fields = new Dictionary<string, object?>();

    public LoxInstance(LoxClass loxLoxClass)
    {
        _loxClass = loxLoxClass;
    }

    public override string ToString()
    {
        return $"{_loxClass} instance";
    }
    
    public object? Get(Token name)
    {
        if (_fields.TryGetValue(name.Lexeme, out object? field))
            return field;
        throw new RuntimeException(name, $"Undefined property {name.Lexeme}.");
    }

    public void Set(Token name, object? value)
    {
        _fields[name.Lexeme] = value;
    }
}