using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Api.Startup;

public sealed class SwaggerResponseTypeAttribute : ProducesResponseTypeAttribute
{
    private const int OK = (int)HttpStatusCode.OK;

    public SwaggerResponseTypeAttribute(int statusCode = OK) : base(statusCode)
    {
    }

    public SwaggerResponseTypeAttribute(Type type, int statusCode = OK) : base(type, statusCode)
    {
    }

    public SwaggerResponseTypeAttribute(Type type, string contentType, int statusCode = OK, params string[] additionalContentTypes) : base(type, statusCode, contentType, additionalContentTypes)
    {
    }
}