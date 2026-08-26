using Griesoft.OrchardCore.CookieConsent.Models;
using Xunit;

namespace Griesoft.OrchardCore.CookieConsent.Tests;

public class CookieConsentSettingsTests
{
    [Fact]
    public void Defaults_AreCompliantAndEnabled()
    {
        var settings = new CookieConsentSettings();

        Assert.True(settings.Enabled);
        Assert.Equal("bottom left", settings.Position);
        Assert.Equal("box", settings.Layout);
        Assert.True(settings.EnableFunctionalityCategory);
        Assert.True(settings.EnableAnalyticsCategory);
        Assert.True(settings.EnableMarketingCategory);
        Assert.True(settings.LogConsentRecords);
    }

    [Fact]
    public void ConsentRecord_Defaults_AreEmptyNotNull()
    {
        var record = new ConsentRecord();

        Assert.NotNull(record.AcceptedCategories);
        Assert.NotNull(record.RejectedCategories);
        Assert.Empty(record.AcceptedCategories);
        Assert.Empty(record.RejectedCategories);
    }
}
