using System.Text.Json;
using System.Text.Json.Serialization;
using DarkContinent.Data;
using DarkContinent.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var namingPolicy = JsonNamingPolicy.SnakeCaseLower;
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = namingPolicy;
    });

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = namingPolicy;
});

builder.Services.AddOpenApi();

builder.Services.AddDbContext<DarkContinentContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<FoodService>();
builder.Services.AddScoped<ActivityService>();


var app = builder.Build();

app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error != null)
        {
            var errorResponse = new { message = exceptionHandlerPathFeature.Error.Message };
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    });
});



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();

