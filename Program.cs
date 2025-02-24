using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WhatToEat.Data;
using DotNetEnv;


var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load();

// Build the connection string from environment variables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

connectionString = connectionString
    .Replace("ENV_DB_HOST", Env.GetString("DB_HOST"))
    .Replace("ENV_DB_PORT", Env.GetString("DB_PORT"))
    .Replace("ENV_DB_NAME", Env.GetString("DB_NAME"))
    .Replace("ENV_DB_USER", Env.GetString("DB_USER"))
    .Replace("ENV_DB_PASSWORD", Env.GetString("DB_PASSWORD"));

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WhatToEat API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WhatToEat API v1");
    });
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();