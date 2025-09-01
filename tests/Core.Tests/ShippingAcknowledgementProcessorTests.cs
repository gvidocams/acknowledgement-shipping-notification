using System.Threading.Channels;
using Core.Models;
using Core.ShippingAcknowledgements;
using NSubstitute;

namespace Core.Tests;

public class ShippingAcknowledgementProcessorTests
{
    private readonly ShippingAcknowledgementProcessor _shippingAcknowledgementProcessor;
    private readonly IShippingAcknowledgementParser _shippingAcknowledgementParser;
    private readonly IShippingAcknowledgementBoxProcessor _shippingAcknowledgementBoxProcessor;
    private readonly IShippingAcknowledgementProvider _shippingAcknowledgementProvider;


    public ShippingAcknowledgementProcessorTests()
    {
        _shippingAcknowledgementParser = Substitute.For<IShippingAcknowledgementParser>();
        _shippingAcknowledgementBoxProcessor = Substitute.For<IShippingAcknowledgementBoxProcessor>();
        _shippingAcknowledgementProvider = Substitute.For<IShippingAcknowledgementProvider>();

        _shippingAcknowledgementProcessor = new ShippingAcknowledgementProcessor(
            _shippingAcknowledgementParser,
            _shippingAcknowledgementBoxProcessor,
            _shippingAcknowledgementProvider,
            1);
    }

    [Fact]
    public async Task ProcessShippingAcknowledgementNotification_WhenExecuted_ShouldCallAllServicesWithExpectedArguments()
    {
        const string notificationLocation = "/path/to/acknowledgement-shipping-notification.txt";

        await _shippingAcknowledgementProcessor.ProcessShippingAcknowledgementNotification(notificationLocation);

        await _shippingAcknowledgementParser
            .Received(1)
            .ParseShippingAcknowledgementNotification(Arg.Any<ChannelWriter<Box>>(), notificationLocation);
        await _shippingAcknowledgementBoxProcessor
            .Received(1)
            .SaveShippingAcknowledgementBoxes(Arg.Any<ChannelReader<Box>>());

        _shippingAcknowledgementProvider
            .Received(1)
            .CompleteShippingAcknowledgementNotification(notificationLocation);
    }
}