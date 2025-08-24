using ShippingAcknowledgementWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IShippingAcknowledgementService>();

builder.Services.AddOptionsWithValidateOnStart<FileScanningOptions>()
    .Bind(builder.Configuration.GetSection(FileScanningOptions.SectionName))
    .ValidateDataAnnotations();

builder.Services.AddHostedService<ShippingAcknowledgementWorker.ShippingAcknowledgementWorker>();

var host = builder.Build();

await host.RunAsync();