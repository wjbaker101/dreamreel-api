using Api.Types;

namespace Api.Services;

public interface IRequestService
{
    RequestUser User(HttpRequest request);
}

public sealed class RequestService : IRequestService
{
    public RequestUser User(HttpRequest request)
    {
        if (request.HttpContext.Items["user"] is not RequestUser user)
            throw new ArgumentException("No user on the request.");

        return user;
    }
}