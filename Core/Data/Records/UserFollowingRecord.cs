using FluentNHibernate.Mapping;

namespace Core.Data.Records;

public class UserFollowingRecord
{
    public virtual UserRecord User { get; init; } = null!;
    public virtual UserRecord FollowingUser { get; init; } = null!;

    public override bool Equals(object? incoming)
    {
        if (incoming == null)
            return false;
        
        var incomingRecord = incoming as UserFollowingRecord;
        if (incomingRecord == null)
            return false;

        if (User.Id == incomingRecord.User.Id && FollowingUser.Id == incomingRecord.FollowingUser.Id)
            return true;

        return false;
    }

    public override int GetHashCode()
    {
        return User.Id.GetHashCode() + FollowingUser.Id.GetHashCode();
    }
}

public sealed class UserFollowingRecordMap : ClassMap<UserFollowingRecord>
{
    public UserFollowingRecordMap()
    {
        Schema("dream_reel");
        Table("user_following");
        CompositeId()
            .KeyReference(x => x.User, "user_id")
            .KeyReference(x => x.FollowingUser, "following_user_id");
        References(x => x.User);
        References(x => x.FollowingUser);
    }
}