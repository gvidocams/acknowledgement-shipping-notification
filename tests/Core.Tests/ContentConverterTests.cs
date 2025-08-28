using Core.Converters;
using Core.Models;
using Shouldly;

namespace Core.Tests;

public class ContentConverterTests
{
    [Fact]
    public void ToContent_WhenExpectedContentInputIsGiven_ShouldReturnContent()
    {
        const string contentInput = "LINE P000001661         9781465121550         12     ";
        
        var result = ContentConverter.ToContent(contentInput);
        
        result.ShouldBeEquivalentTo(new Content
        {
            PoNumber = "P000001661",
            Isbn = "9781465121550",
            Quantity = 12
        });
    }
}