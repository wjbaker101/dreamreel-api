using Core.Models;

namespace Api.Api.Users.Types;

public sealed class UpdateUserRequest
{
    public string Username { get; init; } = null!;
}

public sealed class UpdateUserResponse
{
    public UserModel User { get; init; } = null!;
}