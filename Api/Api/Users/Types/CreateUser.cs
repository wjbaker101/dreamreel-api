namespace Api.Api.Users.Types;

public sealed class CreateUserRequest
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}

public sealed class CreateUserResponse
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Username { get; init; } = null!;
}