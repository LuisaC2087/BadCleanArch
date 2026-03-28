using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Data;
using Infrastructure.Logging;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddSingleton<ILog, LoggerConsole>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

builder.Logging.ClearProviders();
var allowedOrigins = builder.Configuration["Cors:Origins"]?.Split(',');

builder.Services.AddCors(o => o.AddPolicy("cors", p =>
    p.WithOrigins(allowedOrigins ?? Array.Empty<string>())
     .AllowAnyHeader()
     .AllowAnyMethod()
));

builder.Services.AddScoped<IOrderRepository>(sp =>
{
    var config = builder.Configuration;
    var conn = config.GetConnectionString("Sql");

    return new SqlOrderRepository(conn);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("cors");

app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        throw;
    }
}
);

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.MapGet("/health", () => "ok");

app.MapPost("/orders", (CreateOrderUseCase uc, CreateOrderRequest req) =>
{
    var order = uc.Execute(req.Customer, req.Product, req.Qty, req.Price);
    return Results.Ok(order);
}).Produces<object>(StatusCodes.Status200OK);

app.MapGet("/orders/last", (IOrderRepository repo) => repo.GetLast())
   .Produces<object>(StatusCodes.Status200OK);

app.MapGet("/info", (IHostEnvironment env) => new
{
    environment = env.EnvironmentName,
    version = "v1"
}).Produces<object>(StatusCodes.Status200OK);

await app.RunAsync();