namespace Api.Api.Auth.Types;

public sealed class LogInRequest
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}

public sealed class LogInResponse
{
    public string LoginToken { get; init; } = null!;
}