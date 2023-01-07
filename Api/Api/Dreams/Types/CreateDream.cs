using Core.Data.Records;
using Core.Models;

namespace Api.Api.Dreams.Types;

public sealed class CreateDreamRequest
{
    public string Title { get; init; } = null!;
    public string Content { get; init; } = null!;
    public DateTime DreamedAt { get; init; }
    public DreamTypeDb Type { get; init; }
}

public sealed class CreateDreamResponse
{
    public DreamModel Dream { get; init; } = null!;
}