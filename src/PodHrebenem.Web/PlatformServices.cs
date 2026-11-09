using Microsoft.JSInterop;
using PodHrebenem.Shared.Services;

namespace PodHrebenem.Web;

/// <summary>Akce v prohlížeči: odkazy a protokoly tel:/mailto: se otevřou přes okno prohlížeče.</summary>
public sealed class WebPlatformService(IJSRuntime js) : IPlatformService
{
    public async Task OpenUrlAsync(string url) => await js.InvokeVoidAsync("open", url, "_blank", "noopener");

    public async Task DialAsync(string phone) => await js.InvokeVoidAsync("eval", $"window.location.href = {System.Text.Json.JsonSerializer.Serialize("tel:" + phone)}");

    public async Task ComposeEmailAsync(string to, string subject, string body)
    {
        var uri = $"mailto:{to}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";
        await js.InvokeVoidAsync("eval", $"window.location.href = {System.Text.Json.JsonSerializer.Serialize(uri)}");
    }
}

/// <summary>Nastavení v localStorage (synchronně, aby šel jazyk určit už při startu).</summary>
public sealed class LocalStoragePreferenceStore(IJSRuntime js) : IPreferenceStore
{
    private IJSInProcessRuntime Runtime => (IJSInProcessRuntime)js;

    public string? Get(string key)
    {
        try { return Runtime.Invoke<string?>("localStorage.getItem", key); }
        catch { return null; }
    }

    public void Set(string key, string value)
    {
        try { Runtime.InvokeVoid("localStorage.setItem", key, value); }
        catch { /* soukromé okno apod. */ }
    }
}
