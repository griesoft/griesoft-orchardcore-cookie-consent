using Griesoft.OrchardCore.CookieConsent.Models;
using Microsoft.Extensions.Logging;

namespace Griesoft.OrchardCore.CookieConsent.Services;

/// <summary>
/// Placeholder consent record store that writes decisions to the application log.
/// </summary>
public class LoggingConsentRecordService : IConsentRecordService
{
    private readonly ILogger<LoggingConsentRecordService> _logger;

    /// <summary>
    /// </summary>
    public LoggingConsentRecordService(ILogger<LoggingConsentRecordService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Task RecordAsync(ConsentRecord record)
    {
        _logger.LogInformation(
            "Cookie consent recorded. ConsentId: {ConsentId}, AcceptType: {AcceptType}, Accepted: {Accepted}, Rejected: {Rejected}, RecordedUtc: {RecordedUtc}",
            record.ConsentId,
            record.AcceptType,
            string.Join(',', record.AcceptedCategories),
            string.Join(',', record.RejectedCategories),
            record.RecordedUtc);

        return Task.CompletedTask;
    }
}
