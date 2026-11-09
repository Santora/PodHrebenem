using Microsoft.Extensions.DependencyInjection;
using PodHrebenem.Shared.Localization;
using PodHrebenem.Shared.Services;

namespace PodHrebenem.Shared;

public static class ServiceCollectionExtensions
{
    /// <summary>Registruje sdílené služby. Hostitel musí zaregistrovat IPlatformService a IPreferenceStore.</summary>
    public static IServiceCollection AddPodHrebenemShared(this IServiceCollection services, AppConfig? config = null, PriceOptions? prices = null)
    {
        services.AddSingleton(config ?? new AppConfig());
        services.AddSingleton(new PriceCalculator(prices));
        services.AddSingleton<Localizer>();
        return services;
    }
}
