using System.Globalization;
using sharplox.Tokens;

namespace sharplox;

public class Lexer
{
    private readonly string _source;
    private readonly List<Token> _tokens = [];
    // start of currently considered lexeme
    private int _start = 0;
    // end of currently considered lexeme (exclusive)
    private int _current = 0;
    private int _line = 1;

    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        {"and", TokenType.AND},
        {"class", TokenType.CLASS},
        {"else", TokenType.ELSE},
        {"false", TokenType.FALSE},
        {"for", TokenType.FOR},
        {"fun", TokenType.FUN},
        {"if", TokenType.IF},
        {"nil", TokenType.NIL},
        {"or", TokenType.OR},
        {"print", TokenType.PRINT},
        {"return", TokenType.RETURN},
        {"super", TokenType.SUPER},
        {"this", TokenType.THIS},
        {"true", TokenType.TRUE},
        {"var", TokenType.VAR},
        {"while", TokenType.WHILE}
    };
    
    public Lexer(string source)
    {
        _source = source;
    }

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;
            ScanToken();
        }
        
        _tokens.Add(new Token(TokenType.EOF, "", null, _line));
        
        return _tokens;
    }

    private bool IsAtEnd()
    {
        return _current >= _source.Length;
    }

    private string GetSourceFragment(int start, int end)
    {
        return _source.Substring(start, end - start);
    }

    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(':
                AddToken(TokenType.LEFT_PAREN);
                break;
            case ')':
                AddToken(TokenType.RIGHT_PAREN);
                break;
            case '{':
                AddToken(TokenType.LEFT_BRACE);
                break;
            case '}':
                AddToken(TokenType.RIGHT_BRACE);
                break;
            case ',':
                AddToken(TokenType.COMMA);
                break;
            case '.':
                AddToken(TokenType.DOT);
                break;
            case '-':
                AddToken(TokenType.MINUS);
                break;
            case '+':
                AddToken(TokenType.PLUS);
                break;
            case ';':
                AddToken(TokenType.SEMICOLON);
                break;
            case '*':
                AddToken(TokenType.STAR);
                break;
            case '!':
                AddToken(MatchNext('=') ? TokenType.BANG_EQAUl : TokenType.BANG);
                break;
            case '=':
                AddToken(MatchNext('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '<':
                AddToken(MatchNext('=') ? TokenType.LESS_EQAUL : TokenType.LESS);
                break;
            case '>':
                AddToken(MatchNext('>') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
            case '/':
                if (MatchNext('/'))
                {
                    // Comment -> goes until the next line
                    while (Peek() != '\n' && !IsAtEnd())
                        Advance();
                    // We dont add comment token as it's meaningless for us
                }
                else
                {
                    AddToken(TokenType.SLASH);
                }
                break;
            case '\t':
            case '\r':
            case ' ':
                // ignore meaningless characters
                break;
            case '\n':
                _line++;
                break;
            case '"':
                AddString();
                break;
            default:
                // We use IsAsciiDigit insteda of IsDigit to skip things such as Devanagari digits
                if (char.IsAsciiDigit(c))
                {
                    AddNumber();
                }
                else if (IsIdentifierStart(c))
                {
                    AddIdentifier();
                }
                else
                {
                    Lox.Error(_line, "Unexpected character");
                }
                break;
        }
    }

    private char Advance()
    {
        return _source[_current++];
    }

    private bool MatchNext(char expected)
    {
        if (IsAtEnd())
            return false;
        if (_source[_current] != expected)
            return false;
        _current++;
        return true;
    }

    private char Peek()
    {
        if (IsAtEnd())
            return '\0';
        return _source[_current];
    }

    private char PeekNext()
    {
        if (_current + 1 >= _source.Length)
            return '\0';
        return _source[_current + 1];
    }
    
    private void AddToken(TokenType type, object? literal = null)
    {
        var text = GetSourceFragment(_start, _current);
        _tokens.Add(new Token(type, text, literal, _line));
    }

    private void AddString()
    {
        // Consume whole string
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n')
                _line++;
            Advance();
        }

        if (IsAtEnd())
        {
            Lox.Error(_line, "Unterminated string");
            return;
        }
        
        // Consume closing '"'
        Advance();
        
        // Get string without '"' around it
        int start = _start + 1;
        int end = _current - 1;
        var value = GetSourceFragment(start, end);
        AddToken(TokenType.STRING, value);
    }

    private void AddNumber()
    {
        while (char.IsAsciiDigit(Peek()))
            Advance();
        
        // Look for fractional point
        if (Peek() == '.' && char.IsAsciiDigit(PeekNext()))
        {
            // consume '.'
            Advance();
            // get the fractional part
            while (char.IsAsciiDigit(Peek()))
                Advance();
        }

        var value = double.Parse(GetSourceFragment(_start, _current), CultureInfo.InvariantCulture);
        AddToken(TokenType.NUMBER, value);
    }

    private bool IsIdentifierStart(char c)
    {
        return char.IsAsciiLetter(c) || c == '_';
    }

    private bool IsIdentifierCharacter(char c)
    {
        return IsIdentifierStart(c) || char.IsAsciiDigit(c);
    }

    private void AddIdentifier()
    {
        while (IsIdentifierCharacter(Peek()))
            Advance();

        var identifier = GetSourceFragment(_start, _current);
        if (Keywords.TryGetValue(identifier, out TokenType t))
        {
            AddToken(t);
            return;
        }
        
        AddToken(TokenType.IDENTIFIER);
    }
}