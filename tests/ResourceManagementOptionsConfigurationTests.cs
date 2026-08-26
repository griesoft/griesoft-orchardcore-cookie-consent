using Griesoft.OrchardCore.CookieConsent.Services;
using OrchardCore.ResourceManagement;
using Xunit;

namespace Griesoft.OrchardCore.CookieConsent.Tests;

public class ResourceManagementOptionsConfigurationTests
{
    [Fact]
    public void Configure_RegistersCookieConsentResources()
    {
        var options = new ResourceManagementOptions();

        new ResourceManagementOptionsConfiguration().Configure(options);

        var manifest = Assert.Single(options.ResourceManifests);

        var scripts = manifest.GetResources("script");
        var styles = manifest.GetResources("stylesheet");

        Assert.Contains("griesoft-cookieconsent", scripts.Keys);
        Assert.Contains("griesoft-cookieconsent-init", scripts.Keys);
        Assert.Contains("griesoft-cookieconsent", styles.Keys);
    }
}
