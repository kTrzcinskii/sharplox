using sharplox.Exceptions;
using sharplox.Services;
using sharplox.Statements;
using Environment = sharplox.Services.Environment;

namespace sharplox.BuiltIns;

public class LoxFunction : ILoxCallable
{
    private readonly FunctionStatement _declaration;
    private readonly Environment _closure;
    private readonly bool _isInitializer;

    public LoxFunction(FunctionStatement declaration, Environment closure, bool isInitializer)
    {
        _declaration = declaration;
        _closure = closure;
        _isInitializer = isInitializer;
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
            if (_isInitializer)
                return _closure.GetAt(0, LoxClass.ThisKeyword);
            return ex.Value;
        }

        if (_isInitializer)
            return _closure.GetAt(0, LoxClass.ThisKeyword);
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

    public LoxFunction Bind(LoxInstance instance)
    {
        var environment = new Environment(_closure);
        environment.Define(LoxClass.ThisKeyword, instance);
        return new LoxFunction(_declaration, environment, _isInitializer);
    }
}