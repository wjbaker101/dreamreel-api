using Api.Api.Auth.Attributes;
using Core.Models;

namespace Api.Services;

public interface IRequestService
{
    UserModel User(HttpRequest request);
}

public sealed class RequestService : IRequestService
{
    public UserModel User(HttpRequest request)
    {
        if (request.HttpContext.Items[RequireUserAttribute.HTTP_CONTEXT_ITEM_NAME] is not UserModel user)
            throw new ArgumentException("No user on the request.");

        return user;
    }
}