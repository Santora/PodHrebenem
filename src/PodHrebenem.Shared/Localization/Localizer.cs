using System.Globalization;
using PodHrebenem.Shared.Services;

namespace PodHrebenem.Shared.Localization;

/// <summary>Překlady a volba jazyka. Chybějící klíč se zobrazí jako [[klíč]], aby šel snadno najít.</summary>
public sealed class Localizer
{
    public const string PreferenceKey = "language";
    public static IReadOnlyList<string> Languages => Texts.Languages;

    private readonly IPreferenceStore _preferences;
    private int _index;

    public Localizer(IPreferenceStore preferences)
    {
        _preferences = preferences;
        _index = IndexOf(preferences.Get(PreferenceKey))
                 ?? IndexOf(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
                 ?? 0;
    }

    public event Action? Changed;

    public string Language => Texts.Languages[_index];

    public CultureInfo Culture => CultureInfo.GetCultureInfo(Language);

    public string this[string key] =>
        Texts.All.TryGetValue(key, out var values) ? values[_index] : $"[[{key}]]";

    public void SetLanguage(string language)
    {
        var index = IndexOf(language);
        if (index is null || index == _index) return;

        _index = index.Value;
        _preferences.Set(PreferenceKey, Language);
        Changed?.Invoke();
    }

    public string Nights(int n) => Plural.Nights(Language, n);

    private static int? IndexOf(string? language)
    {
        if (string.IsNullOrWhiteSpace(language)) return null;
        var i = Array.IndexOf(Texts.Languages, language.ToLowerInvariant());
        return i < 0 ? null : i;
    }
}
