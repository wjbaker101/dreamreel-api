using Core.Models;

namespace Api.Api.Users.Types;

public sealed class GetUserResponse
{
    public UserModel User { get; init; } = null!;
    public List<DreamModel> Dreams { get; init; } = new();
    public List<UserModel> Follows { get; init; } = new();
    public List<UserModel> Followers { get; init; } = new();
}