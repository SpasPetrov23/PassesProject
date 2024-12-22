using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassesProject.Controllers.Dtos;
using PassesProject.Services;

namespace PassesProject.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserUpdateDto>> GetUser(int userId)
    {
        UserUpdateDto userUpdate = await _userService.GetUser(userId);
        
        return Ok(userUpdate);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<bool>> CreateUser(UserCreateDto inputUserCreate)
    {
        bool userUpdate = await _userService.CreateUser(inputUserCreate);
        
        return Ok(userUpdate);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UserUpdateDto>> UpdateUser(int userId, UserUpdateDto inputUserUpdate)
    {
        UserUpdateDto userUpdate = await _userService.UpdateUser(userId, inputUserUpdate);
        
        return Ok(userUpdate);
    }
}