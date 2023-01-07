using Core.Settings;
using Core.Types;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Api.Auth;

public interface ILoginTokenService
{
    string Create(Guid userReference);
    Result<Guid> Validate(string loginToken);
}

public sealed class LoginTokenService : ILoginTokenService
{
    private const string USER_REFERENCE_CLAIM_TYPE = "userReference";

    private readonly string _secretKey;

    public LoginTokenService(AppSecrets appSecrets)
    {
        _secretKey = appSecrets.LoginToken.SecretKey;
    }

    public string Create(Guid userReference)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var handler = new JwtSecurityTokenHandler();

        var descriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(USER_REFERENCE_CLAIM_TYPE, userReference.ToString())
            })
        };

        return handler.WriteToken(handler.CreateToken(descriptor));
    }

    public Result<Guid> Validate(string loginToken)
    {
        try
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
            };

            var principle = new JwtSecurityTokenHandler().ValidateToken(loginToken, parameters, out var _);
            var userReference = principle.Claims.Single(x => x.Type == USER_REFERENCE_CLAIM_TYPE);

            return Guid.Parse(userReference.Value);
        }
        catch (Exception)
        {
            return Result<Guid>.Failure("Unable to validate or parse login token.");
        }
    }
}