using System.Text;
using JWT_Token_Example.Carts.CartDataAccess;
using JWT_Token_Example.Context;
using JWT_Token_Example.Controllers;
using JWT_Token_Example.ImageServices;
using JWT_Token_Example.Inventory.InventoryDataAccess;
using JWT_Token_Example.Inventory.InventorySearchAccess;
using JWT_Token_Example.Order.OrderDataAccess;
using JWT_Token_Example.Reviews.ReviewModels;
using JWT_Token_Example.UtilityService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("JavsConnectionString"));
});

// builder.Services.Configure<CartController>(builder.Configuration.GetSection("MongoDB"));
// builder.Services.AddSingleton<CartController>();

// builder.Services.Configure<CartController>(builder.Configuration.GetSection("MongoDB"));
// builder.Services.AddSingleton<CartController>();

builder.Services.Configure<DataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<DataAccess>();

builder.Services.Configure<OrderDataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<OrderDataAccess>();

builder.Services.Configure<ReviewDataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<ReviewDataAccess>();

builder.Services.Configure<SearchAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<SearchAccess>();

builder.Services.Configure<CartDataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<CartDataAccess>();

builder.Services.AddScoped<IAWSConfiguration, AWSConfiguration>();
builder.Services.AddSingleton<S3Service>();


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