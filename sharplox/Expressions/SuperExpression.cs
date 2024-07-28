using sharplox.Tokens;

namespace sharplox.Expressions;

public class SuperExpression : BaseExpression
{
    public Token Keyword { get; }
    public Token Method { get; }

    public SuperExpression(Token keyword, Token method)
    {
        Keyword = keyword;
        Method = method;
    }
    
    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitSuperExpression(this);
    }
}