using CloudOrder.Business;
using CloudOrder.EFInfrastructure;
using CloudOrder.EFInfrastructure.Persistence;
using CloudOrder.RestApi.Controllers;
using CloudOrder.RestApi.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCloudOrderEFInfrastructure(builder.Configuration);
builder.Services.AddScoped<IOrderService, OrderService>();
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
