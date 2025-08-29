using Core.Models;
using Infrastructure.Persistence.Converters;
using Infrastructure.Persistence.Entities;
using Shouldly;

namespace Infrastructure.Tests;

public class ContentEntityTests
{
    [Fact]
    public void ToContentEntity_WhenExpectedContentModelPassed_ShouldConvertToEquivalentContentEntity()
    {
        var content = new Content { PoNumber = "TestPoNumber", Isbn = "TestIsbn", Quantity = 13 };

        var result = content.ToContentEntity();

        result.ShouldBeEquivalentTo(new ContentEntity { PoNumber = "TestPoNumber", Isbn = "TestIsbn", Quantity = 13 });
    }

    [Fact]
    public void ToContentEntity_WhenContentWithNullValuesPassed_ShouldConvertToEquivalentContentEntity()
    {
        var content = new Content();

        var result = content.ToContentEntity();

        result.ShouldBeEquivalentTo(new ContentEntity { PoNumber = null!, Isbn = null!, Quantity = 0 });
    }
}