using Api.Api.Users.Types;
using Core.Data.Repositories;
using Core.Types;

namespace Api.Api.Users;

public interface IUserValidationService
{
    Result ValidatePassword(string password);
    Result ValidateUsername(string username, long? userId);
}

public sealed class UserValidationService : IUserValidationService
{
    private readonly IUserRepository _userRepository;

    public UserValidationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Result ValidatePassword(string password)
    {
        const int minLength = 10;
        const int maxLength = 150;

        if (password.Length < minLength)
            return Result.Failure($"Password is too short, length must be {minLength}-{maxLength} characters (inclusive).");
        if (password.Length > maxLength)
            return Result.Failure($"Password is too long, length must be {minLength}-{maxLength} characters (inclusive).");

        var containsNumber = false;
        var containsLowercase = false;
        var containsUppercase = false;
        var containsSymbol = false;

        foreach (var character in password)
        {
            containsNumber |= char.IsDigit(character);
            containsLowercase |= char.IsLower(character);
            containsUppercase |= char.IsUpper(character);
            containsSymbol |= char.IsSymbol(character);
        }

        if (!containsNumber)
            return Result.Failure("Password must contain a number.");
        if (!containsLowercase)
            return Result.Failure("Password must contain a lowercase character.");
        if (!containsUppercase)
            return Result.Failure("Password must contain an uppercase character.");
        if (!containsSymbol)
            return Result.Failure("Password must contain a symbol.");

        return Result.Success();
    }

    public Result ValidateUsername(string username, long? userId)
    {
        var existingUserResult = _userRepository.GetByUsername(username);
        if (existingUserResult.IsSuccess && existingUserResult.Value.Id != userId)
            return Result<CreateUserResponse>.Failure("A user already exists with that username.");

        return Result.Success();
    }
}