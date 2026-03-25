using Infrastructure.Data; 



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
builder.Services.AddScoped<CreateOrderUseCase>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

builder.Logging.ClearProviders(); 

builder.Services.AddCors(o => o.AddPolicy("bad", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())); 

var app = builder.Build(); 

app.UseSwagger(); 
app.UseSwaggerUI(); 

//BadDb.ConnectionString = app.Configuration["ConnectionStrings:Sql"] 
    //?? "Server=localhost;Database=master;User Id=sa;Password=SuperSecret123!;TrustServerCertificate=True"; 

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

app.MapGet("/info", (IConfiguration cfg) => new 
{
    //sql = BadDb.ConnectionString,
    env = Environment.GetEnvironmentVariables(), 
    version = "v0.0.1-unsecure" 
});

app.Run(); 
