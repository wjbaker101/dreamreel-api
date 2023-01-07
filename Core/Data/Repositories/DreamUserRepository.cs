using Core.Data.Database;
using Core.Data.Records;
using Core.Types;
using NHibernate.Linq;

namespace Core.Data.Repositories;

public interface IDreamUserRepository
{
    DreamUserRecord SaveDreamUser(DreamUserRecord dreamUser);
    DreamUserRecord UpdateDreamUser(DreamUserRecord dreamUser);
    void DeleteDreamUser(DreamUserRecord dreamUser);
    Result<DreamUserRecord> Get(DreamRecord dream, UserRecord user);
    void DeleteAllByUser(UserRecord user);
}

public sealed class DreamUserRepository : ApiRepository, IDreamUserRepository
{
    public DreamUserRepository(IPgDatabase database) : base(database)
    {
    }

    public DreamUserRecord SaveDreamUser(DreamUserRecord dreamUser) => Save(dreamUser);
    public DreamUserRecord UpdateDreamUser(DreamUserRecord dreamUser) => Update(dreamUser);
    public void DeleteDreamUser(DreamUserRecord dreamUser) => Delete(dreamUser);

    public Result<DreamUserRecord> Get(DreamRecord dream, UserRecord user)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var dreamUser = session
            .Query<DreamUserRecord>()
            .SingleOrDefault(x => x.Dream == dream && x.User == user);

        transaction.Commit();

        if (dreamUser == null)
            return Result<DreamUserRecord>.Failure("Dream user could not be found.");

        return dreamUser;
    }

    public void DeleteAllByUser(UserRecord user)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session
            .Query<DreamUserRecord>()
            .Where(x => x.User == user)
            .Delete();

        transaction.Commit();
    }
}