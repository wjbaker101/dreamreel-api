using Core.Models;

namespace Api.Api.Users.Types;

public sealed class CreateUserRequest
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}

public sealed class CreateUserResponse
{
    public UserModel User { get; init; } = null!;
}