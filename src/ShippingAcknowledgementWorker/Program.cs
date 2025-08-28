using Core;
using Infrastructure;
using ShippingAcknowledgementWorker;

var builder = Host.CreateApplicationBuilder(args);

//TODO Fix service lifetimes
builder.Services.AddSingleton<IShippingAcknowledgementScanner, ShippingAcknowledgementScanner>();
builder.Services.AddSingleton<IShippingAcknowledgementProvider, ShippingAcknowledgementProvider>();
builder.Services.AddSingleton<IShippingAcknowledgementProcessor, ShippingAcknowledgementProcessor>();
builder.Services.AddSingleton<IShippingAcknowledgementRepository>();
builder.Services
    .AddSingleton<IShippingAcknowledgementBoxProcessor, ShippingAcknowledgementBoxProcessor>(serviceProvider =>
    {
        var shippingAcknowledgementRepository = serviceProvider.GetRequiredService<IShippingAcknowledgementRepository>();

        var batchSize = builder.Configuration.GetValue<int>("BatchSize");

        return new ShippingAcknowledgementBoxProcessor(shippingAcknowledgementRepository, batchSize);
    });

builder.Services.AddOptionsWithValidateOnStart<AcknowledgementScanningOptions>()
    .Bind(builder.Configuration.GetSection(AcknowledgementScanningOptions.SectionName))
    .ValidateDataAnnotations();

builder.Services.AddOptionsWithValidateOnStart<AcknowledgementProviderOptions>()
    .Bind(builder.Configuration.GetSection(AcknowledgementProviderOptions.SectionName))
    .ValidateDataAnnotations();

builder.Services.AddHostedService<ShippingAcknowledgementWorker.ShippingAcknowledgementWorker>();

var host = builder.Build();

await host.RunAsync();