using sharplox.Tokens;

namespace sharplox.Expressions;

public class ThisExpression : BaseExpression
{
    public Token Keyword { get; }

    public ThisExpression(Token keyword)
    {
        Keyword = keyword;
    }

    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitThisExpression(this);
    }
}