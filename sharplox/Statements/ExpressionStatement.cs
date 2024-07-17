using sharplox.Expressions;

namespace sharplox.Statements;

public class ExpressionStatement : BaseStatement
{
    public BaseExpression Expression { get; }
    
    public ExpressionStatement(BaseExpression expression)
    {
        Expression = expression;
    }

    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitExpressionStatement(this);
    }
}