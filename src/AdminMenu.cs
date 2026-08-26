using Griesoft.OrchardCore.CookieConsent.Drivers;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Griesoft.OrchardCore.CookieConsent;

/// <summary>
/// Cookie consent settings admin menu navigation provider.
/// </summary>
public class AdminMenu : AdminNavigationProvider
{
    private readonly IStringLocalizer S;

    /// <summary>
    /// </summary>
    public AdminMenu(IStringLocalizer<AdminMenu> stringLocalizer)
    {
        S = stringLocalizer;
    }

    /// <inheritdoc />
    protected override ValueTask BuildAsync(NavigationBuilder builder)
    {
        builder.Add(S["Configuration"], configuration => configuration
            .Add(S["Settings"], settings => settings
                .Add(S["Cookie Consent"], S["Cookie Consent"], cookieConsent => cookieConsent
                    .AddClass("cookieconsent").Id("cookieconsent")
                    .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = CookieConsentSettingsDisplayDriver.EditorGroupId })
                    .Permission(Permissions.ManageCookieConsentSettings)
                    .LocalNav()
                )));

        return ValueTask.CompletedTask;
    }
}
