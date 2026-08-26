using Griesoft.OrchardCore.CookieConsent.Models;

namespace Griesoft.OrchardCore.CookieConsent.Services;

/// <summary>
/// Persists consent decisions reported by the banner. The default implementation
/// only writes to the application log; a durable store is planned (see docs/PLAN.md).
/// </summary>
public interface IConsentRecordService
{
    /// <summary>
    /// Persist a single consent decision.
    /// </summary>
    Task RecordAsync(ConsentRecord record);
}
