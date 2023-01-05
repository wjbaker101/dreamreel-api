using Api.Api.Auth;
using Api.Api.Dreams;
using Api.Api.Users;
using Api.Services;
using Core.Data.Database;
using Core.Data.Repositories;

namespace Api.Startup;

public static class ServiceCollectionExtensions
{
    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IRequestService, RequestService>();

        services.AddSingleton<IPgDatabase, PgDatabase>();

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<ILoginTokenService, LoginTokenService>();

        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IUserValidationService, UserValidationService>();

        services.AddSingleton<IDreamService, DreamService>();

        services.AddSingleton<IDreamRepository, DreamRepository>();
        services.AddSingleton<IDreamUserRepository, DreamUserRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
    }

    public static void SetupSettings(this WebApplicationBuilder builder)
    {
        var isDev = builder.Environment.IsDevelopment();

        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile(GetFile("appsettings", isDev))
            .AddJsonFile(GetFile("appsecrets", isDev));
    }

    private static string GetFile(string file, bool isDev)
    {
        if (isDev)
            return $"{file}.Development.json";

        return $"{file}.json";
    }
}