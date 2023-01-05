namespace Core.Types;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private readonly string _failureReason;
    public string FailureReason
    {
        get
        {
            if (IsSuccess)
                throw new Exception("Tried to get failure reason from a successful Result.");

            return _failureReason;
        }
    }

    protected Result(bool isSuccess, string failureReason)
    {
        IsSuccess = isSuccess;
        _failureReason = failureReason;
    }

    public static Result Success()
    {
        return new Result(true, "");
    }

    public static Result Failure(string reason)
    {
        return new Result(false, reason);
    }

    public static Result FromFailure<T>(Result<T> result)
    {
        return Failure(result.FailureReason);
    }

    public static Result FromFailure(Result result)
    {
        return Failure(result.FailureReason);
    }
}

public sealed class Result<TValue> : Result
{
    private readonly TValue _value;
    public TValue Value
    {
        get
        {
            if (IsFailure)
                throw new Exception("Tried to get value from a failed Result.");

            return _value;
        }
    }

    private Result(bool isSuccess, TValue value, string failureReason) : base(isSuccess, failureReason)
    {
        _value = value;
    }

    public static Result<TValue> Of(TValue value)
    {
        return new Result<TValue>(true, value, "");
    }
    
    private new static Result<TValue> Success() => throw new NotImplementedException();

    public new static Result<TValue> Failure(string reason)
    {
        return new Result<TValue>(false, default!, reason);
    }

    public new static Result<TValue> FromFailure<T>(Result<T> result)
    {
        return Failure(result.FailureReason);
    }

    public new static Result<TValue> FromFailure(Result result)
    {
        return Failure(result.FailureReason);
    }

    public static implicit operator Result<TValue>(TValue value)
    {
        return Of(value);
    }

    public bool TrySuccess(out TValue value)
    {
        if (IsFailure)
        {
            value = default!;
            return false;
        }

        value = _value;
        return true;
    }
}