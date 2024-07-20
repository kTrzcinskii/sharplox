using sharplox.Errors;
using sharplox.Expressions;
using sharplox.Statements;
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

    public List<BaseStatement> Parse()
    {
        var statements = new List<BaseStatement>();

        while (!IsAtEnd())
        {
            var declaration = Declaration();
            if (declaration != null)
                statements.Add(declaration);
        }
        
        return statements;
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
    
    // Expression
    
    private BaseExpression Expression()
    {
        return Assignment();
    }

    private BaseExpression Assignment()
    {
        var expression = Equality();

        if (MatchCurrent(TokenType.EQUAL))
        {
            Token equals = Previous();
            var value = Assignment();
            if (expression is VariableExpression ve)
            {
                Token name = ve.Name;
                return new AssignExpression(name, value);
            }

            ParseError(equals, "Invalid assignment target.");
        }

        return expression;
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

        if (MatchCurrent(TokenType.IDENTIFIER))
            return new VariableExpression(Previous());
        
        if (MatchCurrent(TokenType.LEFT_PAREN))
        {
            BaseExpression expression = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression");
            return new GroupingExpression(expression);
        }

        throw ParseError(Peek(), "Expect beginning of expression");
    }

    private Token Consume(TokenType type, string message)
    {
        if (CheckCurrent(type))
            return Advance();
        throw ParseError(Peek(), message);
    }

    private ParseException ParseError(Token token, string message)
    {
        Lox.Error(token, message);
        return new ParseException();
    }

    private void Synchronize()
    {
        Advance();
        while (!IsAtEnd())
        {
            if (Previous().Type == TokenType.SEMICOLON)
                return;
            switch (Peek().Type)
            {
                case TokenType.CLASS:
                case TokenType.FUN:
                case TokenType.VAR:
                case TokenType.FOR:
                case TokenType.IF:
                case TokenType.WHILE:
                case TokenType.PRINT:
                case TokenType.RETURN:
                    return;
            }
            Advance();
        }
    }

    // Statements
    
    private BaseStatement? Declaration()
    {
        try
        {
            if (MatchCurrent(TokenType.VAR))
                return Variable();
            return Statement();
        }
        catch (ParseException)
        {
            Synchronize();
            return null;
        }
    }
    
    private BaseStatement Statement()
    {
        if (MatchCurrent(TokenType.IF))
            return If();
        
        if (MatchCurrent(TokenType.PRINT))
            return Print();

        if (MatchCurrent(TokenType.LEFT_BRACE))
            return new BlockStatement(Block());

        return ExpressionStmt();
    }

    private BaseStatement Print()
    {
        BaseExpression value = Expression();
        Consume(TokenType.SEMICOLON, "Expect ';' after value");
        return new PrintStatement(value);
    }

    private BaseStatement ExpressionStmt()
    {
        BaseExpression expression = Expression();
        Consume(TokenType.SEMICOLON, "Expect ';' after expression");
        return new ExpressionStatement(expression);
    }

    private BaseStatement Variable()
    {
        Token name = Consume(TokenType.IDENTIFIER, "Expect variable name.");
        BaseExpression? initializer = null;
        if (MatchCurrent(TokenType.EQUAL))
            initializer = Expression();
        Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration");
        return new VariableStatement(name, initializer);
    }

    private List<BaseStatement> Block()
    {
        var statements = new List<BaseStatement>();

        while (!CheckCurrent(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            var declaration = Declaration();
            if (declaration != null)
                statements.Add(declaration);
        }
        Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
        return statements;
    }

    private BaseStatement If()
    {
        Consume(TokenType.LEFT_PAREN, "Expect '(' after 'if'.");
        var condition = Expression();
        Consume(TokenType.RIGHT_PAREN, "Expect ')' after condition.");
        var thenBranch = Statement();
        BaseStatement? elseBranch = null;
        if (MatchCurrent(TokenType.ELSE))
            elseBranch = Statement();
        return new IfStatement(condition, thenBranch, elseBranch);
    }
}