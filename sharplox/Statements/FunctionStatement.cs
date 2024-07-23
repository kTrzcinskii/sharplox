using sharplox.Tokens;

namespace sharplox.Statements;

public class FunctionStatement : BaseStatement
{
    public Token Name { get; }
    public List<Token> Params { get; }
    public List<BaseStatement> Body { get; }
    
    public FunctionStatement(Token name, List<Token> @params, List<BaseStatement> body)
    {
        Name = name;
        Params = @params;
        Body = body;
    }

    
    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitFunctionStatement(this);
    }
}