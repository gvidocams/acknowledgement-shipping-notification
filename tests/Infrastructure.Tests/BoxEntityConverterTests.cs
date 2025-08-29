using Core.Models;
using Infrastructure.Persistence.Converters;
using Infrastructure.Persistence.Entities;
using Shouldly;

namespace Infrastructure.Tests;

public class BoxEntityConverterTests
{
    [Fact]
    public void ToBoxEntity_WhenNoContentsArePassed_ShouldReturnEquivalentBoxEntityWithNoContents()
    {
        const string supplierIdentifier = "TestSupplierId";
        const string identifier = "TestId";

        var box = new Box { SupplierIdentifier = supplierIdentifier, Identifier = identifier, Contents = [] };

        var result = box.ToBoxEntity();

        result.ShouldBeEquivalentTo(
            new BoxEntity { SupplierIdentifier = supplierIdentifier, Identifier = identifier, Contents = [] });
    }

    [Fact]
    public void ToBoxEntity_WhenContentsArePassed_ShouldReturnEquivalentBoxAndContentsEntity()
    {
        const string supplierIdentifier = "TestSupplierId";
        const string identifier = "TestId";

        var box = new Box 
        {
            SupplierIdentifier = supplierIdentifier, Identifier = identifier, Contents =
            [
                new Content { PoNumber = "TestPoNumber1", Isbn = "TestIsbn1", Quantity = 1 },
                new Content { PoNumber = "TestPoNumber2", Isbn = "TestIsbn2", Quantity = 13 }
            ]
        };

        var result = box.ToBoxEntity();

        result.ShouldBeEquivalentTo(
            new BoxEntity
            {
                SupplierIdentifier = supplierIdentifier, Identifier = identifier, Contents =
                [
                    new ContentEntity { PoNumber = "TestPoNumber1", Isbn = "TestIsbn1", Quantity = 1 },
                    new ContentEntity { PoNumber = "TestPoNumber2", Isbn = "TestIsbn2", Quantity = 13 },
                ]
            });
    }

    [Fact]
    public void ToBoxEntity_WhenBoxWithNullValuesPassed_ShouldReturnEquivalentBoxAndContentsEntity()
    {
        var box = new Box();

        var result = box.ToBoxEntity();

        result.ShouldBeEquivalentTo(new BoxEntity { SupplierIdentifier = null!, Identifier = null!, Contents = [] });
    }
}