using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowDurgeshFrontend",
        policy => policy.WithOrigins("http://localhost:4200", "http://localhost:80")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<IOrderService, OrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowDurgeshFrontend")
app.UseAuthorization();
app.MapControllers();

app.Run();
