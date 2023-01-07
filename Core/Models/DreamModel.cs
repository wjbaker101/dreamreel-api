namespace Core.Models;

public sealed class DreamModel
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Title { get; init; } = null!;
    public string Content { get; init; } = null!;
    public DateTime DreamedAt { get; init; }
    public DreamType Type { get; init; }
}

public enum DreamType
{
    Unknown = 0,
    Dream = 1,
    Nightmare = 2
}