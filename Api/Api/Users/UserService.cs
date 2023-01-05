using Api.Api.Auth;
using Api.Api.Users.Types;
using Api.Types;
using Core.Data.Records;
using Core.Data.Repositories;
using Core.Types;

namespace Api.Api.Users;

public interface IUserService
{
    Result<CreateUserResponse> CreateUser(CreateUserRequest request);
    Result<GetUserResponse> GetUser(Guid userReference);
    Result<UpdateUserResponse> UpdateUser(Guid userReference, UpdateUserRequest request);
    Result FollowUser(RequestUser requestUser, Guid userToFollowReference);
    Result UnFollowUser(RequestUser requestUser, Guid userToUnFollowReference);
}

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidationService _userValidationService;

    public UserService(IUserRepository userRepository, IUserValidationService userValidationService)
    {
        _userRepository = userRepository;
        _userValidationService = userValidationService;
    }

    public Result<CreateUserResponse> CreateUser(CreateUserRequest request)
    {
        var passwordValidResult = _userValidationService.ValidatePassword(request.Password);
        if (passwordValidResult.IsFailure)
            return Result<CreateUserResponse>.FromFailure(passwordValidResult);

        var usernameValidResult = _userValidationService.ValidateUsername(request.Username);
        if (usernameValidResult.IsFailure)
            return Result<CreateUserResponse>.FromFailure(usernameValidResult);

        var salt = Guid.NewGuid();
        var hashedPassword = PasswordService.Hash(request.Password, salt);

        var user = _userRepository.SaveUser(new UserRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Username = request.Username,
            Password = hashedPassword,
            PasswordSalt = salt
        });

        return new CreateUserResponse
        {
            Reference = user.Reference,
            CreatedAt = user.CreatedAt,
            Username = user.Username
        };
    }

    public Result<GetUserResponse> GetUser(Guid userReference)
    {
        var userResult = _userRepository.GetByReference(userReference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetUserResponse>.FromFailure(userResult);

        return new GetUserResponse
        {
            Reference = user.Reference,
            CreatedAt = user.CreatedAt,
            Username = user.Username,
            Dreams = user.Dreams
                .Select(x => new GetUserResponse.Dream
                {
                    Reference = x.Reference,
                    CreatedAt = x.CreatedAt,
                    Title = x.Title,
                    Content = x.Content,
                    DreamedAt = x.DreamedAt,
                    Type = x.Type
                })
                .ToList(),
            Follows = user.Follows
                .Select(x => new GetUserResponse.OtherUser
                {
                    Reference = x.Reference,
                    CreatedAt = x.CreatedAt,
                    Username = x.Username
                })
                .ToList(),
            Followers = user.Followers
                .Select(x => new GetUserResponse.OtherUser
                {
                    Reference = x.Reference,
                    CreatedAt = x.CreatedAt,
                    Username = x.Username
                })
                .ToList()
        };
    }

    public Result<UpdateUserResponse> UpdateUser(Guid userReference, UpdateUserRequest request)
    {
        var userResult = _userRepository.GetByReference(userReference);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateUserResponse>.FromFailure(userResult);

        var usernameValidResult = _userValidationService.ValidateUsername(request.Username);
        if (usernameValidResult.IsFailure)
            return Result<UpdateUserResponse>.FromFailure(usernameValidResult);

        user.Username = request.Username;

        _userRepository.UpdateUser(user);

        return new UpdateUserResponse
        {
            Reference = user.Reference,
            CreatedAt = user.CreatedAt,
            Username = user.Username
        };
    }

    public Result FollowUser(RequestUser requestUser, Guid userToFollowReference)
    {
        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateUserResponse>.FromFailure(userResult);

        var userToFollowResult = _userRepository.GetByReference(userToFollowReference);
        if (!userToFollowResult.TrySuccess(out var userToFollow))
            return Result<UpdateUserResponse>.FromFailure(userToFollowResult);

        if (user.Id == userToFollow.Id)
            return Result.Failure("Cannot follow yourself.");

        user.Follows.Add(userToFollow);

        _userRepository.UpdateUser(user);

        return Result.Success();
    }

    public Result UnFollowUser(RequestUser requestUser, Guid userToUnFollowReference)
    {
        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateUserResponse>.FromFailure(userResult);

        var userToUnFollow = user.Follows.SingleOrDefault(x => x.Reference == userToUnFollowReference);
        if (userToUnFollow == null)
            return Result.Failure("You do not follow or this user does not exist.");

        user.Follows.Remove(userToUnFollow);

        _userRepository.UpdateUser(user);

        return Result.Success();
    }
}