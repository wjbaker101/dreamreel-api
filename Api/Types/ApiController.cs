using Core.Types;
using Microsoft.AspNetCore.Mvc;

namespace Api.Types;

public abstract class ApiController : ControllerBase
{
    protected IActionResult ToApiResponse(Result result)
    {
        if (result.IsFailure)
            return BadRequest(ApiErrorResponse.From(result.FailureReason));

        return Ok(ApiResultResponse<bool>.From(true));
    }

    protected IActionResult ToApiResponse<T>(Result<T> result)
    {
        if (result.IsFailure)
            return BadRequest(ApiErrorResponse.From(result.FailureReason));

        return Ok(ApiResultResponse<T>.From(result.Value));
    }
}