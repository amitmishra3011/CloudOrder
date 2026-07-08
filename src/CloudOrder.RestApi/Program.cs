using CloudOrder.Business.Interfaces;
using CloudOrder.Business.Services;
using CloudOrder.EFInfrastructure;
using CloudOrder.EFInfrastructure.Persistence;
using CloudOrder.RestApi.Controllers;
using CloudOrder.RestApi.ExceptionHandling;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddCloudOrderEFInfrastructure(builder.Configuration);
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(OrdersController).Assembly);

var app = builder.Build();

app.UseExceptionHandler();
// Run migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<CloudOrderDbContext>();

    await DataSeeder.SeedAsync(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
