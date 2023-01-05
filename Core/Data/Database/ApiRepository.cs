namespace Core.Data.Database;

public abstract class ApiRepository
{
    protected IPgDatabase Database { get; }

    protected ApiRepository(IPgDatabase database)
    {
        Database = database;
    }

    public T Save<T>(T record)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session.Save(record);

        transaction.Commit();

        return record;
    }

    public IEnumerable<T> SaveMany<T>(IEnumerable<T> records)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        foreach (var record in records)
            session.Save(record);

        transaction.Commit();

        return records;
    }

    public T Update<T>(T record)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session.Update(record);

        transaction.Commit();

        return record;
    }

    public IEnumerable<T> UpdateMany<T>(IEnumerable<T> records)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        foreach (var record in records)
            session.Update(record);

        transaction.Commit();

        return records;
    }

    public T Delete<T>(T record)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session.Delete(record);

        transaction.Commit();

        return record;
    }

    public IEnumerable<T> DeleteMany<T>(IEnumerable<T> records)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        foreach (var record in records)
            session.Delete(record);

        transaction.Commit();

        return records;
    }
}