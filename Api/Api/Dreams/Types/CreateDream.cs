﻿using Core.Data.Records;

namespace Api.Api.Dreams.Types;

public sealed class CreateDreamRequest
{
    public string Title { get; init; } = null!;
    public string Content { get; init; } = null!;
    public DateTime DreamedAt { get; init; }
    public DreamType Type { get; init; }
}

public sealed class CreateDreamResponse
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Title { get; init; } = null!;
    public string Content { get; init; } = null!;
    public DateTime DreamedAt { get; init; }
    public DreamType Type { get; init; }
}