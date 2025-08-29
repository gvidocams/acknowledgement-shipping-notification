namespace Infrastructure.Persistence.Entities;

public class BoxEntity
{
    public int Id { get; set; }
    public string SupplierIdentifier { get; set; }
    public string Identifier { get; set; }

    public List<ContentEntity> Contents { get; set; } 
}