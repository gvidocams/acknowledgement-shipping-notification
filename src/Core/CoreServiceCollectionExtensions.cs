using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class CoreServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services) =>
        services
            .AddScoped<IShippingAcknowledgementScanner, ShippingAcknowledgementScanner>()
            .AddScoped<IShippingAcknowledgementProcessor, ShippingAcknowledgementProcessor>(serviceProvider =>
            {
                var shippingAcknowledgementParser = serviceProvider.GetRequiredService<IShippingAcknowledgementParser>();
                var shippingAcknowledgementBoxProcessor = serviceProvider.GetRequiredService<IShippingAcknowledgementBoxProcessor>();

                var options = serviceProvider.GetRequiredService<IAcknowledgementProcessingOptions>();

                return new ShippingAcknowledgementProcessor(shippingAcknowledgementParser, shippingAcknowledgementBoxProcessor, options.ChannelCapacitySize);
            })
            .AddScoped<IShippingAcknowledgementParser, ShippingAcknowledgementParser>()
            .AddScoped<IShippingAcknowledgementBoxProcessor, ShippingAcknowledgementBoxProcessor>(serviceProvider =>
            {
                var shippingAcknowledgementRepository =
                    serviceProvider.GetRequiredService<IShippingAcknowledgementRepository>();
                var options = serviceProvider.GetRequiredService<IAcknowledgementProcessingOptions>();

                return new ShippingAcknowledgementBoxProcessor(shippingAcknowledgementRepository, options.BatchSize);
            });
}