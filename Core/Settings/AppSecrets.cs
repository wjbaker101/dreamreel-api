namespace Core.Settings;

public sealed class AppSecrets
{
    public LoginTokenDetails LoginToken { get; init; } = null!;
    public DatabaseDetails Database { get; init; } = null!;

    public sealed class LoginTokenDetails
    {
        public string SecretKey { get; init; } = null!;
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