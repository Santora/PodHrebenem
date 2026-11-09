# Pod Hřebenem – mobilní klient (MAUI Blazor Hybrid + PWA)

Pracovní název „Pod Hřebenem“ platí do rozhodnutí klienta. Obsah a ceny jsou ukázkové.

## Řešení (Visual Studio 2026, .NET 10)
| Projekt | Účel |
|---|---|
| `src/PodHrebenem.Shared` | Razor Class Library: stránky, layout, lokalizace (CS, EN, DE, PL, SK), kalkulačka ceny |
| `src/PodHrebenem.Maui` | Android aplikace (Blazor Hybrid), nativní volání, e-mail, mapa |
| `src/PodHrebenem.Web` | Blazor WebAssembly PWA se stejným UI, ke zveřejnění na GitHub Pages |
| `tests/PodHrebenem.Tests` | xUnit testy kalkulačky, množných čísel a překladů |

Otevřete `PodHrebenem.slnx`. Pro MAUI je potřeba workload **.NET Multi-platform App UI** (Android SDK, emulátor). Spouštěcí projekt: `PodHrebenem.Maui` (Android emulátor) nebo `PodHrebenem.Web` (prohlížeč).

> Kostra vznikla bez možnosti sestavit ji (v prostředí, kde byla psána, nebylo .NET SDK). Před dalším vývojem proto spusťte *Build Solution* a *Run All Tests* a případné drobnosti (verze balíčků, MAUI workload) opravte.

## Texty a překlady
Jediný zdroj textů je `build.py` webu (slovník `T`). Aplikace z něj dostává vygenerovaný soubor `Localization/Texts.g.cs` (needitovat ručně):

```
python tools/export_texts.py ../PodHrebenem/src/PodHrebenem.Shared/Localization/Texts.g.cs
```
Skript je ve zdrojích webu (`chalupa-generator-zdroj.zip`, složka `tools`). Spouští se z kořene tohoto generátoru.

## Nastavení před ostrým provozem
- `AppConfig` (`Services/AppConfig.cs`): adresa webu, telefon, e-mail, odkaz do mapy.
- `PriceOptions` (`Services/PriceCalculator.cs`): ceny a sezóny, musí odpovídat ceníku na webu.
- `ApplicationId` (`cz.podhrebenem.app`) a ikony v `Resources/` při změně názvu.
- Fotografie: nahradit bloky `.ph` obrázky ve `wwwroot` sdílené knihovny.

## PWA na GitHub Pages
1. Nahrát řešení do repozitáře, v *Settings → Pages* zvolit **GitHub Actions**.
2. Workflow `.github/workflows/deploy-pwa.yml` spustí testy, publikuje PWA a upraví `<base href>` i service worker podle názvu repozitáře.
3. Aplikace poběží na `https://UZIVATEL.github.io/REPOZITAR/` a jde „přidat na plochu“.

## Další fáze
1. Kalendář obsazenosti (iCal) a odesílání poptávek přes API místo e-mailu.
2. Hostovský režim po rezervaci (Wi-Fi, příjezd, provozní řád, offline).
3. Notifikace, případně iOS (potřebuje Mac a Apple účet).
