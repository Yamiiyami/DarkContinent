
using System.Diagnostics;

namespace DarkContinent.Middlewares;

public class RequestLoggingMiddleware
{

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private long _requestCount;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        var sw = Stopwatch.StartNew();
        Interlocked.Increment(ref _requestCount);
        await _next(context);

        sw.Stop();
        _logger.LogInformation("{Method} {Path} -> {Status} ({Ms}ms) RequestCount: {RequestCount}",
           context.Request.Method, 
           context.Request.Path,
           context.Response.StatusCode, 
           sw.ElapsedMilliseconds,
            _requestCount);
    }


}
