using System.Security.Cryptography;
using System.Text;

namespace Api.Api.Auth;

public static class PasswordService
{
    private static readonly Guid Pepper = Guid.Parse("8b333d10-fafb-46b5-88e5-b68d2dceb623");

    public static string Hash(string password, Guid salt)
    {
        var toHash = password + salt + Pepper;

        using var sha256 = SHA256.Create();

        var hashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(toHash));

        return Convert.ToBase64String(hashed);
    }

    public static bool Verify(string password, Guid salt, string expectedHashedPassword)
    {
        var hashedPassword = Hash(password, salt);

        return hashedPassword == expectedHashedPassword;
    }
}