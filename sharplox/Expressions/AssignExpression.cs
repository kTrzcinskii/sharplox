using sharplox.Tokens;

namespace sharplox.Expressions;

public class AssignExpression : BaseExpression
{
    public Token Name { get; }
    public BaseExpression Value { get; }

    public AssignExpression(Token name, BaseExpression value)
    {
        Name = name;
        Value = value;
    }

    
    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitAssignExpression(this);
    }
}