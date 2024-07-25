using sharplox.BuiltIns;
using sharplox.Expressions;
using sharplox.Statements;
using sharplox.Tokens;

namespace sharplox.Services;

// We use object? in statement visitor as it's not possible to use 'void' in the
// place of generic in c#
public class Resolver : IExpressionVisitor<object?>, IStatementVisitor<object?>
{
    private readonly Interpreter _interpreter;
    private readonly Stack<Dictionary<string, bool>> _scopes = new Stack<Dictionary<string, bool>>();
    private FunctionType _currentFunction = FunctionType.NONE;

    public Resolver(Interpreter interpreter)
    {
        _interpreter = interpreter;
    }
    
    public void Resolve(List<BaseStatement> statements)
    {
        foreach (var statement in statements)
            Resolve(statement);
    }
    
    // Expressions
    public object? VisitBinaryExpression(BinaryExpression binaryExpression)
    {
        Resolve(binaryExpression.Left);
        Resolve(binaryExpression.Right);
        return null;
    }

    public object? VisitGroupingExpression(GroupingExpression groupingExpression)
    {
        Resolve(groupingExpression.BaseExpression);
        return null;
    }

    public object? VisitLiteralExpression(LiteralExpression literalExpression)
    {
        return null;
    }

    public object? VisitUnaryExpression(UnaryExpression unaryExpression)
    {
        Resolve(unaryExpression.Right);
        return null;
    }

    public object? VisitVariableExpression(VariableExpression variableExpression)
    {
        if (_scopes.Count > 0 && _scopes.Peek().ContainsKey(variableExpression.Name.Lexeme) && !_scopes.Peek()[variableExpression.Name.Lexeme])
            Lox.Error(variableExpression.Name, "Can't read local variable in its own initializer.");
        ResolveLocal(variableExpression, variableExpression.Name);
        return null;
    }

    public object? VisitAssignExpression(AssignExpression assignExpression)
    {
        Resolve(assignExpression.Value);
        ResolveLocal(assignExpression, assignExpression.Name);
        return null;
    }

    public object? VisitLogicalExpression(LogicalExpression logicalExpression)
    {
        Resolve(logicalExpression.Left);
        Resolve(logicalExpression.Right);
        return null;
    }

    public object? VisitCallExpression(CallExpression callExpression)
    {
        Resolve(callExpression.Callee);
        foreach (var argument in callExpression.Arguments)
            Resolve(argument);
        return null;
    }
    
    // Statements

    public object? VisitPrintStatement(PrintStatement statement)
    {
        Resolve(statement.Expression);
        return null;
    }

    public object? VisitExpressionStatement(ExpressionStatement statement)
    {
        Resolve(statement.Expression);
        return null;
    }

    public object? VisitVariableStatement(VariableStatement statement)
    {
        Declare(statement.Name);
        if (statement.Initializer != null)
            Resolve(statement.Initializer);
        Define(statement.Name);
        return null;
    }

    public object? VisitBlockStatement(BlockStatement statement)
    {
        BeginScope();
        Resolve(statement.Statements);
        EndScope();
        return null;
    }

    public object? VisitIfStatement(IfStatement statement)
    {
        Resolve(statement.Condition);
        Resolve(statement.ThenBranch);
        if (statement.ElseBranch != null)
            Resolve(statement.ElseBranch);
        return null;
    }

    public object? VisitWhileStatement(WhileStatement statement)
    {
        Resolve(statement.Condition);
        Resolve(statement.Body);
        return null;
    }

    public object? VisitFunctionStatement(FunctionStatement statement)
    {
        Declare(statement.Name);
        Define(statement.Name);
        ResolveFunction(statement, _currentFunction);
        return null;
    }

    public object? VisitReturnStatement(ReturnStatement statement)
    {
        if (_currentFunction == FunctionType.NONE)
            Lox.Error(statement.Keyword, "Can't return from top-level code.");
        
        if (statement.Value != null)
            Resolve(statement.Value);
        return null;
    }

    public object? VisitClassStatement(ClassStatement statement)
    {
        Declare(statement.Name);
        Define(statement.Name);
        return null;
    }

    // Helpers
    private void Resolve(BaseStatement statement)
    {
        statement.Accept(this);
    }

    private void Resolve(BaseExpression expression)
    {
        expression.Accept(this);
    }

    private void BeginScope()
    {
        _scopes.Push(new Dictionary<string, bool>());
    }

    private void EndScope()
    {
        _scopes.Pop();
    }

    private void Declare(Token name)
    {
        if (_scopes.Count == 0)
            return;
        var scope = _scopes.Peek();
        if (scope.ContainsKey(name.Lexeme))
            Lox.Error(name, "Variable with this name already exists in this scope.");
        scope[name.Lexeme] = false;
    }

    private void Define(Token name)
    {
        if (_scopes.Count == 0)
            return;
        var scope = _scopes.Peek();
        scope[name.Lexeme] = true;
    }

    private void ResolveLocal(BaseExpression expression, Token name)
    {
        int depth = 0;
        foreach (var scope in _scopes)
        {
            if (scope.ContainsKey(name.Lexeme))
            {
                _interpreter.Resolve(expression, depth);
                return;
            }
            depth++;
        }
    }

    private void ResolveFunction(FunctionStatement statement, FunctionType type)
    {
        var encolisingFunction = _currentFunction;
        _currentFunction = type;
        
        BeginScope();
        foreach (var param in statement.Params)
        {
            Declare(param);
            Define(param);
        }
        Resolve(statement.Body);
        EndScope();

        _currentFunction = encolisingFunction;
    }
}