namespace Radancy.APi.Middlewares
{
    public class BasicApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BasicApiKeyMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public BasicApiKeyMiddleware(RequestDelegate next, ILogger<BasicApiKeyMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Allow Scalar without key (optional)
            if (!context.Request.Path.StartsWithSegments("/scalar") && !context.Request.Path.StartsWithSegments("/openapi"))
            {
                if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedKey))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    _logger.LogWarning("API Key missing in request");
                    await context.Response.WriteAsync("API Key missing");
                    return;
                }

                var apiKey = _configuration["ApiSecurity:ApiKey"];

                if (string.IsNullOrWhiteSpace(apiKey) || !apiKey.Equals(extractedKey))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    _logger.LogWarning("Invalid API Key provided");
                    await context.Response.WriteAsync("Invalid API Key");
                    return;
                }
            }
            await _next(context);
        }
    }
}