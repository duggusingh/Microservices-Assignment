using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Services;
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

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBlobService, BlobService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowDurgeshFrontend")
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>(); 
app.MapControllers();

app.Run();
