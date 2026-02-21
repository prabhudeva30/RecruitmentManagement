using Radancy.APi.Extensions;
using Radancy.APi.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Builds up the dependency injection container by registering services defined in the application layer and data access layer
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>(); // Custom middleware for exception handling
app.UseMiddleware<BasicApiKeyMiddleware>(); // basic API key authentication middleware

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Scalar UI
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();