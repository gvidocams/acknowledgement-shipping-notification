using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ShippingAcknowledgementContext(DbContextOptions<ShippingAcknowledgementContext> options) : DbContext(options)
{
    public DbSet<BoxEntity> Boxes { get; set; }
    public DbSet<ContentEntity> Contents { get; set; }
}