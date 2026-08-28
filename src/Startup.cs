using Griesoft.OrchardCore.CookieConsent.Drivers;
using Griesoft.OrchardCore.CookieConsent.Filters;
using Griesoft.OrchardCore.CookieConsent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.ResourceManagement;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;

namespace Griesoft.OrchardCore.CookieConsent;

/// <summary>
/// Registers services for the Cookie Consent module.
/// </summary>
public sealed class Startup : StartupBase
{
    /// <inheritdoc />
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IDisplayDriver<ISite>, CookieConsentSettingsDisplayDriver>();
        services.AddScoped<INavigationProvider, AdminMenu>();
        services.AddScoped<IPermissionProvider, Permissions>();
        services.AddScoped<IConsentRecordService, LoggingConsentRecordService>();
        services.AddResourceConfiguration<ResourceManagementOptionsConfiguration>();

        services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add<CookieConsentFilter>();
        });
    }
}
