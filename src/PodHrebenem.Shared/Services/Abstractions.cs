namespace PodHrebenem.Shared.Services;

/// <summary>Nativní akce, které se v aplikaci (MAUI) a v PWA provádějí různě.</summary>
public interface IPlatformService
{
    Task OpenUrlAsync(string url);
    Task DialAsync(string phone);
    Task ComposeEmailAsync(string to, string subject, string body);
}

/// <summary>Jednoduché úložiště nastavení (MAUI Preferences / localStorage v PWA).</summary>
public interface IPreferenceStore
{
    string? Get(string key);
    void Set(string key, string value);
}
