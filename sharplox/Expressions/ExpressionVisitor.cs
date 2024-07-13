namespace sharplox.Expressions;

public interface ExpressionVisitor<T>
{
    public T VisitBinaryExpression(BinaryExpression binaryExpression);
    public T VisitGroupingExpression(GroupingExpression groupingExpression);
    public T VisitLiteralExpression(LiteralExpression literalExpression);
    public T VisitUnaryExpression(UnaryExpression unaryExpression);
}