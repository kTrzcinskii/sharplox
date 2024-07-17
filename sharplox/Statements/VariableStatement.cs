using sharplox.Expressions;
using sharplox.Tokens;

namespace sharplox.Statements;

public class VariableStatement : BaseStatement
{
    public Token Name { get; }
    public BaseExpression? Initializer { get; }

    public VariableStatement(Token name, BaseExpression? initializer)
    {
        Name = name;
        Initializer = initializer;
    }
    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitVariableStatement(this);
    }
}