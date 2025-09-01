using System.Threading.Channels;
using Core.Models;
using Core.ShippingAcknowledgements;
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
    public async Task SaveShippingAcknowledgementBoxesAsync_WhenBatchSizeIsTheSameAsBoxCount_ShouldSaveTheFullBatch()
    {
        var channel = Channel.CreateUnbounded<Box>();

        var box1 = new Box();
        var box2 = new Box();
        
        await channel.Writer.WriteAsync(box1);
        await channel.Writer.WriteAsync(box2);

        channel.Writer.Complete();

        await _shippingAcknowledgementBoxProcessor.SaveShippingAcknowledgementBoxesAsync(channel.Reader);

        await _shippingAcknowledgementRepository.Received(1).SaveBoxesAsync(Arg.Any<List<Box>>());
        await _shippingAcknowledgementRepository.Received(1).SaveBoxesAsync(Arg.Is<List<Box>>(x => x.SequenceEqual(new[] { box1, box2 })));
    }

    [Fact]
    public async Task SaveShippingAcknowledgementBoxesAsync_WhenBatchSizeNotDivisibleByBoxCount_ShouldSaveTheLastIncompleteBatch()
    {
        var channel = Channel.CreateUnbounded<Box>();

        var box1 = new Box();
        var box2 = new Box();
        var box3 = new Box();

        await channel.Writer.WriteAsync(box1);
        await channel.Writer.WriteAsync(box2);
        await channel.Writer.WriteAsync(box3);

        channel.Writer.Complete();

        await _shippingAcknowledgementBoxProcessor.SaveShippingAcknowledgementBoxesAsync(channel.Reader);

        await _shippingAcknowledgementRepository.Received(2).SaveBoxesAsync(Arg.Any<List<Box>>());
        await _shippingAcknowledgementRepository.Received(1).SaveBoxesAsync(Arg.Is<List<Box>>(x => x.SequenceEqual(new[] { box1, box2 })));
        await _shippingAcknowledgementRepository.Received(1).SaveBoxesAsync(Arg.Is<List<Box>>(x => x.SequenceEqual(new[] { box3 })));
    }
}