namespace sharplox.Expressions;

public abstract class BaseExpression
{
    public abstract T Accept<T>(ExpressionVisitor<T> visitor);
}
