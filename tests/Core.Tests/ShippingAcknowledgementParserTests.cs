using System.Threading.Channels;
using Core.Models;
using Core.ShippingAcknowledgements;
using Shouldly;

namespace Core.Tests;

public class ShippingAcknowledgementParserTests
{
    private readonly ShippingAcknowledgementParser _shippingAcknowledgementParser = new();

    [Fact]
    public async Task ParseShippingAcknowledgementNotification_WhenParsingFinished_ChannelShouldBeCompleted()
    {
        var channel = Channel.CreateUnbounded<Box>();

        await _shippingAcknowledgementParser.ParseShippingAcknowledgementNotification(channel.Writer,
            "./Data/multiple-boxes.txt");

        channel.Writer.TryComplete().ShouldBeFalse();
    }

    [Fact]
    public async Task ParseShippingAcknowledgementNotification_WhenParsingStarted_ChannelShouldBeCompleted()
    {
        var channel = Channel.CreateUnbounded<Box>();

        await _shippingAcknowledgementParser.ParseShippingAcknowledgementNotification(channel.Writer,
            "./Data/multiple-boxes.txt");

        channel.Reader.Count.ShouldBe(3);

        var expectedBoxes = new List<Box>();

        await foreach (var box in channel.Reader.ReadAllAsync())
        {
            expectedBoxes.Add(box);
        }

        expectedBoxes.ShouldBeEquivalentTo(new List<Box>
        {
            new()
            {
                SupplierIdentifier = "TRSP117",
                Identifier = "6874453I",
                Contents =
                [
                    new Content { PoNumber = "P000001661", Isbn = "9781473663800", Quantity = 12 },
                    new Content { PoNumber = "P000001661", Isbn = "9781473667273", Quantity = 2 },
                    new Content { PoNumber = "P000001661", Isbn = "9781473665798", Quantity = 1 }
                ]
            },
            new()
            {
                SupplierIdentifier = "TRSP117",
                Identifier = "6874454I",
                Contents =
                [
                    new Content { PoNumber = "G000009810", Isbn = "9781473669987", Quantity = 1 },
                    new Content { PoNumber = "G000009810", Isbn = "9781473661905", Quantity = 1 }
                ]
            },
            new()
            {
                SupplierIdentifier = "TRSP117",
                Identifier = "6874473I",
                Contents =
                [
                    new Content { PoNumber = "G000009809", Isbn = "9781473676978", Quantity = 1 },
                ]
            }
        });
    }
}