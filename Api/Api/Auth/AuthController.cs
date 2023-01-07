using Api.Api.Auth.Types;
using Api.Services;
using Api.Startup.Swagger;
using Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace Api.Api.Auth;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ApiController
{
    private readonly IRequestService _requestService;
    private readonly IAuthService _authService;

    public AuthController(IRequestService requestService, IAuthService authService)
    {
        _requestService = requestService;
        _authService = authService;
    }

    /// <summary>
    /// Logs in with given credentials
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("log_in")]
    [SwaggerResponseType(typeof(LogInResponse))]
    public IActionResult LogIn([FromBody] LogInRequest request)
    {
        var result = _authService.LogIn(request);

        return ToApiResponse(result);
    }
}