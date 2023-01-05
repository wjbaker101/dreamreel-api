using Api.Exceptions;
using Api.Types;
using Core.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Api.Auth.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RequireUserAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var header = context.HttpContext.Request.Headers["Authorization"].ToString();

        const string bearer = "Bearer ";

        if (!header.StartsWith(bearer))
            throw new ApiForbiddenException();

        var loginToken = header.Substring(bearer.Length);

        var loginTokenService = context.HttpContext.RequestServices.GetRequiredService<ILoginTokenService>();

        var isValidResult = loginTokenService.Validate(loginToken);
        if (isValidResult.IsFailure)
            throw new ApiForbiddenException();

        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

        var user = userRepository.GetByReference(isValidResult.Value);
        if (user.IsFailure)
            throw new ApiForbiddenException();

        context.HttpContext.Items["user"] = new RequestUser
        {
            Reference = user.Value.Reference,
            Username = user.Value.Username
        };
    }
}