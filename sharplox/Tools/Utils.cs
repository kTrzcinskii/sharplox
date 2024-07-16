namespace sharplox.Tools;

public static class Utils
{
    public static void Exit(ExitCode code)
    {
        Environment.Exit((int)code);
    }
    
    public enum ExitCode
    {
        INVALID_USAGE = 64,
        STATIC_ERROR = 65,
        RUNTIME_ERROR = 70,
    }
}