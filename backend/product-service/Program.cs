using Serilog;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using ProductService.Data;
using ProductService.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 


builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Azure Blob
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration["AzureBlob:ConnectionString"]));
builder.Services.AddScoped<IBlobService, BlobService>();

var app = builder.Build();

app.UseSwagger(); 
app.UseSwaggerUI();

app.MapControllers();
app.Run();
