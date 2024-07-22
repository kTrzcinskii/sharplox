using sharplox.Tokens;

namespace sharplox.Expressions;

public class CallExpression : BaseExpression
{
    public BaseExpression Callee { get; }
    public Token ClosingParen { get; }
    public List<BaseExpression> Arguments { get; }
    
    public CallExpression(BaseExpression callee, Token closingParen, List<BaseExpression> arguments)
    {
        Callee = callee;
        ClosingParen = closingParen;
        Arguments = arguments;
    }
    
    public override T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.VisitCallExpression(this);
    }
}