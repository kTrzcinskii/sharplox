using sharplox.Tokens;

namespace sharplox.Expressions;

public class GetExpression : BaseExpression
{
    public BaseExpression Object { get; }
    public Token Name { get; }

    public GetExpression(BaseExpression @object, Token name)
    {
        Object = @object;
        Name = name;
    }
    
    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitGetExpression(this);
    }
}