using sharplox.Services;

namespace sharplox.BuiltIns;

public class LoxClass : ILoxCallable
{
    public const string ThisKeyword = "this";
    public const string InitializerMethod = "init";
    
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

        var initializer = FindMethod(InitializerMethod);
        initializer?.Bind(instance).Call(interpreter, arguments);

        return instance;
    }

    public int GetArity()
    {
        var initializer = FindMethod(InitializerMethod);
        if (initializer == null)
            return 0;
        return initializer.GetArity();
    }

    public LoxFunction? FindMethod(string name)
    {
        if (_methods.TryGetValue(name, out var method))
            return method;
        return null;
    }
}