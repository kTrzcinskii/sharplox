using sharplox.Expressions;
using sharplox.Tokens;

namespace sharplox.Services;

public class Interpreter : IExpressionVisitor<object?>
{
    public object? Evaluate(BaseExpression expression)
    {
        return expression.Accept(this);
    }
    
    public object? VisitBinaryExpression(BinaryExpression binaryExpression)
    {
        throw new NotImplementedException();
    }

    public object? VisitGroupingExpression(GroupingExpression groupingExpression)
    {
        return Evaluate(groupingExpression.BaseExpression);
    }

    public object? VisitLiteralExpression(LiteralExpression literalExpression)
    {
        return literalExpression.Value;
    }

    public object? VisitUnaryExpression(UnaryExpression unaryExpression)
    {
        var right = Evaluate(unaryExpression.Right);

        switch (unaryExpression.Operator.Type)
        {
            case TokenType.MINUS:
                return -(double)right;
            case TokenType.BANG:
                return !IsTruthy(right);
        }
        
        // This code should never be reached
        return null;
    }

    
    // Bool -> just value
    // Nil -> always false
    // Everything else -> always true
    public bool IsTruthy(object? value)
    {
        if (value == null)
            return false;
        if (value is bool b)
            return b;
        return true;
    }
}