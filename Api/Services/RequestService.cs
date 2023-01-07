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
        if (request.HttpContext.Items["user"] is not UserModel user)
            throw new ArgumentException("No user on the request.");

        return user;
    }
}