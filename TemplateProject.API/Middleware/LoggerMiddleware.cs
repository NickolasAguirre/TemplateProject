using System.Text;

namespace TemplateProject.API.Middleware
{
    public class LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            try
            {
                using (_logger.BeginScope("Request {Method} {Path}", context.Request.Method, context.Request.Path))
                {
                    var requestContext = new RequestLog
                    {
                        Method = context.Request.Method,
                        Path = context.Request.Path,
                        Body = await ReadRequestBodyAsync(context),
                        UserId = context.User?.Identity?.Name ?? "Unknown User",
                        QueryParameters = string.Join(", ", context.Request.Query.Select(q => $"{q.Key}={q.Value}")),
                        RouteParameters = string.Join(", ", context.Request.RouteValues.Select(r => $"{r.Key}={r.Value}")),
                        StatusCode = context.Response.StatusCode,
                    };

                    SendLogging(requestContext, null);
                    await _next(context);
                }
            }
            catch (Exception exception)
            {
                var requestLog = new RequestLog
                {
                    Method = context.Request.Method,
                    Path = context.Request.Path,
                    Body = await ReadRequestBodyAsync(context),
                    UserId = context.User?.Identity?.Name ?? "Unknown User",
                    QueryParameters = string.Join(", ", context.Request.Query.Select(q => $"{q.Key}={q.Value}")),
                    RouteParameters = string.Join(", ", context.Request.RouteValues.Select(r => $"{r.Key}={r.Value}")),
                    StatusCode = 500
                };
                SendLogging(requestLog, exception);
                throw;
            }

        }
        private void SendLogging(RequestLog requestLog, Exception? exception)
        {
            var (logLevel, eventId) = GetLogLevel(requestLog.StatusCode);
            var eventIdLogger = new EventId(requestLog.StatusCode, eventId);
            var message = GetLogMessage(requestLog);
            _logger.Log(logLevel, eventIdLogger, exception, message);
        }

        private string GetLogMessage(RequestLog requestLog)
        {
            return $"Method: {requestLog.Method}, " +
                $"Path: {requestLog.Path}, " +
                $"StatusCode: {requestLog.StatusCode}, " +
                $"UserId: {requestLog.UserId}, " +
                $"QueryParameters: {requestLog.QueryParameters}, " +
                $"RouteParameters: {requestLog.RouteParameters}, " +
                $"Body: {requestLog.Body}";
        }
        private (LogLevel, string) GetLogLevel(int statusCode)
        {
            if (statusCode >= 500)
            {
                return (LogLevel.Error, "ServerError");
            }
            else if (statusCode >= 400)
            {
                return (LogLevel.Warning, "ClientError");
            }
            else
            {
                return (LogLevel.Information, "RequestHandled");
            }
        }
        private async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0; // Reset the stream position for further processing
            return body;
        }

        private class RequestLog
        {
            public string Method { get; set; } = string.Empty;
            public string Path { get; set; } = string.Empty;
            public string Body { get; set; } = string.Empty;
            public int StatusCode { get; set; }
            public string QueryParameters { get; set; } = string.Empty; 
            public string RouteParameters { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
        }
    }

}
