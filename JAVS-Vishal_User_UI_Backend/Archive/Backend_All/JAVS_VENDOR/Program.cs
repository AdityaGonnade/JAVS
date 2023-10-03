using JAVS_VENDOR.INVENTORY;
using JAVS_VENDOR.VENDORPROFILE_SQL_DATA;
using Microsoft.EntityFrameworkCore;
using JAVS_VENDOR.ORDERS.OrderDataAccess;
using JAVS_VENDOR.REVIEW.REVIEWDataAccess;
using JAVS_VENDOR.Inventory.InventorySearchAccess;
using JAVS_VENDOR.ImageServices;
using JAVS_VENDOR.CART.CartDataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod();

    });
});

//builder.Services.AddDbContext<VendorProfileDBcontext>(
//o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
//    );

builder.Services.Configure<DataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<DataAccess>();


builder.Services.Configure<OrderDataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<OrderDataAccess>();


builder.Services.Configure<ReviewDataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<ReviewDataAccess>();


builder.Services.Configure<SearchAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<SearchAccess>();

builder.Services.AddScoped<IAWSConfiguration, AWSConfiguration>();
builder.Services.AddSingleton<S3Services>();

builder.Services.Configure<CartDataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<CartDataAccess>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

