namespace Core.Settings;

public sealed class AppSecrets
{
    public AuthDetails Auth { get; init; } = null!;
    public DatabaseDetails Database { get; init; } = null!;

    public sealed class AuthDetails
    {
        public string PasswordPaprika { get; init; } = null!;
        public string LoginTokenSecretKey { get; init; } = null!;
    }

    public sealed class DatabaseDetails
    {
        public string Host { get; init; } = null!;
        public int Port { get; init; }
        public string Database { get; init; } = null!;
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}