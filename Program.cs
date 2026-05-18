using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()      
              .AllowAnyMethod()     
              .AllowAnyHeader();     
    });
});



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CatalogAPIContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))));
builder.Services.AddScoped<CategoryRepository>(); 
builder.Services.AddScoped<AttributeRepository>();
builder.Services.AddScoped<CategoryAttributeRepository>();
builder.Services.AddScoped<BrandRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ProductAttributeRepository>();
builder.Services.AddScoped<ProductVariantRepository>();
builder.Services.AddScoped<VariantAttributeRepository>();
builder.Services.AddScoped<ProductImageRepository>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<AttributeValueRepository>();
builder.Services.AddScoped<CategoryBrandRepository>();


var app = builder.Build();
//app.Urls.Add("http://172.27.48.1:5000");
//app.Urls.Add("http://localhost:5000");

//app.UseMiddleware<ApiKeyMiddleware>();
//var hc = builder.Services.AddHealthChecks();


//hc.AddCheck("API Health Check", () =>
//{
//    // Perform a simple check to determine if the API is healthy
//    bool isHealthy = true; // Replace with actual health check logic
//    if (isHealthy)
//    {
//        return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("The API is healthy.");
//    }
//    else
//    {
//        return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Unhealthy("The API is unhealthy.");
//    }
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
if (!Directory.Exists(uploadsFolder))
{
    Directory.CreateDirectory(uploadsFolder);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsFolder),
    RequestPath = "/Uploads"
});

app.UseHttpsRedirection();

app.UseCors("AllowAll");


app.UseAuthorization();
app.MapGet("/", () => "Hello World! This is my project API");

app.MapControllers();

app.Run();
