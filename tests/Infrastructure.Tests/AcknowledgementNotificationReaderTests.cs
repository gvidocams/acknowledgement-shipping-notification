using Infrastructure.ShippingAcknowledgementNotifications;
using Shouldly;

namespace Infrastructure.Tests;

public class AcknowledgementNotificationReaderTests
{
    private readonly AcknowledgementNotificationReader _acknowledgementNotificationReader = new();

    [Fact]
    public async Task ReadNotificationLinesAsync_WhenMultipleLines_ShouldReturnAllLines()
    {
        var result = new List<string>();
        await foreach (var line in _acknowledgementNotificationReader.ReadNotificationLinesAsync("./Data/multiple-lines.txt"))
        {
            result.Add(line);
        }

        result.Count.ShouldBe(5);
        result.ShouldBeEquivalentTo(new List<string>
        {
            "HDR  TRSP117                                                                                     6874453I                           ",
            "",
            "LINE P000001661                           9781473663800                     12     ",
            "",
            "LINE P000001661                           9781473667273                     2      "
        });
    }

    [Fact]
    public async Task ReadNotificationLinesAsync_WhenEmptyFile_ShouldNotReturnAnyLines()
    {
        var result = new List<string>();
        await foreach (var line in _acknowledgementNotificationReader.ReadNotificationLinesAsync("./Data/no-lines.txt"))
        {
            result.Add(line);
        }

        result.ShouldBeEmpty();
    }
}