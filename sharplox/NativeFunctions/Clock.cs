using sharplox.BuiltIns;
using sharplox.Services;

namespace sharplox.NativeFunctions;

public class Clock : ILoxCallable
{
    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    public int GetArity()
    {
        return 0;
    }

    public override string ToString()
    {
        return "<native fn: clock>";
    }
}