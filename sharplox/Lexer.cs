namespace sharplox;

public class Lexer
{
    private readonly string _source;
    private readonly List<Token> _tokens = new List<Token>();
    // start of currently considered lexeme
    private int _start = 0;
    // currently considered character
    private int _current = 0;
    private int _line = 1;
    
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
                Lox.Error(_line, "Unexpected character");
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
    
    private void AddToken(TokenType type, object? literal = null)
    {
        var text = _source.Substring(_start, _current - _start + 1);
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
        var value = _source.Substring(start, end - start + 1);
        AddToken(TokenType.STRING, value);
    }
}