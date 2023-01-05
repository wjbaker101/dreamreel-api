namespace Api.Types;

public sealed class RequestUser
{
    public Guid Reference { get; init; }
    public string Username { get; init; } = null!;
}