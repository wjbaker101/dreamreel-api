using FluentNHibernate.Mapping;

namespace Core.Data.Records;

public class UserRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; init; }
    public virtual DateTime CreatedAt { get; init; }
    public virtual string Username { get; set; } = null!;
    public virtual string Password { get; set; } = null!;
    public virtual Guid PasswordSalt { get; set; }
    public virtual ISet<DreamRecord> Dreams { get; init; } = new HashSet<DreamRecord>();
    public virtual ISet<UserRecord> Follows { get; init; } = new HashSet<UserRecord>();
    public virtual ISet<UserRecord> Followers { get; init; } = new HashSet<UserRecord>();
}

public sealed class UserRecordMap : ClassMap<UserRecord>
{
    public UserRecordMap()
    {
        Schema("dream_reel");
        Table("user");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Username, "username");
        Map(x => x.Password, "password");
        Map(x => x.PasswordSalt, "password_salt");
        HasMany(x => x.Dreams).KeyColumn("user_id");
        HasManyToMany(x => x.Follows)
            .Schema("dream_reel")
            .Table("user_following")
            .ParentKeyColumn("user_id")
            .ChildKeyColumn("following_user_id")
            .Cascade.All();
        HasManyToMany(x => x.Followers)
            .Schema("dream_reel")
            .Table("user_following")
            .ParentKeyColumn("following_user_id")
            .ChildKeyColumn("user_id")
            .Inverse()
            .Cascade.All();
    }
}