using System.Text.Json;
using Griesoft.OrchardCore.CookieConsent.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.Admin;
using OrchardCore.Entities;
using OrchardCore.ResourceManagement;
using OrchardCore.Settings;

namespace Griesoft.OrchardCore.CookieConsent.Filters;

/// <summary>
/// Injects the cookie consent banner assets and the tenant-specific configuration
/// into every full front-end page.
/// </summary>
public class CookieConsentFilter : IAsyncResultFilter
{
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IResourceManager _resourceManager;
    private readonly ISiteService _siteService;

    /// <summary>
    /// </summary>
    public CookieConsentFilter(IResourceManager resourceManager, ISiteService siteService)
    {
        _resourceManager = resourceManager;
        _siteService = siteService;
    }

    /// <inheritdoc />
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // Only inject into full HTML pages on the site front end.
        if ((context.Result is ViewResult || context.Result is PageResult)
            && !AdminAttribute.IsApplied(context.HttpContext))
        {
            var settings = (await _siteService.GetSiteSettingsAsync()).As<CookieConsentSettings>();

            if (settings.Enabled)
            {
                var config = new
                {
                    bannerTitle = settings.BannerTitle,
                    bannerDescription = settings.BannerDescription,
                    position = settings.Position,
                    layout = settings.Layout,
                    primaryColor = settings.PrimaryColor,
                    logoUrl = settings.LogoUrl,
                    functionalityCategory = settings.EnableFunctionalityCategory,
                    analyticsCategory = settings.EnableAnalyticsCategory,
                    marketingCategory = settings.EnableMarketingCategory,
                    recordUrl = settings.LogConsentRecords ? "/cookieconsent/record" : null,
                };

                _resourceManager.RegisterHeadScript(new HtmlString(
                    $"<script>window.griesoftCookieConsent = {JsonSerializer.Serialize(config, _jsonOptions)};</script>"));

                _resourceManager.RegisterResource("stylesheet", "griesoft-cookieconsent");
                _resourceManager.RegisterResource("script", "griesoft-cookieconsent").AtFoot();
                _resourceManager.RegisterResource("script", "griesoft-cookieconsent-init").AtFoot();
            }
        }

        await next();
    }
}
