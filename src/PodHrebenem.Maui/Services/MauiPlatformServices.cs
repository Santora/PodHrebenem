using PodHrebenem.Shared.Services;

namespace PodHrebenem.Maui.Services;

/// <summary>Nativní akce v Androidu: prohlížeč, telefon, e-mail.</summary>
public sealed class MauiPlatformService : IPlatformService
{
    public Task OpenUrlAsync(string url) =>
        Launcher.Default.OpenAsync(new Uri(url));

    public async Task DialAsync(string phone)
    {
        if (PhoneDialer.Default.IsSupported)
            PhoneDialer.Default.Open(phone);
        else
            await Launcher.Default.OpenAsync(new Uri("tel:" + phone));
    }

    public async Task ComposeEmailAsync(string to, string subject, string body)
    {
        if (Email.Default.IsComposeSupported)
        {
            await Email.Default.ComposeAsync(new EmailMessage
            {
                Subject = subject,
                Body = body,
                To = new List<string> { to }
            });
        }
        else
        {
            var uri = $"mailto:{to}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";
            await Launcher.Default.OpenAsync(new Uri(uri));
        }
    }
}

public sealed class MauiPreferenceStore : IPreferenceStore
{
    public string? Get(string key) => Preferences.Default.Get<string?>(key, null);

    public void Set(string key, string value) => Preferences.Default.Set(key, value);
}
