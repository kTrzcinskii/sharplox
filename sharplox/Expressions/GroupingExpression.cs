namespace sharplox.Expressions;

public class GroupingExpression : BaseExpression
{
    public BaseExpression BaseExpression { get; }

    public GroupingExpression(BaseExpression expression)
    {
        BaseExpression = expression;
    }

    public override T Accept<T>(ExpressionVisitor<T> visitor)
    {
        return visitor.VisitGroupingExpression(this);
    }
}