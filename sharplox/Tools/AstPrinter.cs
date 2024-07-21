using System.Text;
using sharplox.Expressions;

namespace sharplox.Tools;

public class AstPrinter : IExpressionVisitor<string>
{
    public string PrintAstTree(BaseExpression head)
    {
        return head.Accept(this);
    }
    
    public string VisitBinaryExpression(BinaryExpression binaryExpression)
    {
        return Parenthesize(binaryExpression.Operator.Lexeme, binaryExpression.Left, binaryExpression.Right);
    }

    public string VisitGroupingExpression(GroupingExpression groupingExpression)
    {
        return Parenthesize("group", groupingExpression.BaseExpression);
    }

    public string VisitLiteralExpression(LiteralExpression literalExpression)
    {
        if (literalExpression.Value == null)
        {
            return "nil";
        }

        return literalExpression.Value.ToString() ?? "nil";
    }

    public string VisitUnaryExpression(UnaryExpression unaryExpression)
    {
        return Parenthesize(unaryExpression.Operator.Lexeme, unaryExpression.Right);
    }

    public string VisitVariableExpression(VariableExpression variableExpression)
    {
        return $"var {variableExpression.Name}";
    }

    public string VisitAssignExpression(AssignExpression assignExpression)
    {
        return Parenthesize($"assign {assignExpression.Name}", assignExpression.Value);
    }

    public string VisitLogicalExpression(LogicalExpression logicalExpression)
    {
        return Parenthesize(logicalExpression.Operator.Lexeme, logicalExpression.Left, logicalExpression.Right);
    }

    private string Parenthesize(string name, params BaseExpression[] expressions)
    {
        var sb = new StringBuilder();
        sb.Append('(');
        sb.Append(name);
        foreach (var expression in expressions)
        {
            sb.Append(' ');
            sb.Append(expression.Accept(this));
        }
        sb.Append(')');
        return sb.ToString();
    }
}