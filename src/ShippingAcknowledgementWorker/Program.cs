using Core;
using Infrastructure;
using Infrastructure.Persistence;
using ShippingAcknowledgementWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptionsWithValidateOnStart<AcknowledgementScanningOptions>()
    .Bind(builder.Configuration.GetSection(AcknowledgementScanningOptions.SectionName))
    .ValidateDataAnnotations();

builder.Services.AddOptionsWithValidateOnStart<AcknowledgementProviderOptions>()
    .Bind(builder.Configuration.GetSection(AcknowledgementProviderOptions.SectionName))
    .ValidateDataAnnotations();

builder.Services.AddOptionsWithValidateOnStart<AcknowledgementProcessingOptions>()
    .Bind(builder.Configuration.GetSection(AcknowledgementProcessingOptions.SectionName))
    .Validate(options => options.BatchSize >= 0, "Batch size must be greater than zero")
    .Validate(options => options.ChannelCapacitySize >= 0, "Channel capacity size must be greater than zero")
    .Validate(options => options.ChannelCapacitySize > options.BatchSize, "Channel capacity size must exceed batch size");

builder.Services.AddInfrastructureServices(builder.Configuration);

//TODO Fix service lifetimes
builder.Services.AddScoped<IShippingAcknowledgementScanner, ShippingAcknowledgementScanner>();
builder.Services.AddScoped<IShippingAcknowledgementProvider, ShippingAcknowledgementProvider>();
builder.Services.AddScoped<IShippingAcknowledgementProcessor, ShippingAcknowledgementProcessor>();
builder.Services.AddScoped<IShippingAcknowledgementParser, ShippingAcknowledgementParser>();
builder.Services.AddScoped<IShippingAcknowledgementRepository, ShippingAcknowledgementRepository>(); //TODO Is this needed if the same is on line 33
builder.Services
    .AddScoped<IShippingAcknowledgementBoxProcessor, ShippingAcknowledgementBoxProcessor>(serviceProvider =>
    {
        var shippingAcknowledgementRepository = serviceProvider.GetRequiredService<IShippingAcknowledgementRepository>();

        var batchSize = builder.Configuration.GetValue<int>("AcknowledgementProcessingOptions:BatchSize");

        return new ShippingAcknowledgementBoxProcessor(shippingAcknowledgementRepository, batchSize);
    });

builder.Services.AddHostedService<ShippingAcknowledgementWorker.ShippingAcknowledgementWorker>();

var host = builder.Build();

await host.RunAsync();