using sharplox.Tokens;

namespace sharplox.Statements;

public class ClassStatement : BaseStatement
{
    public Token Name { get; }
    public List<FunctionStatement> Methods { get; }
    
    public ClassStatement(Token name, List<FunctionStatement> methods)
    {
        Name = name;
        Methods = methods;
    }
    
    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitClassStatement(this);
    }
}