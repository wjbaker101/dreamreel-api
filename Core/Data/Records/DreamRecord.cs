using FluentNHibernate.Mapping;

namespace Core.Data.Records;

public class DreamRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; init; }
    public virtual DateTime CreatedAt { get; init; }
    public virtual UserRecord User { get; init; } = null!;
    public virtual string Title { get; set; } = null!;
    public virtual string Content { get; set; } = null!;
    public virtual DateTime DreamedAt { get; set; }
    public virtual DreamTypeDb Type { get; set; }
    public virtual ISet<DreamUserRecord> DreamUser { get; init; } = new HashSet<DreamUserRecord>();
}

public enum DreamTypeDb
{
    Unknown = 0,
    Dream = 1,
    Nightmare = 2
}

public sealed class DreamRecordMap : ClassMap<DreamRecord>
{
    public DreamRecordMap()
    {
        Schema("dream_reel");
        Table("dream");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("dream_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        References(x => x.User);
        Map(x => x.Title, "title");
        Map(x => x.Content, "content");
        Map(x => x.DreamedAt, "dreamed_at");
        Map(x => x.Type, "type").CustomType<DreamTypeDb>();
        HasMany(x => x.DreamUser).KeyColumn("dream_id");
    }
}