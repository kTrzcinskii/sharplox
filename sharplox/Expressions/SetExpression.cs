using sharplox.Tokens;

namespace sharplox.Expressions;

public class SetExpression : BaseExpression
{
    public BaseExpression Object { get; }
    public Token Name { get; }
    public BaseExpression Value { get; }

    public SetExpression(BaseExpression @object, Token name, BaseExpression value)
    {
        Object = @object;
        Name = name;
        Value = value;
    }
    
    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitSetExpression(this);
    }
}