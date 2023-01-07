using Api.Types;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

namespace Api.Filters;

public sealed class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var response = context.HttpContext.Response;

        response.StatusCode = (int)HttpStatusCode.InternalServerError;
        response.ContentType = "application/json";

        var body = JsonSerializer.Serialize(ApiErrorResponse.From("An internal server error occurred, please try again later!"), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        response.ContentLength = body.Length;
        response.WriteAsync(body);
    }
}