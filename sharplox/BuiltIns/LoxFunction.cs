using sharplox.Exceptions;
using sharplox.Services;
using sharplox.Statements;
using Environment = sharplox.Services.Environment;

namespace sharplox.BuiltIns;

public class LoxFunction : ILoxCallable
{
    private readonly FunctionStatement _declaration;

    public LoxFunction(FunctionStatement declaration)
    {
        _declaration = declaration;
    }

    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        var environment = new Environment(interpreter.Globals);
        for (int i = 0; i < _declaration.Params.Count; i++)
            environment.Define(_declaration.Params[i].Lexeme, arguments[i]);
        try
        {
            interpreter.ExecuteBlock(_declaration.Body, environment);
        }
        catch (ReturnException ex)
        {
            return ex.Value;
        }
        
        return null;
    }

    public int GetArity()
    {
        return _declaration.Params.Count;
    }

    public override string ToString()
    {
        return $"<fn {_declaration.Name.Lexeme}>";
    }
}