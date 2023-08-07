using Newtonsoft.Json;
using System.Net;

namespace UniqloFromJapan.Middlewares {
    public class GlobalExceptionHandlingMiddleware : IMiddleware {

        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            try {
                await next(context);
            }
            catch (Exception e) {
                _logger.LogError(e, $"Error: {e.Message}");

                var globalExceptionModel = new GlobalExceptionModel() {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Message = e.Message,
                };

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var serializedGlobalException = JsonConvert.SerializeObject(globalExceptionModel);
                await context.Response.WriteAsync(serializedGlobalException);
            }
        }
    }

    public class GlobalExceptionModel {
        public int Status { get; set; }
        public string? Message { get; set; }
    }
}