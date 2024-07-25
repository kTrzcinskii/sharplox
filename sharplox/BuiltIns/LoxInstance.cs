namespace sharplox.BuiltIns;

public class LoxInstance
{
    private LoxClass _loxClass;

    public LoxInstance(LoxClass loxLoxClass)
    {
        _loxClass = loxLoxClass;
    }

    public override string ToString()
    {
        return $"{_loxClass} instance";
    }
}