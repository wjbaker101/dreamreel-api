using Core.Data.Database;
using Core.Data.Records;
using Core.Types;
using NHibernate.Linq;

namespace Core.Data.Repositories;

public interface IUserRepository
{
    UserRecord SaveUser(UserRecord user);
    UserRecord UpdateUser(UserRecord user);
    UserRecord DeleteUser(UserRecord user);
    Result<UserRecord> GetByUsername(string username);
    Result<UserRecord> GetByReference(Guid reference);
}

public sealed class UserRepository : ApiRepository, IUserRepository
{
    public UserRepository(IPgDatabase database) : base(database)
    {
    }

    public UserRecord SaveUser(UserRecord user) => Save(user);
    public UserRecord UpdateUser(UserRecord user) => Update(user);
    public UserRecord DeleteUser(UserRecord user) => Delete(user);

    public Result<UserRecord> GetByUsername(string username)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var user = session
            .Query<UserRecord>()
            .SingleOrDefault(x => x.Username.ToLower() == username.ToLower());

        transaction.Commit();
        
        if (user == null)
            return Result<UserRecord>.Failure("Unable to find user with that username.");

        return user;
    }

    public Result<UserRecord> GetByReference(Guid reference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<UserRecord>()
            .Where(x => x.Reference == reference);

        query
            .FetchMany(x => x.Dreams)
            .ToFuture();
        query
            .FetchMany(x => x.Follows)
            .ToFuture();
        query
            .FetchMany(x => x.Followers)
            .ToFuture();

        var user = query
            .ToFuture()
            .SingleOrDefault(x => x.Reference == reference);

        transaction.Commit();

        if (user == null)
            return Result<UserRecord>.Failure("Unable to find user with that reference.");

        return user;
    }
}