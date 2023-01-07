using Api.Api.Auth.Attributes;
using Api.Api.Users.Types;
using Api.Services;
using Api.Startup.Swagger;
using Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace Api.Api.Users;

[ApiController]
[Route("api/users")]
public sealed class UserController : ApiController
{
    private readonly IRequestService _requestService;
    private readonly IUserService _userService;

    public UserController(IRequestService requestService, IUserService userService)
    {
        _requestService = requestService;
        _userService = userService;
    }

    /// <summary>
    /// Creates a user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    [SwaggerResponseType(typeof(CreateUserResponse))]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        var result = _userService.CreateUser(request);

        return ToApiResponse(result);
    }

    /// <summary>
    /// Gets a user
    /// </summary>
    /// <param name="userReference"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{userReference:guid}")]
    [SwaggerResponseType(typeof(GetUserResponse))]
    public IActionResult GetUser([FromRoute] Guid userReference)
    {
        var result = _userService.GetUser(userReference);

        return ToApiResponse(result);
    }

    /// <summary>
    /// Updates a user
    /// </summary>
    /// <param name="userReference"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{userReference:guid}")]
    [SwaggerResponseType(typeof(UpdateUserResponse))]
    public IActionResult UpdateUser([FromRoute] Guid userReference, [FromBody] UpdateUserRequest request)
    {
        var result = _userService.UpdateUser(userReference, request);

        return ToApiResponse(result);
    }

    /// <summary>
    /// Deletes a user (requires logged in user)
    /// </summary>
    /// <param name="userReference"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{userReference:guid}")]
    [SwaggerResponseType(typeof(bool))]
    [RequireUser]
    public IActionResult DeleteUser([FromRoute] Guid userReference)
    {
        var user = _requestService.User(Request);

        var result = _userService.DeleteUser(user, userReference);

        return ToApiResponse(result);
    }

    /// <summary>
    /// Follows a user (requires logged in user)
    /// </summary>
    /// <param name="userReference"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("follow/{userReference:guid}")]
    [SwaggerResponseType(typeof(bool))]
    [RequireUser]
    public IActionResult FollowUser([FromRoute] Guid userReference)
    {
        var user = _requestService.User(Request);

        var result = _userService.FollowUser(user, userReference);

        return ToApiResponse(result);
    }

    /// <summary>
    /// Un-follows a user (requires logged in user)
    /// </summary>
    /// <param name="userReference"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("follow/{userReference:guid}")]
    [SwaggerResponseType(typeof(bool))]
    [RequireUser]
    public IActionResult UnFollowUser([FromRoute] Guid userReference)
    {
        var user = _requestService.User(Request);

        var result = _userService.UnFollowUser(user, userReference);

        return ToApiResponse(result);
    }
}