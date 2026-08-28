using Griesoft.OrchardCore.CookieConsent.Models;
using Griesoft.OrchardCore.CookieConsent.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Settings;

namespace Griesoft.OrchardCore.CookieConsent.Controllers;

/// <summary>
/// Receives consent decisions posted by the client-side banner.
/// </summary>
[Route("cookieconsent")]
public sealed class ConsentController : Controller
{
    private readonly IConsentRecordService _consentRecordService;
    private readonly ISiteService _siteService;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// </summary>
    public ConsentController(
        IConsentRecordService consentRecordService,
        ISiteService siteService,
        TimeProvider timeProvider)
    {
        _consentRecordService = consentRecordService;
        _siteService = siteService;
        _timeProvider = timeProvider;
    }

    /// <summary>
    /// Records a consent decision. Called by the banner via <c>navigator.sendBeacon</c>,
    /// so it must accept anonymous requests without an antiforgery token.
    /// </summary>
    [HttpPost("record")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Record([FromBody] ConsentRecord record)
    {
        var settings = await _siteService.GetSettingsAsync<CookieConsentSettings>();

        if (!settings.Enabled || !settings.LogConsentRecords)
        {
            return NotFound();
        }

        if (record == null || string.IsNullOrWhiteSpace(record.ConsentId))
        {
            return BadRequest();
        }

        record.RecordedUtc = _timeProvider.GetUtcNow().UtcDateTime;

        await _consentRecordService.RecordAsync(record);

        return NoContent();
    }
}
