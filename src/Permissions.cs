using OrchardCore.Security.Permissions;

namespace Griesoft.OrchardCore.CookieConsent;

/// <summary>
/// Cookie consent settings permissions.
/// </summary>
public class Permissions : IPermissionProvider
{
    /// <summary>
    /// Allows managing the tenant's cookie consent settings.
    /// </summary>
    public static readonly Permission ManageCookieConsentSettings = new(
        nameof(ManageCookieConsentSettings),
        "Manage cookie consent settings.");

    /// <inheritdoc />
    public Task<IEnumerable<Permission>> GetPermissionsAsync() => Task.FromResult(new[]
    {
        ManageCookieConsentSettings,
    }
    .AsEnumerable());

    /// <inheritdoc />
    public IEnumerable<PermissionStereotype> GetDefaultStereotypes() =>
    new[]
    {
        new PermissionStereotype
        {
            Name = "Administrator",
            Permissions = new[] { ManageCookieConsentSettings },
        },
    };
}
