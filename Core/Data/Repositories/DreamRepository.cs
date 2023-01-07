using Core.Data.Database;
using Core.Data.Records;
using Core.Types;
using NHibernate.Linq;

namespace Core.Data.Repositories;

public interface IDreamRepository
{
    DreamRecord SaveDream(DreamRecord dream);
    DreamRecord UpdateDream(DreamRecord dream);
    void DeleteDream(DreamRecord dream);
    Result<DreamRecord> GetByReference(Guid reference);
    List<DreamRecord> GetByUsers(IEnumerable<UserRecord> users);
}

public sealed class DreamRepository : ApiRepository, IDreamRepository
{
    public DreamRepository(IPgDatabase database) : base(database)
    {
    }

    public DreamRecord SaveDream(DreamRecord dream) => Save(dream);
    public DreamRecord UpdateDream(DreamRecord dream) => Update(dream);
    public void DeleteDream(DreamRecord dream) => Delete(dream);

    public Result<DreamRecord> GetByReference(Guid reference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var user = session
            .Query<DreamRecord>()
            .SingleOrDefault(x => x.Reference == reference);

        transaction.Commit();

        if (user == null)
            return Result<DreamRecord>.Failure("Unable to find dream with that reference.");

        return user;
    }

    public List<DreamRecord> GetByUsers(IEnumerable<UserRecord> users)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var dreams = session
            .Query<DreamRecord>()
            .Fetch(x => x.User)
            .FetchMany(x => x.DreamUser)
            .Where(x => users.Contains(x.User))
            .ToList();

        transaction.Commit();

        return dreams;
    }
}