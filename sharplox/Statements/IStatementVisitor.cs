namespace sharplox.Statements;

public interface IStatementVisitor<T>
{
    public T VisitPrintStatement(PrintStatement statement);
    public T VisitExpressionStatement(ExpressionStatement statement);
    public T VisitVariableStatement(VariableStatement statement);
}