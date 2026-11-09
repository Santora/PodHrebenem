namespace PodHrebenem.Shared.Services;

/// <summary>Kontaktní údaje a adresy. Zatím ukázkové hodnoty, po dodání podkladů nahradit.</summary>
public sealed class AppConfig
{
    /// <summary>Adresa webové verze bez lomítka na konci (čeština v kořeni, ostatní jazyky ve složkách en/de/pl/sk).</summary>
    public string WebsiteUrl { get; init; } = "https://santora.github.io/chalupa-web";

    public string Email { get; init; } = "info@example.cz";
    public string Phone { get; init; } = "+420000000000";
    public string PhoneDisplay { get; init; } = "+420 000 000 000";

    /// <summary>Odkaz do mapy. Po doplnění přesné adresy nahradit konkrétním místem.</summary>
    public string MapsUrl { get; init; } = "https://www.google.com/maps/search/?api=1&query=B%C3%ADl%C3%A1+Moravskoslezsk%C3%BD+kraj";

    public string WebUrlFor(string language, string page = "")
    {
        var folder = language == "cs" ? "" : language + "/";
        return $"{WebsiteUrl.TrimEnd('/')}/{folder}{page}";
    }
}
