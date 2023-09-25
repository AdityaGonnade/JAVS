﻿using JAVS_VENDOR.INVENTORY;
using JAVS_VENDOR.VENDORPROFILE_SQL_DATA;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddDbContext<VendorProfileDBcontext>(
//o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
//    );

builder.Services.Configure<DataAccess>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<DataAccess>();

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

