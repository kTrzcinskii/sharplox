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
        var left = Evaluate(binaryExpression.Left);
        var right = Evaluate(binaryExpression.Right);

        switch (binaryExpression.Operator.Type)
        {
            case TokenType.MINUS:
                return (double)left - (double)right;
            case TokenType.PLUS:
                if (left is double ld && right is double rd)
                    return ld + rd;
                if (left is string ls && right is string rs)
                    return ls + rs;
                break;
            case TokenType.SLASH:
                return (double)left / (double)right;
            case TokenType.STAR:
                return (double)left * (double)right;
            case TokenType.GREATER:
                return (double)left > (double)right;
            case TokenType.GREATER_EQUAL:
                return (double)left >= (double)right;
            case TokenType.LESS:
                return (double)left < (double)right;
            case TokenType.LESS_EQAUL:
                return (double)left <= (double)right;
            case TokenType.EQUAL_EQUAL:
                return AreEqual(left, right);
            case TokenType.BANG_EQAUl:
                return !AreEqual(left, right);
        }

        // This code should never be reached
        return null;
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
    private bool IsTruthy(object? value)
    {
        if (value == null)
            return false;
        if (value is bool b)
            return b;
        return true;
    }

    private bool AreEqual(object? lhs, object? rhs)
    {
        if (lhs == null && rhs == null)
            return true;
        if (lhs == null || rhs == null)
            return false;
        return lhs.Equals(rhs);
    }
}