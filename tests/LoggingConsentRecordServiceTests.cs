using Griesoft.OrchardCore.CookieConsent.Models;
using Griesoft.OrchardCore.CookieConsent.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Griesoft.OrchardCore.CookieConsent.Tests;

public class LoggingConsentRecordServiceTests
{
    [Fact]
    public async Task RecordAsync_LogsInformation()
    {
        var logger = new Mock<ILogger<LoggingConsentRecordService>>();
        var service = new LoggingConsentRecordService(logger.Object);

        await service.RecordAsync(new ConsentRecord
        {
            ConsentId = "test-consent-id",
            AcceptType = "custom",
            AcceptedCategories = ["necessary", "analytics"],
            RejectedCategories = ["marketing"],
            RecordedUtc = DateTime.UtcNow,
        });

        logger.Verify(
            l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("test-consent-id")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
