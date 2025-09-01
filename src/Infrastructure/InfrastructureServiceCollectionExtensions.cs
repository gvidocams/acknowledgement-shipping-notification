using Core;
using Infrastructure.Configuration;
using Infrastructure.Persistence;
using Infrastructure.ShippingAcknowledgementNotifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddScoped<IAcknowledgementProcessingOptions, AcknowledgementProcessingOptionsAdapter>()
            .AddScoped<IShippingAcknowledgementRepository, ShippingAcknowledgementRepository>()
            .AddScoped<IShippingAcknowledgementProvider, ShippingAcknowledgementProvider>()
            .AddScoped<IAcknowledgementNotificationReader, AcknowledgementNotificationReader>()
            .AddDbContext<ShippingAcknowledgementContext>(options => 
                options.UseSqlite(configuration.GetConnectionString("ShippingAcknowledgementDatabase")));
}