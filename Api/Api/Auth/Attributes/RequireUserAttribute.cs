using Api.Exceptions;
using Core.Data.Repositories;
using Core.Models.Mappers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Api.Auth.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RequireUserAttribute : Attribute, IAuthorizationFilter
{
    public const string HTTP_CONTEXT_ITEM_NAME = "user";

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

        context.HttpContext.Items[HTTP_CONTEXT_ITEM_NAME] = UserMapper.Map(user.Value);
    }
}