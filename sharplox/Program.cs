namespace sharplox;

public class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: sharplox [script]");
            Utils.Exit(ExitCode.INVALID_USAGE);
        }
        Lox.Initialize(args);
    }
}