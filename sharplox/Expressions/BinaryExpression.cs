using sharplox.Tokens;

namespace sharplox.Expressions;

public class BinaryExpression : Expression
{
    public Expression Left { get; }
    public Expression Right { get; }
    public Token Operator { get; }


    public BinaryExpression(Expression left, Expression right, Token @operator)
    {
        Left = left;
        Right = right;
        Operator = @operator;
    }

    public override T Accept<T>(ExpressionVisitor<T> visitor)
    {
        return visitor.VisitBinaryExpression(this);
    }
}