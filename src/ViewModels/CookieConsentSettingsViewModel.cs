namespace Griesoft.OrchardCore.CookieConsent.ViewModels;

/// <summary>
/// View model for the cookie consent settings editor.
/// </summary>
public class CookieConsentSettingsViewModel
{
    /// <summary>
    /// Whether the consent banner is rendered on the site front end.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Title shown in the consent modal.
    /// </summary>
    public string? BannerTitle { get; set; }

    /// <summary>
    /// Description text shown in the consent modal.
    /// </summary>
    public string? BannerDescription { get; set; }

    /// <summary>
    /// Consent modal position.
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// Consent modal layout.
    /// </summary>
    public string? Layout { get; set; }

    /// <summary>
    /// Primary accent color (CSS color value).
    /// </summary>
    public string? PrimaryColor { get; set; }

    /// <summary>
    /// Optional logo URL displayed in the consent modal.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Whether the "functionality" category is offered.
    /// </summary>
    public bool EnableFunctionalityCategory { get; set; }

    /// <summary>
    /// Whether the "analytics" category is offered.
    /// </summary>
    public bool EnableAnalyticsCategory { get; set; }

    /// <summary>
    /// Whether the "marketing" category is offered.
    /// </summary>
    public bool EnableMarketingCategory { get; set; }

    /// <summary>
    /// Whether consent decisions are logged server-side.
    /// </summary>
    public bool LogConsentRecords { get; set; }
}
