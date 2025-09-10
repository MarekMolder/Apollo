namespace App.BLL.Utils;

/// <summary>
/// Utility class for converting between different measurement units.
/// Supports mass (g, kg, mg) and volume (ml, l, cl), including approximate mass-volume conversions for water-like substances.
/// </summary>
public static class UnitConverter
{
    private static readonly Dictionary<(string from, string to), decimal> ConversionRates = new()
    {
        // Mass
        { ("g", "kg"), 0.001m }, { ("kg", "g"), 1000m },
        { ("g", "mg"), 1000m },  { ("mg", "g"), 0.001m },

        // Volume
        { ("ml", "l"), 0.001m }, { ("l", "ml"), 1000m },
        { ("ml", "cl"), 0.1m },  { ("l", "cl"), 100m },
        { ("cl", "ml"), 10m },   { ("cl", "l"), 0.01m }, // ← lisatud

        // Mass -> Volume (veesarnane, 1 g ≈ 1 ml)
        { ("g", "ml"), 1m },     { ("g", "l"), 0.001m },
        { ("kg", "ml"), 1000m }, { ("kg", "l"), 1m },

        // Volume -> Mass (vastupidised suunad, samuti veesarnase eeldusega)
        { ("ml", "g"), 1m },     { ("l", "kg"), 1m }     // ← lisatud
    };

    public static decimal Convert(decimal value, string from, string to)
    {
        from = from.Trim().ToLowerInvariant();
        to   = to.Trim().ToLowerInvariant();

        if (from == to) return value;
        if (ConversionRates.TryGetValue((from, to), out var rate))
            return value * rate;

        throw new Exception($"Unsupported conversion from {from} to {to}");
    }
}
