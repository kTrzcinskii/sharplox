using sharplox.Services;

namespace sharplox.BuiltIns;

public class LoxClass : ILoxCallable
{
    private readonly string _name;
    private readonly Dictionary<string, LoxFunction> _methods;

    public LoxClass(string name, Dictionary<string, LoxFunction> methods)
    {
        _name = name;
        _methods = methods;
    }

    public override string ToString()
    {
        return _name;
    }

    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        var instance = new LoxInstance(this);
        return instance;
    }

    public int GetArity()
    {
        return 0;
    }

    public LoxFunction? FindMethod(string name)
    {
        if (_methods.TryGetValue(name, out var method))
            return method;
        return null;
    }
}