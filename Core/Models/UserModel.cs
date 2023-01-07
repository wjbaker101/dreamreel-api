namespace Core.Models;

public sealed class UserModel
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Username { get; init; } = null!;
    public string AvatarUrl { get; init; } = null!;
}