namespace sharplox.Expressions;

public class GroupingExpression : Expression
{
    public Expression Expression { get; }

    public GroupingExpression(Expression expression)
    {
        Expression = expression;
    }

    public override T Accept<T>(ExpressionVisitor<T> visitor)
    {
        return visitor.VisitGroupingExpression(this);
    }
}