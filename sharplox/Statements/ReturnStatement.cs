using sharplox.Expressions;
using sharplox.Tokens;

namespace sharplox.Statements;

public class ReturnStatement : BaseStatement
{
    public Token Keyword { get; }
    public BaseExpression? Value { get; }
    
    public ReturnStatement(Token keyword, BaseExpression? value)
    {
        Keyword = keyword;
        Value = value;
    }
    
    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitReturnStatement(this);
    }
}