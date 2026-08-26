using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Griesoft.OrchardCore.CookieConsent.Services;

/// <summary>
/// Registers the vendored vanilla-cookieconsent assets and the module init script
/// as named resources.
/// </summary>
public sealed class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    /// <summary>
    /// The vendored vanilla-cookieconsent version.
    /// </summary>
    public const string CookieConsentVersion = "3.1.0";

    private static readonly ResourceManifest _manifest;

    static ResourceManagementOptionsConfiguration()
    {
        _manifest = new ResourceManifest();

        _manifest
            .DefineScript("griesoft-cookieconsent")
            .SetUrl("~/Griesoft.OrchardCore.CookieConsent/vendor/cookieconsent/cookieconsent.umd.js")
            .SetVersion(CookieConsentVersion);

        _manifest
            .DefineScript("griesoft-cookieconsent-init")
            .SetUrl("~/Griesoft.OrchardCore.CookieConsent/scripts/cookieconsent-init.js")
            .SetDependencies("griesoft-cookieconsent")
            .SetVersion("1.0.0");

        _manifest
            .DefineStyle("griesoft-cookieconsent")
            .SetUrl("~/Griesoft.OrchardCore.CookieConsent/vendor/cookieconsent/cookieconsent.css")
            .SetVersion(CookieConsentVersion);
    }

    /// <inheritdoc />
    public void Configure(ResourceManagementOptions options)
    {
        options.ResourceManifests.Add(_manifest);
    }
}
