using sharplox.Tokens;

namespace sharplox.Expressions;

public class UnaryExpression : Expression
{
    public Expression Right { get; }
    public Token Operator { get; }


    public UnaryExpression(Expression right, Token @operator)
    {
        Right = right;
        Operator = @operator;
    }

    public override T Accept<T>(ExpressionVisitor<T> visitor)
    {
        return visitor.VisitUnaryExpression(this);
    }
}