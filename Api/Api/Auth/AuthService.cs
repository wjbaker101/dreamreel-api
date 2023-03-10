using Api.Api.Auth.Types;
using Core.Data.Repositories;
using Core.Types;

namespace Api.Api.Auth;

public interface IAuthService
{
    Result<LogInResponse> LogIn(LogInRequest request);
}

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoginTokenService _loginTokenService;
    private readonly IPasswordService _passwordService;

    public AuthService(IUserRepository userRepository, ILoginTokenService loginTokenService, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _loginTokenService = loginTokenService;
        _passwordService = passwordService;
    }

    public Result<LogInResponse> LogIn(LogInRequest request)
    {
        var userResult = _userRepository.GetByUsername(request.Username);
        if (!userResult.TrySuccess(out var user))
            return Result<LogInResponse>.FromFailure(userResult);

        var isPasswordValid = _passwordService.Verify(request.Password, user.PasswordSalt, user.Password);
        if (!isPasswordValid)
            return Result<LogInResponse>.Failure("Incorrect password for that user.");

        var loginToken = _loginTokenService.Create(user.Reference);

        return new LogInResponse
        {
            LoginToken = loginToken
        };
    }
}