using System.Threading.Channels;
using Core.Models;
using NSubstitute;

namespace Core.Tests;

public class ShippingAcknowledgementProcessorTests
{
    private readonly ShippingAcknowledgementProcessor _shippingAcknowledgementProcessor;
    private readonly IShippingAcknowledgementParser _shippingAcknowledgementParser;
    private readonly IShippingAcknowledgementBoxProcessor _shippingAcknowledgementBoxProcessor;


    public ShippingAcknowledgementProcessorTests()
    {
        _shippingAcknowledgementParser = Substitute.For<IShippingAcknowledgementParser>();
        _shippingAcknowledgementBoxProcessor = Substitute.For<IShippingAcknowledgementBoxProcessor>();

        _shippingAcknowledgementProcessor = new ShippingAcknowledgementProcessor(
            _shippingAcknowledgementParser,
            _shippingAcknowledgementBoxProcessor,
            1);
    }

    [Fact]
    public async Task ProcessShippingAcknowledgementNotification_WhenExecuted_ShouldCallAllServicesWithExpectedArguments()
    {
        const string filePath = "/path/to/acknowledgement-shipping-notification.txt";

        await _shippingAcknowledgementProcessor.ProcessShippingAcknowledgementNotification(filePath);

        await _shippingAcknowledgementParser
            .Received(1)
            .ParseShippingAcknowledgementNotification(Arg.Any<ChannelWriter<Box>>(), filePath);
        await _shippingAcknowledgementBoxProcessor
            .Received(1)
            .SaveShippingAcknowledgementBoxes(Arg.Any<ChannelReader<Box>>());
    }
}