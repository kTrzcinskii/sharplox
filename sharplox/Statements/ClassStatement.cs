using sharplox.Expressions;
using sharplox.Tokens;

namespace sharplox.Statements;

public class ClassStatement : BaseStatement
{
    public Token Name { get; }
    public VariableExpression? BaseClass { get; }
    public List<FunctionStatement> Methods { get; }
    
    public ClassStatement(Token name, VariableExpression? baseClass,List<FunctionStatement> methods)
    {
        Name = name;
        BaseClass = baseClass;
        Methods = methods;
    }
    
    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitClassStatement(this);
    }
}