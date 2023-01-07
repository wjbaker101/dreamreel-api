using Core.Data.Records;

namespace Api.Api.Dreams.Types;

public sealed class GetReelResponse
{
    public List<Dream> Dreams { get; init; } = new();

    public sealed class Dream
    {
        public Guid Reference { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Title { get; init; } = null!;
        public string Content { get; init; } = null!;
        public DateTime DreamedAt { get; init; }
        public DreamTypeDb Type { get; init; }
        public List<string> Reactions { get; init; } = new();
        public User User { get; init; } = null!;
    }

    public sealed class User
    {
        public Guid Reference { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Username { get; init; } = null!;
    }
}