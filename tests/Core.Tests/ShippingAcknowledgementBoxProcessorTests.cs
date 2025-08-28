using System.Threading.Channels;
using Core.Models;
using NSubstitute;

namespace Core.Tests;

public class ShippingAcknowledgementBoxProcessorTests
{
    private readonly ShippingAcknowledgementBoxProcessor _shippingAcknowledgementBoxProcessor;
    private readonly IShippingAcknowledgementRepository _shippingAcknowledgementRepository;

    public ShippingAcknowledgementBoxProcessorTests()
    {
        _shippingAcknowledgementRepository = Substitute.For<IShippingAcknowledgementRepository>();

        _shippingAcknowledgementBoxProcessor =
            new ShippingAcknowledgementBoxProcessor(_shippingAcknowledgementRepository, 2);
    }

    [Fact]
    public async Task SaveShippingAcknowledgementBoxes_WhenBatchSizeIsTheSameAsBoxCount_ShouldSaveTheFullBatch()
    {
        var channel = Channel.CreateUnbounded<Box>();

        var box1 = new Box();
        var box2 = new Box();
        
        await channel.Writer.WriteAsync(box1);
        await channel.Writer.WriteAsync(box2);

        channel.Writer.Complete();

        await _shippingAcknowledgementBoxProcessor.SaveShippingAcknowledgementBoxes(channel.Reader);

        await _shippingAcknowledgementRepository.Received(1).SaveBoxes(Arg.Any<List<Box>>());
        await _shippingAcknowledgementRepository.Received(1).SaveBoxes(Arg.Is<List<Box>>(x => x.SequenceEqual(new[] { box1, box2 })));
    }

    [Fact]
    public async Task SaveShippingAcknowledgementBoxes_WhenBatchSizeNotDivisibleByBoxCount_ShouldSaveTheLastIncompleteBatch()
    {
        var channel = Channel.CreateUnbounded<Box>();

        var box1 = new Box();
        var box2 = new Box();
        var box3 = new Box();

        await channel.Writer.WriteAsync(box1);
        await channel.Writer.WriteAsync(box2);
        await channel.Writer.WriteAsync(box3);

        channel.Writer.Complete();

        await _shippingAcknowledgementBoxProcessor.SaveShippingAcknowledgementBoxes(channel.Reader);

        await _shippingAcknowledgementRepository.Received(2).SaveBoxes(Arg.Any<List<Box>>());
        await _shippingAcknowledgementRepository.Received(1).SaveBoxes(Arg.Is<List<Box>>(x => x.SequenceEqual(new[] { box1, box2 })));
        await _shippingAcknowledgementRepository.Received(1).SaveBoxes(Arg.Is<List<Box>>(x => x.SequenceEqual(new[] { box3 })));
    }
}