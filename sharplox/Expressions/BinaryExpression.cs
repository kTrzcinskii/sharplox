using sharplox.Tokens;

namespace sharplox.Expressions;

public class BinaryExpression : BaseExpression
{
    public BaseExpression Left { get; }
    public BaseExpression Right { get; }
    public Token Operator { get; }


    public BinaryExpression(BaseExpression left, BaseExpression right, Token @operator)
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