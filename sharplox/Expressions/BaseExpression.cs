namespace sharplox.Expressions;

public abstract class BaseExpression
{
    public abstract T Accept<T>(IExpressionVisitor<T> visitor);
}
