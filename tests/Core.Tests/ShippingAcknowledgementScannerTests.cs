using Core.ShippingAcknowledgements;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Core.Tests;

public class ShippingAcknowledgementScannerTests
{
    private readonly ShippingAcknowledgementScanner _shippingAcknowledgementScanner;
    private readonly IShippingAcknowledgementProvider _shippingAcknowledgementProvider;
    private readonly IShippingAcknowledgementProcessor _shippingAcknowledgementProcessor;
    private readonly ILogger<ShippingAcknowledgementScanner> _logger;

    public ShippingAcknowledgementScannerTests()
    {
        _shippingAcknowledgementProvider = Substitute.For<IShippingAcknowledgementProvider>();
        _shippingAcknowledgementProcessor = Substitute.For<IShippingAcknowledgementProcessor>();
        _logger = Substitute.For<ILogger<ShippingAcknowledgementScanner>>();

        _shippingAcknowledgementScanner = new ShippingAcknowledgementScanner(
            _shippingAcknowledgementProvider,
            _shippingAcknowledgementProcessor,
            _logger);
    }

    [Fact]
    public async Task ScanAndDispatchAcknowledgements_WhenMultipleAcknowledgementsReturned_ShouldDispatchAll()
    {
        _shippingAcknowledgementProvider.GetShippingAcknowledgementPaths().Returns([
            "FirstAcknowledgement",
            "SecondAcknowledgement",
            "ThirdAcknowledgement"
        ]);
        
        await _shippingAcknowledgementScanner.ScanAndDispatchAcknowledgements();

        await _shippingAcknowledgementProcessor.Received(3).ProcessShippingAcknowledgementNotification(Arg.Any<string>());
        
        await _shippingAcknowledgementProcessor.Received().ProcessShippingAcknowledgementNotification("FirstAcknowledgement");
        await _shippingAcknowledgementProcessor.Received().ProcessShippingAcknowledgementNotification("SecondAcknowledgement");
        await _shippingAcknowledgementProcessor.Received().ProcessShippingAcknowledgementNotification("ThirdAcknowledgement");
    }
}