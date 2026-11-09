using Microsoft.Extensions.Logging;
using PodHrebenem.Maui.Services;
using PodHrebenem.Shared;
using PodHrebenem.Shared.Services;

namespace PodHrebenem.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IPlatformService, MauiPlatformService>();
        builder.Services.AddSingleton<IPreferenceStore, MauiPreferenceStore>();
        builder.Services.AddPodHrebenemShared();

        return builder.Build();
    }
}
