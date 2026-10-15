using DarkContinent.DTOs.Responses;

namespace DarkContinent.Middlewares;


public class ApiKeyMiddleware
{
    
    private readonly RequestDelegate _next;
    
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        
        if(context.Request.Path.StartsWithSegments("/scalar") || context.Request.Path.StartsWithSegments("/openapi"))
        {
            await _next(context);
            return;
        }

        var apiKey = _configuration.GetValue<string>("ApiKey");
        if(apiKey is null)
        {
            throw new InvalidOperationException("API key is not configured");
        }

        string? requestApiKey = context.Request.Headers["x-client-request"].ToString();

        if(requestApiKey is null || requestApiKey != apiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new ApiResponse<string>
            {
                Message = "Unauthorized",
                StatusCode = StatusCodes.Status401Unauthorized,
                Success = false,
            });
            return;
        }

        await _next(context);

    }

}