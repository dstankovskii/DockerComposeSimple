using System.Text;
using Contracts.Currency;
using Microsoft.IdentityModel.Tokens;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// WARNING: SecretKey is stored here for demonstration purposes only.
// In a production environment, use an environment variable or a secure secrets management system instead.
var secretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is not configured");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

var grpcConfig = builder.Configuration.GetSection("GrpcServices");
var currencyServiceUrl = grpcConfig["CurrencyService"] ?? throw new InvalidOperationException("CurrencyService URL is not set");

builder.Services.AddGrpcClient<CurrencyGrpcService.CurrencyGrpcServiceClient>(o =>
{
    o.Address = new Uri(currencyServiceUrl);
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
