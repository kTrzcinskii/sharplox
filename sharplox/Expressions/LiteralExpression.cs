namespace sharplox.Expressions;

public class LiteralExpression : Expression
{
    public object? Value { get; }

    public LiteralExpression(object? value)
    {
        Value = value;
    }

    public override T Accept<T>(ExpressionVisitor<T> visitor)
    {
        return visitor.VisitLiteralExpression(this);
    }
}