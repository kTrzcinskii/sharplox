using sharplox.Exceptions;
using sharplox.Services;
using sharplox.Tokens;
using sharplox.Tools;

namespace sharplox; 

public class Lox
{
    private static bool _inErrorState = false;
    private static bool _inRuntimeErrorState = false;
    private static readonly Interpreter Interpreter = new Interpreter();
    
    public static void Initialize(string[] args)
    {
        if (args.Length == 1)
        {
            RunFile(args[0]);
            return;
        }
        RunPrompt();        
    }

    private static void RunFile(string filePath)
    {
        var source = File.ReadAllText(filePath);
        Run(source);
        if (_inErrorState)
        {
            Utils.Exit(Utils.ExitCode.STATIC_ERROR);
        }
        if (_inRuntimeErrorState)
        {
            Utils.Exit(Utils.ExitCode.RUNTIME_ERROR);
        }
    }
    
    private static void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (input == null)
                break;
            Run(input);
            _inErrorState = false;
            _inRuntimeErrorState = false;
        }
    }
    
    private static void Run(string source)
    {
        var scanner = new Lexer(source);
        var tokens = scanner.ScanTokens();

        var parser = new Parser(tokens);
        var statements = parser.Parse();

        if (_inErrorState)
            return;
        
        Interpreter.Interpret(statements);
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }
    
    public static void Error(Token token, string message)
    {
        if (token.Type == TokenType.EOF)
        {
            Report(token.Line, "at end", message);
        }
        else
        {
            Report(token.Line, $"at '{token.Lexeme}'", message);
        }
    }

    public static void RuntimeError(RuntimeException ex)
    {
        Console.WriteLine($"{ex.Message}\n[line {ex.Token.Line}]");
        _inRuntimeErrorState = true;
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error {where}: {message}");
        _inErrorState = true;
    }
    
}