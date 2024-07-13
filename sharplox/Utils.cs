namespace sharplox;

public static class Utils
{
    public static void Exit(ExitCode code)
    {
        Environment.Exit((int)code);
    }
}