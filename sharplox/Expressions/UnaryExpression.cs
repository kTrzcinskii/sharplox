using sharplox.Tokens;

namespace sharplox.Expressions;

public class UnaryExpression : BaseExpression
{
    public BaseExpression Right { get; }
    public Token Operator { get; }


    public UnaryExpression(BaseExpression right, Token @operator)
    {
        Right = right;
        Operator = @operator;
    }

    public override T Accept<T>(ExpressionVisitor<T> visitor)
    {
        return visitor.VisitUnaryExpression(this);
    }
}