using sharplox.Expressions;

namespace sharplox.Statements;

public class PrintStatement : BaseStatement
{
    public BaseExpression Expression { get; }
    
    public PrintStatement(BaseExpression expression)
    {
        Expression = expression;
    }

    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitPrintStatement(this);
    }
}