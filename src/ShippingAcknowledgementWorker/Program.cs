using Core;
using Infrastructure;
using Infrastructure.Configuration;
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

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddCoreServices();

builder.Services.AddHostedService<ShippingAcknowledgementWorker.ShippingAcknowledgementWorker>();

var host = builder.Build();

await host.RunAsync();