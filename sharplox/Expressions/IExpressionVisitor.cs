namespace sharplox.Expressions;

public interface IExpressionVisitor<T>
{
    public T VisitBinaryExpression(BinaryExpression binaryExpression);
    public T VisitGroupingExpression(GroupingExpression groupingExpression);
    public T VisitLiteralExpression(LiteralExpression literalExpression);
    public T VisitUnaryExpression(UnaryExpression unaryExpression);
    public T VisitVariableExpression(VariableExpression variableExpression);
    public T VisitAssignExpression(AssignExpression assignExpression);
    public T VisitLogicalExpression(LogicalExpression logicalExpression);
    public T VisitCallExpression(CallExpression callExpression);
    public T VisitGetExpression(GetExpression getExpression);
    public T VisitSetExpression(SetExpression setExpression);
    public T VisitThisExpression(ThisExpression thisExpression);
}