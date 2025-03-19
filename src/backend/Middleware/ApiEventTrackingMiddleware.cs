using AS_2025.Channels;
using System.Threading.Channels;

namespace AS_2025.Middleware;

public class ApiEventTrackingMiddleware : IMiddleware
{
    private readonly Channel<ApiEvent> _channel;

    public ApiEventTrackingMiddleware(Channel<ApiEvent> channel)
    {
        _channel = channel;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var path = context.Request.Path.ToString();
        var method = context.Request.Method;

        await next(context);

        await _channel.Writer.WriteAsync(new ApiEvent
        {
            Path = path,
            Method = method,
            Timestamp = DateTimeOffset.UtcNow
        });
    }
}