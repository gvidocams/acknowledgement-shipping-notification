using Core.ShippingAcknowledgements;
using Microsoft.Extensions.Options;

namespace ShippingAcknowledgementWorker;

public class ShippingAcknowledgementWorker(
    IServiceScopeFactory serviceScopeFactory,
    IOptions<AcknowledgementScanningOptions> acknowledgementScanningOptions,
    ILogger<ShippingAcknowledgementWorker> logger)
    : BackgroundService
{
    private readonly AcknowledgementScanningOptions _acknowledgementScanningOptions = acknowledgementScanningOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{ServiceName} has been started", nameof(ShippingAcknowledgementWorker));
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_acknowledgementScanningOptions.ScanIntervalInSeconds));

        // What happens if the current iteration is processing too long, and it hits the next iteration?
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            logger.LogInformation("{ServiceName} is starting to process it's current interval", nameof(ShippingAcknowledgementWorker));

            using var scope = serviceScopeFactory.CreateScope(); // TODO Investigate if this is the best way on how to dispose of the scope
            var shippingAcknowledgementScanner = scope.ServiceProvider.GetRequiredService<IShippingAcknowledgementScanner>();
            await shippingAcknowledgementScanner.ScanAndDispatchAcknowledgements();

            logger.LogInformation("{ServiceName} has completed the current interval", nameof(ShippingAcknowledgementWorker));
        }

        logger.LogInformation("{ServiceName} has been stopped", nameof(ShippingAcknowledgementWorker));
    }
}