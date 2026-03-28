using Infrastructure.Data;
using Infrastructure.Logging;
using Application.Interfaces;
using Application.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddSingleton<ILog, LoggerConsole>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

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

app.UseCors("bad"); 

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
});

app.MapGet("/orders/last", (IOrderRepository repo) => repo.GetLast());


app.MapGet("/info", (IHostEnvironment env) => new
{
    environment = env.EnvironmentName,
    version = "v1"
});

await app.RunAsync();
