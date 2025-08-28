using Core.Converters;
using Core.Models;
using Shouldly;

namespace Core.Tests;

public class BoxConverterTests
{
    [Fact]
    public void ToBox_WhenExpectedBoxInputIsGiven_ShouldReturnBox()
    {
        const string boxInput = "HDR  TRSP117                                           6874454I                           ";
        
        var result = BoxConverter.ToBox(boxInput);
        
        result.ShouldBeEquivalentTo(new Box
        {
            SupplierIdentifier = "TRSP117",
            Identifier = "6874454I",
            Contents = []
        });
    }
}