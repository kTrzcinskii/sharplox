using sharplox.Tokens;

namespace sharplox.Expressions;

public class LogicalExpression : BaseExpression
{
    public BaseExpression Left { get; }
    public BaseExpression Right { get; }
    public Token Operator { get; }
    
    public LogicalExpression(BaseExpression left, BaseExpression right, Token @operator)
    {
        Left = left;
        Right = right;
        Operator = @operator;
    }
    
    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitLogicalExpression(this);
    }
}