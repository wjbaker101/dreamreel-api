using Api.Api.Auth;
using Api.Api.Users.Types;
using Core.Data.Records;
using Core.Data.Repositories;
using Core.Extensions;
using Core.Models;
using Core.Models.Mappers;
using Core.Types;

namespace Api.Api.Users;

public interface IUserService
{
    Result<CreateUserResponse> CreateUser(CreateUserRequest request);
    Result<GetUserResponse> GetUser(Guid userReference);
    Result<UpdateUserResponse> UpdateUser(Guid userReference, UpdateUserRequest request);
    Result<GetFollowingResponse> GetFollowing(Guid userReference);
    Result DeleteUser(UserModel requestUser, Guid userToDeleteReference);
    Result FollowUser(UserModel requestUser, Guid userToFollowReference);
    Result UnFollowUser(UserModel requestUser, Guid userToUnFollowReference);
}

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidationService _userValidationService;
    private readonly IPasswordService _passwordService;
    private readonly IUserAvatarService _userAvatarService;
    private readonly IDreamUserRepository _dreamUserRepository;
    private readonly IDreamRepository _dreamRepository;

    public UserService(
        IUserRepository userRepository,
        IUserValidationService userValidationService,
        IPasswordService passwordService,
        IUserAvatarService userAvatarService,
        IDreamUserRepository dreamUserRepository,
        IDreamRepository dreamRepository)
    {
        _userRepository = userRepository;
        _userValidationService = userValidationService;
        _passwordService = passwordService;
        _userAvatarService = userAvatarService;
        _dreamUserRepository = dreamUserRepository;
        _dreamRepository = dreamRepository;
    }

    public Result<CreateUserResponse> CreateUser(CreateUserRequest request)
    {
        var passwordValidResult = _userValidationService.ValidatePassword(request.Password);
        if (passwordValidResult.IsFailure)
            return Result<CreateUserResponse>.FromFailure(passwordValidResult);

        var usernameValidResult = _userValidationService.ValidateUsername(request.Username, null);
        if (usernameValidResult.IsFailure)
            return Result<CreateUserResponse>.FromFailure(usernameValidResult);

        var avatarResult = _userAvatarService.GetUrl();
        if (avatarResult.IsFailure)
            return Result<CreateUserResponse>.FromFailure(avatarResult);

        var salt = Guid.NewGuid();
        var hashedPassword = _passwordService.Hash(request.Password, salt);

        var user = _userRepository.SaveUser(new UserRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Username = request.Username,
            Password = hashedPassword,
            PasswordSalt = salt,
            AvatarUrl = avatarResult.Value
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
            Dreams = user.Dreams.ConvertAll(DreamMapper.Map),
            Follows = user.Follows.ConvertAll(UserMapper.Map),
            Followers = user.Followers.ConvertAll(UserMapper.Map)
        };
    }

    public Result<UpdateUserResponse> UpdateUser(Guid userReference, UpdateUserRequest request)
    {
        var userResult = _userRepository.GetByReference(userReference);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateUserResponse>.FromFailure(userResult);

        var usernameValidResult = _userValidationService.ValidateUsername(request.Username, user.Id);
        if (usernameValidResult.IsFailure)
            return Result<UpdateUserResponse>.FromFailure(usernameValidResult);

        user.Username = request.Username;

        _userRepository.UpdateUser(user);

        return new UpdateUserResponse
        {
            User = UserMapper.Map(user)
        };
    }

    public Result DeleteUser(UserModel requestUser, Guid userToDeleteReference)
    {
        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateUserResponse>.FromFailure(userResult);

        var userToDeleteResult = _userRepository.GetByReference(userToDeleteReference);
        if (!userToDeleteResult.TrySuccess(out var userToDelete))
            return Result<UpdateUserResponse>.FromFailure(userToDeleteResult);

        if (user.Id != userToDelete.Id)
            return Result.Failure("You can only delete your own user.");

        _dreamUserRepository.DeleteAllByUser(userToDelete);

        _userRepository.DeleteUser(userToDelete);

        return Result.Success();
    }

    public Result<GetFollowingResponse> GetFollowing(Guid userReference)
    {
        var userResult = _userRepository.GetByReference(userReference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetFollowingResponse>.FromFailure(userResult);

        var following = _userRepository.GetFollowingByUser(user);

        var dreamCounts = _dreamRepository.GetCountsByUsers(following);

        return new GetFollowingResponse
        {
            Users = following.ConvertAll(x => new GetFollowingResponse.UserDetails
            {
                User = UserMapper.Map(x),
                TotalDreams = dreamCounts.GetValueOrDefault(x.Id, 0)
            })
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