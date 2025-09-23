using CatalogServiceAPI_Electric_Store.Models;
using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.EntityFrameworkCore;

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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CategoryRepository>(); 
builder.Services.AddScoped<AttributeRepository>();
builder.Services.AddScoped<CategoryAttributeRepository>();
builder.Services.AddScoped<BrandRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ProductAttributeRepository>();
builder.Services.AddScoped<ProductVariantRepository>();
builder.Services.AddScoped<VariantAttributeRepository>();


var app = builder.Build();

//app.UseMiddleware<ApiKeyMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    
app.UseHttpsRedirection();

app.UseCors("AllowAll");


app.UseAuthorization();
app.MapGet("/", () => "Hello World! This is my project API");

app.MapControllers();

app.Run();
