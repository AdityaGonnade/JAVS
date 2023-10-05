using System.Text;
using JAVS_VENDOR.CART.CartDataAccess;
using JAVS_VENDOR.INVENTORY;
using JAVS_VENDOR.ORDERS.OrderDataAccess;
using JAVS_VENDOR.Repository;
using JAVS_VENDOR.REVIEW.REVIEWDataAccess;
using JWT_Token_Example.Context;
using JWT_Token_Example.Controllers;
using JWT_Token_Example.ImageServices;
using JWT_Token_Example.Inventory.InventorySearchAccess;
using JWT_Token_Example.Models.MongoDBSettings;
using JWT_Token_Example.Repository;
using JWT_Token_Example.Reviews.ReviewModels;
using JWT_Token_Example.UtilityService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddTransient<INotification, Notification>();

// Add services to the container.
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TPDNmk5ADponVEiQc5tmRkHhOiAFmkAr")),
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//Add Postgres Db
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("JavsConnectionString"));
});

//Add Mongo Db Settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbConnection"));

var logger = new LoggerConfiguration().WriteTo.Console().WriteTo.File("Logs/App_Log.txt",rollingInterval:RollingInterval.Minute).MinimumLevel.Information()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


//Search Access Repository dependency Injection
builder.Services.AddScoped<ISearchAccessRepo, SearchAccessRepo>();
//Search Access Service dependency Injection
builder.Services.AddScoped<ISearchAccess, SearchAccess>();
builder.Services.AddTransient<SearchAccess>();

//builder.Services.Configure<CartDataAccess>(builder.Configuration.GetSection("MongoDB"));
//builder.Services.AddSingleton<CartDataAccess>();

builder.Services.AddScoped<ICartRepo, CartDataRepo>();
builder.Services.AddTransient<CartServices>();

builder.Services.AddScoped<IAWSConfiguration, AWSConfiguration>();
builder.Services.AddTransient<S3Service>();


builder.Services.AddScoped<IReviewRepo, ReviewDataRepo>();
builder.Services.AddTransient<ReviewServices>();

builder.Services.AddScoped<IInventoryRepo, InventoryDataRepo>();
builder.Services.AddTransient<InventoryServices>();

builder.Services.AddScoped<IOrderRepo, OrderDataRepo>();
builder.Services.AddTransient<OrderServices>();


var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();