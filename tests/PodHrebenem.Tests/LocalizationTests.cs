using PodHrebenem.Shared.Localization;
using PodHrebenem.Shared.Services;
using Xunit;

namespace PodHrebenem.Tests;

public class LocalizationTests
{
    private sealed class MemoryStore : IPreferenceStore
    {
        private readonly Dictionary<string, string> _data = new();
        public string? Get(string key) => _data.TryGetValue(key, out var v) ? v : null;
        public void Set(string key, string value) => _data[key] = value;
    }

    [Theory]
    [InlineData("cs", 1, "noc")]
    [InlineData("cs", 2, "noci")]
    [InlineData("cs", 4, "noci")]
    [InlineData("cs", 5, "nocí")]
    [InlineData("cs", 11, "nocí")]
    [InlineData("sk", 3, "noci")]
    [InlineData("sk", 7, "nocí")]
    [InlineData("pl", 1, "noc")]
    [InlineData("pl", 2, "noce")]
    [InlineData("pl", 4, "noce")]
    [InlineData("pl", 5, "nocy")]
    [InlineData("pl", 12, "nocy")]
    [InlineData("pl", 14, "nocy")]
    [InlineData("pl", 22, "noce")]
    [InlineData("en", 1, "night")]
    [InlineData("en", 3, "nights")]
    [InlineData("de", 1, "Nacht")]
    [InlineData("de", 2, "Nächte")]
    public void Nights_uses_correct_plural_form(string language, int n, string expected)
        => Assert.Equal(expected, Plural.Nights(language, n));

    [Fact]
    public void Every_key_has_a_non_empty_text_in_all_five_languages()
    {
        Assert.Equal(5, Texts.Languages.Length);
        foreach (var (key, values) in Texts.All)
        {
            Assert.Equal(Texts.Languages.Length, values.Length);
            Assert.All(values, v => Assert.False(string.IsNullOrWhiteSpace(v), $"Prázdný překlad u klíče {key}"));
        }
    }

    [Fact]
    public void No_unresolved_template_tokens_remain()
    {
        foreach (var (key, values) in Texts.All)
            foreach (var v in values)
                foreach (var token in new[] { "{ph}", "{td}", "{tdl}", "{tdn}" })
                    Assert.DoesNotContain(token, v);
    }

    [Fact]
    public void Estimate_templates_keep_their_placeholders()
    {
        foreach (var text in Texts.All["est_res"])
        {
            Assert.Contains("{n}", text);
            Assert.Contains("{nights}", text);
            Assert.Contains("{total}", text);
        }

        foreach (var text in Texts.All["est_min"])
            Assert.Contains("{min}", text);
    }

    [Fact]
    public void Saved_language_wins_and_unknown_key_is_visible()
    {
        var store = new MemoryStore();
        store.Set(Localizer.PreferenceKey, "de");
        var l = new Localizer(store);

        Assert.Equal("de", l.Language);
        Assert.Equal("Start", l["n_home"]);
        Assert.Equal("[[does_not_exist]]", l["does_not_exist"]);
    }

    [Fact]
    public void SetLanguage_persists_and_raises_event_once()
    {
        var store = new MemoryStore();
        store.Set(Localizer.PreferenceKey, "cs");
        var l = new Localizer(store);
        var raised = 0;
        l.Changed += () => raised++;

        l.SetLanguage("pl");
        l.SetLanguage("pl");      // beze změny, událost se nespustí
        l.SetLanguage("xx");      // neznámý jazyk se ignoruje

        Assert.Equal(1, raised);
        Assert.Equal("pl", l.Language);
        Assert.Equal("pl", store.Get(Localizer.PreferenceKey));
        Assert.Equal("Start", l["n_home"]);
    }

    [Fact]
    public void AppConfig_builds_language_specific_web_urls()
    {
        var cfg = new AppConfig { WebsiteUrl = "https://example.github.io/chalupa/" };
        Assert.Equal("https://example.github.io/chalupa/", cfg.WebUrlFor("cs"));
        Assert.Equal("https://example.github.io/chalupa/de/cenik.html", cfg.WebUrlFor("de", "cenik.html"));
    }
}
