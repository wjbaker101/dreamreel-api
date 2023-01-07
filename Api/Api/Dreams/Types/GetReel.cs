using Core.Models;

namespace Api.Api.Dreams.Types;

public sealed class GetReelResponse
{
    public List<DreamDetails> Dreams { get; init; } = new();

    public sealed class DreamDetails
    {
        public DreamModel Dream { get; init; } = null!;
        public UserModel User { get; init; } = null!;
        public List<string> Reactions { get; init; } = new();
    }
}