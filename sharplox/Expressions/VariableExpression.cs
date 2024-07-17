using sharplox.Tokens;

namespace sharplox.Expressions;

public class VariableExpression : BaseExpression
{
    public Token Name { get; }

    public VariableExpression(Token name)
    {
        Name = name;
    }
    
    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitVariableExpression(this);
    }
}