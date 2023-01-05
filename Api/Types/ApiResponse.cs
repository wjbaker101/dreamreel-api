namespace Api.Types;

public abstract class ApiResponse
{
    public bool IsSuccess { get; }
    public DateTime ResponseAt { get; }

    protected ApiResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
        ResponseAt = DateTime.UtcNow;
    }
}

public sealed class ApiErrorResponse : ApiResponse
{
    public string FailureReason { get; }

    private ApiErrorResponse(string failureReason) : base(false)
    {
        FailureReason = failureReason;
    }

    public static ApiErrorResponse From(string reason)
    {
        return new ApiErrorResponse(reason);
    }
}

public sealed class ApiResultResponse<T> : ApiResponse
{
    public T Result { get; }

    public ApiResultResponse(T result) : base(true)
    {
        Result = result;
    }

    public static ApiResultResponse<T> From(T result)
    {
        return new ApiResultResponse<T>(result);
    }
}