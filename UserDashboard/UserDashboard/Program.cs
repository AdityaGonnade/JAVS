using UserDashboard.Models;
using UserDashboard.Services;
using UserDashboard.Data;
using UserDashboard.Models;
using UserDashboard.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<InventoryDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<InventoryServices>();
builder.Services.AddSingleton<CartServices>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

