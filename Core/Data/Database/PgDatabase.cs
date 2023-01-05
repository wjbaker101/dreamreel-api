using Core.Settings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Core.Data.Database;

public interface IPgDatabase
{
    ISessionFactory SessionFactory { get; }
}

public sealed class PgDatabase : IPgDatabase
{
    public ISessionFactory SessionFactory { get; }

    public PgDatabase(AppSecrets appSecrets)
    {
        var database = appSecrets.Database;

        SessionFactory = Fluently.Configure()
            .Database(PostgreSQLConfiguration.Standard.ConnectionString(c => c
                .Host(database.Host)
                .Port(database.Port)
                .Database(database.Database)
                .Username(database.Username)
                .Password(database.Password)))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PgDatabase>())
            .BuildSessionFactory();
    }
}