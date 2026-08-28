using global::OrchardCore;
using OrchardCore.Security.Permissions;

namespace Griesoft.OrchardCore.CookieConsent;

/// <summary>
/// The permissions defined by the Cookie Consent module.
/// </summary>
public static class CookieConsentPermissions
{
    /// <summary>
    /// Allows managing the tenant's cookie consent settings.
    /// </summary>
    public static readonly Permission ManageCookieConsentSettings = new(
        nameof(ManageCookieConsentSettings),
        "Manage cookie consent settings.");
}

/// <summary>
/// Cookie consent settings permission provider.
/// </summary>
public sealed class Permissions : IPermissionProvider
{
    private readonly IEnumerable<Permission> _allPermissions =
    [
        CookieConsentPermissions.ManageCookieConsentSettings,
    ];

    /// <inheritdoc />
    public Task<IEnumerable<Permission>> GetPermissionsAsync() => Task.FromResult(_allPermissions);

    /// <inheritdoc />
    public IEnumerable<PermissionStereotype> GetDefaultStereotypes() =>
    [
        new PermissionStereotype
        {
            Name = OrchardCoreConstants.Roles.Administrator,
            Permissions = _allPermissions,
        },
    ];
}
