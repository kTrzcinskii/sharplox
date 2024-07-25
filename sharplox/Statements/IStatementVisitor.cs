namespace sharplox.Statements;

public interface IStatementVisitor<T>
{
    public T VisitPrintStatement(PrintStatement statement);
    public T VisitExpressionStatement(ExpressionStatement statement);
    public T VisitVariableStatement(VariableStatement statement);
    public T VisitBlockStatement(BlockStatement statement);
    public T VisitIfStatement(IfStatement statement);
    public T VisitWhileStatement(WhileStatement statement);
    public T VisitFunctionStatement(FunctionStatement statement);
    public T VisitReturnStatement(ReturnStatement statement);
    public T VisitClassStatement(ClassStatement statement);
}