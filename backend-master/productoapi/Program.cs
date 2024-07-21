using Microsoft.EntityFrameworkCore;
using productoapi;
using productoapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer(connectionString));


builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
builder.Services.AddScoped<ProductoService>();

var app = builder.Build();





    app.UseSwagger();
    app.UseSwaggerUI();





app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
