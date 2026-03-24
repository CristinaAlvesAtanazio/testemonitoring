using testemonitoring.Infrastructure.Data;
using Scalar.AspNetCore;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.MapOpenApi();
app.MapScalarApiReference();
app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });

app.MapDefaultEndpoints();
app.MapEndpoints(typeof(Program).Assembly);
app.UseFileServer();
app.MapPrometheusScrapingEndpoint();

var customMeter = new Meter("testemonitoring.custom");
var customCounter = customMeter.CreateCounter<long>(
    "testemonitoring_custom_action_total",
    description: "Quantidade de vezes que a ação customizada foi executada");

app.MapPost("/api/metric-test", () =>
{
    customCounter.Add(
        1,
        new KeyValuePair<string, object?>("endpoint", "metric-test"),
        new KeyValuePair<string, object?>("status", "success"));

    return Results.Ok(new { message = "Métrica registrada com sucesso" });
});

app.Run();