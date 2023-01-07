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
    private const string PEPPER = "7u@fEB$Jk@N!vtNFhc7F$K@8AAYaHHVarE3!e@sSjABL9Y3C3K";

    private readonly string _paprika;

    public PasswordService(AppSecrets appSecrets)
    {
        _paprika = appSecrets.Auth.PasswordPaprika;
    }

    public string Hash(string password, Guid salt)
    {
        var toHash = password + salt + PEPPER + _paprika;

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