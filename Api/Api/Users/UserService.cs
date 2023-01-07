using Api.Api.Auth;
using Api.Api.Users.Types;
using Core.Data.Records;
using Core.Data.Repositories;
using Core.Models;
using Core.Models.Mappers;
using Core.Types;

namespace Api.Api.Users;

public interface IUserService
{
    Result<CreateUserResponse> CreateUser(CreateUserRequest request);
    Result<GetUserResponse> GetUser(Guid userReference);
    Result<UpdateUserResponse> UpdateUser(Guid userReference, UpdateUserRequest request);
    Result FollowUser(UserModel requestUser, Guid userToFollowReference);
    Result UnFollowUser(UserModel requestUser, Guid userToUnFollowReference);
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
            User = UserMapper.Map(user)
        };
    }

    public Result<GetUserResponse> GetUser(Guid userReference)
    {
        var userResult = _userRepository.GetByReference(userReference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetUserResponse>.FromFailure(userResult);

        return new GetUserResponse
        {
            User = UserMapper.Map(user),
            Dreams = user.Dreams.Select(DreamMapper.Map).ToList(),
            Follows = user.Follows.Select(UserMapper.Map).ToList(),
            Followers = user.Followers.Select(UserMapper.Map).ToList()
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
            User = UserMapper.Map(user)
        };
    }

    public Result FollowUser(UserModel requestUser, Guid userToFollowReference)
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

    public Result UnFollowUser(UserModel requestUser, Guid userToUnFollowReference)
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