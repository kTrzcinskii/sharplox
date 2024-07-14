using sharplox.Expressions;
using sharplox.Tokens;

namespace sharplox.Services;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _current = 0;


    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    private Token Peek()
    {
        return _tokens[_current];
    }

    private Token Previous()
    {
        return _tokens[_current - 1];
    }
    
    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }

    private Token Advance()
    {
        if (!IsAtEnd())
            _current++;
        return Previous();
    }
    
    // Check if current token is of given type
    private bool CheckCurrent(TokenType type)
    {
        if (IsAtEnd())
            return false;
        return Peek().Type == type;
    }
    
    // Consume current token if it matches given types
    private bool MatchCurrent(params TokenType[] types)
    {
        if (types.Any(CheckCurrent))
        {
            Advance();
            return true;
        }

        return false;
    }

    private BaseExpression HandleLeftAssociativeBinaryOperation(Func<BaseExpression> higherPrecedenceRule, params TokenType[] types)
    {
        BaseExpression expression = higherPrecedenceRule();

        while (MatchCurrent(types))
        {
            var op = Previous();
            BaseExpression right = higherPrecedenceRule();
            expression = new BinaryExpression(expression, right, op);
        }
        
        return expression;
    }
    
    private BaseExpression Expression()
    {
        return Equality();
    }

    private BaseExpression Equality()
    {
        return HandleLeftAssociativeBinaryOperation(Comparison, TokenType.EQUAL_EQUAL, TokenType.BANG_EQAUl);
    }

    private BaseExpression Comparison()
    {
        return HandleLeftAssociativeBinaryOperation(Term, TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS,
            TokenType.LESS_EQAUL);
    }

    private BaseExpression Term()
    {
        return HandleLeftAssociativeBinaryOperation(Factor, TokenType.PLUS, TokenType.MINUS);
    }

    private BaseExpression Factor()
    {
        return HandleLeftAssociativeBinaryOperation(Unary, TokenType.STAR, TokenType.SLASH);
    }

    private BaseExpression Unary()
    {
        if (MatchCurrent(TokenType.BANG, TokenType.MINUS))
        {
            var op = Previous();
            BaseExpression right = Unary();
            return new UnaryExpression(right, op);
        }

        return Primary();
    }

    private BaseExpression Primary()
    {
        if (MatchCurrent(TokenType.FALSE))
            return new LiteralExpression(false);
        if (MatchCurrent(TokenType.TRUE))
            return new LiteralExpression(true);
        if (MatchCurrent(TokenType.NIL))
            return new LiteralExpression(null);

        if (MatchCurrent(TokenType.NUMBER, TokenType.STRING))
            return new LiteralExpression(Previous().Literal);

        if (MatchCurrent(TokenType.LEFT_PAREN))
        {
            BaseExpression expression = Expression();
            // TODO: create this function
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression");
            return new GroupingExpression(expression);
        }
        
        // TODO: throw an error here
    }
}