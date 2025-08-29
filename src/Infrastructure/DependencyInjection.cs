using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<ShippingAcknowledgementContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("ShippingAcknowledgementDatabase")));
}