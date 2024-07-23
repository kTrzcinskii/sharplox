using sharplox.Exceptions;
using sharplox.Services;
using sharplox.Statements;
using Environment = sharplox.Services.Environment;

namespace sharplox.BuiltIns;

public class LoxFunction : ILoxCallable
{
    private readonly FunctionStatement _declaration;
    private readonly Environment _closure;

    public LoxFunction(FunctionStatement declaration, Environment closure)
    {
        _declaration = declaration;
        _closure = closure;
    }

    public object? Call(Interpreter interpreter, List<object?> arguments)
    {
        var environment = new Environment(_closure);
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