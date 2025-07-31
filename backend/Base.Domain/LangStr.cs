namespace Base.Domain;

/// <summary>
/// A multilingual string implementation that supports language-specific values.
/// Inherits from <see cref="Dictionary{TKey, TValue}"/> where key is the language code (e.g. "en", "et").
/// </summary>
public class LangStr : Dictionary<string, string>
{
    private const string DefaultCulture = "en";

    // s["en"] = "foo";
    // var bar = s["en"];
    public new string this[string key]
    {
        get => base[key];
        set => base[key] = value;
    }

    public LangStr()
    {
    }

    public LangStr(string value) : this(value, Thread.CurrentThread.CurrentUICulture.Name)
    {
    }

    public LangStr(string value, string culture)
    {
        if (culture.Length < 1) throw new ApplicationException("Culture is required!");

        var neutralCulture = culture.Split('-')[0];
        this[neutralCulture] = value;

        if (!ContainsKey(DefaultCulture))
        {
            this[DefaultCulture] = value;
        }
    }

    /// <summary>
    /// Returns the translation for the current or specified culture.
    /// </summary>
    public string? Translate(string? culture = null)
    {
        if (Count == 0) return null;
        culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;

        if (ContainsKey(culture))
        {
            return this[culture];
        }

        var neutralCulture = culture.Split('-')[0];
        if (ContainsKey(neutralCulture))
        {
            return this[neutralCulture];
        }

        if (ContainsKey(DefaultCulture))
        {
            return this[DefaultCulture];
        }

        return null;
    }

    /// <summary>
    /// Sets the translation for the current or specified culture.
    /// </summary>
    public void SetTranslation(string value, string? culture = null)
    {
        culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;
        var neutralCulture = culture.Split('-')[0];
        this[neutralCulture] = value;
    }

    /// <summary>
    /// Returns the default string representation (localized to current culture or fallback).
    /// </summary>
    public override string ToString()
    {
        return Translate() ?? "????";
    }
    
    public static implicit operator string(LangStr? langStr) => langStr?.ToString() ?? "null";
    
    public static implicit operator LangStr(string value) => new(value);
}
