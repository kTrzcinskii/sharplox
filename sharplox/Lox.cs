namespace sharplox; 

public class Lox
{
    private static bool _inErrorState = false;
    
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
            Utils.Exit(ExitCode.CODE_ERROR);
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
        }
    }
    
    private static void Run(string source)
    {
        var scanner = new Lexer(source);
        var tokens = scanner.ScanTokens();
        
        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error {where}: {message}");
        _inErrorState = true;
    }
    
}