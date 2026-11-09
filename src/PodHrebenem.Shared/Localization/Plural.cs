namespace PodHrebenem.Shared.Localization;

/// <summary>Správný tvar slova „noc" podle jazyka a počtu.</summary>
public static class Plural
{
    // jedna, 2 až 4 (nebo „více" u en/de), 5 a více
    private static readonly Dictionary<string, string[]> Forms = new()
    {
        ["cs"] = new[] { "noc", "noci", "nocí" },
        ["sk"] = new[] { "noc", "noci", "nocí" },
        ["pl"] = new[] { "noc", "noce", "nocy" },
        ["de"] = new[] { "Nacht", "Nächte", "Nächte" },
        ["en"] = new[] { "night", "nights", "nights" },
    };

    public static string Nights(string language, int n)
    {
        var w = Forms.TryGetValue(language, out var f) ? f : Forms["cs"];

        if (language is "en" or "de")
            return n == 1 ? w[0] : w[1];

        if (n == 1) return w[0];

        if (language == "pl")
        {
            var m10 = n % 10;
            var m100 = n % 100;
            return m10 is >= 2 and <= 4 && m100 is not (>= 12 and <= 14) ? w[1] : w[2];
        }

        return n is >= 2 and <= 4 ? w[1] : w[2];
    }
}
