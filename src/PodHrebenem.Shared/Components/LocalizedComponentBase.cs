using Microsoft.AspNetCore.Components;
using PodHrebenem.Shared.Localization;

namespace PodHrebenem.Shared.Components;

/// <summary>Základ stránek: vstříknutý Localizer (L) a překreslení při změně jazyka.</summary>
public abstract class LocalizedComponentBase : ComponentBase, IDisposable
{
    [Inject] protected Localizer L { get; set; } = default!;

    protected override void OnInitialized() => L.Changed += OnLanguageChanged;

    private void OnLanguageChanged() => _ = InvokeAsync(StateHasChanged);

    /// <summary>Text obsahující jednoduché HTML (např. &lt;b&gt;), které se má vykreslit.</summary>
    protected MarkupString Html(string key) => new(L[key]);

    public void Dispose() => L.Changed -= OnLanguageChanged;
}

/// <summary>Totéž pro layout.</summary>
public abstract class LocalizedLayoutBase : LayoutComponentBase, IDisposable
{
    [Inject] protected Localizer L { get; set; } = default!;

    protected override void OnInitialized() => L.Changed += OnLanguageChanged;

    private void OnLanguageChanged() => _ = InvokeAsync(StateHasChanged);

    public void Dispose() => L.Changed -= OnLanguageChanged;
}
