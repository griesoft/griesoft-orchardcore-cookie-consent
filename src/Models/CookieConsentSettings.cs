namespace Griesoft.OrchardCore.CookieConsent.Models;

/// <summary>
/// Per-tenant cookie consent settings, stored as a site settings section.
/// Only look-and-feel options are tenant-configurable; compliance-critical
/// behavior (opt-in mode, symmetric buttons, script blocking) is fixed by the module.
/// </summary>
public class CookieConsentSettings
{
    /// <summary>
    /// Whether the consent banner is rendered on the site front end.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Title shown in the consent modal. Falls back to a built-in default when empty.
    /// </summary>
    public string? BannerTitle { get; set; }

    /// <summary>
    /// Description text shown in the consent modal. Falls back to a built-in default when empty.
    /// </summary>
    public string? BannerDescription { get; set; }

    /// <summary>
    /// Consent modal position, e.g. "bottom left", "bottom center", "middle center".
    /// </summary>
    public string Position { get; set; } = "bottom left";

    /// <summary>
    /// Consent modal layout, e.g. "box", "cloud", "bar".
    /// </summary>
    public string Layout { get; set; } = "box";

    /// <summary>
    /// Primary accent color (CSS color value) applied to the banner buttons.
    /// </summary>
    public string? PrimaryColor { get; set; }

    /// <summary>
    /// Optional logo URL displayed in the consent modal title.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Whether the "functionality" cookie category is offered.
    /// The "necessary" category is always present and read-only.
    /// </summary>
    public bool EnableFunctionalityCategory { get; set; } = true;

    /// <summary>
    /// Whether the "analytics" cookie category is offered.
    /// </summary>
    public bool EnableAnalyticsCategory { get; set; } = true;

    /// <summary>
    /// Whether the "marketing" cookie category is offered.
    /// </summary>
    public bool EnableMarketingCategory { get; set; } = true;

    /// <summary>
    /// Whether consent decisions are posted back to the server and logged.
    /// </summary>
    public bool LogConsentRecords { get; set; } = true;
}
