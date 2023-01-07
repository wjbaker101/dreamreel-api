using Api.Api.Dreams.Types;
using Api.Types;
using Core.Data.Records;
using Core.Data.Repositories;
using Core.Models.Mappers;
using Core.Types;

namespace Api.Api.Dreams;

public interface IDreamService
{
    Result<CreateDreamResponse> CreateDream(RequestUser requestUser, CreateDreamRequest request);
    Result<UpdateDreamResponse> UpdateDream(RequestUser requestUser, Guid dreamReference, UpdateDreamRequest request);
    Result ReactToDream(RequestUser requestUser, Guid dreamReference, ReactToDreamRequest request);
    Result UnReactToDream(RequestUser requestUser, Guid dreamReference);
    Result<GetReelResponse> GetReel(RequestUser requestUser);
}

public sealed class DreamService : IDreamService
{
    private readonly IUserRepository _userRepository;
    private readonly IDreamRepository _dreamRepository;
    private readonly IDreamUserRepository _dreamUserRepository;

    public DreamService(IUserRepository userRepository, IDreamRepository dreamRepository, IDreamUserRepository dreamUserRepository)
    {
        _userRepository = userRepository;
        _dreamRepository = dreamRepository;
        _dreamUserRepository = dreamUserRepository;
    }

    public Result<CreateDreamResponse> CreateDream(RequestUser requestUser, CreateDreamRequest request)
    {
        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<CreateDreamResponse>.FromFailure(userResult);

        var dream = _dreamRepository.SaveDream(new DreamRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            User = user,
            Title = request.Title,
            Content = request.Content,
            DreamedAt = request.DreamedAt,
            Type = request.Type
        });

        return new CreateDreamResponse
        {
            Dream = DreamMapper.Map(dream)
        };
    }

    public Result<UpdateDreamResponse> UpdateDream(RequestUser requestUser, Guid dreamReference, UpdateDreamRequest request)
    {
        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateDreamResponse>.FromFailure(userResult);

        var dreamResult = _dreamRepository.GetByReference(dreamReference);
        if (!dreamResult.TrySuccess(out var dream))
            return Result<UpdateDreamResponse>.FromFailure(dreamResult);

        if (dream.User.Id != user.Id)
            return Result<UpdateDreamResponse>.Failure("You cannot update a dream that you did not create.");

        dream.Title = request.Title;
        dream.Content = request.Content;
        dream.DreamedAt = request.DreamedAt;
        dream.Type = request.Type;

        _dreamRepository.UpdateDream(dream);

        return new UpdateDreamResponse
        {
            Dream = DreamMapper.Map(dream)
        };
    }

    public Result ReactToDream(RequestUser requestUser, Guid dreamReference, ReactToDreamRequest request)
    {
        var validReactions = new HashSet<string>
        {
            "😭", // Sad
            "😐", // Neutral
            "😆", // Happy
            "😱", // Scared + Shocked
        };
        var isValid = validReactions.Contains(request.Reaction);
        if (!isValid)
            return Result.Failure("The given reaction is not valid.");

        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateDreamResponse>.FromFailure(userResult);

        var dreamResult = _dreamRepository.GetByReference(dreamReference);
        if (!dreamResult.TrySuccess(out var dream))
            return Result<UpdateDreamResponse>.FromFailure(dreamResult);

        var dreamUserResult = _dreamUserRepository.Get(dream, user);
        if (!dreamUserResult.TrySuccess(out var dreamUser))
        {
            dreamUser = _dreamUserRepository.SaveDreamUser(new DreamUserRecord
            {
                Dream = dream,
                User = user,
                Reaction = null
            });
        }

        dreamUser.Reaction = request.Reaction;

        _dreamUserRepository.UpdateDreamUser(dreamUser);

        return Result.Success();
    }

    public Result UnReactToDream(RequestUser requestUser, Guid dreamReference)
    {
        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateDreamResponse>.FromFailure(userResult);

        var dreamResult = _dreamRepository.GetByReference(dreamReference);
        if (!dreamResult.TrySuccess(out var dream))
            return Result<UpdateDreamResponse>.FromFailure(dreamResult);

        var dreamUserResult = _dreamUserRepository.Get(dream, user);
        if (!dreamUserResult.TrySuccess(out var dreamUser))
            return Result<UpdateDreamResponse>.FromFailure(dreamUserResult);

        dreamUser.Reaction = null;

        _dreamUserRepository.UpdateDreamUser(dreamUser);

        return Result.Success();
    }

    public Result<GetReelResponse> GetReel(RequestUser requestUser)
    {
        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetReelResponse>.FromFailure(userResult);

        var dreams = _dreamRepository.GetByUsers(user.Follows);

        return new GetReelResponse
        {
            Dreams = dreams
                .Select(dream => new GetReelResponse.DreamDetails
                {
                    Dream = DreamMapper.Map(dream),
                    User = UserMapper.Map(user),
                    Reactions = dream.DreamUser.Select(x => x.Reaction).Where(x => x != null).ToList()
                })
                .ToList()
        };
    }
}