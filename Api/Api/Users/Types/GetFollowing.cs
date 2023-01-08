using Core.Models;

namespace Api.Api.Users.Types;

public sealed class GetFollowingResponse
{
    public List<UserDetails> Users { get; init; } = null!;

    public sealed class UserDetails
    {
        public UserModel User { get; init; } = null!;
        public int TotalDreams { get; init; }
    }
}