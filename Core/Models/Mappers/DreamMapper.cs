using Core.Data.Records;

namespace Core.Models.Mappers;

public static class DreamMapper
{
    public static DreamModel Map(DreamRecord dream)
    {
        return new DreamModel
        {
            Reference = dream.Reference,
            CreatedAt = dream.CreatedAt,
            Title = dream.Title,
            Content = dream.Content,
            DreamedAt = dream.DreamedAt,
            Type = (DreamType)dream.Type
        };
    }
}