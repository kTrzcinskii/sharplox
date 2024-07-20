namespace sharplox.Statements;

public class BlockStatement : BaseStatement
{
    public List<BaseStatement?> Statements { get; }

    public BlockStatement(List<BaseStatement?> statements)
    {
        Statements = statements;
    }
    
    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitBlockStatement(this);
    }
}