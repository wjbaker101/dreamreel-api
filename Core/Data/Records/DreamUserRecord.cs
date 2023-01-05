using FluentNHibernate.Mapping;

namespace Core.Data.Records;

public class DreamUserRecord
{
    public virtual long Id { get; init; }
    public virtual DreamRecord Dream { get; init; } = null!;
    public virtual UserRecord User { get; init; } = null!;
    public virtual string? Reaction { get; set; } = null!;
}

public sealed class DreamUserRecordMap : ClassMap<DreamUserRecord>
{
    public DreamUserRecordMap()
    {
        Schema("dream_reel");
        Table("dream_user");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("dream_user_id_seq");
        References(x => x.Dream);
        References(x => x.User);
        Map(x => x.Reaction, "reaction");
    }
}