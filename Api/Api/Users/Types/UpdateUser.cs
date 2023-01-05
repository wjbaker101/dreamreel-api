namespace Api.Api.Users.Types;

public sealed class UpdateUserRequest
{
    public string Username { get; init; } = null!;
}

public sealed class UpdateUserResponse
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Username { get; init; } = null!;
}