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
    public void ScanAndDispatchAcknowledgements_WhenMultipleAcknowledgementsReturned_ShouldDispatchAll()
    {
        _shippingAcknowledgementProvider.GetShippingAcknowledgementPaths().Returns([
            "FirstAcknowledgement",
            "SecondAcknowledgement",
            "ThirdAcknowledgement"
        ]);
        
        _shippingAcknowledgementScanner.ScanAndDispatchAcknowledgements();

        _shippingAcknowledgementProcessor.Received(3).ProcessShippingAcknowledgementNotification(Arg.Any<string>());
        
        _shippingAcknowledgementProcessor.Received().ProcessShippingAcknowledgementNotification("FirstAcknowledgement");
        _shippingAcknowledgementProcessor.Received().ProcessShippingAcknowledgementNotification("SecondAcknowledgement");
        _shippingAcknowledgementProcessor.Received().ProcessShippingAcknowledgementNotification("ThirdAcknowledgement");
    }
}