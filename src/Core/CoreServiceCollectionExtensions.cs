using Core.ShippingAcknowledgements;
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
                var shippingAcknowledgementProvider = serviceProvider.GetRequiredService<IShippingAcknowledgementProvider>();

                var options = serviceProvider.GetRequiredService<IAcknowledgementProcessingOptions>();

                return new ShippingAcknowledgementProcessor(
                    shippingAcknowledgementParser,
                    shippingAcknowledgementBoxProcessor,
                    shippingAcknowledgementProvider,
                    options.ChannelCapacitySize);
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