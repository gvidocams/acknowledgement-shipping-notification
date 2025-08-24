using Microsoft.Extensions.Options;

namespace ShippingAcknowledgementWorker;

public class ShippingAcknowledgementWorker(
    IShippingAcknowledgementService shippingAcknowledgementService,
    IOptions<FileScanningOptions> fileScanningOptions,
    ILogger<ShippingAcknowledgementWorker> logger)
    : BackgroundService
{
    private readonly FileScanningOptions _fileScanningOptions = fileScanningOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{ServiceName} has been started", nameof(ShippingAcknowledgementWorker));

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_fileScanningOptions.FilePathScanIntervalInSeconds));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            logger.LogInformation("{ServiceName} is starting to process it's current interval", nameof(ShippingAcknowledgementWorker));

            shippingAcknowledgementService.HandleShippingAcknowledgements();

            logger.LogInformation("{ServiceName} has completed the current interval", nameof(ShippingAcknowledgementWorker));
        }

        logger.LogInformation("{ServiceName} has been stopped", nameof(ShippingAcknowledgementWorker));
    }
}