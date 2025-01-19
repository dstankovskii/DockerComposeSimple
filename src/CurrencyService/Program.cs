using CurrencyService.GrpcServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.WebHost.ConfigureKestrel(options =>
{
    var grpcUrl = builder.Configuration["Kestrel:Endpoints:Grpc:Url"];
    var certPath = builder.Configuration["Kestrel:Certificates:Default:Path"];
    var certPassword = builder.Configuration["Kestrel:Certificates:Default:Password"];

    // Ensure that the gRPC URL is explicitly set in configuration.
    // This avoids runtime errors when deploying to environments with different setups.
    if (string.IsNullOrEmpty(grpcUrl))
    {
        throw new InvalidOperationException("gRPC URL configuration is missing. Please set it in appsettings.json or via environment variables (Kestrel__Endpoints__Grpc__Url).");
    }

    // options.ListenAnyIP(new Uri(grpcUrl).Port);
    options.ListenAnyIP(new Uri(grpcUrl).Port, o =>
    {
        o.Protocols = HttpProtocols.Http2;
        o.UseHttps(certPath!, certPassword);
    });
});

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<CurrencyServiceImpl>();
// app.MapGrpcService<CurrencyServiceImpl>().EnableGrpcWeb();
app.MapGet("/", () => "Use gRPC client to interact with the service");

app.Run();
