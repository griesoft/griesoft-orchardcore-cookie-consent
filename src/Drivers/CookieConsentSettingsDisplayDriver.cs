using Griesoft.OrchardCore.CookieConsent.Models;
using Griesoft.OrchardCore.CookieConsent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;

namespace Griesoft.OrchardCore.CookieConsent.Drivers;

/// <summary>
/// The display driver for the cookie consent settings editor group.
/// </summary>
public class CookieConsentSettingsDisplayDriver : SiteDisplayDriver<CookieConsentSettings>
{
    /// <summary>
    /// The settings editor group ID.
    /// </summary>
    public const string EditorGroupId = "GriesoftCookieConsent";

    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <inheritdoc />
    protected override string SettingsGroupId => EditorGroupId;

    /// <summary>
    /// </summary>
    public CookieConsentSettingsDisplayDriver(
        IAuthorizationService authorizationService,
        IHttpContextAccessor httpContextAccessor)
    {
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public override async Task<IDisplayResult?> EditAsync(ISite model, CookieConsentSettings section, BuildEditorContext context)
    {
        if (!await IsAuthorizedToManageSettingsAsync())
        {
            return null;
        }

        return Initialize<CookieConsentSettingsViewModel>($"{nameof(CookieConsentSettings)}_Edit", viewModel =>
        {
            viewModel.Enabled = section.Enabled;
            viewModel.BannerTitle = section.BannerTitle;
            viewModel.BannerDescription = section.BannerDescription;
            viewModel.Position = section.Position;
            viewModel.Layout = section.Layout;
            viewModel.PrimaryColor = section.PrimaryColor;
            viewModel.LogoUrl = section.LogoUrl;
            viewModel.EnableFunctionalityCategory = section.EnableFunctionalityCategory;
            viewModel.EnableAnalyticsCategory = section.EnableAnalyticsCategory;
            viewModel.EnableMarketingCategory = section.EnableMarketingCategory;
            viewModel.LogConsentRecords = section.LogConsentRecords;
        })
        .Location("Content:1")
        .OnGroup(SettingsGroupId);
    }

    /// <inheritdoc />
    public override async Task<IDisplayResult?> UpdateAsync(ISite model, CookieConsentSettings section, UpdateEditorContext context)
    {
        if (!await IsAuthorizedToManageSettingsAsync())
        {
            return null;
        }

        var viewModel = new CookieConsentSettingsViewModel();

        await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

        section.Enabled = viewModel.Enabled;
        section.BannerTitle = viewModel.BannerTitle;
        section.BannerDescription = viewModel.BannerDescription;
        section.Position = string.IsNullOrWhiteSpace(viewModel.Position) ? "bottom left" : viewModel.Position;
        section.Layout = string.IsNullOrWhiteSpace(viewModel.Layout) ? "box" : viewModel.Layout;
        section.PrimaryColor = viewModel.PrimaryColor;
        section.LogoUrl = viewModel.LogoUrl;
        section.EnableFunctionalityCategory = viewModel.EnableFunctionalityCategory;
        section.EnableAnalyticsCategory = viewModel.EnableAnalyticsCategory;
        section.EnableMarketingCategory = viewModel.EnableMarketingCategory;
        section.LogConsentRecords = viewModel.LogConsentRecords;

        return await EditAsync(model, section, context);
    }

    private async Task<bool> IsAuthorizedToManageSettingsAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        return user != null && await _authorizationService.AuthorizeAsync(user, Permissions.ManageCookieConsentSettings);
    }
}
