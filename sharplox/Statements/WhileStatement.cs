using sharplox.Expressions;

namespace sharplox.Statements;

public class WhileStatement : BaseStatement
{
    public BaseExpression Condition { get; }
    public BaseStatement Body { get; }
    
    public WhileStatement(BaseExpression condition, BaseStatement body)
    {
        Condition = condition;
        Body = body;
    }

    
    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitWhileStatement(this);
    }
}