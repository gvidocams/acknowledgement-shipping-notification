using Core;
using Infrastructure;
using Infrastructure.Persistence;
using ShippingAcknowledgementWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);

//TODO Fix service lifetimes
builder.Services.AddScoped<IShippingAcknowledgementScanner, ShippingAcknowledgementScanner>();
builder.Services.AddScoped<IShippingAcknowledgementProvider, ShippingAcknowledgementProvider>();
builder.Services.AddScoped<IShippingAcknowledgementProcessor, ShippingAcknowledgementProcessor>();
builder.Services.AddScoped<IShippingAcknowledgementRepository, ShippingAcknowledgementRepository>();
builder.Services
    .AddScoped<IShippingAcknowledgementBoxProcessor, ShippingAcknowledgementBoxProcessor>(serviceProvider =>
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