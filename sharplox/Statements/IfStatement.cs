using sharplox.Expressions;

namespace sharplox.Statements;

public class IfStatement : BaseStatement
{
    public BaseExpression Condition { get; }
    public BaseStatement ThenBranch { get; }
    public BaseStatement? ElseBranch { get; }
 
    public IfStatement(BaseExpression condition, BaseStatement thenBranch, BaseStatement? elseBranch)
    {
        Condition = condition;
        ThenBranch = thenBranch;
        ElseBranch = elseBranch;
    }
    
    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitIfStatement(this);
    }
}