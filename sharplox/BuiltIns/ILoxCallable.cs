using sharplox.Services;

namespace sharplox.BuiltIns;

public interface ILoxCallable
{
    object? Call(Interpreter interpreter, List<object?> arguments);
    int GetArity();
}