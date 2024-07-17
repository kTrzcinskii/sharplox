namespace sharplox.Statements;

public abstract class BaseStatement
{
    public abstract T Accept<T>(IStatementVisitor<T> visitor);
}