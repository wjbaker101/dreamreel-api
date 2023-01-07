using Core.Settings;
using System.Security.Cryptography;
using System.Text;

namespace Api.Api.Auth;

public interface IPasswordService
{
    string Hash(string password, Guid salt);
    bool Verify(string password, Guid salt, string expectedHashedPassword);
}

public sealed class PasswordService : IPasswordService
{
    private readonly string _pepper;

    public PasswordService(AppSecrets appSecrets)
    {
        _pepper = appSecrets.Auth.PasswordPepper;
    }

    public string Hash(string password, Guid salt)
    {
        var toHash = password + salt + _pepper;

        using var sha256 = SHA256.Create();

        var hashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(toHash));

        return Convert.ToBase64String(hashed);
    }

    public bool Verify(string password, Guid salt, string expectedHashedPassword)
    {
        var hashedPassword = Hash(password, salt);

        return hashedPassword == expectedHashedPassword;
    }
}