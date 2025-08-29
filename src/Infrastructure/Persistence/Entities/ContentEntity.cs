namespace Infrastructure.Persistence.Entities;

public class ContentEntity
{
    public int Id { get; set; }
    public string PoNumber { get; set; }
    public string Isbn { get; set; }
    public int Quantity { get; set; }
}