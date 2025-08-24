using Core;
using Infrastructure;
using ShippingAcknowledgementWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IShippingAcknowledgementScanner, ShippingAcknowledgementScanner>();
builder.Services.AddSingleton<IShippingAcknowledgementProvider, ShippingAcknowledgementProvider>();
builder.Services.AddSingleton<IShippingAcknowledgementProcessor>();

builder.Services.AddOptionsWithValidateOnStart<AcknowledgementScanningOptions>()
    .Bind(builder.Configuration.GetSection(AcknowledgementScanningOptions.SectionName))
    .ValidateDataAnnotations();

builder.Services.AddOptionsWithValidateOnStart<AcknowledgementProviderOptions>()
    .Bind(builder.Configuration.GetSection(AcknowledgementProviderOptions.SectionName))
    .ValidateDataAnnotations();

builder.Services.AddHostedService<ShippingAcknowledgementWorker.ShippingAcknowledgementWorker>();

var host = builder.Build();

await host.RunAsync();