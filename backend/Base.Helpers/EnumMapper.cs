namespace Base.Helpers;

public static class EnumMapper
{
    public static TTargetEnum MapEnumByName<TTargetEnum>(Enum source)
        where TTargetEnum : struct, Enum
    {
        if (!Enum.TryParse<TTargetEnum>(source.ToString(), out var result))
        {
            throw new ArgumentException($"Cannot map enum value '{source}' to {typeof(TTargetEnum).Name}");
        }

        return result;
    }
}