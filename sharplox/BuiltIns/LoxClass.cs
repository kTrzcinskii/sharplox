using sharplox.Services;

namespace sharplox.BuiltIns;

public class LoxClass : ILoxCallable
{
    private readonly string _name;

    public LoxClass(string name)
    {
        _name = name;
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
}