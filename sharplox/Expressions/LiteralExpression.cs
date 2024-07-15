namespace sharplox.Expressions;

public class LiteralExpression : BaseExpression
{
    public object? Value { get; }

    public LiteralExpression(object? value)
    {
        Value = value;
    }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitLiteralExpression(this);
    }
}