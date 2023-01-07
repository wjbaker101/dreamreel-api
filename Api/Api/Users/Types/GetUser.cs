using Core.Data.Records;
using Core.Models;

namespace Api.Api.Users.Types;

public sealed class GetUserResponse
{
    public UserModel User { get; init; } = null!;
    public List<Dream> Dreams { get; init; } = new();
    public List<OtherUser> Follows { get; init; } = new();
    public List<OtherUser> Followers { get; init; } = new();

    public sealed class Dream
    {
        public Guid Reference { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Title { get; init; } = null!;
        public string Content { get; init; } = null!;
        public DateTime DreamedAt { get; init; }
        public DreamTypeDb Type { get; init; }
    }

    public sealed class OtherUser
    {
        public Guid Reference { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Username { get; init; } = null!;
    }
}