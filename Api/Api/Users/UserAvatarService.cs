using System.Net.Http.Headers;
using Core.Types;

namespace Api.Api.Users;

public interface IUserAvatarService
{
    Result<string> GetUrl();
}

public sealed class UserAvatarService : IUserAvatarService
{
    private readonly HttpClient _httpClient;

    public UserAvatarService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public Result<string> GetUrl()
    {
        const int imageSize = 200;

        var httpMessage = new HttpRequestMessage(HttpMethod.Get, $"https://picsum.photos/{imageSize}");
        httpMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:108.0) Gecko/20100101 Firefox/108.0");

        var response = _httpClient.Send(httpMessage);

        var url = response.RequestMessage?.RequestUri?.ToString();
        if (url == null)
            return Result<string>.Failure("Unable to retrieve image for avatar.");

        return url;
    }
}