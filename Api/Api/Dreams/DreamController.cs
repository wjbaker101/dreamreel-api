using Api.Api.Auth.Attributes;
using Api.Api.Dreams.Types;
using Api.Services;
using Api.Startup;
using Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace Api.Api.Dreams;

[ApiController]
[Route("api/dreams")]
public sealed class DreamController : ApiController
{
    private readonly IRequestService _requestService;
    private readonly IDreamService _dreamService;

    public DreamController(IRequestService requestService, IDreamService dreamService)
    {
        _requestService = requestService;
        _dreamService = dreamService;
    }

    /// <summary>
    /// Creates a dream (requires logged in user)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    [SwaggerResponseType(typeof(CreateDreamResponse))]
    [RequireUser]
    public IActionResult CreateDream([FromBody] CreateDreamRequest request)
    {
        var user = _requestService.User(Request);

        var result = _dreamService.CreateDream(user, request);

        return ToApiResponse(result);
    }

    /// <summary>
    /// Updates a dream (requires logged in user)
    /// </summary>
    /// <param name="dreamReference"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{dreamReference:guid}")]
    [SwaggerResponseType(typeof(UpdateDreamResponse))]
    [RequireUser]
    public IActionResult UpdateDream([FromRoute] Guid dreamReference, [FromBody] UpdateDreamRequest request)
    {
        var user = _requestService.User(Request);

        var result = _dreamService.UpdateDream(user, dreamReference, request);

        return ToApiResponse(result);
    }

    /// <summary>
    /// Reacts to a dream (requires logged in user)
    /// </summary>
    /// <param name="dreamReference"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("{dreamReference:guid}/react")]
    [SwaggerResponseType(typeof(bool))]
    [RequireUser]
    public IActionResult ReactToDream([FromRoute] Guid dreamReference, [FromBody] ReactToDreamRequest request)
    {
        var user = _requestService.User(Request);

        var result = _dreamService.ReactToDream(user, dreamReference, request);

        return ToApiResponse(result);
    }

    /// <summary>
    /// Un-reacts to a dream (requires logged in user)
    /// </summary>
    /// <param name="dreamReference"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{dreamReference:guid}/react")]
    [SwaggerResponseType(typeof(bool))]
    [RequireUser]
    public IActionResult UnReactToDream([FromRoute] Guid dreamReference)
    {
        var user = _requestService.User(Request);

        var result = _dreamService.UnReactToDream(user, dreamReference);

        return ToApiResponse(result);
    }

    /// <summary>
    /// Gets a list of dreams created by followed users (requires logged in user)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("reel")]
    [SwaggerResponseType(typeof(GetReelResponse))]
    [RequireUser]
    public IActionResult GetReel()
    {
        var user = _requestService.User(Request);

        var result = _dreamService.GetReel(user);

        return ToApiResponse(result);
    }
}