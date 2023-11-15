using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace AuctionAPI.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Sunucu tarafından öngörülen bir hata olduğunda 500 yerine daha uygun bir durum kodu kullanın.
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            // Exception tipine göre uygun HTTP status kodunu ayarlayın.
            if (exception is UnauthorizedAccessException)
                code = HttpStatusCode.Unauthorized;
            // Diğer özel durumlarınız burada kontrol edilebilir.

            // Hata mesajını JSON olarak dönüştür.
            var result = JsonConvert.SerializeObject(new { error = exception.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
