using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassesProject.Services;
using PassesProject.Services.Dtos;

namespace PassesProject.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PassesController : ControllerBase
{
    private readonly PassesService _passesService;

    public PassesController(PassesService passesService)
    {
        _passesService = passesService;
    }

    [Authorize]
    [HttpGet]
    public ActionResult<MostSuccessfulReceiverDto> GetMostCompletePassPercentage()
    {
        MostSuccessfulReceiverDto result = _passesService.CalculateMostCompletePassPercentage();

        return result;
    }

    [Authorize]
    [HttpGet]
    public ActionResult<LongestDistancePassDto> GetLongestPass()
    {
        LongestDistancePassDto result = _passesService.CalculateLongestPass();

        return result;
    }
}