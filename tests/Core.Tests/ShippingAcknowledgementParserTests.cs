using System.Threading.Channels;
using Core.Models;
using Core.ShippingAcknowledgements;
using NSubstitute;
using Shouldly;

namespace Core.Tests;

public class ShippingAcknowledgementParserTests
{
    private readonly ShippingAcknowledgementParser _shippingAcknowledgementParser;
    private readonly IAcknowledgementNotificationReader _acknowledgementNotificationReader;

    public ShippingAcknowledgementParserTests()
    {
        _acknowledgementNotificationReader = Substitute.For<IAcknowledgementNotificationReader>();

        _shippingAcknowledgementParser = new ShippingAcknowledgementParser(_acknowledgementNotificationReader);
    }

    [Fact]
    public async Task ParseShippingAcknowledgementNotificationAsync_WhenParsingFinished_ChannelShouldBeCompleted()
    {
        var channel = Channel.CreateUnbounded<Box>();

        await _shippingAcknowledgementParser.ParseShippingAcknowledgementNotificationAsync(channel.Writer, string.Empty);

        channel.Writer.TryComplete().ShouldBeFalse();
    }

    [Fact]
    public async Task ParseShippingAcknowledgementNotificationAsync_WhenMultipleShippingAcknowledgements_ShouldParseAndWriteAllAcknowledgements()
    {
        const string notificationLocation = "NotificationPath";

        var channel = Channel.CreateUnbounded<Box>();

        var notificationLines = new List<string>
        {
            "HDR  TRSP117                                                                                     6874453I                           ",
            "",
            "LINE P000001661                           9781473663800                     12     ",
            "",
            "LINE P000001661                           9781473667273                     2      ",
            "",
            "LINE P000001661                           9781473665798                     1      ",
            "",
            "HDR  TRSP117                                                                                     6874454I                           ",
            "",
            "LINE G000009810                           9781473669987                     1      ",
            "",
            "LINE G000009810                           9781473661905                     1      ",
            "",
            "HDR  TRSP117                                                                                     6874473I                           ",
            "",
            "LINE G000009809                           9781473676978                     1      "
        }.ToAsyncEnumerable();

        _acknowledgementNotificationReader.ReadNotificationLinesAsync(notificationLocation).Returns(notificationLines);

        await _shippingAcknowledgementParser.ParseShippingAcknowledgementNotificationAsync(channel.Writer, notificationLocation);

        var expectedBoxes = await channel.Reader.ReadAllAsync().ToListAsync();
        expectedBoxes.Count.ShouldBe(3);

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